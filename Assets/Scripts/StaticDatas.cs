using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class StaticDatas
{
    #region Pistol
    public static float _pistolFunctionByDistanceRight()
    {
        return -110f;
    }
    public static float _pistolFunctionByDistanceLeft()
    {
        return 0f;
    }
    #endregion

    #region PP
    public static float _ppFunctionByDistanceRight()
    {
        return 55f;
    }
    public static float _ppFunctionByDistanceLeft()
    {
        return 0f;
    }
    #endregion

    #region P90
    public static float _p90FunctionByDistanceRight()
    {
        return 47f;
    }
    public static float _p90FunctionByDistanceLeft()
    {
        return 0f;
    }
    #endregion

    #region Shotgun
    public static float _shotgunFunctionByDistanceRight()
    {
        return -10f;
    }
    public static float _shotgunFunctionByDistanceLeft()
    {
        return 0f;
    }
    #endregion

    #region Minigun
    public static float _minigunRangeDeltaY = 0.06f;
    public static float _minigunFunctionByDistanceLeft(float x)
    {
        if (x != 0.22155f)
            return 35.3864f / (x - 0.22155f) - 3.22209f;
        else
            return 35.3864f / (0.23f - 0.22155f) - 3.22209f;
    }
    public static float _minigunFunctionByDistanceRight(float x)
    {
        if (x != 0.32535f)
            return -(32.2596f / (x - 0.32535f) + 17.96415f);
        else
            return - (32.2596f / (0.326f - 0.32535f) + 17.96415f);
    }
    #endregion

    #region Rifle
    public static float _rifleFunctionByDistanceRight(float x)
    {
        return 10.3f - (12f / x);
    }
    public static float _rifleFunctionByDistanceLeft(float x)
    {
        return -0.0048605126f * x * x * x * x * x
             + 0.14880632f * x * x * x * x
             - 1.7186733f * x * x * x
             + 9.4107482f * x * x
             - 25.188374f * x
             + 28.692476f;
    }
    #endregion

    #region Sniper
    public static float _sniperFunctionByDistance(float x)
    {
        return 0.01717167f * x * x * x * x
             - 0.40057063f * x * x * x
             + 3.0596866f * x * x
             - 9.0751030f * x
             + 9.4453944f;
    }
    #endregion

    #region Weapon
    public static float _bulletLifeTime = 0.05f;
    public static float _scatterMaxValue = 45f;
    public static float _minAimDistance = 1.5f;
    #endregion

    #region AimController
    public static float _armDeltaRotation = 129f;
    #endregion

    #region ItemInfo
    public static string _buyText = "������";
    public static string _sellText = "�������";
    public static float _coinDelta(int length)
    {
        return 6f + 5.85f * length;
    }
    public static float _upgradeTextDelta(int length, bool isPercentage)
    {
        float extraX = isPercentage ? 10f : 0f;
        return -2f + extraX + 5.85f * length;
    }
    public const int EMPTY_SLOT = -1;

    #endregion

    #region Shop
    public static int _sellPrice(int buyPrice)
    {
        return Mathf.RoundToInt(buyPrice / 2);
    }
    public static string _moneyTextFormat(int money)
    {
        var format = new CultureInfo(CultureInfo.CurrentCulture.Name).NumberFormat;
        format.NumberDecimalDigits = 0;             //Keep the ".00" from appearing
        format.NumberGroupSeparator = " ";           //Set the group separator to a space
        format.NumberGroupSizes = new int[] { 3 };  //Groups of 3 digits

        return money.ToString("N", format);
    }
    public static int _firstWeaponID = 1;
    public static int _firstBootsID = 13;
    public static int _firstHatID = 19;
    public static int _firstRingID = 24;
    public static int _itemsQuantity = 30;
    #endregion

    #region Inventory
    public static int _inventoryLength = 32;
    public static int _quantityItems = 30;
    public static int _weaponID1 = 26;
    public static int _weaponID2 = 27;
    public static int _bootsID = 28;
    public static int _hatID = 29;
    public static int _ringID1 = 30;
    public static int _ringID2 = 31;
    public static int _emptyID = -1;
    #endregion

    #region Deployables
    public static int _deployableCount = 4;
    public static float _valueByLevel(int level, float upgradeValue)
    {
        return upgradeValue * level;
    }
    public static float _upgradeDeployablesTextDelta(int length)
    {
        return -9f + 7.9f * length;
    }
    public static int _upgradePriceByLevel(int level, int buyPrice)
    {
        return buyPrice * (level + 1);
    }
    #endregion

    #region Boosts
    public static int _boostsCount = 8;
    #endregion

    #region GameManager
    public static List<int> expList = new List<int>() 
    {
        100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400
    };
    #endregion
}
