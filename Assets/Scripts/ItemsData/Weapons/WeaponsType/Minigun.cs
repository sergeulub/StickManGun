using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Minigun : Weapon
{
    public Minigun(Weapons data) : base(data) { }

    protected override void FireBullet()
    {
        // === Обновляем поворот firePoint под угол дула ===
        float weaponAngle = weaponSprite.transform.rotation.eulerAngles.z;
        firePoint.rotation = Quaternion.Euler(0f, 0f, weaponAngle + 180f + extraWeaponAngle);

        float scatterAngle = Mathf.Lerp(0f, 45f, data.scatterValue);
        float offset = Random.Range(-scatterAngle, scatterAngle);
        Quaternion rotation = Quaternion.Euler(0f, 0f, firePoint.rotation.eulerAngles.z + offset);
        Vector3 direction = rotation * Vector3.right;

        float firePointDeltaY = Random.Range(-StaticDatas._minigunRangeDeltaY, StaticDatas._minigunRangeDeltaY);
        Vector3 start = firePoint.position;
        start.y += firePointDeltaY;
        Vector3 end = start + direction.normalized * data.flightLength;

        // Создаём LineRenderer
        GameObject lineObj = BulletLinePoolManager.Instance.GetBulletLine();
        LineRenderer lr = lineObj.GetComponent<LineRenderer>();

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        // Удалить через 0.05 сек, чтобы просто мигнул
        ownerMono.StartCoroutine(DisableAfterDelay(lineObj, StaticDatas._bulletLifeTime));
    }

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

        // ?? Проверка расстояния до firePoint
        float distanceToFirePoint = Vector2.Distance(playerTransform.position + new Vector3(0f, 1f, 0f), targetPos);
        Debug.
            Log(distanceToFirePoint);
        float deltaWeaponAngleByDistance;
        //сделать IF от поворота и другую функцию добавить
        bool isLookingRight = playerTransform.localScale.x < 0;
        if (isLookingRight)
        {
            deltaWeaponAngleByDistance = StaticDatas._minigunFunctionByDistanceRight(distanceToFirePoint);
        }
        else
        {
            deltaWeaponAngleByDistance = StaticDatas._minigunFunctionByDistanceLeft(distanceToFirePoint);
        }

        //поврот на +180 если смотрим вправо
        if (playerTransform.localScale.x > 0) base.extraWeaponAngle = 0f;
        else base.extraWeaponAngle = 180f;

        //наводка
        Vector2 aimDirection = Aim();
        if (aimDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            float deltaWeaponAngle = data._weaponDeltaAngle;
            weaponPivot.rotation = Quaternion.Euler(0, 0, angle + deltaWeaponAngle - deltaWeaponAngleByDistance + base.extraWeaponAngle);
        }

        base.StartFiring();
    }
}
