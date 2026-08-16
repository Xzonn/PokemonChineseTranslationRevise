#ifndef NITRO_FS_H
#define NITRO_FS_H

#include <nds/ndstypes.h>
#include "nitro/sdk_ver.h"

typedef struct {
#if NITROSDK_VER >= MAKE_NITROSDK_VER(5, 0)
    u8 _[0x48];
#else
    u8 _[0x3c];
#endif
} FSFile;

typedef struct {
    u8 _[44];
} FSOverlayInfo;


void FS_InitFile(FSFile *p_file);
bool FS_OpenFile(FSFile *p_file, const char *path);

#if NITROSDK_VER >= MAKE_NITROSDK_VER(5, 0)
u32 FS_GetLength(const FSFile *p_file);
#elif NITROSDK_VER >= MAKE_NITROSDK_VER(3, 0)
static inline u32 FS_GetLength(const FSFile *p_file) {
    return *(u32 *)((u8*)p_file + 0x28) - *(u32 *)((u8*)p_file + 0x24);
}
#else
static inline u32 FS_GetLength(const FSFile *p_file) {
    return *(u32 *)((u8*)p_file + 0x24) - *(u32 *)((u8*)p_file + 0x20);
}
#endif

bool FS_SeekFile(FSFile *p_file, s32 offset, s32 origin);
s32 FS_ReadFile(FSFile *p_file, void *dst, s32 len);
bool FS_CloseFile(FSFile *p_file);

bool FS_LoadOverlay(u32 target, u32 id);
bool FS_LoadOverlayInfo(FSOverlayInfo *p_ovi, u32 target, u32 id);
bool FS_LoadOverlayImage(FSOverlayInfo *p_ovi);
void FS_StartOverlay(FSOverlayInfo *p_ovi);

#endif
