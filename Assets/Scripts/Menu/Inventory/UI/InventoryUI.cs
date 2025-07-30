using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI1 : MonoBehaviour
{
    public List<InventorySlotUI> inventorySlotsUI;
    public Animator playerAnimator;

    [ContextMenu("load")]
    public void Load()
    {
        for (int i = 1; i < 27; i++)
        {
            inventorySlotsUI.Add(transform.GetChild(0).GetChild(i).GetComponent<InventorySlotUI>());
        }
        for (int i = 1; i < 7; i++)
        {
            inventorySlotsUI.Add(transform.GetChild(1).GetChild(i).GetComponent<InventorySlotUI>());
        }
    }
}
