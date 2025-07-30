using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    public Sniper(Weapons data) : base(data) { }

    //-----НЕ ОТКРЫВАТЬ------->/(*)  |  (*)\
    public override void RotateWeapon()
    {
        Vector3 targetPos;

#if UNITY_EDITOR
        targetPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
#else
    if (Input.touchCount == 0)
        return Vector2.zero;

    Touch touch = Input.GetTouch(0);
    targetPos = mainCamera.ScreenToWorldPoint(touch.position);
#endif

        targetPos.z = 0;

        float distanceToFirePoint = Vector2.Distance(playerTransform.position + new Vector3(0f, 1f, 0f), targetPos);
        float deltaWeaponAngleByDistance;
        if (distanceToFirePoint <= 2f)
            deltaWeaponAngleByDistance = StaticDatas._sniperFunctionByDistance(distanceToFirePoint);
        else
            deltaWeaponAngleByDistance = 0f;

        //поврот на +180 если смотрим вправо
        if (playerTransform.localScale.x > 0) base.extraWeaponAngle = 0f;
        else base.extraWeaponAngle = 180f;

        deltaWeaponAngleByDistance = playerTransform.localScale.x < 0 ? 1 : 0;
        //наводка
        Vector2 aimDirection = Aim();
        if (aimDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            float deltaWeaponAngle = data._weaponDeltaAngle;
            weaponPivot.rotation = Quaternion.Euler(0, 0, angle + deltaWeaponAngle + base.extraWeaponAngle);
        }
    }
    protected override IEnumerator FireContinuously()
    {
        while (isFiring)
        {
            if (isReloading)
            {
                yield break;
            }

            if (currentAmmo <= 0)
            {
                yield return ownerMono.StartCoroutine(Reload());
                continue;
            }

            if (Time.time >= nextShotTime)
            {
                // === Проверка точности наведения ===
                Vector2 aimDirection = Aim().normalized;
                Vector2 fireDirection = firePoint.right; // firePoint должен быть повернут по дула

                
                float angleDiff = Vector2.Angle(aimDirection, fireDirection);
                
                if (Mathf.Abs(angleDiff) <= 10f || Mathf.Abs(angleDiff + 180) <= 10f || Mathf.Abs(angleDiff - 180) <= 10f) // допустимая погрешность (в градусах)
                {
                    FireBullet();
                    currentAmmo--;
                    nextShotTime = Time.time + data.shotTime;
                }
            }

            yield return null;
        }
    }
}
