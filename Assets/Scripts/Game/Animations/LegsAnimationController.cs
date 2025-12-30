using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsAnimationController : MonoBehaviour
{
    public Animator legsAnimator;
    public PlayerLoadout playerLoadout;
    public Info info;
    private void OnEnable()
    {
        EventManager.Subscribe<int>(GameEvents.WeaponChanged, UpdateSpeedParameters);
        EventManager.Subscribe<bool>(GameEvents.RunningChanged, UpdateRunningParameters);
        EventManager.Subscribe<bool>(GameEvents.GroundingChanged, UpdateGroundParameters);
        EventManager.Subscribe<bool>(GameEvents.WalkingBackChanged, UpdateWalkingBackParameters);

    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<int>(GameEvents.WeaponChanged, UpdateSpeedParameters);
        EventManager.Unsubscribe<bool>(GameEvents.RunningChanged, UpdateRunningParameters);
        EventManager.Unsubscribe<bool>(GameEvents.GroundingChanged, UpdateGroundParameters);
        EventManager.Unsubscribe<bool>(GameEvents.WalkingBackChanged, UpdateWalkingBackParameters);
    }
    void Start()
    {
        int weaponID = playerLoadout.activeItems[0];
        UpdateSpeedParameters(weaponID); // �� ��������� �����
    }
    public void UpdateGroundParameters(bool isGrounded)
    {
        legsAnimator.SetBool("IsGrounded", isGrounded);
    }
    public void UpdateSpeedParameters(int weaponID)
    {
        int speedLevel = info.weapons[weaponID].speedValue;
        legsAnimator.SetInteger("SpeedState", speedLevel);
    }
    public void UpdateRunningParameters(bool isRunning)
    {
        legsAnimator.SetBool("IsRunning", isRunning);
    }
    public void UpdateWalkingBackParameters(bool isWalkingBack)
    {
        legsAnimator.SetBool("IsWalkingBack", isWalkingBack);
    }
}
