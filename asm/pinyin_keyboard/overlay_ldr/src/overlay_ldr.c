#include <nds/ndstypes.h>
#include "nitro/fs.h"

#ifndef OVERLAY_ID
#define OVERLAY_ID -1
#endif

extern void (*Orig_OverlayStaticInitBegin[])();
extern void (*Orig_OverlayStaticInitEnd[])();

void LoadOverlay();

typedef int (*NameInMainFunc)(u32 proc, int *seq);
extern int NameInProc_Main(u32 proc, int *seq);
extern u8 NameInProcMainSlot[];

int NativeNameIn_LazyMain(u32 proc, int *seq)
{
    NameInMainFunc *slot = (NameInMainFunc *)NameInProcMainSlot;
    /* PROC caches this callback when it is created.  Loading the overlay can
     * replace the global procedure table, but it cannot replace the callback
     * already held by the current PROC.  Therefore every frame must dispatch
     * through the installed table entry; only the first frame performs I/O. */
    if (*slot == NativeNameIn_LazyMain)
    {
        FS_LoadOverlay(0, OVERLAY_ID);
        /* Do not enter freshly loaded overlay code from inside the loader's
         * first callback.  Let the retail state machine advance this frame;
         * the cached lazy callback dispatches through the patched slot on the
         * next frame. */
        return NameInProc_Main(proc, seq);
    }
    return (*slot)(proc, seq);
}
void (*const OverlayStaticInitFunc)() = LoadOverlay;

void LoadOverlay() {
    /* Keep this in .data so the tiny loader needs no extra ARM9 BSS section. */
    static u32 loadCookie = 0x50494E59;
    if (loadCookie == 0x50494E59) {
        FS_LoadOverlay(0, OVERLAY_ID);
        loadCookie = 0;
    }
    if (Orig_OverlayStaticInitBegin && Orig_OverlayStaticInitEnd) {
        for (void (**func)() = Orig_OverlayStaticInitBegin; func < Orig_OverlayStaticInitEnd; func++) {
            if (*func)
                (*func)();
        }
    }
}
