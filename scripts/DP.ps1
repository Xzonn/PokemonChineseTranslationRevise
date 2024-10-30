$PCTRTools = "tools/PCTRTools/bin/Release/net8.0/publish/PCTRTools.exe"

# Replace narc
& "$PCTRTools" "replace-narc" -i "original_files/DP/data/" -n "textures/DP/" -o "out/D/data/"

# Create new font
& "$PCTRTools" "font" -c "files/CharTable.txt" -i "original_files/DP/data/graphic/font.narc" -o "out/D/data/graphic/font.narc"

# Import text
& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "original_files/DP/data/msgdata/msg.narc" -t "temp/DP/messages.txt" -o "temp/msg.narc"
& "$PCTRTools" "append-narc" -i "temp/msg.narc" -a "files/CharTable_3to4.bin" -o "out/D/data/msgdata/msg.narc"
python "scripts\edit_convert_table_address.py" "out/D/arm9.bin" "out/D/data/msgdata/msg.narc" "out/D/symbols.txt"
python "scripts\edit_convert_table_address.py" "out/P/arm9.bin" "out/D/data/msgdata/msg.narc" "out/P/symbols.txt"

# Create patch for Diamond
Compress-Archive -Path "out/D/*" -DestinationPath "out/Patch-D.zip" -Force
Move-Item out/Patch-D.zip out/Patch-D.xzp -Force

# Create patch for Pearl
dotnet script scripts/copy_narc.csx "out/D/data/" "out/P/data/"
Compress-Archive -Path "out/P/*" -DestinationPath "out/Patch-P.zip" -Force
Move-Item out/Patch-P.zip out/Patch-P.xzp -Force
