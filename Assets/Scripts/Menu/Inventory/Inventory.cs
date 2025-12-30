using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("References")]
    public Info info;

    [Header("UI")]
    public InventoryUI1 inventoryUI;
    public MenuSprites sprites;
    public InventoryItemInfoUI itemInfoUI;
    public MainMenuUI mainMenuUI;

    [Header("Player Visual")]
    public PlayerVisual playerVisual;

    private List<ItemInfo> itemInfos;

    private void OnEnable()
    {
        EventManager.Subscribe(GameEvents.InventoryOpened, LoadInventoryInfo);
        EventManager.Subscribe(GameEvents.ItemDragEnded, LoadInventoryInfo);
        EventManager.Subscribe<ItemInfo>(GameEvents.ItemClicked, LoadItemInfo);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.InventoryOpened, LoadInventoryInfo);
        EventManager.Unsubscribe(GameEvents.ItemDragEnded, LoadInventoryInfo);
        EventManager.Unsubscribe<ItemInfo>(GameEvents.ItemClicked, LoadItemInfo);
    }
    private void Awake()
    {
        itemInfos = info.GetAllItems();
    }
    public void _ConfirmUpgrade()
    {
        OpenConfirmUpgradeWindow();
    }
    public void _ConfirmSell()
    {
        OpenConfirmSellWindow();
    }

    [ContextMenu("Log")]
    public void LogInfo()
    {   
        for (int i = 0; i < StaticDatas._quantityItems; i++)
        {   
            ItemInfo item = info._allItems[i];
            string _name = $"Предмет {item.itemName} (ID: {item.id})";
            string _level = $"; уровень прокачки: {GameManager.InventoryData.levels[i]}"; 
            string _pos = GameManager.InventoryData.arsenal[i] == 1? $"; куплен": $"; не куплен";
            
            
            Debug.Log(_name + _level + _pos);
        }
        for (int i = 0; i < StaticDatas._boostsCount; i++)
        {   
            ItemInfo item = info.boosts[i];
            string _name = $"Бонус {item.itemName} (ID: {item.id})";
            string _level = $"; уровень прокачки: {GameManager.ArtefactsData.boostsLevels[i]}";            
            
            Debug.Log(_name + _level);
        }
        for (int i = 0; i < StaticDatas._deployableCount; i++)
        {   
            ItemInfo item = info.deployables[i];
            string _name = $"Бонус {item.itemName} (ID: {item.id})";
            string _level = $"; уровень прокачки: {GameManager.ArtefactsData.deployablesLevels[i]}";    
            string _pos = GameManager.ArtefactsData.deployablesArsenal[i] == 1? $"; доступен": $"; не доступен ";

            
            Debug.Log(_name + _level + _pos);
        }
    }
    public void _Upgrade()
    {
        Upgrade();
    }
    public void _Sell()
    {
        Sell();
    }
    private void LoadInventoryInfo()
    {
        List<int> playerInventory = GameManager.InventoryData.slotItemIDs; // ���� ������ �� 30 ����� � ID ���������
        List<ItemInfo> allItems = info.GetAllItems(); // ������ ���� ��������� (������� Weapons, Boots � �.�.)

        for (int i = 0; i < StaticDatas._inventoryLength; i++)
        {
            InventorySlotUI slotUI = inventoryUI.inventorySlotsUI[i];
            InventorySlot slot = slotUI.transform.GetComponent<InventorySlot>();

            int itemID = playerInventory[i];

            if (itemID == StaticDatas._emptyID)
            {
                slot.itemInfo = null;
                SetEmptySlotVisual(slotUI, i);
            }
            else
            {
                ItemInfo itemInfo = allItems[playerInventory[i]];
                if (itemInfo != null)
                {
                    slot.itemInfo = itemInfo;
                    itemInfo.LoadInventorySlotUI(slotUI);
                }
                else
                {
                    Debug.LogWarning($"�� ������ ������� � ID {itemID}");
                }
            }
        }

        UpdatePlayerVisual();

        // for (int i = 0; i < StaticDatas._inventoryLength; i++)
        // {
        //     int ID = GameManager.InventoryData.slotItemIDs[i];
        //     if (ID != StaticDatas._emptyID)
        //     {
        //         Debug.Log($"� ������ {i} ��������� {itemInfos[ID].itemName}");
        //     }
        //     else
        //     {
        //         Debug.Log($"� ������ {i} �����");
        //     }
        // }
    }
    private void SetEmptySlotVisual(InventorySlotUI slotUI, int slotIndex)
    {
        slotUI.imageTransform.sizeDelta = Vector2.zero;
        slotUI.imageTransform.localPosition = Vector2.zero;

        
        if (slotIndex >= StaticDatas._weaponID1) // ����� ������
        {   
            switch (slotIndex)
            {
                case 26:
                    slotUI.image.sprite 
                        = sprites.emptyWeaponSprite;
                    slotUI.imageTransform.sizeDelta = sprites.emptyWeaponSprite.rect.size;
                    break;
                case 27:
                    slotUI.image.sprite = sprites.emptyWeaponSprite;
                    slotUI.imageTransform.sizeDelta = sprites.emptyWeaponSprite.rect.size;
                    break;
                case 28:
                    slotUI.image.sprite = sprites.emptyBootsSprite;
                    slotUI.imageTransform.sizeDelta = sprites.emptyBootsSprite.rect.size;
                    break;
                case 29:
                    slotUI.image.sprite = sprites.emptyHatSprite;
                    slotUI.imageTransform.sizeDelta = sprites.emptyHatSprite.rect.size;
                    break;
                case 30:
                case 31:
                    slotUI.image.sprite = sprites.emptyRingSprite;
                    slotUI.imageTransform.sizeDelta = sprites.emptyRingSprite.rect.size;
                    break;
            }
        }
        else // ������� ������
        {
            slotUI.image.sprite = null;
        }

    }
    private void UpdatePlayerVisual()
    {
        List<int> inventory = GameManager.InventoryData.slotItemIDs;
        List<ItemInfo> allItems = info.GetAllItems();

        // ���� � ID 29
        int hatID = inventory[StaticDatas._hatID];
        if (hatID != -1)
        {
            Hat hat = allItems[hatID] as Hat;
            if (hat != null)
                playerVisual.UpdateHat(hat.animationSprite);
        }
        else
        {
            playerVisual.UpdateHat(null);
        }

        // ������� � ID 28
        int bootsID = inventory[StaticDatas._bootsID];
        if (bootsID != -1)
        {
            Boots boots = allItems[bootsID] as Boots;
            if (boots != null)
                playerVisual.UpdateBoots(boots.animationSprite);
        }
        else
        {
            playerVisual.UpdateBoots(null);
        }

        // ������ � ID 26 ��� 27
        int weaponID = inventory[StaticDatas._weaponID1];
        if (weaponID == StaticDatas._emptyID)
            weaponID = inventory[StaticDatas._weaponID2];

        if (weaponID != StaticDatas._emptyID)
        {
            Weapons weapon = allItems[weaponID] as Weapons;
            if (weapon != null)
            {
                playerVisual.UpdateWeapon(weapon.id);
            }
        }
        else
        {
            playerVisual.UpdateWeapon(StaticDatas._emptyID); // -1: ����� ����
        }
    }
    private void LoadItemInfo(ItemInfo item)
    {
        Debug.Log($"{item.itemName} opened");
        itemInfoUI.gameObject.SetActive(true);
        item.LoadInventoryItemInfo(itemInfoUI, sprites);
    }
    private void OpenConfirmUpgradeWindow()
    {
        ConfirmInventoryUI confirmUI = itemInfoUI.confirmUI;

        confirmUI.gameObject.SetActive(true);
        confirmUI.upgrade.SetActive(true);
        confirmUI.sell.SetActive(false);
        confirmUI.upgradePriceText.text = itemInfoUI.upgradePriceText.text;
        confirmUI.upgradeCoinIcon.transform.localPosition = itemInfoUI.upgradeCoinIcon.transform.localPosition;
    }
    private void OpenConfirmSellWindow()
    {
        ConfirmInventoryUI confirmUI = itemInfoUI.confirmUI;

        confirmUI.gameObject.SetActive(true);
        confirmUI.upgrade.SetActive(false);
        confirmUI.sell.SetActive(true);
        confirmUI.sellPriceText.text = itemInfoUI.sellPriceText.text;
        confirmUI.sellCoinIcon.transform.localPosition = itemInfoUI.sellCoinIcon.transform.localPosition;
    }
    private void Upgrade()
    {
        itemInfoUI.itemInfo.UpgradeItem();

        itemInfoUI.confirmUI.gameObject.SetActive(false);

        LoadInventoryInfo();
        itemInfoUI.itemInfo.LoadInventoryItemInfo(itemInfoUI, sprites);

        mainMenuUI.moneyText.text = StaticDatas._moneyTextFormat(GameManager.money);
    }
    private void Sell()
    {
        itemInfoUI.itemInfo.SellItem();

        itemInfoUI.confirmUI.gameObject.SetActive(false);
        itemInfoUI.gameObject.SetActive(false);

        LoadInventoryInfo();

        mainMenuUI.moneyText.text = StaticDatas._moneyTextFormat(GameManager.money);
    }
}
