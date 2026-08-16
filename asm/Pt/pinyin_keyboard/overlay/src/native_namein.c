#include <nds/ndstypes.h>
#include "native_pinyin.h"

#define IMPORT __attribute__((naked))

#define NAMEIN_PROC_MAIN_SLOT 0x020F2440
#define NAMEIN_PROC_END_SLOT 0x020F2444
#define STR_NAME_TABLE 0x02100048

#define WORK_WORDMAP_OFFSET 0x03A
#define WORK_CURSOR_X_OFFSET 0x01C
#define WORK_CURSOR_Y_OFFSET 0x020
#define WORK_CURSOR_ON_OFFSET 0x030
#define WORK_INPUT_OFFSET 0x0D8
#define WORK_NOW_INPUT_OFFSET 0x158
#define WORK_NAMELINE_OFFSET 0x364
#define WORK_WINDOWS_OFFSET 0x3B8
#define WORK_MODE_OFFSET 0x460
#define WORK_BGCHAR_OFFSET 0x4B0

#define WINDOW_SIZE 0x10
#define WINDOW_WORDPANEL0 0
#define WINDOW_WORDPANEL1 1
#define WINDOW_RESULT 3

#define INPUT_WORD_W 13
#define INPUT_WORD_H 6
#define INPUT_WORD_MAX 32
#define MODE_HIRA 0
#define MODE_KANA 1
#define MODE_ALPHA 2
#define MODE_KIGOU 3
#define MODE_NUMCODE 4

#define EOM_CODE 0xFFFF
#define SKIP_CODE 0xD004
#define NAMEIN_HIRA 0xE002
#define NAMEIN_KANA 0xE003
#define NAMEIN_ALPHA 0xE004
#define NAMEIN_KIGOU 0xE005
#define NAMEIN_MODORU 0xE007

#define UPPERCASE_A_CODE 0x00AC
#define LOWERCASE_A_CODE 0x00C6
#define DIGIT_ZERO_CODE 0x00A2
#define PAGE_PREV_CODE 0x01FC
#define PAGE_NEXT_CODE 0x01FD
#define SLASH_CODE 0x00E7
#define CANDIDATES_PER_PAGE 26
#define PANEL_COLOR 0x000E0F00
#define RESULT_COLOR 0x000E0F01

IMPORT int NameInProc_Main(u32 proc, int *seq) {}
IMPORT int NameInProc_End(u32 proc, int *seq) {}
IMPORT void FillTextWindow(void *window, u16 value) {}
IMPORT void BmpWinOn(void *window) {}
IMPORT void NameinWordPanelPrint(
    void *window, const u16 *text, int x, int y,
    int spacing, int putMode, u32 color, void *dakuten) {}
IMPORT void InputResultUnderLineMove(void **actors, int position, int maximum) {}
IMPORT void MakeWordMap(u16 *map, int mode) {}
IMPORT void WordPanelSetUp(void *window, u16 background, int frame, u32 color, void *dakuten) {}
IMPORT int DecideMainButton(void *work, u16 code, int pad) {}

static void *sContext;
static int sNeedsRedraw;
static int sCandidatePage;
static int sKanaIsKatakana = 1;
static int sRetailTablesCaptured;
static const u16 *sRetailHiraRows[5];
static const u16 *sRetailKanaRows[5];
static u16 sRows[5][INPUT_WORD_W + 1];

static u16 *WordMap(void *work)
{
    return (u16 *)((u8 *)work + WORK_WORDMAP_OFFSET);
}

static u16 *InputWord(void *work)
{
    return (u16 *)((u8 *)work + WORK_INPUT_OFFSET);
}

static u16 *NowInput(void *work)
{
    return (u16 *)((u8 *)work + WORK_NOW_INPUT_OFFSET);
}

static int *WorkMode(void *work)
{
    return (int *)((u8 *)work + WORK_MODE_OFFSET);
}

static void *Window(void *work, int index)
{
    return (u8 *)work + WORK_WINDOWS_OFFSET + index * WINDOW_SIZE;
}

