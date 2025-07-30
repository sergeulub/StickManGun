using Unity.VisualScripting;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;               //  амера (об€зательно!)
    public Transform weaponPivot;           // ѕоворотный узел (куда вложены оружие и руки)

    void Update()
    {
        Vector2 aimDirection = GetAimDirection();

        if (aimDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            weaponPivot.rotation = Quaternion.Euler(0, 0, angle + StaticDatas._armDeltaRotation);
        }
    }

    private Vector2 GetAimDirection()
    {
#if UNITY_EDITOR
        // ƒл€ тестировани€ в редакторе: используем мышь
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        return (mouseWorldPos - weaponPivot.position).normalized;
#else
    // ƒл€ мобильных устройств: используем касани€
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        Vector3 touchWorldPos = mainCamera.ScreenToWorldPoint(touch.position);
        touchWorldPos.z = 0;
        return (touchWorldPos - weaponPivot.position).normalized;
    }

    return Vector2.zero;
#endif
    }
}
