using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Deployables : ItemInfo
{
    [Header("Position")]
    public Vector2 position;
    public Vector2 shadowPos;
    public Vector2 spriteSizeMenu;

    [Space]
    [Header("Bonus1")]
    public string bonus1Name;
    public string bonus1Sign;
    public float bonus1Value;
    public Sprite bonus1Sprite;
    public float upgrade1Value;

    [Space]
    [Header("Bonus2")]
    public string bonus2Name;
    public string bonus2Sign;
    public float bonus2Value;
    public Sprite bonus2Sprite;
    public float upgrade2Value;

    [Space]
    [Header("Описание")]
    public string description;

    public void LoadDeployablesInfo(DeployableItemCellUI cell)
    {
        if (GameManager.playerLevel < requiredLevel)
        {
            cell.lockSprite.SetActive(true);
            cell.lockTextGO.SetActive(true);
            cell.lockText.text = $"откроется на {requiredLevel + 1} ур.";
        }
        else if (this.isNew())
        {
            cell.lockAnim.Play();
        }
        else
        {
            cell.lockSprite.SetActive(false);
            cell.lockTextGO.SetActive(false);
        }
        cell.image.sprite = base.itemSprite;
        cell.imageTransform.sizeDelta = spriteSizeMenu;
        cell.imageTransform.localPosition = position;
        cell.imageShadow.localPosition = shadowPos;

        cell.nameText.text = base.itemName;

        int level = GameManager.ArtefactsData.deployablesLevels[id];
        cell.priceText.text = StaticDatas._upgradePriceByLevel(level, buyPrice).ToString();

        //положение монетки
        Vector3 pos = cell.coinIcon.transform.localPosition;
        pos.x = StaticDatas._coinDelta(cell.priceText.text.Length);
        cell.coinIcon.transform.localPosition = pos;

        //логика замка
    }
    public void LoadDeployablesItemInfo(DeployableItemInfoUI itemInfoUI)
    {
        itemInfoUI.itemInfo = this;

        itemInfoUI.gameObject.SetActive(true);//включаем

        itemInfoUI.nameText.text = itemName;//имя

        itemInfoUI.descriptionText.text = description;

        itemInfoUI.image.sprite = itemSprite;//картинка
        if (spriteSizeMenu.y > 51)
            itemInfoUI.imageTransform.sizeDelta = spriteSizeMenu / 1.4f;
        else
            itemInfoUI.imageTransform.sizeDelta = spriteSizeMenu;
       itemInfoUI.imageTransform.localPosition = position;

        int level = GameManager.ArtefactsData.deployablesLevels[base.id];

        itemInfoUI.upgradeButton.gameObject.SetActive(true);
        itemInfoUI.otherButton.SetActive(false);

        int upgradePrice = StaticDatas._upgradePriceByLevel(level, buyPrice);
        if (GameManager.EnoughMoney(upgradePrice))
        {
            itemInfoUI.upgradeButton.interactable = true;
        }
        else
        {
            itemInfoUI.upgradeButton.interactable= false;
        }
        itemInfoUI.upgradePriceText.text = upgradePrice.ToString();

        //положение монетки
        Vector3 pos = itemInfoUI.upgradeCoinIcon.transform.localPosition;
        pos.x = StaticDatas._coinDelta(itemInfoUI.upgradePriceText.text.Length);
        itemInfoUI.upgradeCoinIcon.transform.localPosition = pos;

        itemInfoUI.lvlText.text = $"УР. {level + 1}";


        List<BonusUI> bonuses = itemInfoUI.bonusesUI;

        BonusInventoryUI bonus1 = bonuses[0] as BonusInventoryUI;

        bonus1.nameText.text = bonus1Name;

        float value1 = bonus1Value + StaticDatas._valueByLevel(level, upgrade1Value);
        bonus1.valueText.text = $"{value1} {bonus1Sign}";

        bonus1.image.sprite = bonus1Sprite;

        bonus1.upgradeValueText.text = $"+{upgrade1Value}";

        //положение  bonus1 upgradeText
        Vector3 pos1 = bonus1.upgradeTextTransform.transform.localPosition;
        pos1.x = StaticDatas._upgradeDeployablesTextDelta(bonus1.valueText.text.Length-1);
        bonus1.upgradeTextTransform.transform.localPosition = pos1;

        BonusInventoryUI bonus2 = bonuses[1] as BonusInventoryUI;
        if (bonus2Name != "")
        {
            bonus2.gameObject.SetActive(true);
            bonus2.nameText.text = bonus2Name;

            float value2 = bonus2Value + StaticDatas._valueByLevel(level, upgrade2Value);

            bonus2.valueText.text = $"{value2} {bonus2Sign}";

            bonus2.image.sprite = bonus2Sprite;

            bonus2.upgradeValueText.text = $"+{upgrade2Value}";

            //положение  bonus2 upgradeText
            Vector3 pos2 = bonus2.upgradeTextTransform.transform.localPosition;
            pos2.x = StaticDatas._upgradeDeployablesTextDelta(bonus2.valueText.text.Length - 1);
            bonus2.upgradeTextTransform.transform.localPosition = pos2;
        }
        else
        {
            bonus2.gameObject.SetActive(false);
        }
    }
    public override void UpgradeItem()
    {
        int level = GameManager.ArtefactsData.deployablesLevels[id];
        int buyPrice = StaticDatas._upgradePriceByLevel(level, base.buyPrice);

        GameManager.DecreaseMoney(buyPrice);

        GameManager.ArtefactsData.UpgradeDeploable(id);

        
    }
    private bool isNew()
    {
        return GameManager.ArtefactsData.deployablesIsNew[id] == 1;
    }
}
