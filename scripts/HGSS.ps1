$PCTRTools = "tools/PCTRTools/bin/Release/PCTRTools.exe"

# Heart Gold
New-Item -ItemType "Directory" -Path "out/HG/" -Force
Copy-Item -Path "files/HGSS/*" -Destination "out/HG/" -Recurse -Force
Copy-Item -Path "files/HG/*" -Destination "out/HG/" -Recurse -Force
& "$PCTRTools" "font" -c "files/CharTable.txt" -i "files/HGSS/data/a/0/1/6" -o "out/HG/data/a/0/1/6"
& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "files/HGSS/data/a/0/2/7" -t "files/HGSS/Messages.txt" -o "out/HG/data/a/0/2/7"
Remove-Item -Path "out/HG/Messages.txt" -Force
Compress-Archive -Path "out/HG/*" -DestinationPath "out/Patch-HG.zip" -Force

# Soul Silver
New-Item -ItemType "Directory" -Path "out/SS/" -Force
Copy-Item -Path "files/HGSS/*" -Destination "out/SS/" -Recurse -Force
Copy-Item -Path "files/SS/*" -Destination "out/SS/" -Recurse -Force
Copy-Item -Path "out/HG/data/a/0/1/6" -Destination "out/SS/data/a/0/1/6"
Copy-Item -Path "out/HG/data/a/0/2/7" -Destination "out/SS/data/a/0/2/7"
Compress-Archive -Path "out/SS/*" -DestinationPath "out/Patch-SS.zip" -Force