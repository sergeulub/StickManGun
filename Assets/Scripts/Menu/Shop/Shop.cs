using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header ("References")]
    public Info info;
    public Inventory inventory;
    public MenuSprites sprites;
    [SerializeField] private Header[] headers;

    [Header("UI")]
    public ShopCellUI[] cellsUI;
    public ShopItemInfoUI itemInfoUI;
    public ConfirmUI confirmUI;
    public MainMenuUI mainMenuUI;


    private int header;
    private int currentID;
    private List<ItemInfo> itemInfos;

    private void OnEnable()
    {
        EventManager.Subscribe(GameEvents.ShopOpened, LoadShop); 
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.ShopOpened, LoadShop);
    }
    private void Awake()
    {
        //загрузка данных
        itemInfos = info.GetAllItems();
    }
    public void _ChangeHeader(int headerIndex)
    {
        header = headerIndex;
        LoadHeader(header);
    }
    public void _BuyItem(int _indexItem)
    {
        switch (header)
        {
            case 0: currentID = StaticDatas._firstWeaponID + _indexItem; break;
            case 1: currentID = StaticDatas._firstBootsID + _indexItem; break;
            case 2: currentID = StaticDatas._firstHatID + _indexItem; break;
            case 3: currentID = StaticDatas._firstRingID + _indexItem; break;
        }

        LoadItemInfo(currentID);
    }
    public void _Confirm()
    {
        OpenConfirmWindow();
    }
    public void _Sell()
    {
        Sell();
    }
    public void _Buy()
    {
        Buy();
    }
    private void LoadShop()
    {
        header = 0;
        LoadHeader(header);
    }
    private void LoadHeader(int headerID)
    {
        //проигрывание анимаций
        for (int i = 0; i < 4; i++)
        {
            headers[i]._animation.clip = headers[i]._clips[1];
            headers[i]._animation.Play();
        }
        headers[header]._animation.clip = headers[header]._clips[0];
        headers[header]._animation.Play();
        headers[header]._transform.SetAsLastSibling();

        //логика выгрузки информации в UI

        Dictionary<int, int> headerLength = new Dictionary<int, int>() { {0, 12}, {1, 6}, {2, 5}, {3, 7} };

        //активайция нужных окошек
        for (int i = 0; i < cellsUI.Length; i++)
        {
            cellsUI[i].gameObject.SetActive(true);
        }

        for (int i = headerLength[headerID]-1; i < cellsUI.Length; i++)
        {
            cellsUI[i].gameObject.SetActive(false);
        }

        //рассчеты позиций ID
        int startID = 0;
        int endID = 0;
        switch (headerID)
        {
            case 0: startID = StaticDatas._firstWeaponID; endID = StaticDatas._firstBootsID - 1; break;
            case 1: startID = StaticDatas._firstBootsID; endID = StaticDatas._firstHatID - 1; break;
            case 2: startID = StaticDatas._firstHatID; endID = StaticDatas._firstRingID - 1; break;
            case 3: startID = StaticDatas._firstRingID; endID = StaticDatas._itemsQuantity; break;
        }

        for (int i = startID; i < endID; i++)
        {
            ShopCellUI cell = cellsUI[i - startID];

            itemInfos[i].LoadShopInfo(cell, sprites);
        }
    }
    private void LoadItemInfo(int cellIndex)
    {
        itemInfos[cellIndex].LoadShopItemInfo(itemInfoUI, sprites);        
    }
    private void OpenConfirmWindow()
    {   
        itemInfos[currentID].Confirm(confirmUI);
    }
    private void Sell()
    {
        itemInfos[currentID].SellItem();

        confirmUI.gameObject.SetActive(false);
        itemInfoUI.gameObject.SetActive(false);

        LoadHeader(header);

        mainMenuUI.moneyText.text = StaticDatas._moneyTextFormat(GameManager.money);
    }
    private void Buy()
    {
        itemInfos[currentID].BuyItem();

        confirmUI.gameObject.SetActive(false);
        itemInfoUI.gameObject.SetActive(false);

        LoadHeader(header);

        mainMenuUI.moneyText.text = StaticDatas._moneyTextFormat(GameManager.money);
    }
}


[Serializable]
public class Header
{
    public Transform _transform;
    public Animation _animation;
    public AnimationClip[] _clips;
}