using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI ammoText;
    public Image reloadBar;
    public Weapon currentWeapon;

    private bool isReloading;
    private float reloadDuration;
    private float reloadTimer;

    private void Update()
    {
        if (currentWeapon == null)
            return;

        ammoText.text = $"{currentWeapon.CurrentAmmo} / {currentWeapon.MaxAmmo}";

        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            float fill = 1f - (reloadTimer / reloadDuration);
            reloadBar.fillAmount = fill;

            if (reloadTimer >= reloadDuration)
            {
                isReloading = false;
                reloadBar.gameObject.SetActive(false);
            }
        }
    }

    public void StartReload(float duration)
    {
        isReloading = true;
        reloadDuration = duration;
        reloadTimer = 0f;
        reloadBar.fillAmount = 1f;
        reloadBar.gameObject.SetActive(true);
    }

    public void StopReload()
    {
        isReloading = false;
        reloadDuration = 0f;
        reloadTimer = 0f;
        reloadBar.fillAmount = 0f;
        reloadBar.gameObject.SetActive(false);
    }

    public void SetWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
    }

    public void SwapX(bool isFacongRight)
    {   
        int sight = isFacongRight ? 1 : -1;
        Vector3 scale = reloadBar.transform.localScale;
        float x = Mathf.Abs(scale.x) * -1 * sight;
        reloadBar.transform.localScale = new Vector3(x, scale.y, scale.z);
    }
}