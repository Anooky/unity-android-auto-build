

using System;
using System.Linq;
using UnityEditor; //Note, this script must reside in a folder called 'Editor' or the compilation will fail at this point!

public class BuildProject
{
    static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }
        return scenes;
    }

    /**
     * returns false if one or more required environment variables are not defined
     * */
    static bool EnvironmentVariablesMissing(string[] envvars)
    {
        string value;
        bool missing = false;
        foreach (string envvar in envvars)
        {
            value = Environment.GetEnvironmentVariable(envvar);
            if (value == null)
            {
                Console.Write("BUILD ERROR: Required Environment Variable is not set: ");
                Console.WriteLine(envvar);
                missing = true;
            }
        }

        return missing;
    }

    /**
     * Main entry point
     * - check if all required environment variables are defined
     * - configure the android build
     * - build the apk (path read from the command line argument)
     */
    public static void BuildAndroid()
    {
        string[] envvars = new string[]
        {
          "ANDROID_KEYSTORE_NAME", "ANDROID_KEYSTORE_PASSWORD", "ANDROID_KEYALIAS_NAME", "ANDROID_KEYALIAS_PASSWORD", "ANDROID_SDK_ROOT"
        };
        if (EnvironmentVariablesMissing(envvars))
        {
            Environment.ExitCode = -1;
            return; // note, we can not use Environment.Exit(-1) - the buildprocess will just hang afterwards
        }

        //Available Playersettings: https://docs.unity3d.com/ScriptReference/PlayerSettings.Android.html

        //set the internal apk version to the current unix timestamp, so this increases with every build
        PlayerSettings.Android.bundleVersionCode = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; 

        //set the other settings from environment variables
        PlayerSettings.Android.keystoreName = Environment.GetEnvironmentVariable("ANDROID_KEYSTORE_NAME");
        PlayerSettings.Android.keystorePass = Environment.GetEnvironmentVariable("ANDROID_KEYSTORE_PASSWORD");
        PlayerSettings.Android.keyaliasName = Environment.GetEnvironmentVariable("ANDROID_KEYALIAS_NAME");
        PlayerSettings.Android.keyaliasPass = Environment.GetEnvironmentVariable("ANDROID_KEYALIAS_PASSWORD");
   
        EditorPrefs.SetString("AndroidSdkRoot", Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT"));

        //Get the apk file to be built from the command line argument
        string outputapk = Environment.GetCommandLineArgs().Last();
        BuildPipeline.BuildPlayer(GetScenePaths(), outputapk, BuildTarget.Android, BuildOptions.None);
    }
}