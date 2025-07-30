using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeployableItemInfoUI : MonoBehaviour
{
    public ItemInfo itemInfo;

    [Space]
    [Header("Info")]
    public TMP_Text nameText;
    public Text lvlText;
    public TMP_Text descriptionText;

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
    public GameObject otherButton;

    [Space]
    [Header("Bonuses")]
    public List<BonusUI> bonusesUI;

    [Space]
    [Header("Confirm")]
    public ConfirmArtefactsUI confirmUI;
}
