$PCTRTools = "tools/PCTRTools/bin/Release/PCTRTools.exe"

# Diamond
New-Item -ItemType "Directory" -Path "out/D/data/" -Force
Copy-Item -Path "files/D/*" -Destination "out/D/" -Recurse -Force
& "$PCTRTools" "replace-narc" -i "files/DP/data/" -n "textures/DP/" -o "out/D/data/"
& "$PCTRTools" "font" -c "files/CharTable.txt" -i "files/DP/data/graphic/font.narc" -o "out/D/data/graphic/font.narc"
& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "files/DP/data/msgdata/msg.narc" -t "files/DP/Messages.txt" -o "out/D/data/msgdata/msg.narc"
Compress-Archive -Path "out/D/*" -DestinationPath "out/Patch-D.zip" -Force

# Pearl
New-Item -ItemType "Directory" -Path "out/P/data/" -Force
Copy-Item -Path "out/D/data/*" -Destination "out/P/data/" -Recurse -Force
Copy-Item -Path "files/P/*" -Destination "out/P/" -Recurse -Force
Compress-Archive -Path "out/P/*" -DestinationPath "out/Patch-P.zip" -Force