static int IsPinyinMode(int mode)
{
    return mode == MODE_HIRA;
}

static u16 PinyinNoOpCode(int mode)
{
    (void)mode;
    return NAMEIN_HIRA;
}

static int CandidatePageCount(void)
{
    int count = NativePinyin_GetCandidateCount();
    return (count + CANDIDATES_PER_PAGE - 1) / CANDIDATES_PER_PAGE;
}

static void ClampCandidatePage(void)
{
    int pageCount = CandidatePageCount();
    if (pageCount == 0)
    {
        sCandidatePage = 0;
    }
    else if (sCandidatePage >= pageCount)
    {
        sCandidatePage = pageCount - 1;
    }
}

static int DecodeLatin(u16 code, u8 *letter)
{
    if (code >= LOWERCASE_A_CODE && code < LOWERCASE_A_CODE + 26)
    {
        *letter = (u8)('a' + code - LOWERCASE_A_CODE);
        return 1;
    }
    if (code >= UPPERCASE_A_CODE && code < UPPERCASE_A_CODE + 26)
    {
        *letter = (u8)('a' + code - UPPERCASE_A_CODE);
        return 1;
    }
    return 0;
}

static void CaptureRetailTables(void)
{
    const u16 **nameTables = (const u16 **)STR_NAME_TABLE;
    int i;

    if (sRetailTablesCaptured)
    {
        return;
    }
    for (i = 0; i < 5; i++)
    {
        sRetailHiraRows[i] = nameTables[MODE_HIRA * 5 + i];
        sRetailKanaRows[i] = nameTables[MODE_KANA * 5 + i];
    }
    sRetailTablesCaptured = 1;
}

static void InstallDynamicTables(void)
{
    const u16 **nameTables = (const u16 **)STR_NAME_TABLE;
    int i;

    /* Pinyin owns tab 0; tab 1 switches between the two retail kana tables. */
    for (i = 0; i < 5; i++)
    {
        nameTables[MODE_HIRA * 5 + i] = sRows[i];
        nameTables[MODE_KANA * 5 + i] =
            sKanaIsKatakana ? sRetailKanaRows[i] : sRetailHiraRows[i];
    }
}

static void BuildRows(void)
{
    int row;
    int column;
    int candidateCount = NativePinyin_GetCandidateCount();
    int letterCount = NativePinyin_GetLetterCount();
    int pageCount;
    int candidateStart;
    const u8 *letters = NativePinyin_GetLetters();

    ClampCandidatePage();
    pageCount = CandidatePageCount();
    candidateStart = sCandidatePage * CANDIDATES_PER_PAGE;

    for (row = 0; row < 5; row++)
    {
        for (column = 0; column < INPUT_WORD_W; column++)
        {
            sRows[row][column] = SKIP_CODE;
        }
        sRows[row][INPUT_WORD_W] = EOM_CODE;
    }

    for (column = 0; column < 13; column++)
    {
        sRows[0][column] = LOWERCASE_A_CODE + column;
        sRows[1][column] = LOWERCASE_A_CODE + 13 + column;
    }

    if (letterCount > 0)
    {
        int start = pageCount > 1 ? 1 + (6 - letterCount) / 2
                                  : (INPUT_WORD_W - letterCount) / 2;
        for (column = 0; column < letterCount; column++)
        {
            sRows[2][start + column] = LOWERCASE_A_CODE + letters[column] - 'a';
        }
    }

    if (pageCount > 1)
    {
        if (sCandidatePage > 0)
        {
            sRows[2][11] = PAGE_PREV_CODE;
        }
        if (sCandidatePage + 1 < pageCount)
        {
            sRows[2][12] = PAGE_NEXT_CODE;
        }
        sRows[2][8] = DIGIT_ZERO_CODE + sCandidatePage + 1;
        sRows[2][9] = SLASH_CODE;
        sRows[2][10] = DIGIT_ZERO_CODE + pageCount;
    }

    for (column = 0;
         column < CANDIDATES_PER_PAGE && candidateStart + column < candidateCount;
         column++)
    {
        sRows[3 + column / INPUT_WORD_W][column % INPUT_WORD_W] =
            NativePinyin_GetCandidate(candidateStart + column);
    }
}

