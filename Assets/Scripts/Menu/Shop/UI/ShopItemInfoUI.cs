using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemInfoUI : MonoBehaviour
{
    public TMP_Text nameText;
    public Image image;
    public RectTransform imageTransform;
    public Button buyOrSellButton;
    public Image buyOrSellImage;
    public Text buyOrSellText;
    public TMP_Text priceText;
    public GameObject coinIcon;
    public List<BonusUI> bonusesUI;
    public ConfirmUI confirm;
    
}
