using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMovementController : MonoBehaviour
{
    [Header("References")]
    public PlayerLoadout playerLoadout;
    [SerializeField] private PlayerActionController playerActionController;
    public Info info;
    public Rigidbody2D playerRb;
    public PlayerUIController playerUIController;

    private List<ItemInfo> itemInfos;


    [Header("Movement")]
    public float runSpeed = 6f;
    public float fastWalkSpeed = 4f;
    public float walkSpeed = 2.5f;
    private float speed = 0f;
    private float jumpForce = 9f;

    public int speedLevel; // 1, 2, 3
    private bool isGrounded = true;
    private bool isRunning = false;
    private bool isFiring = false;
    private bool walkBack = false;
    private int moveDirection = 0;

    private bool isLeftPressed = false;
    private bool isRightPressed = false;
    private float horizontalInput = 0f;

    private float lastLeftTap = -1f;
    private float lastRightTap = -1f;
    private float doubleTapTime = 0.3f;

    private void OnEnable()
    {
        EventManager.Subscribe<int>(GameEvents.WeaponChanged, UpdateSpeedParameters);
        EventManager.Subscribe<bool>(GameEvents.FiringChanged, UpdateFiringParameters);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<int>(GameEvents.WeaponChanged, UpdateSpeedParameters);
        EventManager.Unsubscribe<bool>(GameEvents.FiringChanged, UpdateFiringParameters);
    }

    private void Awake()
    {
        itemInfos = info.GetAllItems();
    }
    void Update()
    {
        UpdateDirection();

        if (isFiring)
        {
            HandleAnimationAndDirection();
        }
    }

    void FixedUpdate()
    {
        playerRb.velocity = new Vector2(horizontalInput * speed, playerRb.velocity.y);
    }

    void UpdateDirection()
    {
        horizontalInput = 0f;

        if (isLeftPressed)
        {
            horizontalInput -= 1f;
            transform.localScale = new Vector3(0.35f, 0.35f, 1);
        }
        if (isRightPressed)
        {
            horizontalInput += 1f;
            transform.localScale = new Vector3(-0.35f, 0.35f, 1);
        }
    }

    float GetSpeedByLevel()
    {
        switch (speedLevel)
        {
            case 1: return runSpeed;
            case 2: return fastWalkSpeed;
            case 3: return walkSpeed;
            default: return 0f;
        }
    }
    void UpdateSpeedParameters(int weaponID)
    {
        speedLevel = (itemInfos[weaponID] as Weapons).speedValue;
        speed = GetSpeedByLevel();
    }
    void UpdateFiringParameters(bool isFiring)
    {
        this.isFiring = isFiring;
    }
    public void OnLeftPressed()
    {
        isLeftPressed = true;
        moveDirection = -1;
        TryJump(ref lastLeftTap);

        if(isGrounded)
            isRunning = true;
        else
            isRunning = false;

        EventManager.Trigger(GameEvents.RunningChanged, isRunning);
    }

    public void OnRightPressed()
    {
        isRightPressed = true;
        moveDirection = 1;
        TryJump(ref lastRightTap);

        if (isGrounded)
            isRunning = true;
        else
            isRunning = false;

        EventManager.Trigger(GameEvents.RunningChanged, isRunning);
    }

    public void OnButtonReleased()
    {
        isLeftPressed = false;
        isRightPressed = false;
        moveDirection = 0;

        isRunning = false;

        EventManager.Trigger(GameEvents.RunningChanged, isRunning);
    }

    void TryJump(ref float lastTapTime)
    {
        if (Time.time - lastTapTime < doubleTapTime && isGrounded)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            isGrounded = false;

            EventManager.Trigger(GameEvents.GroundingChanged, isGrounded);
        }
        lastTapTime = Time.time;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            if (isLeftPressed || isRightPressed)
                isRunning = true;
            else isRunning = false;

            EventManager.Trigger(GameEvents.GroundingChanged, isGrounded);
            EventManager.Trigger(GameEvents.RunningChanged, isRunning);
        }
    }


    //-----НЕ ОТКРЫВАТЬ----отвечает за поворот во время стрельбы
    private void HandleAnimationAndDirection()
    {
        float aimX = playerActionController.currentWeapon.Aim().x;

        bool facingRight = (isFiring && aimX > 0) || (!isFiring && moveDirection > 0);
        if (facingRight)
        {
            transform.localScale = new Vector3(-0.35f, 0.35f, 1);
            playerUIController.SwapX(true);
        }
        else
        {
            transform.localScale = new Vector3(0.35f, 0.35f, 1);
            playerUIController.SwapX(false);
        }

        // Условия анимаций
        bool isMoving = moveDirection != 0;
        bool movingTowardFire = (moveDirection > 0 && aimX > 0) || (moveDirection < 0 && aimX < 0);
        walkBack = isFiring && isMoving && !movingTowardFire;
        
        EventManager.Trigger(GameEvents.WalkingBackChanged, walkBack);

        if (walkBack)
        {
            speed = walkSpeed;
        }
        else
        {
            speed = GetSpeedByLevel();
        }
    }
}
