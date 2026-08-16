OUTPUT_FORMAT("elf32-littlearm", "elf32-littlearm", "elf32-littlearm")
OUTPUT_ARCH(arm)

ENTRY(_start)

SECTIONS
{
        .text : {
            *src/*.o(.text*)
        }
        . = ALIGN(4);

        .rodata : {
            *src/*.o(.rodata*)
        }
        . = ALIGN(4);

        .data : {
            *src/*.o(.data*)
        }
        . = ALIGN(4);

        __bss_start__ = .;
        .bss : {
            *src/*.o(.bss*)
        }
        . = ALIGN(4);
        __bss_end__ = .;
        __bss_size__ = __bss_end__ - __bss_start__;



        /DISCARD/ :
        {
            *(*)
        }
}
