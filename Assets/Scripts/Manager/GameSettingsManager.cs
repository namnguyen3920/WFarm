using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSettingsManager : Singleton_Mono_Method<GameSettingsManager>
{
    public static event Action<Dictionary<string, string>> OnStarterDataLoaded;
    private Dictionary<string, string> settings = new Dictionary<string, string>();
    private void Start()
    {
        string game_settingsPath = Path.Combine(Application.streamingAssetsPath, "csv/game_settings.csv");
        LoadData(game_settingsPath);
    }
    public void LoadData(string path)
    {
        settings.Clear();
        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] data = lines[i].Split(',');
            if (data.Length >= 2)
            {
                settings[data[0].Trim()] = data[1].Trim();
            }
        }
        OnStarterDataLoaded?.Invoke(settings);
    }

    public string GetValue(string key)
    {
        settings.TryGetValue(key, out string value);
        return value;
    }
}
