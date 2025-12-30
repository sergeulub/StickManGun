using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLoadout", menuName = "PlayerLoadout")]
public class PlayerLoadout : ScriptableObject
{
    // То, что реально идёт в бой (экипировка)
    public List<int> activeItems = new List<int>();

    public List<int> itemsLevels = new List<int>();

    // Прокачка бонусов (itemID -> level)
    public List<int> bonusLevels = new List<int>();

    // Прокачка deployables (itemID -> level)
    public List<int> deployableLevels = new List<int>();

    public static int weapon1 = 0;
    public static int weapon2 = 1;
    public static int hat = 3;
    public static int boots = 2;
    public static int ring1 = 4;
    public static int ring2 = 5;

    public void FillPlayerLoadout(Info info)
    {
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
                activeItems[weapon1] = weapon2ID; // основное
                itemsLevels[weapon1] = f(weapon2ID, StaticDatas._firstWeaponID);

                activeItems[weapon2] = StaticDatas._firstWeaponID - 1;// второе пустое
                itemsLevels[weapon2] = -1;
            }
            else
            {
                activeItems[weapon1] = weapon1ID;
                itemsLevels[weapon1] = f(weapon1ID, StaticDatas._firstWeaponID);


                activeItems[weapon2] = weapon2ID;
                itemsLevels[weapon2] = f(weapon2ID, StaticDatas._firstWeaponID);
            }

            // --- 3. Остальная экипировка (без логики сдвига) ---
            int f(int id, int firstInTypeID)
            {
                if (id != -1)
                {
                    return id;
                }
                else
                {
                    return firstInTypeID - 1;
                }
            }
            activeItems[boots] = f(bootsID, StaticDatas._firstBootsID);
            itemsLevels[boots] = inventory.levels[f(bootsID, StaticDatas._firstBootsID)];
            activeItems[hat] = f(hatID, StaticDatas._firstHatID);
            itemsLevels[hat] = inventory.levels[f(bootsID, StaticDatas._firstHatID)];
            activeItems[ring1] = f(ring1ID, StaticDatas._firstRingID);
            itemsLevels[ring1] = inventory.levels[f(bootsID, StaticDatas._firstRingID)];
            activeItems[ring2] = f(ring2ID, StaticDatas._firstRingID);
            itemsLevels[ring2] = inventory.levels[f(bootsID, StaticDatas._firstRingID)];

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