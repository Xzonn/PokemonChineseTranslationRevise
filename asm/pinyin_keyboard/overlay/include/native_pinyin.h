#ifndef NATIVE_PINYIN_H
#define NATIVE_PINYIN_H

#include <nds/ndstypes.h>

void NativePinyin_Init(void);
void NativePinyin_Reset(void);
int NativePinyin_Append(u8 letter);
int NativePinyin_Delete(void);
int NativePinyin_GetLetterCount(void);
const u8 *NativePinyin_GetLetters(void);
int NativePinyin_GetCandidateCount(void);
u16 NativePinyin_GetCandidate(int index);

#endif
