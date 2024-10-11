$PCTRTools = "tools/PCTRTools/bin/Release/net8.0/publish/PCTRTools.exe"

# Replace narc
& "$PCTRTools" "replace-narc" -i "original_files/HGSS/data/" -n "textures/HGSS/" -o "out/HG/data/"

# Create new font
& "$PCTRTools" "font" -c "files/CharTable.txt" -i "original_files/HGSS/data/a/0/1/6" -o "out/HG/data/a/0/1/6"

# Import text
& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "original_files/HGSS/data/a/0/2/7" -t "temp/HGSS/messages.txt" -o "out/HG/data/a/0/2/7"

dotnet script scripts/pad_text.csx "original_files/HGSS/data/a/0/2/7" "out/HG/data/a/0/2/7"

# Create patch for HeartGold
Compress-Archive -Path "out/HG/*" -DestinationPath "out/Patch-HG.zip" -Force
Move-Item out/Patch-HG.zip out/Patch-HG.xzp -Force

# Create patch for SoulSilver
dotnet script scripts/copy_narc.csx "out/HG/data/" "out/SS/data/"
Compress-Archive -Path "out/SS/*" -DestinationPath "out/Patch-SS.zip" -Force
Move-Item out/Patch-SS.zip out/Patch-SS.xzp -Force
