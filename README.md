# 2017-18
Code base for the 2017-18 buld season

## UI Dependencies

### Python VLC
pip3 install python-vlc
ALSO make sure 64 bit VLC player (required for built version of UI) is installed to the OS:

| Operating System | Installation |
| :----------------: | :----------------------------------------------: |
| Linux (Ubuntu) | Usually comes by default, otherwise usually in the package manager |
| Windows | http://get.videolan.org/vlc/2.2.5.1/win64/vlc-2.2.5.1-win64.exe |

### PyQT5 Installation:
pip3 install PyQt5

### Numpy Installation:
pip3 install numpy

### SDL2 Installation:
pip3 install PySDL2

Also download SDL.dll from http://libsdl.org/release/SDL2-2.0.5-win32-x64.zip
Add to Python 3 install directory under DLLs

### UI Build Tools
pip3 install cx_freeze
