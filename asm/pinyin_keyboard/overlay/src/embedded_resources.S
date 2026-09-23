    .section .rodata
    .balign 4

    .global gPinyinTableBin
    .global gPinyinTableBinEnd
gPinyinTableBin:
    .incbin "src/pinyin_table.data"
gPinyinTableBinEnd:
