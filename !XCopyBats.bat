@echo off
setlocal

set "source_dir=Bats"
set "target_dir=..\"


echo Copying from %source_dir% to %target_dir%...
xcopy "%source_dir%\*" "%target_dir%\" /C /I /Y

echo Completed!
pause