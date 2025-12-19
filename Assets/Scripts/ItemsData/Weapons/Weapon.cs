using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon
{
    protected Weapons data;
    protected MonoBehaviour ownerMono; // ����� ��� ������� �������
    protected Transform firePoint; // �����, ������ �������� ����
    protected GameObject bulletLinePrefab;
    protected Transform weaponSprite;
    protected Camera mainCamera;
    protected Transform weaponPivot;
    protected Transform playerTransform;

    protected int currentAmmo;
    protected bool isReloading = false;
    protected bool isFiring = false;
    protected float nextShotTime = 0f;
    protected float extraWeaponAngle = 0f;
    private Coroutine fireRoutine;
    private Coroutine reloadCoroutine;

    public Weapon(Weapons data)
    {
        this.data = data;
        this.currentAmmo = data.cageValue;
    }

    public virtual void Initialize(MonoBehaviour mono, Transform firePoint, GameObject bulletLinePrefab, Transform weaponSprite, Camera mainCamera, Transform weaponPivot, Transform playerTransform)
    {
        this.ownerMono = mono;
        this.firePoint = firePoint;
        this.bulletLinePrefab = bulletLinePrefab;
        this.weaponSprite = weaponSprite;
        this.mainCamera = mainCamera;
        this.weaponPivot = weaponPivot;
        this.playerTransform = playerTransform;

    }

    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => data.cageValue;
    private PlayerUIController uiController => ownerMono.GetComponent<PlayerUIController>();
    
    public virtual void StartFiring()
    {
        if (isFiring || isReloading) return;
        isFiring = true;
        fireRoutine = ownerMono.StartCoroutine(FireContinuously());
    }

    public virtual void StopFiring()
    {
        isFiring = false;
        if (fireRoutine != null)
        {
            ownerMono.StopCoroutine(fireRoutine);
            fireRoutine = null;
        }

        weaponPivot.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    protected virtual IEnumerator FireContinuously()
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
                FireBullet();
                currentAmmo--;
                nextShotTime = Time.time + data.shotTime;
            }

            yield return null; // ��� 1 ���� ����� ��������� ���������
        }
    }

    protected virtual void FireBullet()
    {

        // === ��������� ������� firePoint ��� ���� ���� ===
        float weaponAngle = weaponSprite.transform.rotation.eulerAngles.z;
        firePoint.rotation = Quaternion.Euler(0f, 0f, weaponAngle + 180f + extraWeaponAngle);

        float scatterAngle = Mathf.Lerp(0f, StaticDatas._scatterMaxValue, data.scatterValue);
        float offset = UnityEngine.Random.Range(-scatterAngle, scatterAngle);
        Quaternion rotation = Quaternion.Euler(0f, 0f, firePoint.rotation.eulerAngles.z + offset);
        Vector3 direction = rotation * Vector3.right;

        Vector3 start = firePoint.position;
        Vector3 end = start + direction.normalized * data.flightLength;

        // ������ LineRenderer
        GameObject lineObj = BulletLinePoolManager.Instance.GetBulletLine();
        LineRenderer lr = lineObj.GetComponent<LineRenderer>();

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        // ������� ����� 0.05 ���, ����� ������ ������
        ownerMono.StartCoroutine(DisableAfterDelay(lineObj, StaticDatas._bulletLifeTime));

        //����, �������
    }


    protected IEnumerator DisableAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        BulletLinePoolManager.Instance.ReturnBulletLine(obj);
    }
    public virtual IEnumerator Reload()
    {
        if (isReloading) yield break;
        isReloading = true;
        StopFiring();


        reloadCoroutine = ownerMono.StartCoroutine(ReloadRoutine());
    }
    private IEnumerator ReloadRoutine()
    {
        uiController?.StartReload(data.reloadTime);

        yield return new WaitForSeconds(data.reloadTime);

        currentAmmo = data.cageValue;
        isReloading = false;
    }
    public virtual void CancelReload()
    {
        if (isReloading)
        {
            isReloading = false;
            ownerMono.StopCoroutine(reloadCoroutine); // <--- �����
            uiController.StopReload();
        }
    }

    public virtual Vector2 Aim()
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

        return (targetPos - weaponPivot.position);
    }
    public virtual void RotateWeapon()
    {
        //������ �� +180 ���� ������� ������
        if (playerTransform.localScale.x > 0) extraWeaponAngle = 0f;
        else extraWeaponAngle = 180f;

        //�������
        Vector2 aimDirection = Aim();
        if (aimDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            float deltaWeaponAngle = data._weaponDeltaAngle;
            weaponPivot.rotation = Quaternion.Euler(0, 0, angle + deltaWeaponAngle + extraWeaponAngle);
        }

        //��������
        StartFiring();
        Debug.Log("Fire");
    }
    
}
