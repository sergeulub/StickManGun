using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public Shotgun(Weapons data) : base(data) { }

    protected override void FireBullet()
    {
        float weaponAngle = weaponSprite.transform.rotation.eulerAngles.z;
        firePoint.rotation = Quaternion.Euler(0f, 0f, weaponAngle + 180f + extraWeaponAngle);

        float maxScatterAngle = Mathf.Lerp(0f, StaticDatas._scatterMaxValue, data.scatterValue);

        for (int i = 0; i < 10; i++) // 10 дробинок
        {
            float offset = Random.Range(-maxScatterAngle, maxScatterAngle);
            Quaternion rotation = Quaternion.Euler(0f, 0f, firePoint.rotation.eulerAngles.z + offset);
            Vector3 direction = rotation * Vector3.right;

            Vector3 start = firePoint.position;
            Vector3 end = start + direction.normalized * data.flightLength;

            GameObject lineObj = BulletLinePoolManager.Instance.GetBulletLine();
            LineRenderer lr = lineObj.GetComponent<LineRenderer>();

            lr.SetPosition(0, start);
            lr.SetPosition(1, end);

            ownerMono.StartCoroutine(DisableAfterDelay(lineObj, 0.05f));
        }
    }
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
        Debug.
            Log(distanceToFirePoint);
        float deltaWeaponAngleByDistance;
        //сделать IF от поворота и другую функцию добавить
        bool isLookingRight = playerTransform.localScale.x < 0;
        if (isLookingRight)
        {
            deltaWeaponAngleByDistance = StaticDatas._rifleFunctionByDistanceRight(distanceToFirePoint) - 10f;
        }
        else
        {
            deltaWeaponAngleByDistance = StaticDatas._rifleFunctionByDistanceLeft(distanceToFirePoint) + 1.5f;
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
