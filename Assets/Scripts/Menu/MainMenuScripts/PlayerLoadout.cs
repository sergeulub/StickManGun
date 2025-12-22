using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLoadout", menuName = "PlayerLoadout")]
public class PlayerLoadout : ScriptableObject
{
    // То, что реально идёт в бой (экипировка)
    public List<int> activeItems = new List<int>(){0,0,0,0,0,0};

    public List<int> itemsLevels = new List<int>(){0,0,0,0,0,0};

    // Прокачка бонусов (itemID -> level)
    public List<int> bonusLevels = new List<int>(){0,0,0,0,0,0,0,0};

    // Прокачка deployables (itemID -> level)
    public List<int> deployableLevels = new List<int>(){0,0,0,0};

    public void Clear()
    {      
        Debug.Log(itemsLevels.Count);
        for (int i = 0; i < 6; i++)
        {
            activeItems[i] = -1;
            itemsLevels[i] = -1;
        }
        for (int i = 0; i < 4; i++)
        {
            deployableLevels[i] = -1;
        }
        for (int i = 0; i < 8; i++)
        {
            bonusLevels[i] = 0;
        }
    }
    public void FillPlayerLoadout(Info info)
    {
        Clear();

        InventoryData inventory = GameManager.InventoryData;
        List<ItemInfo> allItems = info.GetAllItems();

        // 🔹 1. Перенос экипировки (последние 6 ячеек)
        // Обычно это 26–31, но используем константы
            // --- 1. Забираем оружие из инвентаря ---
            int weapon1ID = inventory.slotItemIDs[StaticDatas._weaponID1];
            int weapon2ID = inventory.slotItemIDs[StaticDatas._weaponID2];
            int bootsID = inventory.slotItemIDs[StaticDatas._bootsID];
            int hatID = inventory.slotItemIDs[StaticDatas._hatID];
            int ring1ID = inventory.slotItemIDs[StaticDatas._ringID1];
            int ring2ID = inventory.slotItemIDs[StaticDatas._ringID2];


            // --- 2. Нормализация порядка ---
            if (weapon1ID < 0 && weapon2ID >= 0)
            {
                activeItems[0] = weapon2ID; // основное
                itemsLevels[0] = inventory.levels[weapon2ID];

                activeItems[1] = StaticDatas.EMPTY_SLOT;// второе пустое
                itemsLevels[1] = -1;
            }
            else
            {
                activeItems[0] = weapon1ID;
                itemsLevels[0] = inventory.levels[weapon1ID];
                
                activeItems[1] = weapon2ID;
                itemsLevels[1] = inventory.levels[weapon2ID];
            }

            // --- 3. Остальная экипировка (без логики сдвига) ---
            activeItems[2] = bootsID;
            itemsLevels[2] = inventory.levels[bootsID];
            activeItems[3] = hatID;
            itemsLevels[3] = inventory.levels[hatID];
            activeItems[4] = ring1ID;
            itemsLevels[4] = inventory.levels[ring1ID];
            activeItems[5] = ring2ID;
            itemsLevels[5] = inventory.levels[ring2ID];
            
        // 🔹 2. Перенос прокачки бонусов и deployables
        bonusLevels = GameManager.ArtefactsData.boostsLevels;
        deployableLevels = GameManager.ArtefactsData.deployablesLevels;
    }

    private void LogItem(string label, int itemID, List<ItemInfo> items)
    {
        if (itemID < 0)
        {
            Debug.Log($"{label}: EMPTY");
        }
        else
        {
            Debug.Log($"{label}: {items[itemID].itemName} (ID {itemID}) (LVL {GameManager.InventoryData.levels[itemID]})");
        }
    }
    public void DebugPlayerLoadout(Info info)
    {
        List<ItemInfo> items = info.GetAllItems();

        Debug.Log("===== PLAYER LOADOUT =====");

        // 1–2. Оружие
        LogItem("Main weapon", activeItems[0], items);
        LogItem("Second weapon", activeItems[1], items);

        // 3–6. Экипировка
        LogItem("Boots", activeItems[2], items);
        LogItem("Helmet", activeItems[3], items);
        LogItem("Ring 1", activeItems[4], items);
        LogItem("Ring 2", activeItems[5], items);
        Debug.Log("==========================");

        for (int i = 0; i < 4; i++)
        {
            Debug.Log($"{info.deployables[i].itemName} LVL {deployableLevels[i]}");
        }
        for (int i = 0; i < 8; i++)
        {
            Debug.Log($"{info.boosts[i].itemName} LVL {bonusLevels[i]}");
        }
    }
}