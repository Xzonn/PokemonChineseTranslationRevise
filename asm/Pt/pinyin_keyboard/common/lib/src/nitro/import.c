#include <nds/ndstypes.h>
#include "nitro/sdk_ver.h"
#include "nitro/fs.h"
#include "nitro/heap.h"
#include "nitro/thread.h"
#include "nitro/tp.h"
#include "nitro/twl.h"

#define IMPORT __attribute__((naked))

IMPORT void FS_InitFile(FSFile *p_file) {}

IMPORT bool FS_OpenFile(FSFile *p_file, const char *path) {}

#if NITROSDK_VER >= MAKE_NITROSDK_VER(5, 0)
IMPORT u32 FS_GetLength(const FSFile *p_file) {}
#endif

IMPORT bool FS_SeekFile( FSFile *p_file, s32 offset, s32 origin ) {}

IMPORT s32 FS_ReadFile(FSFile *p_file, void *dst, s32 len) {}

IMPORT bool FS_CloseFile(FSFile *p_file) {}

IMPORT bool FS_LoadOverlay(u32 target, u32 id) {}

IMPORT bool FS_LoadOverlayInfo(FSOverlayInfo *p_ovi, u32 target, u32 id) {}

IMPORT bool FS_LoadOverlayImage(FSOverlayInfo *p_ovi) {}

IMPORT void FS_StartOverlay(FSOverlayInfo *p_ovi) {}

IMPORT void * OS_AllocFromHeap(s32 id, s32 heap, u32 size) {}

IMPORT void OS_FreeToHeap(s32 id, s32 heap, void *ptr) {}

IMPORT void *FndAllocFromExpHeapEx(void *heapHandle, u32 size, s32 flag) {}

IMPORT void FndFreeToExpHeap(void *heapHandle, void *ptr) {}

IMPORT void OS_CreateThread(OSThread *thread, void (*func) (void *), void *arg, void *stack, u32 stackSize, u32 prio) {}

IMPORT void OS_WakeupThreadDirect(OSThread *thread) {}

IMPORT void OS_ExitThread(void) {}

IMPORT void OS_SleepThread(void *queue) {}

IMPORT void TP_GetCalibratedPoint(TPData *disp, const TPData *raw) {}

#if NITROSDK_VER >= MAKE_NITROSDK_VER(5, 0)
IMPORT bool OS_IsRunOnTwl() {}
#endif
