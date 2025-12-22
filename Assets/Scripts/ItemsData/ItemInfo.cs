using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemInfo
{
    [Header("��������")]
    public string itemName;
    public int id;


    [Space(10)]
    [Header("���")]
    public ItemType itemType;

    [Space(10)]
    [Header("������")]
    public Sprite itemSprite;
    public Vector2 spriteSize;

    [Space(10)]
    [Header("����")]
    public int buyPrice;
    public List<int> upgradePrices;

    [Space(10)]
    [Header("������� ��������")]
    public int requiredLevel;

    
    private ShopCellUI cell;

    public void LoadShopInfo(ShopCellUI cell, MenuSprites sprites)
    {   
        this.cell = cell;
        if (GameManager.playerLevel < requiredLevel)
        {
            cell.lockSprite.SetActive(true);
            cell.lockTextGO.SetActive(true);
            cell.lockText.text = $"откроется на {requiredLevel + 1} ур.";
        }
        else
        {
            cell.lockSprite.SetActive(false);
            cell.lockTextGO.SetActive(false);
        }

        if (GameManager.InventoryData.isNew[id] == 1)
        {
            cell.newText.SetActive(true);
        }
        else
        {
            cell.newText.SetActive(false);
        }

        cell.nameText.text = itemName;

        cell.image.sprite = itemSprite;
        if (spriteSize.x < 90)
            cell.imageTransform.sizeDelta = spriteSize;
        else
        {
            float y = spriteSize.y * 110 / spriteSize.x;
            cell.imageTransform.sizeDelta = new Vector2(110, y);
        }


        if (!GameManager.InventoryData.isBought(id))
        {
            cell.priceText.text = buyPrice.ToString();
            cell.buyOrSellText.text = StaticDatas._buyText;
            cell.buyOrSellImage.sprite = sprites.buySprite;
        }
        else
        {
            cell.priceText.text = StaticDatas._sellPrice(buyPrice).ToString();
            cell.buyOrSellText.text = StaticDatas._sellText;
            cell.buyOrSellImage.sprite = sprites.sellSprite;
        }

        //��������� �������
        Vector3 pos = cell.coinIcon.transform.localPosition;
        pos.x = StaticDatas._coinDelta(cell.priceText.text.Length);
        cell.coinIcon.transform.localPosition = pos;
        
    }
    public virtual void LoadShopItemInfo(ShopItemInfoUI itemInfoUI, MenuSprites sprites)
    {   
        if (GameManager.InventoryData.isNew[id] == 1)
        {
            GameManager.InventoryData.isNew[id] = 0;
            cell.newText.SetActive(false);
        }


        itemInfoUI.gameObject.SetActive(true);//��������

        itemInfoUI.nameText.text = itemName;//���

        itemInfoUI.image.sprite = itemSprite;//��������
        if (spriteSize.x < 90)
            itemInfoUI.imageTransform.sizeDelta = spriteSize;
        else
        {
            float y = spriteSize.y * 87 / spriteSize.x;
            itemInfoUI.imageTransform.sizeDelta = new Vector2(87, y);
        }

        //������� ��� �������
        if (!GameManager.InventoryData.isBought(id))
        {
            itemInfoUI.priceText.text = buyPrice.ToString();
            itemInfoUI.buyOrSellText.text = StaticDatas._buyText;
            itemInfoUI.buyOrSellImage.sprite = sprites.buySprite;

            if (!GameManager.EnoughMoney(buyPrice))
            {
                itemInfoUI.buyOrSellButton.interactable = false;
            }
            else
            {
                itemInfoUI.buyOrSellButton.interactable = true;
            }
        }
        else
        {
            itemInfoUI.priceText.text = StaticDatas._sellPrice(buyPrice).ToString();
            itemInfoUI.buyOrSellText.text = StaticDatas._sellText;
            itemInfoUI.buyOrSellImage.sprite = sprites.sellSprite;
            itemInfoUI.buyOrSellButton.interactable = true;
        }

        //��������� �������
        Vector3 pos = itemInfoUI.coinIcon.transform.localPosition;
        pos.x = StaticDatas._coinDelta(itemInfoUI.priceText.text.Length);
        itemInfoUI.coinIcon.transform.localPosition = pos;

    }
    public void Confirm(ConfirmUI confirmUI)
    {
        confirmUI.gameObject.SetActive(true);
        if (!GameManager.InventoryData.isBought(id))
        {
            confirmUI.sell.SetActive(false);
            confirmUI.buy.SetActive(true);
            confirmUI.buyPriceText.text = buyPrice.ToString();

            Vector3 pos = confirmUI.buyCoinIcon.transform.localPosition;
            pos.x = StaticDatas._coinDelta(confirmUI.buyPriceText.text.Length);
            confirmUI.buyCoinIcon.transform.localPosition = pos;
        }
        else
        {
            confirmUI.buy.SetActive(false);
            confirmUI.sell.SetActive(true);
            confirmUI.sellPriceText.text = StaticDatas._sellPrice(buyPrice).ToString();

            Vector3 pos = confirmUI.sellCoinIcon.transform.localPosition;
            pos.x = StaticDatas._coinDelta(confirmUI.sellPriceText.text.Length);
            confirmUI.sellCoinIcon.transform.localPosition = pos;
        }
    }
    public void SellItem()
    {
        int sellPrice = StaticDatas._sellPrice(buyPrice);

        GameManager.InventoryData.SetArsenal(id, false);
        GameManager.InventoryData.SetZeroLevel(id);
        GameManager.AddMoney(sellPrice);

        for (int i = 0; i < StaticDatas._inventoryLength; i++)
        {
            if (GameManager.InventoryData.GetItem(i) == id)
            {
                GameManager.InventoryData.SetItem(i, -1);
                break;
            }
        }

        //���������� �����������
        EventManager.Trigger(GameEvents.ItemSold, id);
    }
    public void BuyItem()
    {
        int index = -1;
        for (int i = 0; i <= StaticDatas._inventoryLength; i++)
        {
            if (GameManager.InventoryData.GetItem(i) == -1)
            {
                index = i; break;
            }
        }
        GameManager.InventoryData.SetItem(index, id);
        GameManager.InventoryData.SetArsenal(id, true);
        GameManager.InventoryData.UpgradeItem(id);

        GameManager.DecreaseMoney(buyPrice);

        //���������� �����������
        EventManager.Trigger(GameEvents.ItemBought, id);
    }
    public virtual void UpgradeItem()
    {
        int level = GameManager.InventoryData.levels[id];
        int buyPrice = upgradePrices[level];

        GameManager.DecreaseMoney(buyPrice);

        GameManager.InventoryData.UpgradeItem(id);

        //���������� �����������
        EventManager.Trigger(GameEvents.ItemBought, id);
    }
    public void LoadInventorySlotUI(InventorySlotUI slotUI)
    {
        slotUI.image.sprite = itemSprite;

        if (spriteSize.x < 80)
            slotUI.imageTransform.sizeDelta = spriteSize;
        else
        {
            float y = spriteSize.y * 80 / spriteSize.x;
            slotUI.imageTransform.sizeDelta = new Vector2(80, y);
        }
        slotUI.imageTransform.localPosition = Vector2.zero;
    }
    public virtual void LoadInventoryItemInfo(InventoryItemInfoUI itemInfoUI, MenuSprites sprites)
    {
        itemInfoUI.itemInfo = this;

        itemInfoUI.gameObject.SetActive(true);//��������

        itemInfoUI.nameText.text = itemName;//���

        itemInfoUI.image.sprite = itemSprite;//��������
        if (spriteSize.x < 90)
            itemInfoUI.imageTransform.sizeDelta = spriteSize;
        else
        {
            float y = spriteSize.y * 87 / spriteSize.x;
            itemInfoUI.imageTransform.sizeDelta = new Vector2(87, y);
        }
        int level = GameManager.InventoryData.levels[id];

        itemInfoUI.lvlText.text = $"��. {level + 1}";

        //Sell

        itemInfoUI.sellPriceText.text = StaticDatas._sellPrice(buyPrice).ToString();
        
        //��������� ������� Sell
        Vector3 posS = itemInfoUI.sellCoinIcon.transform.localPosition;
        posS.x = StaticDatas._coinDelta(itemInfoUI.sellPriceText.text.Length);
        itemInfoUI.sellCoinIcon.transform.localPosition = posS;


        //Upgrade
        if (level != 9)
        {
            itemInfoUI.upgradeButton.gameObject.SetActive(true);

            itemInfoUI.upgradePriceText.text = upgradePrices[level].ToString();

            //��������� ������� Upgrade
            Vector3 posU = itemInfoUI.upgradeCoinIcon.transform.localPosition;
            posU.x = StaticDatas._coinDelta(itemInfoUI.upgradePriceText.text.Length);
            itemInfoUI.upgradeCoinIcon.transform.localPosition = posU;

            //������� �� �����
            if (!GameManager.EnoughMoney(upgradePrices[level + 1]))
            {
                itemInfoUI.upgradeButton.interactable = false;
            }
            else
            {
                itemInfoUI.upgradeButton.interactable = true;
            }
        }
        else
        {
            itemInfoUI.upgradeButton.gameObject.SetActive(false);
        }
    }
    public int LevelUpLoad(Image image)
    {
        if (GameManager.playerLevel == GameManager.InventoryData.levels[id])
        {
            image.sprite = itemSprite;
            return 1;
        }
        return 0; 
    }
}

public enum ItemType
{
    Weapon,
    Boots,
    Hat,
    Ring,
    Deployables,
    Boost
}