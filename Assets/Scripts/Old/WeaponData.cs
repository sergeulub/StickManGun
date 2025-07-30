using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/WeaponData")]
public class WeaponData : ScriptableObject
{
    public WeaponType weaponType;

    [Space(40)]
    public int cageValue;
    public int damageValue;
    public List<int> damageValues;

    [Space(40)]
    public float flightLength;
    public float scatterValue;
    public int speedValue;
    public Vector2 firePointLocalPos;
    public float _weaponDeltaAngle;
    public float _weaponDeltaAngleByDistance;
    public GameObject flamePrefab;//только для огнемёта

    [Space(10)]
    public float shotTime;
    public float reloadTime;
}