#ifndef NITRO_PAD_H
#define NITRO_PAD_H

#include <nds/ndstypes.h>
#include <nds/input.h>

#define PAD_PLUS_KEY_MASK (KEY_RIGHT | KEY_LEFT | KEY_UP | KEY_DOWN)
#define PAD_BUTTON_MASK (KEY_A | KEY_B | KEY_SELECT | KEY_START | KEY_R |KEY_L | KEY_X | KEY_Y)

extern u16 HW_BUTTON_XY_BUF;

static inline u16 PAD_Read(void) {
    return (u16)(((REG_KEYINPUT | HW_BUTTON_XY_BUF) ^
                  (PAD_PLUS_KEY_MASK | PAD_BUTTON_MASK)) & (PAD_PLUS_KEY_MASK | PAD_BUTTON_MASK));
}

#define KEY_PRESSED(mask) ((PAD_Read() & (mask)) == (mask))

#endif
