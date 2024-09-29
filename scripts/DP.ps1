$PCTRTools = "tools/PCTRTools/bin/Release/PCTRTools.exe"

# Replace narc
& "$PCTRTools" "replace-narc" -i "original_files/DP/data/" -n "textures/DP/" -o "out/D/data/"

# Create new font
& "$PCTRTools" "font" -c "files/CharTable.txt" -i "original_files/DP/data/graphic/font.narc" -o "out/D/data/graphic/font.narc"

# Import text
& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "original_files/DP/data/msgdata/msg.narc" -t "temp/DP/messages.txt" -o "out/D/data/msgdata/msg.narc"

dotnet script scripts/pad_text.csx "original_files/DP/data/msgdata/msg.narc" "out/D/data/msgdata/msg.narc"

# Create patch for Diamond
Compress-Archive -Path "out/D/*" -DestinationPath "out/Patch-D.zip" -Force
Move-Item out/Patch-D.zip out/Patch-D.xzp -Force

# Create patch for Pearl
Copy-Item -Path "out/D/data/*" -Destination "out/P/data/" -Recurse -Force
Compress-Archive -Path "out/P/*" -DestinationPath "out/Patch-P.zip" -Force
Move-Item out/Patch-P.zip out/Patch-P.xzp -Force
