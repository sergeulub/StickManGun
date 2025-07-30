using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployablesMenu : MonoBehaviour
{
    [Header("References")]
    public Info info;


    [Space]
    [Header("UI")]
    public List<DeployableItemCellUI> cells;
    public DeployableItemInfoUI itemInfoUI;
    public MainMenuUI mainMenuUI;

    private List<Deployables> deployablesInfo => info.deployables;

    private void OnEnable()
    {
        EventManager.Subscribe(GameEvents.ArtifactsOpened, LoadDeployables);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.ArtifactsOpened, LoadDeployables);
    }

    public void _LoadDeployableInfo(int ID)
    {
        LoadDeployableInfo(ID);
    }
    public void _Confirm()
    {
        Confirm();
    }
    public void _Upgrade()
    {
        Upgrade();
    }
    private void LoadDeployables()
    {
        for (int i = 0; i < StaticDatas._deployableCount; i++)
        {
            deployablesInfo[i].LoadDeployablesInfo(cells[i]);
        }
    }
    private void LoadDeployableInfo(int ID)
    {
        deployablesInfo[ID].LoadDeployablesItemInfo(itemInfoUI);
    }
    private void Confirm()
    {
        ConfirmArtefactsUI confirm = itemInfoUI.confirmUI;

        confirm.otherUpgrade.SetActive(false);
        confirm.gameObject.SetActive(true);

        confirm.upgradePriceText.text = itemInfoUI.upgradePriceText.text;
        confirm.upgradeCoinIcon.transform.localPosition = itemInfoUI.upgradeCoinIcon.transform.localPosition;
    }
    private void Upgrade()
    {
        int currentID = itemInfoUI.itemInfo.id;
        deployablesInfo[currentID].UpgradeItem();
        LoadDeployableInfo(currentID);
        LoadDeployables();
        mainMenuUI.moneyText.text = GameManager.money.ToString();
    }
}
