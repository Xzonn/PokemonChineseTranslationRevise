$PCTRTools = "tools/PCTRTools/bin/Release/PCTRTools.exe"

# Heart Gold
New-Item -ItemType "Directory" -Path "out/HG/data/" -Force
New-Item -ItemType "Directory" -Path "out/HG/overlay/" -Force
Copy-Item -Path "files/HGSS/overlay/*" -Destination "out/HG/overlay/" -Recurse -Force
Copy-Item -Path "files/HG/*.bin" -Destination "out/HG/" -Force
Copy-Item -Path "files/HG/overlay/*" -Destination "out/HG/overlay/" -Recurse -Force

dotnet script scripts/HG.csx
& "$PCTRTools" "replace-narc" -i "files/HGSS/data/" -n "textures/HGSS/" -o "out/HG/data/"
& "$PCTRTools" "font" -c "files/CharTable.txt" -i "files/HGSS/data/a/0/1/6" -o "out/HG/data/a/0/1/6"
& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "files/HGSS/data/a/0/2/7" -t "files/HGSS/Messages.txt" -o "out/HG/data/a/0/2/7"
Compress-Archive -Path "out/HG/*" -DestinationPath "out/Patch-HG.zip" -Force

# Soul Silver
New-Item -ItemType "Directory" -Path "out/SS/data/" -Force
New-Item -ItemType "Directory" -Path "out/SS/overlay/" -Force
Copy-Item -Path "files/HGSS/overlay/*" -Destination "out/SS/overlay/" -Recurse -Force
Copy-Item -Path "files/SS/*.bin" -Destination "out/SS/" -Force
Copy-Item -Path "files/SS/overlay/*" -Destination "out/SS/overlay/" -Recurse -Force

dotnet script scripts/SS.csx
Copy-Item -Path "out/HG/data/*" -Destination "out/SS/data/" -Recurse -Force
Compress-Archive -Path "out/SS/*" -DestinationPath "out/Patch-SS.zip" -Force