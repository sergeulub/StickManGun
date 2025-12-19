using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Weapons: ItemInfo
{
    [Space(10)]
    [Header("���������� �� ������")]

    [Space(10)]
    [Header("���")]
    public WeaponType weaponType;

    [Space(10)]
    [Header("����")]
    public int damageValue;
    public List<int> upgradeDamageValues;

    [Space(10)]
    [Header("������")]
    public int cageValue;

    [Space(10)]
    [Header("��������")]
    public int speedValue;

    [Space(10)]
    [Header("�����������")]
    public float shotTime;
    public float reloadTime;

    [Space(10)]
    [Header("�������")]
    public float scatterValue;

    [Space(10)]
    [Header("��������")]
    public float flightLength;
    public Vector2 firePointLocalPos;
    public float _weaponDeltaAngle;
    public float _weaponDeltaAngleByDistance;
    public GameObject flamePrefab;//������ ��� �������



    private Dictionary<int, string> speedDict = new Dictionary<int, string> { { 1, "���" }, { 2, "������� ������" }, { 3, "������" } };

    public override void LoadShopItemInfo(ShopItemInfoUI itemInfo, MenuSprites sprites)
    {
        base.LoadShopItemInfo(itemInfo, sprites);
        List<BonusUI> bonuses = itemInfo.bonusesUI;

        bonuses[0].gameObject.SetActive(true);
        bonuses[0].nameText.text = "����";
        bonuses[0].valueText.text = damageValue.ToString();
        bonuses[0].image.sprite = sprites.damageSprite;

        bonuses[1].gameObject.SetActive(true);
        bonuses[1].nameText.text = "���� � �������";
        bonuses[1].valueText.text = Mathf.RoundToInt(damageValue / shotTime).ToString();
        bonuses[1].image.sprite = sprites.damagePerSecSprite;

        bonuses[2].gameObject.SetActive(true);
        bonuses[2].nameText.text = "�����������";
        string reloadSpeedStr;
        if (reloadTime > 2.5f)
            reloadSpeedStr = "���������";
        else if (reloadTime > 1.75f)
            reloadSpeedStr = "����������";
        else if (reloadTime > 1f)
            reloadSpeedStr = "�������";
        else
            reloadSpeedStr = "����� �������";
        bonuses[2].valueText.text = reloadSpeedStr;
        bonuses[2].image.sprite = sprites.reloadSprite;

        bonuses[3].gameObject.SetActive(true);
        bonuses[3].nameText.text = "������������";
        bonuses[3].valueText.text = speedDict[speedValue];
        bonuses[3].image.sprite = sprites.speedSlowdownSprite;

        bonuses[4].gameObject.SetActive(true);
        bonuses[4].nameText.text = "������";
        bonuses[4].valueText.text = cageValue.ToString();
        bonuses[4].image.sprite = sprites.cageSprite;

        bonuses[5].gameObject.SetActive(true);
        bonuses[5].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "�������� ��������";
        string shootSpeedStr;
        if (shotTime > 0.25f)
            shootSpeedStr = "���������";
        else if (shotTime > 0.13f)
            shootSpeedStr = "����������";
        else if (shotTime > 0.06f)
            shootSpeedStr = "�������";
        else
            shootSpeedStr = "����� �������";
        bonuses[5].valueText.text = shootSpeedStr;
        bonuses[5].image.sprite = sprites.shootSpeedSprite;
    }

    public override void LoadInventoryItemInfo(InventoryItemInfoUI itemInfoUI, MenuSprites sprites)
    {
        base.LoadInventoryItemInfo(itemInfoUI, sprites);

        int level = GameManager.InventoryData.levels[id];

        List<BonusUI> bonuses = itemInfoUI.bonusesUI;
        for (int i = 0; i <  bonuses.Count; i++)
        {
            bonuses[i].gameObject.SetActive(true);
        }
        
        BonusInventoryUI bonus1 = bonuses[0] as BonusInventoryUI;
        bonus1.gameObject.SetActive(true);
        bonus1.nameText.text = "����";
        bonus1.valueText.text = upgradeDamageValues[level].ToString();
        bonus1.image.sprite = sprites.damageSprite;

        BonusInventoryUI bonus2 = bonuses[1] as BonusInventoryUI;
        bonus2.gameObject.SetActive(true);
        bonus2.nameText.text = "���� � �������";
        bonus2.valueText.text = Mathf.RoundToInt(upgradeDamageValues[level] / shotTime).ToString();
        bonus2.image.sprite = sprites.damagePerSecSprite;
        if (level != 9)
        {
            bonus1.upgradeValueText.text = $"+{upgradeDamageValues[level + 1] - upgradeDamageValues[level]}";
            bonus2.upgradeValueText.text = $"+{Mathf.RoundToInt((upgradeDamageValues[level + 1] - upgradeDamageValues[level]) / shotTime)}";

            //��������� bonus1 upgradeText
            Vector3 pos1 = bonus1.upgradeTextTransform.transform.localPosition;
            pos1.x = StaticDatas._upgradeTextDelta(bonus1.valueText.text.Length, bonus1.valueText.text.Contains("%"));
            bonus1.upgradeTextTransform.transform.localPosition = pos1;

            //��������� bonus2 upgradeText
            Vector3 pos2 = bonus2.upgradeTextTransform.transform.localPosition;
            pos2.x = StaticDatas._upgradeTextDelta(bonus2.valueText.text.Length, bonus2.valueText.text.Contains("%"));
            bonus2.upgradeTextTransform.transform.localPosition = pos2;
        }
        else
            bonus2.upgradeValueText.text = "";

        bonuses[2].gameObject.SetActive(true);
        bonuses[2].nameText.text = "�����������";
        string reloadSpeedStr;
        if (reloadTime > 2.5f)
            reloadSpeedStr = "���������";
        else if (reloadTime > 1.75f)
            reloadSpeedStr = "����������";
        else if (reloadTime > 1f)
            reloadSpeedStr = "�������";
        else
            reloadSpeedStr = "����� �������";
        bonuses[2].valueText.text = reloadSpeedStr;
        bonuses[2].image.sprite = sprites.reloadSprite;

        bonuses[3].gameObject.SetActive(true);
        bonuses[3].nameText.text = "������������";
        bonuses[3].valueText.text = speedDict[speedValue];
        bonuses[3].image.sprite = sprites.speedSlowdownSprite;

        bonuses[4].gameObject.SetActive(true);
        bonuses[4].nameText.text = "������";
        bonuses[4].valueText.text = cageValue.ToString();
        bonuses[4].image.sprite = sprites.cageSprite;

        bonuses[5].gameObject.SetActive(true);
        bonuses[5].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "�������� ��������";
        string shootSpeedStr;
        if (shotTime > 0.25f)
            shootSpeedStr = "���������";
        else if (shotTime > 0.13f)
            shootSpeedStr = "����������";
        else if (shotTime > 0.06f)
            shootSpeedStr = "�������";
        else
            shootSpeedStr = "����� �������";
        bonuses[5].valueText.text = shootSpeedStr;
        bonuses[5].image.sprite = sprites.shootSpeedSprite;
    }


}
public enum WeaponType
{
    Pistol,
    Shotgun,
    Rifle,
    Flame,
    Sniper,
    PP,
    Minigun,
    P90
    /*Usp,
    Deagle,
    MP5,
    UZI,
    Shotgun,
    P90,
    M4A1,
    AK47,
    AWP,
    Minigun,
    Flameshot*/
}