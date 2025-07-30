using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostCellUI : MonoBehaviour
{
    public TMP_Text nameText;
    public Image image;
    public TMP_Text levelText;

    [ContextMenu("Load")]
    public void Load()
    {
        nameText = transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        levelText = transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>();
        image = transform.GetChild(2).GetComponent<Image>();
    } 
}


