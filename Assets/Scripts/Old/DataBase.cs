using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DataBase")]
public class DataBase : ScriptableObject
{
    public List<Item> items = new();

}

[System.Serializable]
public class Item
{
    public int id;
    public string name;
    public Sprite sprite;
    public Vector2 size;
    [Space]
    public int buyPriñe;
    public List<int> upgradePrices;
    [Space]
    public ItemType type;
    [Space]
    public Sprite animationSprite;
    [Space(40)]
    public string bonus1Name;
    public float bonus1Value;
    public Sprite bonus1Sprite;
    public List<float> upgrade1Values;
    [Space(10)]
    public string bonus2Name;
    public float bonus2Value;
    public Sprite bonus2Sprite;
    public List<float> upgrade2Values;

    [Space(40)]
    public int cageValue;
    public int damageValue;
    public List<int> damageValues;
    public int speedValue;
    public Sprite cageSprite;
    public Sprite damageSprite;
    public Sprite damagePerSecSprite;
    public Sprite speedSlowdownSprite;
    [Space(10)]
    public float shotTime;
    public float reloadTime;
    public Sprite reloadSprite;
    public Sprite shootSpeedSprite;
    [Space]
    public Vector3 animationPos;
    public Vector3 animationRot;
    public Vector2 animationSize;

}

