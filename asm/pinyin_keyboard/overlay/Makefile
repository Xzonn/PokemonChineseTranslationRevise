.SECONDEXPANSION:
.DEFAULT_GOAL := all

include ../common/common.mk

OUTPUT     := $(OVERLAY_NAME)
OUTPUT_ELF := $(BUILD_DIR)/$(OUTPUT).elf
OUTPUT_BIN := $(BUILD_DIR)/$(OUTPUT).bin
OUTPUT_SYM := $(BUILD_DIR)/$(OUTPUT).sym

INC_DIRS := include $(COMMON_INC_DIRS)

C_SRCS  := $(PINYIN_SOURCE_DIR)/native_namein.c $(PINYIN_SOURCE_DIR)/native_pinyin.c $(PINYIN_SOURCE_DIR)/overlay_entry.c
ASM_SRCS:= $(PINYIN_SOURCE_DIR)/embedded_resources.S $(PINYIN_SOURCE_DIR)/misc.S
C_OBJS  := $(C_SRCS:%.c=$(OBJ_DIR)/%.o)
ASM_OBJS:= $(ASM_SRCS:%.S=$(OBJ_DIR)/%.o)
OBJS    := $(C_OBJS) $(ASM_OBJS)

ALL_OBJS := $(OBJS)
DEPS   := $(ALL_OBJS:%.o=%.d)

COMPILE_MODE := -mthumb
DEFS    := -DARM9 -D__NDS__ -D__thumb__ -D__ARM_ARCH=5 -DNITROSDK_VER=$(NITROSDK_VER)
CFLAGS  := -g -w -Os -std=gnu99 -ffreestanding -mabi=aapcs -march=armv5te -mtune=arm946e-s -mthumb-interwork $(foreach dir,$(INC_DIRS),-I $(dir))
LDFLAGS := -nostartfiles -nodefaultlibs -Ttext=$(OVERLAY_ADDR) -T linker.ld -T $(SYMBOLS_LD) -Wl,--use-blx

$(OBJ_DIR)/src/overlay_entry.o : COMPILE_MODE := -marm

$(shell mkdir -p $(sort $(dir $(ALL_OBJS))))

.PHONY: all clean

all: $(OUTPUT_BIN) $(OUTPUT_SYM)
	@:

-include $(DEPS)

$(OBJ_DIR)/%.o: %.c
	$(CC) -MMD -MP -MF $(OBJ_DIR)/$*.d $(CFLAGS) $(COMPILE_MODE) $(DEFS) -o $(OBJ_DIR)/$*.o -c $<

$(OBJ_DIR)/%.o: %.S
	$(CC) -D__ASSEMBLY__ $(CFLAGS) -c $< -o $@

$(OUTPUT_ELF): $(OBJS) linker.ld $(SYMBOLS_LD)
	$(LD) $(LDFLAGS) -o $@ $(OBJS) $(LIBS)

$(OUTPUT_BIN): $(OUTPUT_ELF)
	$(OBJCOPY) --set-section-flags .init_functions=alloc,load,contents -O binary $< $@

$(OUTPUT_SYM): $(OUTPUT_ELF)
	$(NM) -n $< > $@

clean:
	rm -rf $(OBJ_DIR)
	rm -f $(OUTPUT_ELF) $(OUTPUT_BIN) $(OUTPUT_SYM)