static void PrepareWordMap(void *work)
{
    u16 *map = WordMap(work);
    int mode = *WorkMode(work);
    u16 noOp;
    int column;

    MakeWordMap(map, mode);

    /* Keep four tabs: pinyin, kana, ABC, and number/symbol. */
    map[0] = NAMEIN_HIRA;
    map[1] = NAMEIN_KANA;
    map[2] = NAMEIN_KANA;
    map[3] = NAMEIN_ALPHA;
    map[4] = NAMEIN_KIGOU;
    map[5] = NAMEIN_KIGOU;

    if (!IsPinyinMode(mode))
    {
        return;
    }
    noOp = PinyinNoOpCode(mode);

    /* Composition and empty candidate cells do not insert characters. */
    for (column = 0; column < INPUT_WORD_W; column++)
    {
        map[3 * INPUT_WORD_W + column] = noOp;
        if (sRows[3][column] == SKIP_CODE)
        {
            map[4 * INPUT_WORD_W + column] = noOp;
        }
        if (sRows[4][column] == SKIP_CODE)
        {
            map[5 * INPUT_WORD_W + column] = noOp;
        }
    }
    if (sCandidatePage > 0)
    {
        map[3 * INPUT_WORD_W + 11] = PAGE_PREV_CODE;
    }
    if (sCandidatePage + 1 < CandidatePageCount())
    {
        map[3 * INPUT_WORD_W + 12] = PAGE_NEXT_CODE;
    }
}

static void RedrawPanels(void *work)
{
    int mode = *WorkMode(work);
    u16 background = mode == MODE_KANA ? 0x0707 : 0x0404;
    void *characterData = *(void **)((u8 *)work + WORK_BGCHAR_OFFSET);
    void *dakuten = characterData ? *(void **)((u8 *)characterData + 20) : 0;

    WordPanelSetUp(Window(work, WINDOW_WORDPANEL0), background,
                   mode, PANEL_COLOR, dakuten);
    WordPanelSetUp(Window(work, WINDOW_WORDPANEL1), background,
                   mode, PANEL_COLOR, dakuten);
    sNeedsRedraw = 0;
}

static void RedrawResult(void *work)
{
    u16 length = *NowInput(work);
    void *window = Window(work, WINDOW_RESULT);

    FillTextWindow(window, 0x0101);
    if (length != 0)
    {
        NameinWordPanelPrint(window, InputWord(work), 0, 0, 12, 0,
                             RESULT_COLOR, 0);
    }
    else
    {
        BmpWinOn(window);
    }
    InputResultUnderLineMove(
        (void **)((u8 *)work + WORK_NAMELINE_OFFSET),
        length, *(int *)((u8 *)work + 12));
}

static void RestoreInput(void *work, const u16 *oldInput, u16 oldLength)
{
    u16 *input = InputWord(work);
    int i;
    for (i = 0; i < INPUT_WORD_MAX; i++)
    {
        input[i] = oldInput[i];
    }
    *NowInput(work) = oldLength;
    RedrawResult(work);
}

/*
 * Direct target for all four retail DecideMainButton call sites.  Handling
 * semantic button codes here avoids inferring an action from nowinput after
 * the fact (which cannot distinguish backspace on an empty confirmed name).
 */
