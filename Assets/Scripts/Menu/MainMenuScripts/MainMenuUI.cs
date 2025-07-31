using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Money")]
    public TMP_Text moneyText;

    [Space]
    [Header("New Level")]
    public int newItems;

    private void Awake()
    {
        int money = GameManager.money;
        moneyText.text = StaticDatas._moneyTextFormat(money);
    }
}
