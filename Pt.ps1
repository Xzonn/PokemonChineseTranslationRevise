$PCTRTools = "tools/PCTRTools/bin/Release/PCTRTools.exe"

# Platinum
New-Item -ItemType "Directory" -Path "out/Pt" -Force
Copy-Item -Path "files/Pt/*" -Destination "out/Pt/" -Recurse -Force
& "$PCTRTools" "font" -c "files/CharTable.txt" -i "files/Pt/data/graphic/pl_font.narc" -o "out/Pt/data/graphic/pl_font.narc"
& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "files/Pt/data/msgdata/pl_msg.narc" -t "files/Pt/Messages.txt" -o "out/Pt/data/msgdata/pl_msg.narc"
Remove-Item -Path "out/Pt/Messages.txt" -Force
Compress-Archive -Path "out/Pt/*" -DestinationPath "out/Patch-Pt.zip" -Force