$PCTRTools = "tools/PCTRTools/bin/Release/PCTRTools.exe"

# Platinum
New-Item -ItemType "Directory" -Path "out/Pt/data/" -Force
New-Item -ItemType "Directory" -Path "out/Pt/overlay/" -Force
Copy-Item -Path "files/Pt/*.bin" -Destination "out/Pt/" -Force
Copy-Item -Path "files/Pt/overlay/*" -Destination "out/Pt/overlay/" -Recurse -Force

dotnet script scripts/Pt.csx
& "$PCTRTools" "replace-narc" -i "files/Pt/data/" -n "textures/Pt/" -o "out/Pt/data/"
& "$PCTRTools" "font" -c "files/CharTable.txt" -i "files/Pt/data/graphic/pl_font.narc" -o "out/Pt/data/graphic/pl_font.narc"
& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "files/Pt/data/msgdata/pl_msg.narc" -t "files/Pt/Messages.txt" -o "out/Pt/data/msgdata/pl_msg.narc"

Compress-Archive -Path "out/Pt/*" -DestinationPath "out/Patch-Pt.zip" -Force