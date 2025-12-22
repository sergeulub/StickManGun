using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BonusInventoryUI : BonusUI
{
    public bool canBeUpgraded = true;

    public Image darkFon;

    public Image lightFon;
    public TMP_Text upgradeValueText;
    public RectTransform upgradeTextTransform;

    [ContextMenu("loadUpg")]
    public void LoadUPG()
    {
        darkFon = transform.GetChild(0).GetComponent<Image>();
        image = transform.GetChild(4).GetComponent<Image>();
        nameText = transform.GetChild(2).GetComponent<TMP_Text>();
        valueText = transform.GetChild(3).GetComponent<TMP_Text>();
        upgradeValueText = transform.GetChild(5).GetComponent<TMP_Text>();
        upgradeTextTransform = transform.GetChild(5).GetComponent<RectTransform>();
    }
}
