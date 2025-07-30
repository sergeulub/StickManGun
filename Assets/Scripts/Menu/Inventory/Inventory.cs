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
        List<int> playerInventory = GameManager.InventoryData.slotItemIDs; // твой список из 30 €чеек с ID предметов
        List<ItemInfo> allItems = info.GetAllItems(); // список всех предметов (включа€ Weapons, Boots и т.д.)

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
                    Debug.LogWarning($"Ќе найден предмет с ID {itemID}");
                }
            }
        }

        UpdatePlayerVisual();

        for (int i = 0; i < StaticDatas._inventoryLength; i++)
        {
            int ID = GameManager.InventoryData.slotItemIDs[i];
            if (ID != StaticDatas._emptyID)
            {
                Debug.Log($"¬ €чейке {i} находитс€ {itemInfos[ID].itemName}");
            }
            else
            {
                Debug.Log($"¬ €чейке {i} пусто");
            }
        }
    }
    private void SetEmptySlotVisual(InventorySlotUI slotUI, int slotIndex)
    {
        slotUI.imageTransform.sizeDelta = Vector2.zero;
        slotUI.imageTransform.localPosition = Vector2.zero;

        
        if (slotIndex >= StaticDatas._weaponID1) // слоты игрока
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
        else // обычные €чейки
        {
            slotUI.image.sprite = null;
        }

    }
    private void UpdatePlayerVisual()
    {
        List<int> inventory = GameManager.InventoryData.slotItemIDs;
        List<ItemInfo> allItems = info.GetAllItems();

        // Ўлем Ч ID 29
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

        // Ѕотинки Ч ID 28
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

        // ќружие Ч ID 26 или 27
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
            playerVisual.UpdateWeapon(StaticDatas._emptyID); // -1: голые руки
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
