using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCellUI : MonoBehaviour
{
    public Transform cell;
    public Image image;
    public RectTransform imageTransform;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Text buyOrSellText;
    public Image buyOrSellImage;
    public GameObject coinIcon;
    public GameObject lockSprite;
    public TMP_Text lockText;
    public GameObject lockTextGO;
    public Animation lockAnim;
    public GameObject newText;



    [ContextMenu("LoadInfo")]
    public void LoadInfo()
    {   
        if (cell == null) cell = GetComponent<Transform>();
        if (image == null) image = cell.GetChild(0).GetComponent<Image>();
        if (imageTransform == null) imageTransform = cell.GetChild(0).GetComponent<RectTransform>();
        nameText = cell.GetChild(1).GetComponent<TMP_Text>();
        priceText = cell.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        if (buyOrSellText == null) buyOrSellText = cell.GetChild(2).GetChild(1).GetComponent<Text>();
        if (buyOrSellImage == null) buyOrSellImage = cell.GetChild(2).GetComponent<Image>();
        if (coinIcon == null) coinIcon = cell.GetChild(2).GetChild(2).gameObject;
        lockText = cell.GetChild(3).GetChild(0).GetComponent<TMP_Text>();
        lockTextGO = cell.GetChild(3).gameObject;
        lockSprite = cell.GetChild(4).gameObject;
        lockAnim = cell.GetComponent<Animation>();
        newText = cell.GetChild(5).gameObject;
    }
}
