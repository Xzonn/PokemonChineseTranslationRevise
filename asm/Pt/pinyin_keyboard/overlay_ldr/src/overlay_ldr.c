#include <nds/ndstypes.h>
#include "nitro/fs.h"

#ifndef OVERLAY_ID
#define OVERLAY_ID -1
#endif

extern void (*Orig_OverlayStaticInitBegin[])();
extern void (*Orig_OverlayStaticInitEnd[])();

void LoadOverlay();

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
