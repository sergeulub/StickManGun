using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeployableItemCellUI : MonoBehaviour
{
    public Transform cell;

    [Header("Sprite")]
    public Image image;
    public RectTransform imageTransform;
    public RectTransform imageShadow;

    [Header("Name")]
    public TMP_Text nameText;

    [Header("Price")]
    public TMP_Text priceText;

    [Header("Upgrade")]
    public Button upgradeButton;
    public GameObject coinIcon;

    [Header("Lock")]
    public GameObject lockSprite;
    public TMP_Text lockText;
    public GameObject lockTextGO;
    public Animation lockAnim;


    [ContextMenu("LoadInfo")]
    public void LoadInfo()
    {
        /*cell = GetComponent<Transform>();
        image = cell.GetChild(2).GetComponent<Image>();
        imageTransform = cell.GetChild(2).GetComponent<RectTransform>();
        imageShadow = cell.GetChild(1).GetComponent<RectTransform>();
        nameText = cell.GetChild(3).GetComponent<TMP_Text>();
        priceText = cell.GetChild(4).GetChild(0).GetComponent<TMP_Text>();
        upgradeButton = cell.GetChild(4).GetComponent<Button>();
        coinIcon = cell.GetChild(4).GetChild(2).gameObject;
        */
        lockText = cell.Find("ImageText").GetComponentInChildren<TMP_Text>();
        lockTextGO = lockText.transform.parent.gameObject;
        lockSprite = cell.Find("LockEmpty").gameObject;
        lockAnim = cell.GetComponent<Animation>();
    }
}
