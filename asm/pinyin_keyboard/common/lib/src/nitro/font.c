#include "nitro/font.h"

typedef struct {
    u16 charCode;
    u16 index;
} NitroCharToGlyphMapEntry;

typedef struct {
    u16 entryCount;
    const NitroCharToGlyphMapEntry entries[];
} NitroCharToGlyphMap;

static inline s32 FindGlyphIndex(const NitroFontInfoSection *font, u16 charCode) {
    NitroFontCharMappingSection *section = font->charMappingSection;
    while (section) {
        if (charCode >= section->charBegin && charCode <= section->charEnd) {
            if (section->mappingMethod == 0) {
                return charCode - section->charBegin + section->mappingInfo[0];
            }
            else if (section->mappingMethod == 1) {
                u16 index = section->mappingInfo[charCode - section->charBegin];
                if ((s16)index == -1)
                    break;
                return index;
            }
            else if (section->mappingMethod == 2) {
                const NitroCharToGlyphMap *map = (const NitroCharToGlyphMap *)&section->mappingInfo[0];
                const NitroCharToGlyphMapEntry *begin = &map->entries[0];
                const NitroCharToGlyphMapEntry *end = &map->entries[map->entryCount - 1];
                while (begin <= end) {
                    const NitroCharToGlyphMapEntry *mid = begin + (end - begin) / 2;
                    if (mid->charCode < charCode)
                        begin = mid + 1;
                    else if (mid->charCode > charCode)
                        end = mid - 1;
                    else
                        return mid->index;
                }
            }
        }
        section = section->nextSection;
    }
    return -1;
}

static inline const NitroGlyphMetrics *GetGlyphMetrics(const NitroFontInfoSection *font, u16 index) {
    NitroFontMetricsSection *section = font->metricsSection;
    while (section) {
        if (index >= section->indexBegin && index <= section->indexEnd) {
            return &section->glyphMetrics[index - section->indexBegin];
        }
        section = section->nextSection;
    }
    return &font->defalutGlyphMetrics;
}

bool LoadGlyphData(
    const NitroFontInfoSection *font,
    u16 charCode,
    const u8 **outGlyphBitmap,
    NitroGlyphMetrics *outMetrics
)
{
    s32 index = FindGlyphIndex(font, charCode);
    if (index < 0)
        index = font->defalutGlyphIndex;
    const NitroGlyphMetrics *glyphMetrics = GetGlyphMetrics(font, (u16)index);

    outMetrics->left = glyphMetrics->left;
    outMetrics->width = glyphMetrics->width;
    outMetrics->advance = glyphMetrics->advance;

    *outGlyphBitmap = font->glyphSection->glpyhData + font->glyphSection->cellSize * index;
    return true;
}
