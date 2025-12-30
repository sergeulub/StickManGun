using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public MainMenuUI menuUI;
    public Info info;

    private List<ItemInfo> itemInfos;

    private void OnEnable()
    {
        EventManager.Subscribe(GameEvents.LevelUp, LevelUP);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.LevelUp, LevelUP);
    }
    private void Awake()
    {
        itemInfos = info.GetAllItems();
    }
    private void LevelUP()
    {
        showNewItemsWindow();
    }

    private void showNewItemsWindow()
    {
        menuUI.newItems.window.SetActive(true);
        menuUI.newItems.blackScreen.SetActive(true);

        menuUI.newItems.levelText.text = GameManager.playerLevel.ToString();

        int cellID = 0;
        for (int i = 0; i < itemInfos.Count; i++)
        {
            Debug.Log(i + ": " + cellID);
            cellID += itemInfos[i].LevelUpLoad(menuUI.newItems.newItemsImage[cellID]);
        }
    }
}
