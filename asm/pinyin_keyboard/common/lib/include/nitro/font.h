#ifndef NITRO_FONT_H
#define NITRO_FONT_H

#include <nds/ndstypes.h>

// reference: https://github.com/hadashisora/NintyFont/tree/master/formats/NFTR

struct NitroFontCharMappingSection;

typedef struct {
    s8 left;
    u8 width;
    u8 advance;
} NitroGlyphMetrics;

typedef struct {
    u8 cellWidth;
    u8 cellHeight;
    u16 cellSize;
    s8 baselinePos;
    u8 maxGlyphWidth;
    u8 bpp;
    u8 flags;
    u8 glpyhData[];
} NitroFontGlyphSection;

typedef struct NitroFontMetricsSection {
    u16 indexBegin;
    u16 indexEnd;
    struct NitroFontMetricsSection *nextSection;
    NitroGlyphMetrics glyphMetrics[];
} NitroFontMetricsSection;

typedef struct NitroFontCharMappingSection {
    u16 charBegin;
    u16 charEnd;
    u16 mappingMethod;
    u16 reserved;
    struct NitroFontCharMappingSection *nextSection;
    u16 mappingInfo[];
} NitroFontCharMappingSection;

typedef struct {
    u8 fontType;
    u8 lineFeed;
    u16 defalutGlyphIndex;
    NitroGlyphMetrics defalutGlyphMetrics;
    u8 encoding;
    NitroFontGlyphSection *glyphSection;
    NitroFontMetricsSection *metricsSection;
    NitroFontCharMappingSection *charMappingSection;
} NitroFontInfoSection;

bool LoadGlyphData(
    const NitroFontInfoSection *font,
    u16 charCode,
    const u8 **outGlyphBitmap,
    NitroGlyphMetrics *outMetrics
);

#endif // NITRO_FONT_H
