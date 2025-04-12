#---------------------------------------------------------------------------------
.SUFFIXES:
#---------------------------------------------------------------------------------

ifeq ($(strip $(DEVKITARM)),)
$(error "Please set DEVKITARM in your environment. export DEVKITARM=<path to>devkitARM")
endif

#---------------------------------------------------------------------------------
# canned command sequence for binary data
#---------------------------------------------------------------------------------
define bin2o
	bin2s $< | $(AS) -o $(@)
	echo "extern const u8" `(echo $(<F) | sed -e 's/^\([0-9]\)/_\1/' | tr . _)`"_end[];" > `(echo $(<F) | tr . _)`.h
	echo "extern const u8" `(echo $(<F) | sed -e 's/^\([0-9]\)/_\1/' | tr . _)`"[];" >> `(echo $(<F) | tr . _)`.h
	echo "extern const u32" `(echo $(<F) | sed -e 's/^\([0-9]\)/_\1/' | tr . _)`_size";" >> `(echo $(<F) | tr . _)`.h
endef


#---------------------------------------------------------------------------------
# path to tools
#---------------------------------------------------------------------------------
export PORTLIBS	:=	$(DEVKITPRO)/portlibs/arm
export PATH		:=	$(DEVKITARM)/bin:$(PORTLIBS)/bin:$(PATH)
LIBNDS	:=	$(DEVKITPRO)/libnds

#---------------------------------------------------------------------------------
# the prefix on the compiler executables
#---------------------------------------------------------------------------------
PREFIX		:=	arm-none-eabi-

export CC	:=	$(PREFIX)gcc
export CXX	:=	$(PREFIX)g++
export AS	:=	$(PREFIX)as
export AR	:=	$(PREFIX)ar
export OBJCOPY	:=	$(PREFIX)objcopy
export OBJDUMP	:=	$(PREFIX)objdump
export LD	:=	$(PREFIX)ld


#---------------------------------------------------------------------------------
# TARGET is the name of the output
# BUILD is the directory where object files & intermediate files will be placed
# SOURCES is a list of directories containing source code
# INCLUDES is a list of directories containing extra header files
# DATA is a list of directories containing binary files embedded using bin2o
# GRAPHICS is a list of directories containing image files to be converted with grit
#---------------------------------------------------------------------------------
TARGET		?=	newcode
BUILD		?=  build
SOURCES		?=	source
INCLUDES	:=	include
DATA		:=	data
GRAPHICS	:=	gfx

#---------------------------------------------------------------------------------
# options for code generation
#---------------------------------------------------------------------------------
ARCH	:= -mabi=aapcs -march=armv5te -mtune=arm946e-s -mthumb

CFLAGS	:=	-g -Wall -Os\
 		-fomit-frame-pointer\
		-ffast-math -fshort-wchar \
		$(ARCH)

CFLAGS	+=	$(INCLUDE) -DARM9 -nodefaultlibs -I. -fno-builtin -c
CXXFLAGS	:= $(CFLAGS) -fno-rtti -fno-exceptions

ASFLAGS	:=	-g $(ARCH)
ifeq ($(strip $(GAME)),HG)
	CFLAGS += -DHEARTGOLD
	ASFLAGS += -DHEARTGOLD
	SYMBOLST_NAME = symbols_HG.x
else
	CFLAGS += -DSOULSILVER
	ASFLAGS += -DSOULSILVER
	SYMBOLST_NAME = symbols_SS.x
endif
SYMBOLST := -T $(shell dirname $(CURDIR))/$(shell dirname $(shell dirname $(SOURCES)))/$(SYMBOLST_NAME)
NEWSYMT :=
LDFLAGS	=	--use-blx $(SYMBOLST) -T $(shell dirname $(CURDIR))/linker.x $(NEWSYMT) -g -Map newcode.map

ifdef CODEADDR
  LDFLAGS += -Ttext $(CODEADDR)
endif

IMPORT_FILE :=
ifeq ($(CODEADDR),0x21EB5CE)
	IMPORT_FILE = import.o
else ifeq ($(CODEADDR),0x21FA93C)
	IMPORT_FILE = import.o
endif

#---------------------------------------------------------------------------------
# any extra libraries we wish to link with the project (order is important)
#---------------------------------------------------------------------------------
LIBS	:= -lnds9 -lc