int NativeNameIn_DecideMainButton(void *work, u16 code, int pad)
{
    int mode = *WorkMode(work);

    if (mode == MODE_HIRA)
    {
        u8 letter;

        if (code == NAMEIN_MODORU && NativePinyin_GetLetterCount() > 0)
        {
            NativePinyin_Delete();
            sCandidatePage = 0;
            sNeedsRedraw = 1;
            return DecideMainButton(work, NAMEIN_HIRA, pad);
        }
        if (DecodeLatin(code, &letter))
        {
            NativePinyin_Append(letter);
            sCandidatePage = 0;
            sNeedsRedraw = 1;
            return DecideMainButton(work, NAMEIN_HIRA, pad);
        }
        if (code == PAGE_PREV_CODE)
        {
            if (sCandidatePage > 0)
            {
                sCandidatePage--;
            }
            sNeedsRedraw = 1;
            return DecideMainButton(work, NAMEIN_HIRA, pad);
        }
        if (code == PAGE_NEXT_CODE)
        {
            if (sCandidatePage + 1 < CandidatePageCount())
            {
                sCandidatePage++;
            }
            sNeedsRedraw = 1;
            return DecideMainButton(work, NAMEIN_HIRA, pad);
        }
        if (NativePinyin_GetLetterCount() > 0 &&
            code != NAMEIN_HIRA && code != SKIP_CODE)
        {
            NativePinyin_Reset();
            sCandidatePage = 0;
            sNeedsRedraw = 1;
        }
    }
    else if (mode == MODE_KANA && code == NAMEIN_KANA)
    {
        sKanaIsKatakana ^= 1;
        InstallDynamicTables();
        sNeedsRedraw = 1;
        return DecideMainButton(work, NAMEIN_KANA, pad);
    }

    return DecideMainButton(work, code, pad);
}

static void BeginContext(void *work)
{
    sContext = work;
    NativePinyin_Reset();
    sCandidatePage = 0;
    BuildRows();
    InstallDynamicTables();
    PrepareWordMap(work);
    if (IsPinyinMode(*WorkMode(work)))
    {
        sNeedsRedraw = 1;
    }
    else
    {
        sNeedsRedraw = 0;
    }
}

static int Hook_NameInProc_Main(u32 proc, int *seq)
{
    void *work = *(void **)(proc + 0x1C);
    int oldMode;
    int wasPinyin;
    int result;

    if (!work || *(int *)work == MODE_NUMCODE)
    {
        return NameInProc_Main(proc, seq);
    }

    if (work != sContext)
    {
        BeginContext(work);
    }

    BuildRows();
    InstallDynamicTables();
    oldMode = *WorkMode(work);
    wasPinyin = IsPinyinMode(oldMode);
    PrepareWordMap(work);
    if (wasPinyin)
    {
        if (sNeedsRedraw)
        {
            RedrawPanels(work);
        }
    }

    result = NameInProc_Main(proc, seq);

    if (*(void **)(proc + 0x1C) == work)
    {
        int newMode = *WorkMode(work);

        if (newMode != oldMode)
        {
            NativePinyin_Reset();
            sCandidatePage = 0;
            /*
             * PanelFunc has already redrawn only the incoming BG for this
             * mode-change frame.  Redrawing both buffers here would overwrite
             * the outgoing tab and make it briefly look like the pinyin tab.
             */
            sNeedsRedraw = 0;
        }

        BuildRows();
        InstallDynamicTables();
        PrepareWordMap(work);
        if (IsPinyinMode(newMode) || newMode == MODE_KANA)
        {
            if (sNeedsRedraw)
            {
                RedrawPanels(work);
            }
        }
        else
        {
            sNeedsRedraw = 0;
        }
    }

    return result;
}

static int Hook_NameInProc_End(u32 proc, int *seq)
{
    int result = NameInProc_End(proc, seq);
    sContext = 0;
    NativePinyin_Reset();
    sCandidatePage = 0;
    return result;
}

void NativeNameIn_Install(void)
{
    void **mainSlot = (void **)NAMEIN_PROC_MAIN_SLOT;
    void **endSlot = (void **)NAMEIN_PROC_END_SLOT;

    NativePinyin_Init();
    CaptureRetailTables();
    if ((u32)*mainSlot == ((u32)NameInProc_Main | 1))
    {
        *mainSlot = Hook_NameInProc_Main;
    }
    if ((u32)*endSlot == ((u32)NameInProc_End | 1))
    {
        *endSlot = Hook_NameInProc_End;
    }
}
