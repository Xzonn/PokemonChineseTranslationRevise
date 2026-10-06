    .section .rodata
    .balign 4

    .global gPinyinTableBin
    .global gPinyinTableBinEnd
gPinyinTableBin:
    .incbin "../build/pinyin_table.data"
gPinyinTableBinEnd:
