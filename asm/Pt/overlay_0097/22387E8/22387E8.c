#include "nds/ndstypes.h"

#define LANGUAGE_JAPANESE 1
#define LANGUAGE_ENGLISH 2
#define LANGUAGE_FRENCH 3
#define LANGUAGE_ITALIAN 4
#define LANGUAGE_GERMAN 5
#define LANGUAGE_SPANISH 7
#define LANGUAGE_KOREAN 8

const u16 conversion_table_quote[][8] =
{
  {
    [0]                 = 0xEA,
    [LANGUAGE_JAPANESE] = 0xEA,
    [LANGUAGE_ENGLISH ] = 0x1B4,
    [LANGUAGE_FRENCH  ] = 0x1B7,
    [LANGUAGE_ITALIAN ] = 0x1B4,
    [LANGUAGE_GERMAN  ] = 0x1B6,
    [6]                 = 0xEA,
    [LANGUAGE_SPANISH ] = 0x1B4,
  },
  {
    [0]                 = 0xEB,
    [LANGUAGE_JAPANESE] = 0xEB,
    [LANGUAGE_ENGLISH ] = 0x1B5,
    [LANGUAGE_FRENCH  ] = 0x1B8,
    [LANGUAGE_ITALIAN ] = 0x1B5,
    [LANGUAGE_GERMAN  ] = 0x1B4,
    [6]                 = 0xEB,
    [LANGUAGE_SPANISH ] = 0x1B5,
  },
};
