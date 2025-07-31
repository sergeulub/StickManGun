using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLoadout", menuName = "PlayerLoadout")]
public class PlayerLoadout : ScriptableObject
{
    public List<int> activeItems = new List<int>(new int[6]);
}