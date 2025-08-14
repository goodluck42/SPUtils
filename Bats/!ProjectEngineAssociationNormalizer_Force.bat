@echo off
set Engine_CLSID="{12345678-9ABC-DEF1-2345-6789ABCDEF12}"
Utils\SPUtils.exe ProjectEngineAssociationNormalizer %Engine_CLSID%
pause