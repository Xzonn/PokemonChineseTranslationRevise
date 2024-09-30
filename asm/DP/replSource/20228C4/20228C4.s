.thumb

@ Replace `DecompressGlyphTiles_FromPreloaded()` with `GetGlyphWidth_VariableWidth()`, from Pt and HGSS

arepl_20228C4:
  LDR R2, [R0, #0x64]
  CMP R1, R2
  BCS loc_20228D0
  LDR R0, [R0, #0x74]
  LDRB R0, [R0, R1]
  BX LR

loc_20228D0:
  LDR R1, [R0, #0x74]
  LDR R0, =0x1FB
  LDRB R0, [R1, R0]
  BX LR
