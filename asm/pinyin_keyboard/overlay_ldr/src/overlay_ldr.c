#include <nds/ndstypes.h>
#include "nitro/fs.h"

#ifndef OVERLAY_ID
#define OVERLAY_ID -1
#endif

extern void (*Orig_OverlayStaticInitBegin[])();
extern void (*Orig_OverlayStaticInitEnd[])();

void LoadOverlay();

typedef int (*NameInMainFunc)(u32 proc, int *seq);
/* Mark this absolute linker import as Thumb, as in native_namein.c. */
__attribute__((naked)) int NameInProc_Main(u32 proc, int *seq) {}
extern NameInMainFunc NameInProcMainSlot;

__attribute__((section(".text.loader_entry")))
int NativeNameIn_LazyMain(u32 proc, int *seq)
{
    NameInMainFunc *slot = &NameInProcMainSlot;
    /* PROC caches this callback when it is created.  Loading the overlay can
     * replace the global procedure table, but it cannot replace the callback
     * already held by the current PROC.  Therefore every frame must dispatch
     * through the installed table entry; only the first frame performs I/O. */
    if (*slot == NativeNameIn_LazyMain)
    {
        FS_LoadOverlay(0, OVERLAY_ID);
    }
    /* The overlay initializer replaces the table entry with the real hook.
     * Enter it immediately so BeginContext runs before any patched decision
     * call site in the retail main routine can be reached. */
    if (*slot != NativeNameIn_LazyMain)
    {
        return (*slot)(proc, seq);
    }
    return NameInProc_Main(proc, seq);
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
