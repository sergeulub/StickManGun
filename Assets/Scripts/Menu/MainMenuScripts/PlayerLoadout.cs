using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLoadout", menuName = "PlayerLoadout")]
public class PlayerLoadout : ScriptableObject
{
    // То, что реально идёт в бой (экипировка)
    public List<int> activeItems = new List<int>(6);

    // Прокачка бонусов (itemID -> level)
    public Dictionary<int, int> bonusLevels = new Dictionary<int, int>();

    // Прокачка deployables (itemID -> level)
    public Dictionary<int, int> deployableLevels = new Dictionary<int, int>();

    public void Clear()
    {
        activeItems.Clear();
        bonusLevels.Clear();
        deployableLevels.Clear();
    }
}