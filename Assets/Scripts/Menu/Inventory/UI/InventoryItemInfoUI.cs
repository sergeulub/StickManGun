using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemInfoUI : MonoBehaviour
{
    public ItemInfo itemInfo;

    [Space]
    [Header("Info")]
    public TMP_Text nameText;
    public Text lvlText;

    [Space]
    [Header("Sprite")]
    public Image image;
    public RectTransform imageTransform;

    [Space]
    [Header("Upgrade")]
    public Button upgradeButton;
    public Image upgradeImage;
    public Text upgradeText;
    public TMP_Text upgradePriceText;
    public GameObject upgradeCoinIcon;

    [Space]
    [Header("Sell")]
    public Button sellButton;
    public Image sellImage;
    public Text sellText;
    public TMP_Text sellPriceText;
    public GameObject sellCoinIcon;

    [Space]
    [Header("Bonuses")]
    public List<BonusUI> bonusesUI;

    [Space]
    [Header("Confirm")]
    public ConfirmInventoryUI confirmUI;
}
