$PCTRTools = "tools/PCTRTools/bin/Release/PCTRTools.exe"

# Copy files and edit banner
dotnet script scripts/B2W2.csx

# Create new font
# & "$PCTRTools" "font" -c "files/CharTable_Unicode.txt" -i "files/B2W2/data/a/0/2/3" -o "out/B2/data/a/0/2/3"

# Import text
& "$PCTRTools" "text-import" -c "files/CharTable_Unicode.txt" -i "files/B2W2/data/a/0/0/2" -t "files/B2W2/Messages_common.txt" -o "out/B2/data/a/0/0/2"
& "$PCTRTools" "text-import" -c "files/CharTable_Unicode.txt" -i "files/B2W2/data/a/0/0/3" -t "files/B2W2/Messages_script.txt" -o "out/B2/data/a/0/0/3"

# Create patch for Black 2
Compress-Archive -Path "out/B2/*" -DestinationPath "out/Patch-B2.zip" -Force
Move-Item out/Patch-B2.zip out/Patch-B2.xzp -Force

# Create patch for White 2
Copy-Item -Path "out/B2/data/*" -Destination "out/W2/data/" -Recurse -Force
Compress-Archive -Path "out/W2/*" -DestinationPath "out/Patch-W2.zip" -Force
Move-Item out/Patch-W2.zip out/Patch-W2.xzp -Force
