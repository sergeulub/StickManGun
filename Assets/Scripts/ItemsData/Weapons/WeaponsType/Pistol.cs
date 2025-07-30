using UnityEngine;

public class Pistol : Weapon
{
    public Pistol(Weapons data) : base(data) { }

    public override void RotateWeapon()
    {

        float deltaWeaponAngleByDistance;
        bool isLookingRight = playerTransform.localScale.x < 0;
        if (isLookingRight)
        {
            deltaWeaponAngleByDistance = StaticDatas._pistolFunctionByDistanceRight();
        }
        else
        {
            deltaWeaponAngleByDistance = StaticDatas._pistolFunctionByDistanceLeft();
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