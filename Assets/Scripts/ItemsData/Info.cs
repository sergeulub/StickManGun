using Bonuses;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsInformation")]
public class Info : ScriptableObject
{
    public List<Weapons> weapons;
    public List<Boots> boots;
    public List<Hat> hats;
    public List<Ring> rings;
    public List<Deployables> deployables;
    public List<Boosts> boosts;
    public ItemInfo emptyItem;


    public List<ItemInfo> _allItems;

    public List<ItemInfo> GetAllItems()
    {   
        _allItems.Clear();
        if (_allItems == null || _allItems.Count == 0)
        {
            _allItems.AddRange(weapons);
            _allItems.AddRange(boots);
            _allItems.AddRange(hats);
            _allItems.AddRange(rings);
            _allItems.Add(emptyItem);
        }
        return _allItems;
    }
    [ContextMenu("load")]
    public void Load()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].id = i;
        }
        for (int i = weapons.Count; i < weapons.Count + boots.Count; i++)
        {
            boots[i - weapons.Count].id = i;
        }
        for (int i = weapons.Count + boots.Count ; i < weapons.Count + boots.Count  + hats.Count; i++)
        {
            hats[i - (weapons.Count + boots.Count)].id = i;
        }
        for (int i = weapons.Count + boots.Count + hats.Count; i < weapons.Count + boots.Count + hats.Count + rings.Count; i++)
        {
            rings[i - (weapons.Count + boots.Count + hats.Count)].id = i;
        }
    }
}
