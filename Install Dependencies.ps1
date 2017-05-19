function elevateSelf {
    if (!([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) { 
        Start-Process powershell.exe "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`"" -Verb RunAs; 
        exit 
    }
}

function startUp {
    if (-not (Test-Path C:\temporary)) {
		mkdir C:\temporary | Out-Null
    }

    cd C:\temporary
}

function printStart {
	echo "============================="
	echo "========== Welcome =========="
	echo "=== to the Husky Robotics ==="
	echo "========= Installer ========="
	echo "============================="
}

function cleanUp {
	cd C:\
    if (Test-Path C:\temporary) {
        rmdir C:\temporary\ -force -recurse
    }
}

function setup($url, $file, $name, $options) {

    echo ""
    echo "Downloading $name"
    $client = New-Object System.Net.WebClient
    $client.DownloadFile($url, "$pwd\$file")

	echo "Installing $name"
    Rename-Item $file installer.exe # Required by NSIS Installer (VLC)
    cmd /c (".\installer.exe $options")
    Rename-Item installer.exe $file
	
	if($LastExitCode -eq 0) {
		echo "Successfully Installed $name"
	} else {
		echo "Failed to Install $name"
	}
}

function unzip($zipFile, $outPath) {
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    [System.IO.Compression.ZipFile]::ExtractToDirectory("$pwd\$zipfile", "$pwd\$outpath")
}

function install {
	if ($vlc -eq 0) {
		setup $vlcPath "vlc.exe" "VLC Player" "/S /V/qn"
	} else {
        echo ""
		echo "VLC Player already installed"
	}

	if ($py -eq 0) {
		setup $pythonPath "python.exe" "Python 3" "/quiet DefaultAllUsersTargetDir=C:\Python36 InstallAllUsers=1 PrependPath=1"
	} else {
        echo ""
		echo "Python 3 already installed"
		$pythonPreinstallFlag = "1"
	}

	if ($git -eq 0) {
		setup $gitPath "git.exe" "Git" "/verysilent"
	} else {
        echo ""
		echo "Git already installed"
	}
}

$pythonPreinstallFlag = "0"
elevateSelf
printStart
startUp

$vlcPath = "http://videolan.mirror.pacificservers.com/vlc/2.2.5.1/win64/vlc-2.2.5.1-win64.exe"
$pythonPath = "https://www.python.org/ftp/python/3.6.1/python-3.6.1-amd64.exe"
$gitPath = "https://github.com/git-for-windows/git/releases/download/v2.13.0.windows.1/Git-2.13.0-64-bit.exe"

# Get list of all software installed with executable files
# NOTE: Use WMIC to get software installed with msi files
$temp = Get-ItemProperty HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\* | Select DisplayName
$vlc = ($temp | select-string -pattern "VLC" | measure -line).Lines
$py = ($temp | select-string -pattern "Python 3" | measure -line).Lines
$git = ($temp | select-string -pattern "Git version" | measure -line).Lines

install

echo ""
echo "Will Now Install Required Python Libraries"
echo ""
Write-Host "Press any key to continue ..."
$x = $host.UI.RawUI.ReadKey("NoEcho, IncludeKeyDown")

Clear-Host

# If Python is already installed we need to know where the install is located
# Then we can add pip3 to path and execute it in the session
if($pythonPreinstallFlag) {
    echo "Please Provide the root to Python 3 Installation (ex: C:\Python36)."
    echo "Add quotation marks around path if the path has spaces."
    $p = Read-Host "Root Directory"

    # Add pip3 to path for this session only
    $Env:Path += "$p\Scripts"
}

pip3 install PyQt5
pip3 install cx_freeze
pip3 install numpy
pip3 install python-vlc
pip3 install PySDL2

echo "Downloading and Installing SDL2 DLLs"
(New-Object System.Net.WebClient).DownloadFile("http://libsdl.org/release/SDL2-2.0.5-win32-x64.zip", "$pwd\SDL2.zip")
unzip SDL2.zip sdl
cd sdl
Copy-Item "SDL2.dll" "$p\DLLs"
echo ""
echo "============================="
echo "==== Thank You! Goodbye! ===="
echo "============================="
echo ""
Write-Host "Press any key to continue ..."
$x = $host.UI.RawUI.ReadKey("NoEcho, IncludeKeyDown")

cleanUp