$PCTRTools = "tools/PCTRTools/bin/Release/PCTRTools.exe"

# Diamond
New-Item -ItemType "Directory" -Path "out/D/data/" -Force
New-Item -ItemType "Directory" -Path "out/D/overlay/" -Force
Copy-Item -Path "files/D/*.bin" -Destination "out/D/" -Recurse -Force
Copy-Item -Path "files/D/overlay/*" -Destination "out/D/overlay/" -Recurse -Force

dotnet script scripts/D.csx
& "$PCTRTools" "replace-narc" -i "files/DP/data/" -n "textures/DP/" -o "out/D/data/"
& "$PCTRTools" "font" -c "files/CharTable.txt" -i "files/DP/data/graphic/font.narc" -o "out/D/data/graphic/font.narc"
& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "files/DP/data/msgdata/msg.narc" -t "files/DP/Messages.txt" -o "out/D/data/msgdata/msg.narc"
Compress-Archive -Path "out/D/*" -DestinationPath "out/Patch-D.zip" -Force

# Pearl
New-Item -ItemType "Directory" -Path "out/P/data/" -Force
New-Item -ItemType "Directory" -Path "out/P/overlay/" -Force
Copy-Item -Path "files/P/*.bin" -Destination "out/P/" -Recurse -Force
Copy-Item -Path "files/P/overlay/*" -Destination "out/P/overlay/" -Recurse -Force

dotnet script scripts/P.csx
Copy-Item -Path "out/D/data/*" -Destination "out/P/data/" -Recurse -Force
Compress-Archive -Path "out/P/*" -DestinationPath "out/Patch-P.zip" -Force