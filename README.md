
# Universal app updater
This app allows to update specific app (or any other file) from remote location to local disk.


I'm mainly using it to update company-specific apps on different PC stations.

### Usage
Create apps.cfg in main app directory with entries like so:	
```
#local path | remote path
C:\Inst\App\Main.exe | \\appRepo\App\Main.exe
C:\Inst\App2\Main2.exe | \\appRepo\App2\Main2.exe
``` 
If you want to disable entry put `#` in front of it:
```
local path | remote path
#C:\Inst\App\Main.exe | \\appRepo\App\Main.exe
C:\Inst\App2\Main2.exe | \\appRepo\App2\Main2.exe
```

### Development
You can change this application for your needs. If you want to update application by itself create `self.cfg` in main directory of app and provide data in the same schema like in `apps.cfg`. 

**This file allows for one line only and not allows comments!**

```
C:\Inst\Updater\Updater.exe | \\appRepo\Updater\Updater.exe
```

### Issues / ideas
Post them in inssues tab.

### Licence

GPL
  
