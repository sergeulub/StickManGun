using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    public Rifle(Weapons data) : base(data) { }


    //-----Ќ≈ ќ“ –џ¬ј“№------->/(*)  |  (*)\
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
             deltaWeaponAngleByDistance = StaticDatas._rifleFunctionByDistanceRight(distanceToFirePoint);
        }
        else
        {
            if (distanceToFirePoint <= 7f)
                deltaWeaponAngleByDistance = StaticDatas._rifleFunctionByDistanceLeft(distanceToFirePoint);
            else
                deltaWeaponAngleByDistance = 0f;
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
