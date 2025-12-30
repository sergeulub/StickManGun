using System.Collections.Generic;
using System.Data;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int money;
    public static int playerLevel;
    public static int boostPoint;

    public static int playerExp;

    public static InventoryData InventoryData = new InventoryData();
    public static ArtefactsData ArtefactsData = new ArtefactsData();

    private void Awake()
    {
        //EventManagerOld.OnWantBuyItem += CheckBuy;
        PlayerPrefs.DeleteAll();
        #if UNITY_WEBGL && !UNITY_EDITOR
            SaveManager.Init(new LocalSaveProvider());
        #else
            SaveManager.Init(new LocalSaveProvider());
        #endif

        LoadGame();

    }

    public static void AddMoney(int amount)
    {
        money += amount;
    }
    public static void DecreaseMoney(int amount)
    {
        money -= amount;
    }
    public static void DecreaseBoostPoint()
    {
        boostPoint -= 1;
    }
    public static void AddBoostsPoints(int quantity)
    {
        boostPoint += quantity;
    }
    public static bool EnoughMoney(int amount)
    {
        if (money - amount >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void SaveGame()
    {
        SaveData data = new SaveData
        {
            inventoryData = GameManager.InventoryData,
            money = GameManager.money,
            playerLevel = GameManager.playerLevel,
            boostPoint = boostPoint,

            boostsLevels = GameManager.ArtefactsData.boostsLevels,
            deployablesArsenal = GameManager.ArtefactsData.deployablesArsenal,
            deployablesLevels = GameManager.ArtefactsData.deployablesLevels,
            deployablesIsNew = GameManager.ArtefactsData.deployablesIsNew
        };

        SaveManager.Save(data);
    }
    public static void LoadGame()
    {
        SaveManager.Load(data =>
        {
            if (data == null)
            {
                InitNewGame();
                return;
            }

            ApplySave(data);
        });

    }
    private static void InitNewGame()
    {
        Debug.Log("Никаких данных нет. Игры начинается заново.");
        money = 12000;
        playerLevel = 1;
        playerExp = 0;
        boostPoint = 10;
        InventoryData.isNew[1] = 1;
    }
    private static void ApplySave(SaveData data)
    {
        GameManager.InventoryData = data.inventoryData;
        GameManager.money = data.money;
        GameManager.playerLevel = data.playerLevel;
        GameManager.boostPoint = data.boostPoint;

        GameManager.ArtefactsData.boostsLevels = data.boostsLevels;
        GameManager.ArtefactsData.deployablesArsenal = data.deployablesArsenal;
        GameManager.ArtefactsData.deployablesLevels = data.deployablesLevels;
        GameManager.ArtefactsData.deployablesIsNew = data.deployablesIsNew;
    }

}
