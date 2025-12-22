using System.Collections.Generic;
using System.Data;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int money
    {
        get => PlayerPrefs.GetInt("money", 10000);
        set => PlayerPrefs.SetInt("money", value); 
    }
    public static int playerLevel = 4;
    /*{
        get => PlayerPrefs.GetInt("playerLevel", 0);
        set => PlayerPrefs.SetInt("playerLevel", value);
        
    }*/
    public static int boostPoint
    {
        get => PlayerPrefs.GetInt("boostPoint", 2);
        set => PlayerPrefs.SetInt("boostPoint", value);
    }

    public static int playerExp
    {
        get => PlayerPrefs.GetInt("playerExp", 0);
        set => PlayerPrefs.SetInt("playerExp", value);
    }


    public static InventoryData InventoryData = new InventoryData();
    public static ArtefactsData ArtefactsData;

    private void Awake()
    {
        EventManagerOld.OnWantBuyItem += CheckBuy;
        PlayerPrefs.DeleteAll();
    }
    private void CheckBuy(int price, int index)
    {
        if (money - price >= 0)
        {
            Pay(price);

            EventManagerOld.SendBuyItem(price, index);
        }
    }
    private void Pay(int price)
    {
        money -= price;
        Debug.Log("U just have paid " + price+". Left " + money);
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
    


}
