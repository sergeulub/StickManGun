using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BonusUI : MonoBehaviour
{
    public Image fonImage;
    public Image image;
    public TMP_Text nameText;
    public TMP_Text valueText;

    [ContextMenu("load")]

    public void Load()
    {
        fonImage = transform.GetChild(0).GetComponent<Image>();
        image = transform.GetChild(3).GetComponent<Image>();
        nameText = transform.GetChild(1).GetComponent<TMP_Text>();
        valueText = transform.GetChild(2).GetComponent<TMP_Text>();
    }
}
