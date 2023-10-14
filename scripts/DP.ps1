$PCTRTools = "tools/PCTRTools/bin/Release/PCTRTools.exe"

# Copy files and edit banner
dotnet script scripts/DP.csx

# Replace narc
& "$PCTRTools" "replace-narc" -i "files/DP/data/" -n "textures/DP/" -o "out/D/data/"

# Create new font
& "$PCTRTools" "font" -c "files/CharTable.txt" -i "files/DP/data/graphic/font.narc" -o "out/D/data/graphic/font.narc"

# Import text
& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "files/DP/data/msgdata/msg.narc" -t "out/Messages_DP.txt" -o "out/D/data/msgdata/msg.narc"

# Create patch for Diamond
Compress-Archive -Path "out/D/*" -DestinationPath "out/Patch-D.zip" -Force

# Create patch for Pearl
Copy-Item -Path "out/D/data/*" -Destination "out/P/data/" -Recurse -Force
Compress-Archive -Path "out/P/*" -DestinationPath "out/Patch-P.zip" -Force
