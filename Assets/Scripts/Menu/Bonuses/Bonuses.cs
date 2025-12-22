using System;
using System.Collections.Generic;
using UnityEngine;
namespace Bonuses {
    public class Bonuses : MonoBehaviour
    {
        [SerializeField] List<Bonus> bonuses = new List<Bonus>();
        [SerializeField] List<int> bonusesLvls = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        [SerializeField] Minas.Item _item;

        int curIndex;
        public void _OpenInfoBonuses(int index)
        {   
            curIndex = index;
            _item.go.SetActive(true);
            _item.upgradeMine.SetActive(false);
            _item.upgradeBonus.SetActive(true);
            _item.image.sprite = bonuses[index].sprite;
            _item.rectTransformImage.sizeDelta = new Vector2(75, 75);
            _item.name.text = bonuses[index].name;
            _item.lvl.text = "Óð. " + (bonusesLvls[index] + 1).ToString();
            _item.descriptionText.text = bonuses[index].description;
            _item.bonusOne.bonusNameText.text = bonuses[index].bonusName;
            _item.bonusOne.bonusValueText.text = (bonuses[index].bonusValue + bonuses[index].bonusUpgrade * bonusesLvls[curIndex]).ToString();
            _item.bonusOne.bonusUpgradeText.text = "+"+bonuses[index].bonusUpgrade.ToString();
            _item.bonusOne.bonusImage.sprite = bonuses[index].sprite;
            _item.bonusOne.bonusUpgradeRectTransform.localPosition = new Vector3(-2f  + 7.8f * _item.bonusOne.bonusValueText.text.Length, -3.2595f, 0);
            _item.upgradeGO.SetActive(false);
            _item.bonusTwo.gameObject.SetActive(false);
            if (index != 1 || index != 3)
            {
                _item.bonusOne.bonusValueText.text = _item.bonusOne.bonusValueText.text + "%";
                _item.bonusOne.bonusUpgradeText.text = _item.bonusOne.bonusUpgradeText.text + "%";
            }
        }
        public void _UpgradeBonuses()
        {
            bonusesLvls[curIndex]++;

            _OpenInfoBonuses(curIndex);
        }
    }
    [Serializable]
    public class Bonus
    {
        public Sprite sprite;
        public string name;
        public string description;
        public string bonusName;
        public float bonusValue;
        public float bonusUpgrade;
        
    }
}

