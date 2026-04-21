using System;
using UnityEngine;

[RequireComponent (typeof(Player))]
[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        //player.movementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;
        player.idleEvent.OnIdle += IdleEvent_OnIdle;
        player.aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;
    }

    private void OnDisable()
    {
        player.movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;
        player.idleEvent.OnIdle -= IdleEvent_OnIdle;
        player.aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;
    }

    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent movementByVelocityEvent, MovementByVelocityArgs args)
    {
        SetMovement();
    }

    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        SetIdle();
    }

    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        InitialiseAim();
        SetAim(aimWeaponEventArgs.aimDirection);
    }

    private void SetIdle()
    {
        player.animator.SetBool(Settings.isIdle, true);
        player.animator.SetBool(Settings.isWalking, false);
    }

    private void SetMovement()
    {
        player.animator.SetBool(Settings.isIdle, false);
        player.animator.SetBool(Settings.isWalking, true);
    }


    private void InitialiseAim()
    {
        player.animator.SetBool(Settings.aimLeft, false);
        player.animator.SetBool(Settings.aimRight, false);
    }

    private void SetAim(Direction aimDirection)
    {
        switch (aimDirection)
        {
            case Direction.Left:
                player.animator.SetBool(Settings.aimLeft, true);
                break;
            case Direction.Right:
                player.animator.SetBool(Settings.aimRight, true);
                break;
            default:
                player.animator.SetBool(Settings.aimRight, true);
                break;
        }
    }
}
