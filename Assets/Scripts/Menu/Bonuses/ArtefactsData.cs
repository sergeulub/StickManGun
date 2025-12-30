using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtefactsData
{   
    public List<int> boostsLevels = new List<int>(new int[StaticDatas._boostsCount]);
    public List<int> deployablesArsenal = new List<int>(new int[StaticDatas._deployableCount]);
    public List<int> deployablesLevels = new List<int>(new int[StaticDatas._deployableCount]);
    public List<int> deployablesIsNew = new List<int>(new int[StaticDatas._deployableCount]);

    public ArtefactsData()
    {
        for (int i = 0; i < StaticDatas._deployableCount; i++)
            deployablesLevels[i] = -1;
        for (int i = 0; i < StaticDatas._deployableCount; i++)
            deployablesArsenal[i] = 0;
        for (int i = 0; i < StaticDatas._boostsCount; i++)
            boostsLevels[i] = 0;
        for (int i = 0; i < StaticDatas._deployableCount; i++)
            deployablesIsNew[i] = 0;
        
        deployablesArsenal[0] = 1;
        deployablesArsenal[1] = 1;
        deployablesLevels[0] = 0;
        deployablesLevels[1] = 0;
    }
    
    public void UpgradeDeploable(int itemID)
    {
        deployablesLevels[itemID] += 1;
        GameManager.SaveGame();
    }
    public void UpgradeBoost(int itemID)
    {
        boostsLevels[itemID] += 1;
        GameManager.SaveGame();
    }
}
