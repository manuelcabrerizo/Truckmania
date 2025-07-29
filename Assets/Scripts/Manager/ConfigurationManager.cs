using System.Collections.Generic;
using UnityEngine;

public static class ConfigurationManager
{
    public const string CurrentResolutionIndexID = "CurrentResolutionIndex";
    public const string FullScreenToggleID = "FullScreenToggle";
    public const string CurrentFrameRateIndexID = "CurrentFrameRateIndex";
    public const string VsyncToggleID = "VsyncToggle";
    
    private static bool initialized = false; 
    private static List<Resolution> resolutions = new List<Resolution>();
    private static List<string> resolutionOptions = new List<string>();

    public static List<Resolution> GetResolutions()
    {
        if (!initialized)
        {
            LoadConfigurations();
        }
        return resolutions;
    }

    public static List<string> GetResolutionsOptions()
    {
        if (!initialized)
        { 
            LoadConfigurations();
        }
        return resolutionOptions;
    }

    public static void LoadConfigurations()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            Resolution resolution = Screen.resolutions[i];

            string option = resolution.width + " x " + resolution.height;
            if (!resolutionOptions.Contains(option))
            {
                resolutions.Add(resolution);
                resolutionOptions.Add(option);
            }
        }
        initialized = true;
    }
}
