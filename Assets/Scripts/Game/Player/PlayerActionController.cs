using System.Collections;
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
    public GameFlowManager gameFlowManager;


    [Header("Runtime")]
    public int currentWeaponIndex = 0; // 0 ��� 1


    private List<Weapon> equippedWeapons;//������ � ��������. ����� ������
    private List<Weapons> weaponDatas;//������ �� ������� �� ��������
    private List<ItemInfo> itemInfos;


    private void OnEnable()
    {
        EventManager.Subscribe(GameEvents.VisualWeaponChanged, VisualWeaponChanged);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.VisualWeaponChanged, VisualWeaponChanged);
    }
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
        playerLoadout.DebugPlayerLoadout(info);
    }

    


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            _SwitchWeapon();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(equippedWeapons[currentWeaponIndex].Reload());
        }

        if (Input.GetMouseButtonDown(0))
        {
            EventManager.Trigger(GameEvents.FiringChanged, true);
        }
        if (Input.GetMouseButtonUp(0) && !gameFlowManager.isPaused)
        {
            EventManager.Trigger(GameEvents.FiringChanged, false);
            equippedWeapons[currentWeaponIndex].StopFiring();
        }

        if (Input.GetMouseButton(0) && !gameFlowManager.isPaused)
        {   
            //�������
            Vector2 aimDirection = equippedWeapons[currentWeaponIndex].Aim().normalized;
            if (aimDirection != Vector2.zero)
            {
                //�������
                equippedWeapons[currentWeaponIndex].RotateWeapon();
                //Debug.Log("aim");
            }
            else
            {
                equippedWeapons[currentWeaponIndex].StopFiring();
            }
            
        }
    }

    public void _SwitchWeapon()
    {   
        SwitchWeapon();
    }
    private void SwitchWeapon()
    {   
        if (playerLoadout.activeItems[PlayerLoadout.weapon2] != -1)
        {
            equippedWeapons[currentWeaponIndex].SetSwitchingParametr(true);
            equippedWeapons[currentWeaponIndex].CancelReload();
            equippedWeapons[currentWeaponIndex].StopFiring();

            currentWeaponIndex = 1 - currentWeaponIndex; // 0 -> 1, 1 -> 0

            equippedWeapons[currentWeaponIndex].SetSwitchingParametr(true);
            EquipWeapon(currentWeaponIndex);
            /*
            Debug.Log($"������ � ����� � ����� ������ {weaponDatas[currentWeaponIndex].weaponName}. ��������������: ��� {weaponDatas[currentWeaponIndex].type},���� {weaponDatas[currentWeaponIndex].damageValue}, " +
                $"����� ����� ��������� {weaponDatas[currentWeaponIndex].shotTime}, �������� � ������ {weaponDatas[currentWeaponIndex].cageValue}");
            Debug.Log($"��������� ������ {weaponDatas[1 - currentWeaponIndex].weaponName}.");*/
        }
        else
        {
            Debug.Log("Нельзя! Только одно оружие");
        }
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

    public void _Reload()
    {
        Reload();
    }
    private void Reload()
    {
        StartCoroutine(equippedWeapons[currentWeaponIndex].Reload());
    }

    private void VisualWeaponChanged()
    {
        StartCoroutine(VisualWeaponChangedIE());
    }
    private IEnumerator VisualWeaponChangedIE()
    {
        Debug.Log("cor started");
        yield return new WaitForSeconds(0.1f);

        
        currentWeapon.SetSwitchingParametr(false);
        Debug.Log("cor finished");
    }
}

