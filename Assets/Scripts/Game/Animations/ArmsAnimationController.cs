using UnityEngine;

public class ArmsAnimationController : MonoBehaviour
{
    public Animator armsAnimator;
    public PlayerLoadout playerLoadout;

    private void OnEnable()
    {
        EventManager.Subscribe<int>(GameEvents.WeaponChanged, UpdateWeaponParameters);
        EventManager.Subscribe<bool>(GameEvents.RunningChanged, UpdateRunningParameters);
        EventManager.Subscribe<bool>(GameEvents.FiringChanged, UpdateFiringParameters);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<int>(GameEvents.WeaponChanged, UpdateWeaponParameters); 
        EventManager.Subscribe<bool>(GameEvents.RunningChanged, UpdateRunningParameters);
        EventManager.Subscribe<bool>(GameEvents.FiringChanged, UpdateFiringParameters);
    }
    void Start()
    {
        int weaponID = playerLoadout.activeItems[0];
        UpdateWeaponParameters(weaponID); // по умолчанию стоим
    }
    private void UpdateWeaponParameters(int weaponID)
    {
        armsAnimator.SetInteger("WeaponID", weaponID);
        EventManager.Trigger(GameEvents.VisualWeaponChanged);
    }
    private void UpdateRunningParameters(bool isRunning)
    {
        armsAnimator.SetBool("IsRunning", isRunning);
    }
    private void UpdateFiringParameters(bool isFiring)
    {
        armsAnimator.SetBool("IsFiring", isFiring);
    }
}