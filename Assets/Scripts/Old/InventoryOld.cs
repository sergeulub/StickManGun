using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryOld : MonoBehaviour
{
    [NonSerialized]
    public int prevCell = 0;
    
    public DataBase data; 
    public PlayerLoadout playerLoadout; // ScriptObj для 6 объектов, которые отправятся в бой

    [SerializeField] TMP_Text _moneyText;

    [Space]
    [SerializeField] Animator animator;
    [SerializeField] Image[] animationBoots;
    [SerializeField] Image animationHat;
    [SerializeField] Image weapon2;
    [Space]
    [SerializeField] GameObject[] itemInfo;
    [Space]
    [SerializeField] public List<int> inventory = new List<int>();// в каждой ячейке свой ID предмета
    [SerializeField] public List<int> arsenal = new List<int>();//1 - если предмет куплен, 0 - если нет
    [SerializeField] public List<int> lvls = new List<int>();


    private Item currentItem;
    private int currentCell = 0;

    private Dictionary<int, string> speedDict = new Dictionary<int, string> { { 1, "Бег" }, { 2, "Быстрая ходьба" }, { 3, "Ходьба" } };

    private void Awake()
    {
        //UpdateInventory();
        //EventManagerOld.OnInventoryOpened += UpdateInventory;
        //EventManagerOld.OnBuyItem += AddItem;
        //EventManagerOld.OnClickItem += ItemInfo;
        //EventManagerOld.OnGamePrepereToBeStarted += PrepareForBattle;
    }
    private void Start()
    {
        var ni = new CultureInfo(CultureInfo.CurrentCulture.Name).NumberFormat;

        ni.NumberDecimalDigits = 0;             //Keep the ".00" from appearing
        ni.NumberGroupSeparator = " ";          //Set the group separator to a space
        ni.NumberGroupSizes = new int[] { 3 };  //Groups of 3 digits

        _moneyText.text = GameManager.money.ToString("N", ni);
    }
    public void UpdateInventory()
    {
        for (int i = 0; i < 32; i++)
        {
            SetItemInCell(i, inventory[i]);
        }
        UpdateArsenal();
        SetAnimation();
    }
    private void SetItemInCell(int cellID, int itemID)
    {
        Item item = data.items[itemID];

        GameObject _child;
        if (cellID < 26)
        {
            _child = transform.GetChild(0).GetChild(0).GetChild(cellID+1).GetChild(0).gameObject;
        }
        else
        {
            _child = transform.GetChild(0).GetChild(1).GetChild(cellID-26+1).GetChild(0).gameObject;
        }
        
        _child.GetComponent<Image>().sprite = item.sprite;
        _child.GetComponent<RectTransform>().sizeDelta = item.size;
        if (item.size.x > 90)
        {
            _child.GetComponent<RectTransform>().sizeDelta = new Vector2(90, (90 *  item.size.y / item.size.x));
            _child.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
    }
    public void AddItem(int price, int index)
    {
        for (int i =0; i<inventory.Count; i++)
        {
            if (inventory[i] == 0)
            {
                inventory[i] = index;
                break;
            }
        }
    }
    public void UpdateArsenal()
    {   
        for (int i = 0; i<inventory.Count; i++)
        {
            arsenal[i] = 0;
        }
        for (int i = 0; i<inventory.Count; i++)
        {
            if (inventory[i] != 0)
            {
                arsenal[inventory[i]] = 1;
            }
        }
    }
    private void SetAnimation()
    {   
        int idWeapon;
        if (inventory[26] != 12)
        {
            idWeapon = inventory[26];
            if (inventory[27] != 12)
            {
                weapon2.gameObject.SetActive(true);
                weapon2.sprite = data.items[inventory[27]].sprite;
                weapon2.gameObject.GetComponent<RectTransform>().localPosition = data.items[inventory[27]].animationPos;
                weapon2.gameObject.GetComponent<RectTransform>().sizeDelta = data.items[inventory[27]].animationSize;
                weapon2.gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(data.items[inventory[27]].animationRot);
                if (data.items[inventory[27]].animationSize.x > 90)
                    weapon2.transform.SetAsFirstSibling();
                else
                    weapon2.transform.SetSiblingIndex(1);
            }
            else
            {
                weapon2.gameObject.SetActive (false);
            }
        }
        else if (inventory[27] != 12)
        {
            idWeapon = inventory[27];
            weapon2.gameObject.SetActive(false);
        }
        else
        {
            idWeapon = 12;
            weapon2.gameObject.SetActive(false);
        }

        animator.SetInteger("id", idWeapon);

        if (inventory[28] != 18) {
            animationBoots[0].sprite = data.items[inventory[28]].animationSprite;
            animationBoots[1].sprite = data.items[inventory[28]].animationSprite;
            animationBoots[0].color = new Color32(255,255,255,255);
            animationBoots[1].color = new Color32(255, 255, 255, 255);
        }
        else 
        {
            animationBoots[0].sprite = null;
            animationBoots[1].sprite = null;
            animationBoots[0].color = new Color32(255, 255, 255, 0);
            animationBoots[1].color = new Color32(255, 255, 255, 0);
        }

        if (inventory[29] != 23)
        {
            animationHat.sprite = data.items[inventory[29]].animationSprite;
            animationHat.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            animationHat.sprite = null;
            animationHat.color = new Color32(255, 255, 255, 0);
        }

    }
    private void ItemInfo(int cellID)
    {   
        Item item = data.items[inventory[cellID]];
        currentItem = item;
        currentCell = cellID;

        if (!item.sprite.name.Contains("empty"))
        {
            itemInfo[0].SetActive(true);

            itemInfo[1].GetComponent<Image>().sprite = item.sprite;
            if (item.size.x < 90)
                itemInfo[1].GetComponent<RectTransform>().sizeDelta = item.size;
            else
            {
                float y = item.size.y * 87 / item.size.x;
                itemInfo[1].GetComponent<RectTransform>().sizeDelta = new Vector2(87, y);
            }

            itemInfo[2].GetComponent<TMP_Text>().text = item.name;

            itemInfo[3].GetComponent<Text>().text = "Ур. " + (lvls[item.id] + 1).ToString();
            if (lvls[item.id] != 9)
            {
                itemInfo[4].GetComponent<TMP_Text>().text = item.upgradePrices[lvls[item.id]].ToString();

                Vector3 pos1 = itemInfo[15].transform.localPosition;
                pos1.x = 3.7f + 5.85f * itemInfo[4].GetComponent<TMP_Text>().text.Length;
                itemInfo[15].transform.localPosition = pos1;

                itemInfo[4].transform.parent.gameObject.GetComponent<Button>().interactable = true;
                if (!GameManager.EnoughMoney(item.upgradePrices[lvls[item.id]]))
                    itemInfo[4].transform.parent.gameObject.GetComponent<Button>().interactable = false;
            }
            else
            {
                itemInfo[4].GetComponent<TMP_Text>().text = "";
                itemInfo[4].transform.parent.gameObject.GetComponent<Button>().interactable = false;
            }

            itemInfo[5].GetComponent<TMP_Text>().text = (item.buyPriсe/2).ToString();

            Vector3 pos2 = itemInfo[16].transform.localPosition;
            pos2.x = 3.7f + 5.85f * itemInfo[5].GetComponent<TMP_Text>().text.Length;
            itemInfo[16].transform.localPosition = pos2;

            if (item.type == ItemType.Weapon)
            {
                for (int i = 8; i < 12; i++)
                {
                    itemInfo[i].SetActive(true);
                }
                itemInfo[7].SetActive(true);
                itemInfo[6].transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = "урон";
                if (item.id != 5)
                {
                    itemInfo[6].transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = item.damageValues[lvls[item.id]].ToString();
                    if (lvls[item.id] != 9)
                        itemInfo[6].transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text = "+" + (item.damageValues[lvls[item.id] + 1] - item.damageValues[lvls[item.id]]).ToString();
                    else
                        itemInfo[6].transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text = "";
                }
                else
                {
                    itemInfo[6].transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = (item.damageValues[lvls[item.id]] * 30).ToString();
                    if (lvls[item.id] != 9)
                        itemInfo[6].transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text = "+" + (item.damageValues[lvls[item.id] + 1] * 30 - (item.damageValues[lvls[item.id]] 
                            * 30)).ToString();
                    else
                        itemInfo[6].transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text = "";
                }
                itemInfo[6].transform.GetChild(5).gameObject.GetComponent<RectTransform>().localPosition = 
                    new Vector3(-9.45f + itemInfo[6].transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text.Length * 9.1f, -4.2228f, 0);
                itemInfo[6].transform.GetChild(4).gameObject.GetComponent<Image>().sprite = item.damageSprite;

                itemInfo[7].transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = "урон в секунду";
                if (item.id != 5)
                {
                    itemInfo[7].transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = ((int)(item.damageValues[lvls[item.id]] / item.shotTime)).ToString();
                    if (lvls[item.id] != 9)
                        itemInfo[7].transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text = "+" + ((int)((item.damageValues[lvls[item.id] + 1]/item.shotTime) - 
                            (int)(item.damageValues[lvls[item.id]]/item.shotTime))).ToString();
                    else
                        itemInfo[7].transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text = "";
                }
                else
                {
                    itemInfo[7].transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = ((int)((item.damageValues[lvls[item.id]]) / item.shotTime * 30)).ToString();
                    if (lvls[item.id] != 9)
                        itemInfo[7].transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text = "+" + ((int)((item.damageValues[lvls[item.id] + 1] / item.shotTime * 30) -
                            (int)(item.damageValues[lvls[item.id]] / item.shotTime * 30))).ToString();
                    else
                        itemInfo[7].transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text = "";
                }
                itemInfo[7].transform.GetChild(5).gameObject.GetComponent<RectTransform>().localPosition = 
                    new Vector3(-9.45f + itemInfo[7].transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text.Length * 9.1f, -4.2228f, 0);
                itemInfo[7].transform.GetChild(4).gameObject.GetComponent<Image>().sprite = item.damagePerSecSprite;

                itemInfo[8].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "перезарядка";
                string reloadSpeedStr;
                if (item.reloadTime > 2.5f)
                    reloadSpeedStr = "медленная";
                else if (item.reloadTime > 1.75f)
                    reloadSpeedStr = "нормальная";
                else if (item.reloadTime > 1f)
                    reloadSpeedStr = "быстрая";
                else
                    reloadSpeedStr = "очень быстрая";
                itemInfo[8].transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = reloadSpeedStr;
                itemInfo[8].transform.GetChild(3).gameObject.GetComponent<Image>().sprite = item.reloadSprite;

                itemInfo[9].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "передвижение";
                itemInfo[9].transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = speedDict[item.speedValue];
                itemInfo[9].transform.GetChild(3).gameObject.GetComponent<Image>().sprite = item.speedSlowdownSprite;

                itemInfo[10].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "обойма";
                itemInfo[10].transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = item.cageValue.ToString();
                itemInfo[10].transform.GetChild(3).gameObject.GetComponent<Image>().sprite = item.cageSprite;

                itemInfo[11].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "скорость стрельбы";
                string shootSpeedStr;
                if (item.shotTime > 0.25f)
                    shootSpeedStr = "медленная";
                else if (item.shotTime > 0.13f)
                    shootSpeedStr = "нормальная";
                else if (item.shotTime > 0.06f)
                    shootSpeedStr = "быстрая";
                else
                    shootSpeedStr = "очень быстрая";
                itemInfo[11].transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = shootSpeedStr;
                itemInfo[11].transform.GetChild(3).gameObject.GetComponent<Image>().sprite = item.shootSpeedSprite;
            }
            else
            {
                for (int i = 8; i < 12; i++)
                {
                    itemInfo[i].SetActive(false);

                    itemInfo[6].transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = item.bonus1Name;
                    string sign1;
                    if (item.bonus1Value < 0f)
                        sign1 = "-";
                    else
                        sign1 = "";
                    if (lvls[item.id] != 9)
                    {
                        itemInfo[6].transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = sign1 + (Math.Round(item.upgrade1Values[lvls[item.id]] * 100f,1)).ToString() + "%";
                        itemInfo[6].transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text =
                            "+" + Math.Round((item.upgrade1Values[lvls[item.id] + 1] - item.upgrade1Values[lvls[item.id]])*100f,1).ToString();
                    }
                    else
                    {
                        itemInfo[6].transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = sign1 + (Math.Round(item.upgrade1Values[lvls[item.id]] * 100f, 1)).ToString() + "%";
                        itemInfo[6].transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text = "";
                    }
                    itemInfo[6].transform.GetChild(5).gameObject.GetComponent<RectTransform>().localPosition =
                            new Vector3(-7f + itemInfo[6].transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text.Length * 9.1f, -4.2228f, 0);
                    itemInfo[6].transform.GetChild(4).gameObject.GetComponent<Image>().sprite = item.bonus1Sprite;

                    if (item.bonus2Name != "")
                    {
                        itemInfo[7].SetActive(true);
                        itemInfo[7].transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = item.bonus2Name;
                        string sign2;
                        if (item.bonus2Value < 0f)
                            sign2 = "";
                        else
                            sign2 = "";
                        if (lvls[item.id] != 9)
                        {
                            itemInfo[7].transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = sign2 + (Math.Round(item.upgrade2Values[lvls[item.id]] * 100f, 1)).ToString() + "%";
                            if (Math.Round((item.upgrade2Values[lvls[item.id] + 1] - item.upgrade2Values[lvls[item.id]]) * 100f, 1) != 0)
                                itemInfo[7].transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text =
                                "+" + Math.Round((item.upgrade2Values[lvls[item.id] + 1] - item.upgrade2Values[lvls[item.id]]) * 100f, 1).ToString();
                            else
                                itemInfo[7].transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text = "";
                        }
                        else
                        {
                            itemInfo[7].transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = sign1 + (Math.Round(item.upgrade2Values[lvls[item.id]] * 100f, 1)).ToString() + "%";
                            itemInfo[7].transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text = "";
                        }
                        itemInfo[7].transform.GetChild(5).gameObject.GetComponent<RectTransform>().localPosition =
                            new Vector3(-7f + itemInfo[7].transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text.Length * 9.1f, -4.2228f, 0);
                        itemInfo[7].transform.GetChild(4).gameObject.GetComponent<Image>().sprite = item.bonus2Sprite;
                    }
                    else
                    {
                        itemInfo[7].SetActive(false);
                    }
                }
            }

        }

    }
    public void _UpgradeСonfirm()
    {
        itemInfo[12].SetActive(true);
        itemInfo[13].SetActive(false);
        itemInfo[14].SetActive(true);
        itemInfo[14].transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>().text = itemInfo[4].GetComponent<TMP_Text>().text;

        Vector3 pos2 = itemInfo[17].transform.localPosition;
        pos2.x = 3.7f + 5.85f * itemInfo[4].GetComponent<TMP_Text>().text.Length;
        itemInfo[17].transform.localPosition = pos2;
    }
    public void _SellСonfirm()
    {
        itemInfo[12].SetActive(true);
        itemInfo[13].SetActive(true);
        itemInfo[14].SetActive(false);
        itemInfo[13].transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>().text = itemInfo[5].GetComponent<TMP_Text>().text;

        Vector3 pos2 = itemInfo[18].transform.localPosition;
        pos2.x = 3.7f + 5.85f * itemInfo[5].GetComponent<TMP_Text>().text.Length;
        itemInfo[18].transform.localPosition = pos2;
    }
    public void _Sell()
    {
        if (currentCell < 26)
            inventory[currentCell] = 0;
        else if (currentCell == 26 || currentCell == 27)
            inventory[currentCell] = 12;
        else if (currentCell == 28)
            inventory[currentCell] = 18;
        else if (currentCell == 29)
            inventory[currentCell] = 23;
        else if (currentCell == 30 || currentCell == 31)
            inventory[currentCell] = 30;
        UpdateInventory();
        itemInfo[12].SetActive(false);
        itemInfo[0].SetActive(false);
        GameManager.AddMoney(currentItem.buyPriсe / 2);

        var ni = new CultureInfo(CultureInfo.CurrentCulture.Name).NumberFormat;

        ni.NumberDecimalDigits = 0;             //Keep the ".00" from appearing
        ni.NumberGroupSeparator = " ";           //Set the group separator to a space
        ni.NumberGroupSizes = new int[] { 3 };  //Groups of 3 digits

        _moneyText.text = GameManager.money.ToString("N", ni);
    }
    public void _Upgrade()
    {
        if (lvls[currentItem.id] != 9)
        {
            if (GameManager.EnoughMoney(currentItem.upgradePrices[lvls[currentItem.id]]))
                {
                GameManager.DecreaseMoney(-currentItem.upgradePrices[lvls[currentItem.id]]);
                lvls[currentItem.id]++;
                itemInfo[12].SetActive(false);
                ItemInfo(currentCell);

                var ni = new CultureInfo(CultureInfo.CurrentCulture.Name).NumberFormat;

                ni.NumberDecimalDigits = 0;             //Keep the ".00" from appearing
                ni.NumberGroupSeparator = " ";           //Set the group separator to a space
                ni.NumberGroupSizes = new int[] { 3 };  //Groups of 3 digits

                _moneyText.text = GameManager.money.ToString("N", ni);
            }
            else
            {
                Debug.Log("Не достаточно денег");
            }
        }
    }
    
    private void PrepareForBattle()
    {
        for (int i = 0; i < 6; i++)
        {
            playerLoadout.activeItems[i] = inventory[26 + i];
            Debug.Log($"Герой берёт с собой {data.items[playerLoadout.activeItems[i]].name}");
        }
        Debug.Log($"В бой!");
        
    }
}