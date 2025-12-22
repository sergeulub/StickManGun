using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[Serializable]
public class Boosts: ItemInfo
{
    public string description;

    public string bonusName;
    public string bonusSign;
    public float bonusValue;
    public float upgradeValue;
    public Sprite bonusSprite;

    public void LoadInfo(BoostCellUI cell)
    {
        cell.image.sprite = base.itemSprite;

        cell.nameText.text = base.itemName;

        int level = GameManager.ArtefactsData.boostsLevels[base.id];

        cell.levelText.text = (level+1).ToString();
    }
    public void LoadItemInfo(BoostItemInfoUI itemInfoUI)
    {
        itemInfoUI.itemInfo = this;

        itemInfoUI.gameObject.SetActive(true);//включаем

        itemInfoUI.nameText.text = itemName;//имя

        itemInfoUI.descriptionText.text = description;

        itemInfoUI.image.sprite = itemSprite;//картинка

        int level = GameManager.ArtefactsData.boostsLevels[base.id];

        itemInfoUI.lvlText.text = $"УР. {level + 1}";

        itemInfoUI.upgradeButton.gameObject.SetActive(true);
        itemInfoUI.otherButton.SetActive(false);

        if (GameManager.boostPoint > 0)
        {
            itemInfoUI.upgradeButton.interactable = true;
        }
        else
        {
            itemInfoUI.upgradeButton.interactable = false;
        }

        List<BonusUI> bonuses = itemInfoUI.bonusesUI;

        BonusInventoryUI bonus1 = bonuses[0] as BonusInventoryUI;

        bonus1.nameText.text = bonusName;

        float value1 = bonusValue + StaticDatas._valueByLevel(level, upgradeValue);
        bonus1.valueText.text = $"{value1} {bonusSign}";

        bonus1.image.sprite = bonusSprite;

        string empty = "";
        if (bonusSign != "")
            empty = "  ";
        else
            empty = "";

        bonus1.upgradeValueText.text = $"{empty}+{upgradeValue}";

        //положение  bonus1 upgradeText
        Vector3 pos1 = bonus1.upgradeTextTransform.transform.localPosition;
        pos1.x = StaticDatas._upgradeDeployablesTextDelta(bonus1.valueText.text.Length - 1);
        bonus1.upgradeTextTransform.transform.localPosition = pos1;

        bonuses[1].gameObject.SetActive(false);

        
    }
    public override void UpgradeItem()
    {
        int level = GameManager.ArtefactsData.boostsLevels[id];

        GameManager.ArtefactsData.UpgradeBoost(id);
        GameManager.DecreaseBoostPoint();
    }
}
