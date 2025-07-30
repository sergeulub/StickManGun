using System;
using System.Collections.Generic;

public static class EventManager
{
    private static Dictionary<string, Delegate> eventTable = new();

    public static void Subscribe<T>(string eventName, Action<T> listener)
    {
        if (eventTable.TryGetValue(eventName, out var del))
            eventTable[eventName] = Delegate.Combine(del, listener);
        else
            eventTable[eventName] = listener;
    }

    public static void Unsubscribe<T>(string eventName, Action<T> listener)
    {
        if (eventTable.TryGetValue(eventName, out var del))
        {
            del = Delegate.Remove(del, listener);
            if (del == null) eventTable.Remove(eventName);
            else eventTable[eventName] = del;
        }
    }

    public static void Trigger<T>(string eventName, T arg)
    {
        if (eventTable.TryGetValue(eventName, out var del))
        {
            var callback = del as Action<T>;
            callback?.Invoke(arg);
        }
    }
    public static void Subscribe(string eventName, Action listener)
    {
        if (eventTable.TryGetValue(eventName, out var del))
            eventTable[eventName] = Delegate.Combine(del, listener);
        else
            eventTable[eventName] = listener;
    }

    public static void Unsubscribe(string eventName, Action listener)
    {
        if (eventTable.TryGetValue(eventName, out var del))
        {
            del = Delegate.Remove(del, listener);
            if (del == null) eventTable.Remove(eventName);
            else eventTable[eventName] = del;
        }
    }
    public static void Trigger(string eventName)
    {
        if (eventTable.TryGetValue(eventName, out var del))
        {
            var callback = del as Action;
            callback?.Invoke();
        }
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