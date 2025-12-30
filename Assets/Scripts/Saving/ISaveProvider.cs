using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveProvider
{
    void Save(string json);
    void Load(Action<string> onLoaded);
}
