using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Money")]
    public TMP_Text moneyText;

    [Space]
    [Header("New Level")]
    public NewLevelUI newItems;

    [Space]
    [Header("exp")]
    public ExpUI expUI;

    private void Awake()
    {
        int money = GameManager.money;
        moneyText.text = StaticDatas._moneyTextFormat(money);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            GameManager.AddExp(50);
        }


    }
}
