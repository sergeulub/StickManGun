using System;
using UnityEngine;

public static class SaveManager
{
    private static ISaveProvider saveProvider;

    public static void Init(ISaveProvider provider)
    {
        saveProvider = provider;
    }

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        saveProvider.Save(json);
    }

    public static void Load(Action<SaveData> onLoaded)
    {
        saveProvider.Load(json =>
        {
            if (string.IsNullOrEmpty(json))
            {
                onLoaded(null);
                return;
            }

            SaveData data = JsonUtility.FromJson<SaveData>(json);
            onLoaded(data);
        });
    }
}
