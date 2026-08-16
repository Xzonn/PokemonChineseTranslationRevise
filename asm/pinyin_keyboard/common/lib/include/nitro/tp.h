#ifndef NITRO_TP_H
#define NITRO_TP_H

#include <nds/ndstypes.h>

typedef struct {
    u16     x;
    u16     y;
    u16     touch;
    u16     validity;
}
TPData;

void TP_GetCalibratedPoint(TPData *disp, const TPData *raw);

#endif
