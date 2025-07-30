using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public InventoryItem inventoryItem;
    public Image image;
    public RectTransform imageTransform;

    [ContextMenu("load")]
    public void Load()
    {
        inventoryItem = GetComponentInChildren<InventoryItem>();
        image = transform.GetChild(0).GetComponent<Image>();
        imageTransform = transform.GetChild(0).GetComponent<RectTransform>();
    }
}
