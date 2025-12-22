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
    public int currentWeaponIndex = 0; // 0 ��� 1


    private List<Weapon> equippedWeapons;//������ � ��������. ����� ������
    private List<Weapons> weaponDatas;//������ �� ������� �� ��������
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
        equippedWeapons[0] =  WeaponFactory.CreateWeapon(weaponDatas[0]);
        equippedWeapons[0].Initialize(this, firePoint, bulletLinePrefab, weaponSprite, mainCamera, weaponPivot, playerTransform);

        if (playerLoadout.activeItems[1] != StaticDatas.EMPTY_SLOT)
        {
            weaponDatas[1] = itemInfos[playerLoadout.activeItems[1]] as Weapons;
            equippedWeapons[1] = WeaponFactory.CreateWeapon(weaponDatas[1]);
            equippedWeapons[1].Initialize(this, firePoint, bulletLinePrefab, weaponSprite, mainCamera, weaponPivot, playerTransform);
        }
        
        EquipWeapon(0);
        /*
        Debug.Log($"����� �������� � ������� . ��������������: ��� {weaponDatas[0].weaponType},���� {weaponDatas[0].damageValue}, " +
            $"����� ����� ��������� {weaponDatas[0].shotTime}, �������� � ������ {weaponDatas[0].cageValue}");
        Debug.Log($"������ ������. ��������������: ��� {weaponDatas[1].weaponType},���� {weaponDatas[1].damageValue}, ����� ����� ��������� {weaponDatas[1].shotTime}, " +
            $"�������� � ������ {weaponDatas[1].cageValue}");
        */
        DebugPlayerLoadout();
    }

    private void LogItem(string label, int itemID, List<ItemInfo> items)
    {
        if (itemID < 0)
        {
            Debug.Log($"{label}: EMPTY");
        }
        else
        {
            Debug.Log($"{label}: {items[itemID].itemName} (ID {itemID}) (LVL {GameManager.InventoryData.levels[itemID]})");
        }
    }
    private void DebugPlayerLoadout()
    {
        List<ItemInfo> items = info.GetAllItems();

        Debug.Log("===== PLAYER LOADOUT =====");

        // 1–2. Оружие
        LogItem("Main weapon", playerLoadout.activeItems[0], items);
        LogItem("Second weapon", playerLoadout.activeItems[1], items);

        // 3–6. Экипировка
        LogItem("Boots", playerLoadout.activeItems[2], items);
        LogItem("Helmet", playerLoadout.activeItems[3], items);
        LogItem("Ring 1", playerLoadout.activeItems[4], items);
        LogItem("Ring 2", playerLoadout.activeItems[5], items);
        Debug.Log("==========================");

        for (int i = 0; i < 4; i++)
        {
            Debug.Log($"{info.deployables[i].itemName} LVL {playerLoadout.deployableLevels[i]}");
        }
        for (int i = 0; i < 8; i++)
        {
            Debug.Log($"{info.boosts[i].itemName} LVL {playerLoadout.bonusLevels[i]}");
        }
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
            //�������
            Vector2 aimDirection = equippedWeapons[currentWeaponIndex].Aim().normalized;
            if (aimDirection != Vector2.zero)
            {
                //�������
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
        Debug.Log($"������ � ����� � ����� ������ {weaponDatas[currentWeaponIndex].weaponName}. ��������������: ��� {weaponDatas[currentWeaponIndex].type},���� {weaponDatas[currentWeaponIndex].damageValue}, " +
            $"����� ����� ��������� {weaponDatas[currentWeaponIndex].shotTime}, �������� � ������ {weaponDatas[currentWeaponIndex].cageValue}");
        Debug.Log($"��������� ������ {weaponDatas[1 - currentWeaponIndex].weaponName}.");*/
    }
    private void EquipWeapon(int index)
    {
        int weaponID = playerLoadout.activeItems[index];
        playerUIController.SetWeapon(equippedWeapons[currentWeaponIndex]);
        if (equippedWeapons[index].CurrentAmmo == 0)
        {
            StartCoroutine(equippedWeapons[currentWeaponIndex].Reload());
        }

        // ��������� firePoint �������
        firePoint.localPosition = weaponDatas[currentWeaponIndex].firePointLocalPos;

        // ���������� ����������� � ����� ������
        EventManager.Trigger(GameEvents.WeaponChanged, weaponID);
    }
    public Weapon currentWeapon => equippedWeapons[currentWeaponIndex];
    
}

