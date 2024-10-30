#define LANGUAGE_JAPANESE #1
#define LANGUAGE_ENGLISH #2
#define LANGUAGE_FRENCH #3
#define LANGUAGE_ITALIAN #4
#define LANGUAGE_GERMAN #5
#define LANGUAGE_SPANISH #7
#define LANGUAGE_KOREAN #8

#define FS_SEEK_SET #0 /* seek from begin*/
#define FS_SEEK_CUR #1 /* seek from current*/
#define FS_SEEK_END #2 /* seek from end*/

.thumb

IfisJapanese:
  //检查语种
  CMP R5, LANGUAGE_JAPANESE
  BEQ NotChinese

IfisChineseChar:
  //检查gen3编码高位字节
  CMP R0, #0x01
  BLT NotChinese
  CMP R0, #0x1E
  BGT NotChinese
  CMP R0, #0x06
  BEQ NotChinese
  CMP R0, #0x1B
  BEQ NotChinese
  B ConvertChineseChar

NotChinese:
  //读取列表获取对应gen4原字符双字节编码
  //初始化文件
  PUSH {R2-R7}
  PUSH {R0}
  LDR R0, =FSFile_Var
  BLX FS_InitFile
  //打开文件
  LDR R0, =FSFile_Var
  LDR R1, =name_conversion_table
  BLX FS_OpenFile
  //根据gen3原编码确定索引地址
  LDR R3, conversion_table_origin
  CMP R5, LANGUAGE_JAPANESE
  BEQ GetOriginIndex
NotFullWidth:
  ADD R3, R3, #0x02
GetOriginIndex:
  POP {R1}
  LSL R1, R1, #0x02
  ADD R1, R1, R3
  LDR R0, =FSFile_Var
  MOV R2, FS_SEEK_SET
  BLX FS_SeekFile
  //读取gen4对应编码并存入内存
  LDR R0, =FSFile_Var
  LDR R1, =(Gen4Char_Var+2)
  MOV R2, #0x02
  BLX FS_ReadFile
  //关闭文件
  LDR R0, =FSFile_Var
  BLX FS_CloseFile
  POP {R2-R7}
  //从内存中提取gen4编码
  LDR R1, =Gen4Char_Var
  LDRH R0, [R1, #0x02]
  //原函数判别处理转码
  CMP R0, #0x01
  BEQ Back1
  CMP R0, #0xEA
  BEQ Back2
  CMP R0, #0xEB
  BEQ Back3
  b Back4
Back1:
  LDR R0, =ConvertRSStringToDPStringInternational + 0x72 //0x02016532
  MOV PC, R0
Back2:
  LDR R0, =ConvertRSStringToDPStringInternational + 0x7C //0x0201653C
  MOV PC, R0
Back3:
  LDR R0, =ConvertRSStringToDPStringInternational + 0x86 //0x02016546
  MOV PC, R0
Back4:
  STRH R0, [R4]
  LDR R0, =ConvertRSStringToDPStringInternational + 0x92 //0x02016552
  MOV PC, R0

ConvertChineseChar:
  //获取gen3汉字双字节编码
  LSL R0, R0, #0x08
  ADD R6, R6, #0x01
  LDR R1, [SP]
  LDRB R1, [R1, R6]
  ADD R0, R0, R1
  //读取列表获取对应gen4汉字双字节编码
  //初始化文件
  PUSH {R2-R7}
  PUSH {R0}
  LDR R0, =FSFile_Var
  BLX FS_InitFile
  //打开文件
  LDR R0, =FSFile_Var
  LDR R1, =name_conversion_table
  BLX FS_OpenFile
  //根据gen3汉字编码确定索引地址
  LDR R3, conversion_table_chinese
  POP {R1}
  LSL R1, R1, #0x01
  ADD R1, R1, R3
  LDR R0, =FSFile_Var
  MOV R2, FS_SEEK_SET
  BLX FS_SeekFile
  //读取gen4对应编码并存入内存
  LDR R0, =FSFile_Var
  LDR R1, =Gen4Char_Var
  MOV R2, #0x02
  BLX FS_ReadFile
  //关闭文件
  LDR R0, =FSFile_Var
  BLX FS_CloseFile
  POP {R2-R7}
  //从内存中提取gen4编码
  LDR R1, =Gen4Char_Var
  LDRH R0, [R1]
  B Back4

.align
name_conversion_table:
  .ascii "msgdata/msg.narc"
  .byte 0x0
.align
conversion_table_origin:
  .word 0xFFFFFFFF
conversion_table_chinese:
  .word 0xFFFFFFFF
Gen4Char_Var:
  .word 0x0
FSFile_Var:
  .word 0x0
  .word 0x0
  .word 0x0
  .word 0x0
  .word 0x0
  .word 0x0
  .word 0x0
  .word 0x0
  .word 0x0
  .word 0x0
  .word 0x0
  .word 0x0
  .word 0x0
  .word 0x0
  .word 0x0
