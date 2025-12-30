using UnityEngine;
using UnityEngine.UI;

public class PlayerVisual : MonoBehaviour
{
    [Header("Equipment Images")]
    public Image hatImage;
    public Image[] bootsImage;

    [Header("Animator")]
    public Animator playerAnimator;

    public void UpdateHat(Sprite sprite)
    {
        hatImage.sprite = sprite;
        if (sprite != null)
        {
            hatImage.rectTransform.localScale = Vector3.one;
        }
        else
        {
            hatImage.rectTransform.localScale = Vector3.zero;
        }
    }

    public void UpdateBoots(Sprite sprite)
    {
        bootsImage[0].sprite = sprite;
        bootsImage[1].sprite = sprite;
        if (sprite != null)
        {
            bootsImage[0].rectTransform.localScale = Vector3.one;
            bootsImage[1].rectTransform.localScale = Vector3.one;
        }
        else
        {
            bootsImage[0].rectTransform.localScale = Vector3.zero;
            bootsImage[1].rectTransform.localScale = Vector3.zero;
        }
    }

    public void UpdateWeapon(int weaponID)
    {
        playerAnimator.SetInteger("ID", weaponID);
    }
}
