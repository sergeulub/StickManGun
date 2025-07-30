using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Minas
{
    public class Minas : MonoBehaviour
    {
        [SerializeField] Item _item;
        [SerializeField] List<MinaIcon> minas;

        [Space]
        [SerializeField] List<int> minasLvls = new List<int>() { 0, 1, 2, 3 };

        [Space]
        [SerializeField] List<Mina> minasInfo;
        [Space]
        [SerializeField] TMP_Text _moneyText;


        private int curIndex;
        private void Awake()
        {
            EventManagerOld.OnArtifactsOpened += Load;
        }
        private void Load()
        {
            for (int i = 0; i < 4; i++)
            {
                MinaIcon mina = minas[i];
                mina.image.sprite = minasInfo[i].sprite;
                mina.rectTransform.sizeDelta = minasInfo[i].size;
                mina.rectTransform.localPosition = minasInfo[i].pos;
                mina.name.text = minasInfo[i].name;
                mina.action.text = "улучшить";
                if (minasLvls[i] != -1)
                {
                    mina.prizeGO.SetActive(true);
                    int prize = minasInfo[i].priceFirstUpdrade * (minasLvls[i] + 1);

                    mina.prize.text = prize.ToString();

                    Vector3 pos = mina.coinImageTransform.localPosition;
                    pos.x = 5.15f + 5.85f * prize.ToString().Length;
                    mina.coinImageTransform.localPosition = pos;

                    mina.lockGO.SetActive(false);
                    Color color = mina.fon.color;
                    color.a = 1f;
                    mina.fon.color = color;
                }
                else
                {
                    mina.prizeGO.SetActive(false);

                    Color color = mina.fon.color;
                    color.a = 0.6f;
                    mina.fon.color = color;

                    mina.lockGO.SetActive(true);
                    mina.lockText.text = "Откроется на " + minasInfo[i].requiredLvl + " Ур.";
                }
            }
        }
        public void _OpenInfoMinas(int index)
        {   
            curIndex = index;

            _item.go.SetActive(true);
            _item.upgradeMine.SetActive(true);
            _item.upgradeBonus.SetActive(false);
            _item.image.sprite = minasInfo[index].sprite;
            
            if (minasInfo[index].size.y < 76)
                _item.rectTransformImage.sizeDelta = minasInfo[index].size;
            else
            {
                float x = minasInfo[index].size.x * 75 / minasInfo[index].size.y;
                _item.rectTransformImage.sizeDelta = new Vector2(x, 75);
            }
            _item.name.text = minasInfo[index].name;
            _item.lvl.text = "Ур. " + (minasLvls[index] + 1).ToString();
            _item.descriptionText.text = minasInfo[index].description;
            _item.upgradeGO.SetActive(true);

            int prize = (minasInfo[index].priceFirstUpdrade * (minasLvls[index] + 1));
            _item.prizeText.text = prize.ToString();
            _item.confirmTextPrize.text = prize.ToString();

            Vector3 pos = _item.coinImageTransform.localPosition;
            pos.x = 3.7f + 5.85f * prize.ToString().Length;
            _item.coinImageTransform.localPosition = pos;
            _item.confirmCoinImageTransform.localPosition = pos;


            if (GameManager.EnoughMoney(minasInfo[curIndex].priceFirstUpdrade * (minasLvls[curIndex] + 1)))
            {
                _item.upgradeButton.interactable = true;
            }
            else
            {
                _item.upgradeButton.interactable = false;
            }

            _item.bonusOne.bonusImage.sprite = minasInfo[index].sprites[0];
            _item.bonusOne.bonusNameText.text = minasInfo[index].names[0];
            _item.bonusOne.bonusValueText.text = Math.Round(minasInfo[index].bonusFirst + minasInfo[index].bonusUpdrade * minasLvls[index], 3).ToString();
            _item.bonusOne.bonusUpgradeRectTransform.localPosition = new Vector3(-3.4f -7.8f + 7.8f * _item.bonusOne.bonusValueText.text.Length, -3.2595f, 0);
            _item.bonusOne.bonusUpgradeText.text = "+" + (minasInfo[index].bonusUpdrade).ToString();

            _item.bonusTwo.gameObject.SetActive(false);
            if (index == 3)
            {
                _item.bonusTwo.gameObject.SetActive(true);
                _item.bonusTwo.bonusImage.sprite = minasInfo[index].sprites[1];
                _item.bonusTwo.bonusNameText.text = minasInfo[index].names[1];
                _item.bonusTwo.bonusValueText.text = ((int)minasInfo[index].duration).ToString() + " сек.";
            }
        }
        public void _Upgrade()
        {
            _item.confirmGO.SetActive(false);
            GameManager.money -= minasInfo[curIndex].priceFirstUpdrade * (minasLvls[curIndex] + 1);

            var ni = new CultureInfo(CultureInfo.CurrentCulture.Name).NumberFormat;

            ni.NumberDecimalDigits = 0;             //Keep the ".00" from appearing
            ni.NumberGroupSeparator = " ";           //Set the group separator to a space
            ni.NumberGroupSizes = new int[] { 3 };  //Groups of 3 digits

            _moneyText.text = GameManager.money.ToString("N", ni);


            minasLvls[curIndex]++;
            _OpenInfoMinas(curIndex);
        }
    }
    [Serializable]
    public class Mina
    {
        public Sprite sprite;
        public Vector2 size;
        public Vector2 pos;
        public string description;
        public string name;
        public float bonusUpdrade;
        public float bonusFirst;
        public int priceFirstUpdrade;
        public int requiredLvl;
        public Sprite[] sprites;
        public List<string> names;

        public float duration;
    }
    [Serializable]
    public class MinaIcon
    {
        [field: SerializeField] public Image image;
        [field: SerializeField] public RectTransform rectTransform;
        [field: SerializeField] public TMP_Text name;
        [field: SerializeField] public TMP_Text prize;
        [field: SerializeField] public GameObject prizeGO;
        [field: SerializeField] public Text action;
        [field: SerializeField] public Image fon;
        [field: SerializeField] public GameObject lockGO;
        [field: SerializeField] public TMP_Text lockText;
        [SerializeField] public RectTransform coinImageTransform;
    }
    [Serializable]
    public class Item
    {
        [SerializeField] public GameObject go;
        [SerializeField] public RectTransform rectTransformImage;
        [SerializeField] public Image image;
        [SerializeField] public TMP_Text name;
        [SerializeField] public Text lvl;
        [SerializeField] public Bonus bonusOne;
        [SerializeField] public Bonus bonusTwo;
        [SerializeField] public TMP_Text descriptionText;
        [SerializeField] public TMP_Text prizeText;
        [SerializeField] public TMP_Text confirmTextPrize;
        [SerializeField] public Button upgradeButton;
        [SerializeField] public GameObject confirmGO;
        [SerializeField] public GameObject upgradeGO;
        [SerializeField] public GameObject upgradeBonus;
        [SerializeField] public GameObject upgradeMine;
        [SerializeField] public RectTransform coinImageTransform;
        [SerializeField] public RectTransform confirmCoinImageTransform;
    }
    [Serializable]
    public class Bonus
    {
        public GameObject gameObject;
        public Image bonusImage;
        public TMP_Text bonusNameText;
        public TMP_Text bonusValueText;
        public TMP_Text bonusUpgradeText;
        public RectTransform bonusUpgradeRectTransform;
    }
}