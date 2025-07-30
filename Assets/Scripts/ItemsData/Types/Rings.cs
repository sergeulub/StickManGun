using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Ring : ItemInfo
{
    [Space]
    [Header("Rings")]

    [Space]
    [Header("Bonus1")]
    public string bonus1Name;
    public string bonus1Sign;
    public float bonus1Value;
    public Sprite bonus1Sprite;
    public List<float> upgrade1Values;

    [Space]
    [Header("Bonus2")]
    public string bonus2Name;
    public string bonus2Sign;
    public float bonus2Value;
    public Sprite bonus2Sprite;
    public List<float> upgrade2Values;

    public override void LoadShopItemInfo(ShopItemInfoUI itemInfo, MenuSprites sprites)
    {
        base.LoadShopItemInfo(itemInfo, sprites);
        List<BonusUI> bonuses = itemInfo.bonusesUI;

        for (int i = 0; i < bonuses.Count; i++)
            bonuses[i].gameObject.SetActive(false);

        bonuses[0].gameObject.SetActive(true);
        bonuses[0].nameText.text = bonus1Name;
        bonuses[0].valueText.text = $"+{bonus1Value}{bonus1Sign}";
        bonuses[0].image.sprite = bonus1Sprite;

        if (bonus2Name != "")
        {
            bonuses[1].gameObject.SetActive(true);
            bonuses[1].nameText.text = bonus2Name;

            string sign2;
            if (bonus2Value < 0f)
                sign2 = "";
            else
                sign2 = "+";

            bonuses[1].valueText.text = $"{sign2}{bonus2Value}{bonus2Sign}";
            bonuses[1].image.sprite = bonus2Sprite;
        }
    }
    public override void LoadInventoryItemInfo(InventoryItemInfoUI itemInfoUI, MenuSprites sprites)
    {
        base.LoadInventoryItemInfo(itemInfoUI, sprites);

        int level = GameManager.InventoryData.levels[id];

        List<BonusUI> bonuses = itemInfoUI.bonusesUI;
        for (int i = 2; i < bonuses.Count; i++)
        {
            bonuses[i].gameObject.SetActive(false);
        }

        BonusInventoryUI bonus1 = bonuses[0] as BonusInventoryUI;
        bonus1.gameObject.SetActive(true);
        bonus1.nameText.text = bonus1Name;
        bonus1.valueText.text = $"+{upgrade1Values[level]} {bonus1Sign}";
        bonus1.image.sprite = bonus1Sprite;
        if (level != 9)
            bonus1.upgradeValueText.text = $"+{upgrade1Values[level + 1] - upgrade1Values[level]}";
        else
            bonus1.upgradeValueText.text = "";

        //положение  bonus1 upgradeText
        Vector3 pos1 = bonus1.upgradeTextTransform.transform.localPosition;
        pos1.x = StaticDatas._upgradeTextDelta(bonus1.valueText.text.Length, bonus1.valueText.text.Contains("%"));
        bonus1.upgradeTextTransform.transform.localPosition = pos1;

        BonusInventoryUI bonus2 = bonuses[1] as BonusInventoryUI;
        if (bonus2Name != "")
        {
            bonus2.gameObject.SetActive(true);
            bonus2.nameText.text = bonus2Name;

            string sign2;
            if (bonus2Value < 0f)
                sign2 = "";
            else
                sign2 = "+";

            bonus2.valueText.text = $"{sign2}{upgrade2Values[level]} {bonus2Sign}";
            bonus2.image.sprite = bonus2Sprite;
            if (level != 9)
                bonus2.upgradeValueText.text = $"+{upgrade2Values[level + 1] - upgrade2Values[level]}";
            else
                bonus2.upgradeValueText.text = "";

            //положение bonus2 upgradeText
            Vector3 pos2 = bonus2.upgradeTextTransform.transform.localPosition;
            pos2.x = StaticDatas._upgradeTextDelta(bonus2.valueText.text.Length, bonus2.valueText.text.Contains("%"));
            bonus2.upgradeTextTransform.transform.localPosition = pos2;
        }

        else
        {
            bonus2.gameObject.SetActive(false);
        }
    }
}
