using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisual : MonoBehaviour
{
    [Header("Equipment Images")]
    public Image hatImage;
    public Image[] bootsImage;

    [Header("Animator")]
    public Animator playerAnimator;

    [Header("Info)")]
    public Info info;

    private void Awake()
    {
        UpdateVisual();
    }
    public void UpdateHat(Sprite sprite)
    {
        hatImage.sprite = sprite;
        if (sprite != null)
        {
            hatImage.rectTransform.localScale = Vector3.one;
        }
        else
        {
            hatImage.rectTransform.localScale = Vector3.zero;
        }
    }

    public void UpdateBoots(Sprite sprite)
    {
        bootsImage[0].sprite = sprite;
        bootsImage[1].sprite = sprite;
        if (sprite != null)
        {
            bootsImage[0].rectTransform.localScale = Vector3.one;
            bootsImage[1].rectTransform.localScale = Vector3.one;
        }
        else
        {
            bootsImage[0].rectTransform.localScale = Vector3.zero;
            bootsImage[1].rectTransform.localScale = Vector3.zero;
        }
    }

    public void UpdateWeapon(int weaponID)
    {
        playerAnimator.SetInteger("ID", weaponID);
    }
    public void UpdateVisual()
    {
        List<int> inventory = GameManager.InventoryData.slotItemIDs;
        List<ItemInfo> allItems = info.GetAllItems();

        // Ўлем Ч ID 29
        int hatID = inventory[StaticDatas._hatID];
        if (hatID != -1)
        {
            Hat hat = allItems[hatID] as Hat;
            if (hat != null)
                UpdateHat(hat.animationSprite);
        }
        else
        {
            UpdateHat(null);
        }

        // Ѕотинки Ч ID 28
        int bootsID = inventory[StaticDatas._bootsID];
        if (bootsID != -1)
        {
            Boots boots = allItems[bootsID] as Boots;
            if (boots != null)
                UpdateBoots(boots.animationSprite);
        }
        else
        {
            UpdateBoots(null);
        }

        // ќружие Ч ID 26 или 27
        int weaponID = inventory[StaticDatas._weaponID1];
        if (weaponID == StaticDatas._emptyID)
            weaponID = inventory[StaticDatas._weaponID2];

        if (weaponID != StaticDatas._emptyID)
        {
            Weapons weapon = allItems[weaponID] as Weapons;
            if (weapon != null)
            {
                UpdateWeapon(weapon.id);
            }
        }
        else
        {
            UpdateWeapon(StaticDatas._emptyID); // -1: голые руки
        }
    }
}
