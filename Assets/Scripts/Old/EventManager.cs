using UnityEngine;
using System;

public class EventManagerOld : MonoBehaviour
{
    public static Action OnShopOpened;
    public static void SendShopOpened()
    {
        OnShopOpened?.Invoke();
    }

    public static Action OnInventoryOpened;
    public static void SendInventoryOpened()
    {
        OnInventoryOpened?.Invoke();
    }

    public static Action OnGamePrepereToBeStarted;
    public static void SendGamePrepereToBeStarted()
    {
        OnGamePrepereToBeStarted?.Invoke();
    }

    public static Action<int, int> OnBuyItem;
    public static void SendBuyItem(int money, int index)
    {
        OnBuyItem?.Invoke(money, index);
    }
    public static Action<int, int> OnWantBuyItem;
    public static void SendWantBuyItem(int money, int index)
    {
        OnWantBuyItem?.Invoke(money, index);
    }

    public static Action<int> OnClickItem;
    public static void SendClickItem(int cellID)
    {
        OnClickItem?.Invoke(cellID);
    }

    public static Action OnArtifactsOpened;
    public static void SendArtifactsOpened()
    {
        OnArtifactsOpened?.Invoke();
    }
}
