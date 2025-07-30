using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoostsMenu : MonoBehaviour
{
    [Header("References")]
    public Info info;

    [Space]
    [Header ("UI")]
    public List<BoostCellUI> cells;
    public BoostItemInfoUI itemInfoUI;
    public TMP_Text pointsText;

    private List<Boosts> boostsInfo => info.boosts;

    private void OnEnable()
    {
        EventManager.Subscribe(GameEvents.ArtifactsOpened, LoadBoosts);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.ArtifactsOpened, LoadBoosts);
    }

    public void _LoadBoostInfo(int ID)
    {
        LoadBoostInfo(ID);
    }
    public void _Confirm()
    {
        Confirm();
    }
    public void _Upgrade()
    {
        Upgrade();
    }
    private void LoadBoosts()
    {
        for (int i = 0; i < StaticDatas._boostsCount; i++)
        {
            boostsInfo[i].LoadInfo(cells[i]);
        }
        pointsText.text = GameManager.boostPoint.ToString();
    }
    private void LoadBoostInfo(int ID)
    {
        boostsInfo[ID].LoadItemInfo(itemInfoUI);
    }
    private void Confirm()
    {
        ConfirmArtefactsUI confirm = itemInfoUI.confirmUI;
        confirm.otherUpgrade.SetActive(false);
        confirm.gameObject.SetActive(true);
    }
    private void Upgrade()
    {
        int currentID = itemInfoUI.itemInfo.id;
        boostsInfo[currentID].UpgradeItem();
        LoadBoostInfo(currentID);
        LoadBoosts();
    }
}
