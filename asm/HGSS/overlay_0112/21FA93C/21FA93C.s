.thumb

@ https://github.com/enler/HGSS_PokeWalkerChineseLocalization/blob/master/Patch/HG.asm

HookClearPokeWalkerPlayerData:
  PUSH {R4-R5, LR}
  MOV R4, R0
  LDR R1, =0x12C
  LDR R5, [R4, R1]
  BL ClearPokeWalkerPlayerData
  LDR R0, =0x12C
  STR R5, [R4, R0]
  POP {R4-R5, PC}
