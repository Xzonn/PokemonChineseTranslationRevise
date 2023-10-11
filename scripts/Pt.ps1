$PCTRTools = "tools/PCTRTools/bin/Release/PCTRTools.exe"

# Copy files and edit banner
dotnet script scripts/Pt.csx

# Replace narc
& "$PCTRTools" "replace-narc" -i "files/Pt/data/" -n "textures/Pt/" -o "out/Pt/data/"

# Create new font
& "$PCTRTools" "font" -c "files/CharTable.txt" -i "files/Pt/data/graphic/pl_font.narc" -o "out/Pt/data/graphic/pl_font.narc"

# Import text
& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "files/Pt/data/msgdata/pl_msg.narc" -t "files/Pt/Messages.txt" -o "out/Pt/data/msgdata/pl_msg.narc"

# Create patch for Platinum
Compress-Archive -Path "out/Pt/*" -DestinationPath "out/Patch-Pt.zip" -Force
