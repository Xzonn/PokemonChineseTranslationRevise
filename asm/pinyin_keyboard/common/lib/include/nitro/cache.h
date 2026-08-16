#ifndef NITRO_CACHE_H
#define NITRO_CACHE_H

#include <nds/ndstypes.h>

void DC_InvalidateRange(void *startAddr, u32 nBytes);
void DC_StoreRange(const void *startAddr, u32 nBytes);
void DC_FlushRange(const void *startAddr, u32 nBytes);

#endif
