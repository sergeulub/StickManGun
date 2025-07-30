using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public TMP_Text moneyText;

    private void Awake()
    {
        int money = GameManager.money;
        moneyText.text = StaticDatas._moneyTextFormat(money);
    }
}
