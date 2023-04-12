@echo off

if "%2"=="m" (   
    CALL :BuildMainActivity %1
)

if "%2"=="all" (
    echo Build All started
    CALL :BuildMainActivity build
    echo Build All finished
)

EXIT /B %ERRORLEVEL%

:BuildMainActivity
    "D:\Projets\ReplaceLine\ReplaceLine\bin\Release\net6.0\ReplaceLine.exe" ".\src\App.vue" 10 "import Activity from '@/activities/MainActivity.vue';"
    if "%1"=="build" (
        echo Build MainActivity
        npm run build
        robocopy /s "D:\Projets\TransportTime\developement\UI\transport-time-ui\dist" "D:\Projets\TransportTime\developement\TransportTime\TransportTime\Assets\UI\mainActivity"
    )
    if "%1"=="serve" (npm run serve)
EXIT /B 0