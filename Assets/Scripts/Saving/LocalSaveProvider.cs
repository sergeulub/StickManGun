using System;
using UnityEngine;
using System.IO;

public class LocalSaveProvider : ISaveProvider
{
    private string Path =>
        Application.persistentDataPath + "/save.json";

    public void Save(string json)
    {
        File.WriteAllText(Path, json);
    }

    public void Load(Action<string> onLoaded)
    {
        if (!File.Exists(Path))
        {
            onLoaded(null);
            return;
        }

        onLoaded(File.ReadAllText(Path));
    }
}
