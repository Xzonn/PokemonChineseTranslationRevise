.SECONDEXPANSION:
.DEFAULT_GOAL := all

include ../common/common.mk

OUTPUT     := overlay_ldr
OUTPUT_ELF := ../$(OUTPUT).elf
OUTPUT_BIN := ../$(OUTPUT).bin
OUTPUT_SYM := ../$(OUTPUT).sym
INC_DIRS := include $(COMMON_INC_DIRS)


C_SRCS  := $(wildcard src/*.c)
ASM_SRCS:= $(wildcard src/*.S)
C_OBJS  := $(C_SRCS:%.c=$(OBJ_DIR)/%.o)
ASM_OBJS:= $(ASM_SRCS:%.S=$(OBJ_DIR)/%.o)
OBJS    := $(C_OBJS) $(ASM_OBJS)

ALL_OBJS := $(OBJS) $(NITRO_LIB_OBJS)
DEPS   := $(ALL_OBJS:%.o=%.d)

CFLAGS	:= -g -w -Os -std=gnu99 -ffreestanding -mabi=aapcs -march=armv5te -mthumb -mtune=arm946e-s -mthumb-interwork $(foreach dir,$(INC_DIRS),-I $(dir)) -DOVERLAY_ID=$(OVERLAY_ID) -DARM9=1 -D__NDS__ -DNITROSDK_VER=$(NITROSDK_VER)
LDFLAGS := -nostartfiles -nodefaultlibs -Ttext=$(OVERLAY_LDR_ADDR) -T linker.ld -T $(SYMBOLS_LD) -Wl,--use-blx

$(shell mkdir -p $(sort $(dir $(ALL_OBJS))))

.PHONY: all clean

all: $(OUTPUT_BIN) $(OUTPUT_SYM)
	@:

-include $(DEPS)

$(OBJ_DIR)/%.o: %.c
	$(CC) -MMD -MP -MF $(OBJ_DIR)/$*.d $(CFLAGS) -o $(OBJ_DIR)/$*.o -c $<

$(OBJ_DIR)/%.o: %.S
	$(CC) -D__ASSEMBLY__ $(CFLAGS) -c $< -o $@

$(OUTPUT_ELF): $(OBJS) $(NITRO_LIB) linker.ld $(SYMBOLS_LD)
	$(LD) $(LDFLAGS) -o $@ $(OBJS) $(NITRO_LIB) $(LIBS)

$(OUTPUT_BIN): $(OUTPUT_ELF)
	$(OBJCOPY) --set-section-flags .bss=alloc,load,contents -O binary $< $@

$(OUTPUT_SYM): $(OUTPUT_ELF)
	$(NM) -n $< > $@

clean:
	rm -rf $(OBJ_DIR)
	rm -f $(OUTPUT_ELF) $(OUTPUT_BIN) $(OUTPUT_SYM)
