#ifndef NITRO_HEAP_H
#define NITRO_HEAP_H

#include <nds/ndstypes.h>

void * OS_AllocFromHeap(s32 id, s32 heap, u32 size);
void OS_FreeToHeap(s32 id, s32 heap, void *ptr);

void *FndAllocFromExpHeapEx(void *heapHandle, u32 size, s32 flag);
void FndFreeToExpHeap(void *heapHandle, void *ptr);

#endif
