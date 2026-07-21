@echo off
echo ============================================
echo   Project - Clean & Rebuild
echo ============================================

echo.
echo Deleting bin and obj folders...
for /d /r %%i in (bin,obj) do (
    echo Removing: %%i
    rmdir /s /q "%%i"
)

echo.
echo Running dotnet clean...
dotnet clean

echo.
echo Running dotnet build...
dotnet build

echo.
echo ============================================
echo   Build complete!
echo ============================================
pause

