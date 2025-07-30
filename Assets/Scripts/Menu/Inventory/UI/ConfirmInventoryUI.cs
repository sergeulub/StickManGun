using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmInventoryUI : MonoBehaviour
{
    public GameObject fon;

    [Space]
    [Header("Upgrade")]
    public GameObject upgrade;
    public TMP_Text upgradePriceText;
    public GameObject upgradeCoinIcon;

    [Space]
    [Header("Sell")]
    public GameObject sell;
    public TMP_Text sellPriceText;
    public GameObject sellCoinIcon;

    
}
