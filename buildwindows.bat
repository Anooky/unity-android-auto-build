IF "%APK_OUTPUT_DIR%"=="" (
 ECHO APK_OUTPUT_DIR is undefined
 EXIT /B 1
)

IF "%APK_OUTPUT_FILE%"=="" (
 ECHO APK_OUTPUT_FILE is undefined
 EXIT /B 1
)

IF "%UNITY_EXE_PATH%"=="" (
 SET UNITY_EXE_PATH=C:\Program Files\Unity\Editor\Unity.exe
)
SET APK=%APK_OUTPUT_DIR%/%APK_OUTPUT_FILE%

echo Creating build directory / removing previous build
mkdir %APK_OUTPUT_DIR%
IF EXIST %APK% del /F %APK%

echo Compiling.. this will take a while
"C:\Program Files\Unity\Editor\Unity.exe" -quit -batchmode -logFile build.log  -executeMethod BuildProject.BuildAndroid  %APK%

IF NOT EXIST %apk% (
 echo The apk was not built , please check the build.log from the artifacts.
 exit /b 1
) ELSE (
 echo %APK% built successfully.
)
