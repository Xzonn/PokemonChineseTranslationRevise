$PCTRTools = "tools/PCTRTools/bin/Release/PCTRTools.exe"

# Copy files and edit banner
dotnet script scripts/BW.csx

# Download Zfull-GB
try {
  Get-ItemPropertyValue -Path "HKCU:\Software\Microsoft\Windows NT\CurrentVersion\Fonts" -Name "Zfull-GB (TrueType)" -ErrorAction Stop
} catch {
  Invoke-WebRequest "https://github.com/andot/zfull-for-yosemite/raw/master/fonts/Zfull-GB.ttf" -OutFile "out/Zfull-GB.ttf"
  Set-ItemProperty -Path "HKCU:\Software\Microsoft\Windows NT\CurrentVersion\Fonts" -Name "Zfull-GB (TrueType)" -Value "$pwd\out\Zfull-GB.ttf" -Type String
}

# Create new font
& "$PCTRTools" "font" -c "files/CharTable_Unicode.txt" -i "files/BW/data/a/0/2/3" -o "out/B/data/a/0/2/3"

# Import text
& "$PCTRTools" "text-import" -c "files/CharTable_Unicode.txt" -i "files/BW/data/a/0/0/2" -t "files/BW/Messages_common.txt" -o "out/B/data/a/0/0/2"
& "$PCTRTools" "text-import" -c "files/CharTable_Unicode.txt" -i "files/BW/data/a/0/0/3" -t "files/BW/Messages_script.txt" -o "out/B/data/a/0/0/3"

# Create patch for Black 2
Compress-Archive -Path "out/B/*" -DestinationPath "out/Patch-B.zip" -Force
Move-Item out/Patch-B.zip out/Patch-B.xzp -Force

# Create patch for White 2
Copy-Item -Path "out/B/data/*" -Destination "out/W/data/" -Recurse -Force
Compress-Archive -Path "out/W/*" -DestinationPath "out/Patch-W.zip" -Force
Move-Item out/Patch-W.zip out/Patch-W.xzp -Force
