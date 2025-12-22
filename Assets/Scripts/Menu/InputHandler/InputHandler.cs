using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    private Camera _mainCamera => Camera.main;


    public GameObject blackScreen;
    public GameObject shop;
    public GameObject inventory;
    public GameObject artifacts;
    public PlayerLoadout playerLoadout;
    public Info info;


    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;

        if (blackScreen.activeSelf) return;

        if (rayHit.collider.gameObject.name == "Shop")
        {
            OpenShop();
        }
        else if (rayHit.collider.gameObject.name == "Inventory")
        {
            OpenInventory();
        }
        else if (rayHit.collider.gameObject.name == "Play")
        {
            LoadGame();
        }
        else if (rayHit.collider.gameObject.name == "Artefacts")
        {
            OpenArtifacts();
        }
    }
    
    private void OpenShop()
    {
        shop.gameObject.SetActive(true);
        blackScreen.gameObject.SetActive(true);

        //EventManagerOld.SendShopOpened();
        EventManager.Trigger(GameEvents.ShopOpened);
    }
    private void OpenInventory()
    {
        inventory.gameObject.SetActive(true);
        blackScreen.gameObject.SetActive(true);

        //EventManagerOld.SendInventoryOpened();
        EventManager.Trigger(GameEvents.InventoryOpened);
    }
    private void LoadGame()
    {
        //EventManagerOld.SendGamePrepereToBeStarted();
            
        FillPlayerLoadout();
        if (playerLoadout.activeItems[0] == StaticDatas.EMPTY_SLOT && playerLoadout.activeItems[1] == StaticDatas.EMPTY_SLOT )
        {
            Debug.Log("Нет оружия в руках!");
        }
        else
        {
            EventManager.Trigger(GameEvents.PrepareForGame);
            SceneManager.LoadScene("GameScene");
        }
    }
    private void OpenArtifacts()
    {
        artifacts.gameObject.SetActive(true);
        blackScreen.gameObject.SetActive(true);

        //EventManagerOld.SendArtifactsOpened();
        EventManager.Trigger(GameEvents.ArtifactsOpened);
    }
    private void FillPlayerLoadout()
    {
        playerLoadout.Clear();

        InventoryData inv = GameManager.InventoryData;
        List<ItemInfo> allItems = info.GetAllItems();

        // 🔹 1. Перенос экипировки (последние 6 ячеек)
        // Обычно это 26–31, но используем константы
            // --- 1. Забираем оружие из инвентаря ---
            int weaponSlot1 = inv.slotItemIDs[StaticDatas._weaponID1];
            int weaponSlot2 = inv.slotItemIDs[StaticDatas._weaponID2];

            // --- 2. Нормализация порядка ---
            if (weaponSlot1 < 0 && weaponSlot2 >= 0)
            {
                playerLoadout.activeItems.Add(weaponSlot2); // основное
                playerLoadout.activeItems.Add(-1);          // второе пустое
            }
            else
            {
                playerLoadout.activeItems.Add(weaponSlot1);
                playerLoadout.activeItems.Add(weaponSlot2);
            }

            // --- 3. Остальная экипировка (без логики сдвига) ---
            for (int i = StaticDatas._bootsID; i < StaticDatas._inventoryLength; i++)
            {
                int itemID = inv.slotItemIDs[i];

                if (itemID < 0)
                    playerLoadout.activeItems.Add(StaticDatas.EMPTY_SLOT);
                else
                    playerLoadout.activeItems.Add(itemID);
            }
        // 🔹 2. Перенос прокачки бонусов и deployables
        for (int itemID = 0; itemID < allItems.Count; itemID++)
        {
            ItemInfo item = allItems[itemID];
            int level = inv.levels[itemID];

            if (level <= 0) continue;

            if (item.itemType == ItemType.Boost)
            {
                playerLoadout.bonusLevels[itemID] = level;
            }
            else if (item.itemType == ItemType.Deployables)
            {
                playerLoadout.deployableLevels[itemID] = level;
            }
        }
        for (int i = 0; i < 6 ; i++)
        {
            Debug.Log(playerLoadout.activeItems[i]);
        }
    }

}
