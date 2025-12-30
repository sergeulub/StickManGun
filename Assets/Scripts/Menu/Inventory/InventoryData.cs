using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[Serializable]
public class InventoryData
{
    // 30 �����: 0-25 �������, 26-31 � ���� ������
    public List<int> slotItemIDs = new List<int>(new int[StaticDatas._inventoryLength]);
    public List<int> arsenal = new List<int>(new int[StaticDatas._quantityItems]);
    public List<int> isNew = new List<int>(new int[StaticDatas._quantityItems]); 
    public List<int> levels = new List<int>(new int[StaticDatas._quantityItems]);

    public InventoryData()  
    {
        for (int i = 0; i < StaticDatas._inventoryLength; i++)
        {
            slotItemIDs[i] = StaticDatas._emptyID; // -1 �������� ������ ������
        }
        for (int i = 0; i < StaticDatas._quantityItems; i++)
        {
            arsenal[i] = 0;
        }
        for (int i = 0; i < StaticDatas._quantityItems; i++)
        {
            levels[i] = -1;
        }
        for (int i = 0; i < StaticDatas._quantityItems; i++)
        {
            isNew[i] = 0;
        }
        //Добавление элементов  в инвентарь
        //slotItemIDs[0] = StaticDatas._firstWeaponID;
        //arsenal[StaticDatas._firstWeaponID] = 1;
        //levels[StaticDatas._firstWeaponID] = 8;
        //slotItemIDs[1] = StaticDatas._firstBootsID;
        //arsenal[StaticDatas._firstBootsID] = 1;
        //levels[StaticDatas._firstBootsID] = 8;
        //slotItemIDs[2] = StaticDatas._firstHatID;
        //arsenal[StaticDatas._firstHatID] = 1;
        //levels[StaticDatas._firstHatID] = 8;
        //slotItemIDs[3] = StaticDatas._firstRingID;
        //arsenal[StaticDatas._firstRingID] = 1;
        //levels[StaticDatas._firstRingID] = 8;

        // slotItemIDs[1] = StaticDatas._firstBootsID;
        // slotItemIDs[2] = StaticDatas._firstHatID;
        // slotItemIDs[3] = StaticDatas._firstRingID;

        //нОВЫЕ ПРЕДМЕТЫ ДЛЯ ТЕСТА
        // isNew[2] = 1;
        // isNew[1] = 1;
    }

    public void SetItem(int slotIndex, int itemID)
    {
        if (slotIndex >= 0 && slotIndex < slotItemIDs.Count)
            slotItemIDs[slotIndex] = itemID;
        GameManager.SaveGame();
    }

    public int GetItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slotItemIDs.Count)
            return slotItemIDs[slotIndex];
        return StaticDatas._emptyID;
    }

    public void ClearSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slotItemIDs.Count)
            slotItemIDs[slotIndex] = StaticDatas._emptyID;
        GameManager.SaveGame();
    }

    public bool isBought(int itemID)
    {
        return arsenal[itemID] == 1;
    }

    public void SetArsenal(int itemID, bool isBought)
    {
        arsenal[itemID] = isBought ? 1 : 0;
        GameManager.SaveGame();
    }

    public void UpgradeItem(int itemID)
    {
        levels[itemID] += 1;
        GameManager.SaveGame();
    }
    public void SetZeroLevel(int itemID)
    {
        levels[itemID] = 0;
        GameManager.SaveGame();
    }
}
