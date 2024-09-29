$PCTRTools = "tools/PCTRTools/bin/Release/PCTRTools.exe"

# Replace narc
& "$PCTRTools" "replace-narc" -i "original_files/Pt/data/" -n "textures/Pt/" -o "out/Pt/data/"

# Create new font
& "$PCTRTools" "font" -c "files/CharTable.txt" -i "original_files/Pt/data/graphic/pl_font.narc" -o "out/Pt/data/graphic/pl_font.narc"

# Import text
& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "original_files/Pt/data/msgdata/pl_msg.narc" -t "temp/Pt/messages.txt" -o "out/Pt/data/msgdata/pl_msg.narc"

dotnet script scripts/pad_text.csx "original_files/Pt/data/msgdata/pl_msg.narc" "out/Pt/data/msgdata/pl_msg.narc"

# Create patch for Platinum
Compress-Archive -Path "out/Pt/*" -DestinationPath "out/Patch-Pt.zip" -Force
Move-Item out/Patch-Pt.zip out/Patch-Pt.xzp -Force
