#include <nds/ndstypes.h>
#include "native_pinyin.h"

#define MAX_PINYIN_LENGTH 6

typedef struct
{
    u32 pinyinNum;
    u32 candidateOffset;
    u32 maxCandidateNum;
    u32 reserved;
} PinyinTableHeader;

typedef struct
{
    u32 pinyin;
    u16 candidateOffset;
    u16 candidateNum;
} PinyinTableEntry;

extern const u8 gPinyinTableBin[];

static const PinyinTableHeader *sHeader;
static const PinyinTableEntry *sEntries;
static const u16 *sCandidates;
static int sCandidateCount;
static u8 sLetters[MAX_PINYIN_LENGTH];
static int sLetterCount;

static u32 EncodePinyin(void)
{
    u32 value = 0;
    int i;
    for (i = 0; i < sLetterCount; i++)
    {
        value |= (((sLetters[i] - 'a' + 1) & 0x1f) << (25 - i * 5));
    }
    return value;
}

static void FindCandidates(void)
{
    int left;
    int right;
    u32 value;

    sCandidates = 0;
    sCandidateCount = 0;
    if (!sHeader || sLetterCount == 0)
    {
        return;
    }

    value = EncodePinyin();
    left = 0;
    right = (int)sHeader->pinyinNum - 1;
    while (left <= right)
    {
        int middle = (left + right) / 2;
        const PinyinTableEntry *entry = &sEntries[middle];
        if (entry->pinyin == value)
        {
            sCandidates = (const u16 *)(gPinyinTableBin + sHeader->candidateOffset +
                                        entry->candidateOffset * sizeof(u16));
            sCandidateCount = entry->candidateNum;
            return;
        }
        if (entry->pinyin < value)
        {
            left = middle + 1;
        }
        else
        {
            right = middle - 1;
        }
    }
}

void NativePinyin_Init(void)
{
    sHeader = (const PinyinTableHeader *)gPinyinTableBin;
    sEntries = (const PinyinTableEntry *)(gPinyinTableBin + sizeof(PinyinTableHeader));
    NativePinyin_Reset();
}

void NativePinyin_Reset(void)
{
    sLetterCount = 0;
    sCandidates = 0;
    sCandidateCount = 0;
}

int NativePinyin_Append(u8 letter)
{
    if (letter < 'a' || letter > 'z' || sLetterCount >= MAX_PINYIN_LENGTH)
    {
        return 0;
    }
    sLetters[sLetterCount++] = letter;
    FindCandidates();
    return 1;
}

int NativePinyin_Delete(void)
{
    if (sLetterCount == 0)
    {
        return 0;
    }
    sLetterCount--;
    FindCandidates();
    return 1;
}

int NativePinyin_GetLetterCount(void)
{
    return sLetterCount;
}

const u8 *NativePinyin_GetLetters(void)
{
    return sLetters;
}

int NativePinyin_GetCandidateCount(void)
{
    return sCandidateCount;
}

u16 NativePinyin_GetCandidate(int index)
{
    if (!sCandidates || index < 0 || index >= sCandidateCount)
    {
        return 0;
    }
    return sCandidates[index];
}