#---------------------------------------------------------------------------------
# list of directories containing libraries, this must be the top level containing
# include and lib
#---------------------------------------------------------------------------------
LIBDIRS	:=	$(LIBNDS)  $(DEVKITARM) $(DEVKITARM)/arm-none-eabi

#---------------------------------------------------------------------------------
ifneq ($(BUILD),$(notdir $(CURDIR)))
#---------------------------------------------------------------------------------

export OUTPUT	:=	$(CURDIR)/$(TARGET)

export VPATH	:=	$(foreach dir,$(SOURCES),$(CURDIR)/$(dir)) \
					$(foreach dir,$(DATA),$(CURDIR)/$(dir)) \
					$(foreach dir,$(GRAPHICS),$(CURDIR)/$(dir))

export DEPSDIR	:=	$(CURDIR)/$(BUILD)

CFILES		:=	$(foreach dir,$(SOURCES),$(notdir $(wildcard $(dir)/*.c)))
CPPFILES	:=	$(foreach dir,$(SOURCES),$(notdir $(wildcard $(dir)/*.cpp)))
SFILES		:=	$(foreach dir,$(SOURCES),$(notdir $(wildcard $(dir)/*.s)))
PNGFILES	:=	$(foreach dir,$(GRAPHICS),$(notdir $(wildcard $(dir)/*.png)))
BINFILES	:=	$(foreach dir,$(DATA),$(notdir $(wildcard $(dir)/*.*)))

export OFILES	:=	$(addsuffix .o,$(BINFILES)) \
					$(PNGFILES:.png=.o) \
					$(CPPFILES:.cpp=.o) $(CFILES:.c=.o) $(SFILES:.s=.o)

export INCLUDE	:=	$(foreach dir,$(INCLUDES),-iquote $(CURDIR)/$(dir)) \
					$(foreach dir,$(LIBDIRS),-I$(dir)/include) \
					-I$(CURDIR)/$(BUILD)

export LIBPATHS	:=	$(foreach dir,$(LIBDIRS),-L$(dir)/lib)


.PHONY: $(BUILD) clean

#---------------------------------------------------------------------------------
$(BUILD):
	@[ -d $@ ] || mkdir -p $@
	@make --no-print-directory -C $(BUILD) -f $(CURDIR)/Makefile

#---------------------------------------------------------------------------------
clean:
	@echo clean ...
	@rm -fr $(BUILD) $(TARGET).elf $(TARGET).nds

#---------------------------------------------------------------------------------
else

#---------------------------------------------------------------------------------
# main targets
#---------------------------------------------------------------------------------

all: $(OUTPUT).bin $(OUTPUT).sym

$(OUTPUT).bin : $(OUTPUT).elf
	$(OBJCOPY) -O binary $< $@
	@echo built ... $(notdir $@)

$(OUTPUT).sym : $(OUTPUT).elf
	$(OBJDUMP) -t $< > $@
	@echo written the symbol table ... $(notdir $@)

#---------------------------------------------------------------------------------
%.elf: $(OFILES)
	@echo linking $(notdir $@)
	$(LD)  $(LDFLAGS) $(OFILES) $(IMPORT_FILE) $(LIBPATHS) $(LIBS) -o $@

#---------------------------------------------------------------------------------
%.bin.o	:	%.bin
#---------------------------------------------------------------------------------
	@echo $(notdir $<)
	$(bin2o)

#---------------------------------------------------------------------------------
%.o: %.c
	@echo $(notdir $<)
	$(CC) -MMD -MP -MF $(DEPSDIR)/$*.d $(CFLAGS) -c $< -o $@ $(ERROR_FILTER)

#---------------------------------------------------------------------------------
%.o: %.s
	@echo $(notdir $<)
	$(CC) -MMD -MP -MF $(DEPSDIR)/$*.d -x assembler-with-cpp $(ASFLAGS) -c $< -o $@ $(ERROR_FILTER)

#---------------------------------------------------------------------------------
# This rule creates assembly source files using grit
# grit takes an image file and a .grit describing how the file is to be processed
# add additional rules like this for each image extension
# you use in the graphics folders
#---------------------------------------------------------------------------------
%.s %.h   : %.png %.grit
#---------------------------------------------------------------------------------
	grit $< -fts -o$*

-include $(DEPSDIR)/*.d

#---------------------------------------------------------------------------------------
endif
#---------------------------------------------------------------------------------------
