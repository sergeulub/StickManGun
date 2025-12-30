using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public InventoryData inventoryData;

    public int money;
    public int playerLevel;
    public int boostPoint;

    // deployables
    public List<int> deployablesArsenal;
    public List<int> deployablesLevels;
    public List<int> deployablesIsNew;

    // boosts
    public List<int> boostsLevels;

}
