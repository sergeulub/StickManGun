using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerActionController : MonoBehaviour
{
    [Header("References")]
    public PlayerLoadout playerLoadout;
    public Info info;
    public Transform firePoint;
    public GameObject bulletLinePrefab;
    public Transform weaponSprite;
    public Camera mainCamera;
    public Transform weaponPivot;
    public Transform playerTransform;
    public PlayerUIController playerUIController;


    [Header("Runtime")]
    public int currentWeaponIndex = 0; // 0 или 1


    private List<Weapon> equippedWeapons;//оружия в арсенале. любое оружие
    private List<Weapons> weaponDatas;//данные об оружиях из арсенала
    private List<ItemInfo> itemInfos;

    private void Awake()
    {
        equippedWeapons = new List<Weapon>(new Weapon[2]);
        weaponDatas = new List<Weapons>(new Weapons[2]);
        itemInfos = info.GetAllItems();
    }
    void Start()
    {
        weaponDatas[0] = itemInfos[playerLoadout.activeItems[0]] as Weapons;
        weaponDatas[1] = itemInfos[playerLoadout.activeItems[1]] as Weapons;

        equippedWeapons[0] =  WeaponFactory.CreateWeapon(weaponDatas[0]);
        equippedWeapons[1] = WeaponFactory.CreateWeapon(weaponDatas[1]);

        equippedWeapons[0].Initialize(this, firePoint, bulletLinePrefab, weaponSprite, mainCamera, weaponPivot, playerTransform);
        equippedWeapons[1].Initialize(this, firePoint, bulletLinePrefab, weaponSprite, mainCamera, weaponPivot, playerTransform);

        EquipWeapon(0);
        /*
        Debug.Log($"Героя начинает с оружием . Характеристики: тип {weaponDatas[0].weaponType},урон {weaponDatas[0].damageValue}, " +
            $"время между патронами {weaponDatas[0].shotTime}, патронов в обойме {weaponDatas[0].cageValue}");
        Debug.Log($"Второе оружие. Характеристики: тип {weaponDatas[1].weaponType},урон {weaponDatas[1].damageValue}, время между патронами {weaponDatas[1].shotTime}, " +
            $"патронов в обойме {weaponDatas[1].cageValue}");
        */
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            SwitchWeapon();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(equippedWeapons[currentWeaponIndex].Reload());
        }

        if (Input.GetMouseButtonDown(0))
        {
            EventManager.Trigger(GameEvents.FiringChanged, true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            EventManager.Trigger(GameEvents.FiringChanged, false);
            equippedWeapons[currentWeaponIndex].StopFiring();
        }

        if (Input.GetMouseButton(0))
        {   
            //наводка
            Vector2 aimDirection = equippedWeapons[currentWeaponIndex].Aim().normalized;
            if (aimDirection != Vector2.zero)
            {
                //наводка
                equippedWeapons[currentWeaponIndex].RotateWeapon();
                Debug.Log("aim");
            }
            else
            {
                equippedWeapons[currentWeaponIndex].StopFiring();
            }
            
        }
    }
    public void SwitchWeapon()
    {
        equippedWeapons[currentWeaponIndex].CancelReload();
        

        currentWeaponIndex = 1 - currentWeaponIndex; // 0 -> 1, 1 -> 0
        EquipWeapon(currentWeaponIndex);
        /*
        Debug.Log($"Теперь в руках у героя оружие {weaponDatas[currentWeaponIndex].weaponName}. Характеристики: тип {weaponDatas[currentWeaponIndex].type},урон {weaponDatas[currentWeaponIndex].damageValue}, " +
            $"время между патронами {weaponDatas[currentWeaponIndex].shotTime}, патронов в обойме {weaponDatas[currentWeaponIndex].cageValue}");
        Debug.Log($"Запазухой оружие {weaponDatas[1 - currentWeaponIndex].weaponName}.");*/
    }
    private void EquipWeapon(int index)
    {
        int weaponID = playerLoadout.activeItems[index];
        playerUIController.SetWeapon(equippedWeapons[currentWeaponIndex]);
        if (equippedWeapons[index].CurrentAmmo == 0)
        {
            StartCoroutine(equippedWeapons[currentWeaponIndex].Reload());
        }

        // Обновляем firePoint позицию
        firePoint.localPosition = weaponDatas[currentWeaponIndex].firePointLocalPos;

        // Уведомляем подписчиков о смене оружия
        EventManager.Trigger(GameEvents.WeaponChanged, weaponID);
    }
    public Weapon currentWeapon => equippedWeapons[currentWeaponIndex];
    
}

