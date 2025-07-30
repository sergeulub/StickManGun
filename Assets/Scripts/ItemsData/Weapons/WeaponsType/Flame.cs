using UnityEngine;

public class Flame : Weapon
{
    private GameObject flameInstance;
    private ParticleSystem flameParticles;

    public Flame(Weapons data) : base(data) { }

    public override void Initialize(MonoBehaviour ownerMono, Transform firePoint, GameObject bulletLinePrefab, Transform weaponSprite, Camera mainCamera, Transform weaponPivot, Transform playerTransform)
    {
        base.Initialize(ownerMono, firePoint, bulletLinePrefab, weaponSprite, mainCamera, weaponPivot, playerTransform);

        // Создаём визуал огня
        flameInstance = GameObject.Instantiate(data.flamePrefab, firePoint.position, firePoint.rotation, firePoint);
        flameInstance.transform.localRotation = Quaternion.Euler(0f, 90f, 90f);
        flameParticles = flameInstance.GetComponent<ParticleSystem>();
        flameInstance.SetActive(false);
    }

    public override void StartFiring()
    {
        if (isReloading || isFiring) return;

        isFiring = true;

        // 🔥 Включаем визуал пламени
        flameInstance.SetActive(true);
        flameParticles.Play();

        ownerMono.StartCoroutine(FireContinuously());
        Debug.Log("Flame");
    }

    public override void StopFiring()
    {
        if (!isFiring) return;

        isFiring = false;

        // 🧯 Выключаем визуал пламени
        flameParticles.Stop();
        flameInstance.SetActive(false);
    }

    protected override void FireBullet()
    {
        // Здесь не создаём пули, просто можем оставить Debug
        
    }
    public override void RotateWeapon()
    {
        //поврот на +180 если смотрим вправо
        if (playerTransform.localScale.x > 0) extraWeaponAngle = 0f;
        else extraWeaponAngle = 180f;

        firePoint.localRotation = Quaternion.Euler(0, 0f, -90f);

        //наводка
        Vector2 aimDirection = Aim();
        if (aimDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            float deltaWeaponAngle = data._weaponDeltaAngle;
            weaponPivot.rotation = Quaternion.Euler(0, 0, angle + deltaWeaponAngle + extraWeaponAngle);
        }

        //стрельба
        StartFiring();
    }
}