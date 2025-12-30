using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public PlayerLoadout playerLoadout;

    void Start()
    {
        List<int> itemsToSpawn = playerLoadout.activeItems;
        // тут логика спауна оружия, брони и т.д.
        
    }
}
