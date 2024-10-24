$PCTRTools = "tools/PCTRTools/bin/Release/net8.0/publish/PCTRTools.exe"

python scripts/merge_messages_for_japanese_names.py

& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "original_files/DP/data/msgdata/msg.narc" -t "temp/DP/messages_J.txt" -o "out/DP_J/data/msgdata/msg.narc"
& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "original_files/Pt/data/msgdata/pl_msg.narc" -t "temp/Pt/messages_J.txt" -o "out/Pt_J/data/msgdata/pl_msg.narc"
& "$PCTRTools" "text-import" -c "files/CharTable.txt" -i "original_files/HGSS/data/a/0/2/7" -t "temp/HGSS/messages_J.txt" -o "out/HGSS_J/data/a/0/2/7"

dotnet script scripts/pad_text.csx "original_files/DP/data/msgdata/msg.narc" "out/DP_J/data/msgdata/msg.narc"
dotnet script scripts/pad_text.csx "original_files/Pt/data/msgdata/pl_msg.narc" "out/Pt/data/msgdata/pl_msg.narc"
dotnet script scripts/pad_text.csx "original_files/HGSS/data/a/0/2/7" "out/HGSS_J/data/a/0/2/7"

Compress-Archive -Path "out/DP_J/*" -DestinationPath "out/Patch-DP_J.zip" -Force
Move-Item out/Patch-DP_J.zip out/Patch-DP_J.xzp -Force
Compress-Archive -Path "out/Pt_J/*" -DestinationPath "out/Patch-Pt_J.zip" -Force
Move-Item out/Patch-Pt_J.zip out/Patch-Pt_J.xzp -Force
Compress-Archive -Path "out/HGSS_J/*" -DestinationPath "out/Patch-HGSS_J.zip" -Force
Move-Item out/Patch-HGSS_J.zip out/Patch-HGSS_J.xzp -Force
Compress-Archive -Path "out/Patch-*_J.xzp" -DestinationPath "out/Patches-J.zip" -Force
Remove-Item -Path "out/Patch-*_J.xzp"
