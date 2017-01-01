# Unity Android build script for CI

This set of scripts can be used in CI systems (like Gitlab CI) to automatically build unity android projects and store the resulting APKs.

## Usage
 * Copy `buildwindows.bat` to your project root folder
 * Copy `buildproject.cs` to `Assets/_scritps/Editor` (create the folder if necessary)
 * Define the following environment variables (gitlab project settings -> variables):
   * `ANDROID_KEYSTORE_NAME` : Filename of the key store. For example `yourcompany.keystore`
   * `ANDROID_KEYSTORE_PASSWORD` : Password for the key store
   * `ANDROID_KEYALIAS_NAME` : Name of the key alias used to sign the application
   * `ANDROID_KEYALIAS_PASSWORD` : Password for the key alias
   * `ANDROID_SDK_ROOT` : Path to the android SDK on the build host
   * `APK_OUTPUT_DIR` : path where the APK should be saved, for example `build\android` (this will be generated relative to the project root on the ci runner)
   * `APK_OUTPUT_FILE` : name of the APK to be generated, for example `myproject.apk`
   * `UNITY_EXE_PATH` : full path to Unity.exe on the build host, for example `C:\Program Files\Unity\Editor\Unity.exe`
 * Create your CI configuration ( `.gitlab-ci.yml`) like this example:
```
stages:
  - compile
compile:
  stage: compile
  script: buildwindows.bat
  artifacts:
    paths:
    - build/android/myproject.apk
    - build.log
    expire_in: 7 days
    when: always
  tags:
    - windows-unity
 ```
 
