$PCTRTools = "tools/PCTRTools/bin/Release/PCTRTools.exe"

# Copy files and edit banner
dotnet script scripts/HGSS.csx

# Replace narc
& "$PCTRTools" "replace-narc" -i "files/HGSS/data/" -n "textures/HGSS/" -o "out/HG/data/"

# Create new font
& "$PCTRTools" "font" -c "files/CharTable.txt" -i "files/HGSS/data/a/0/1/6" -o "out/HG/data/a/0/1/6"

# Import text
& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "files/HGSS/data/a/0/2/7" -t "files/HGSS/Messages.txt" -o "out/HG/data/a/0/2/7"

# Create patch for HeartGold
Compress-Archive -Path "out/HG/*" -DestinationPath "out/Patch-HG.zip" -Force

# Create patch for SoulSilver
Copy-Item -Path "out/HG/data/*" -Destination "out/SS/data/" -Recurse -Force
Compress-Archive -Path "out/SS/*" -DestinationPath "out/Patch-SS.zip" -Force
