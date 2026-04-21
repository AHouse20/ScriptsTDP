using System;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System.Collections.Generic;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private MovementDetailsSO movementDetails;

    private Player player;
    private bool fireInputPreviousFrame = false;
    private int currentWeaponIndex = 1;
    private float moveSpeed;
    private void Awake()
    {
        player = GetComponent<Player>();
        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        SetStartingWeapon();
    }

    private void SetStartingWeapon()
    {
        int index = 1;

        foreach(Weapon weapon in player.weapons)
        {
            if(weapon.weaponDetails == player.playerDetails.startingWeapon)
            {
                SetWeaponByIndex(index);
                break;
            }
            index++;
        }
    }

    private void SetWeaponByIndex(int index)
    {
        if(index - 1 < player.weapons.Count)
        {
            currentWeaponIndex = index;
            player.setActiveWeaponEvent.CallSetActiveWeaponEvent(player.weapons[index - 1]);
        }
    }

    private void Update()
    {
        MovementInput();

        WeaponInput();
    }

    private void MovementInput()
    {
        player.idleEvent.CallIdleEvent();
    }
    public void Move(InputAction.CallbackContext ctx)
    {
        if (!player.alive) return;
        player.movementByVelocityEvent.CallMovementByVelocityEvent(ctx.ReadValue<Vector2>(), moveSpeed);

    }
    private void WeaponInput()
    {
        Vector3 weaponDirection;
        float weaponAngleDegrees, playerAngleDegrees;
        Direction playerAimDirection;

        AimWeaponInput(out  weaponDirection, out playerAimDirection, out weaponAngleDegrees, out playerAngleDegrees);

        FireWeaponInput(weaponDirection, weaponAngleDegrees, playerAngleDegrees, playerAimDirection);

        //SwitchWeaponInput();

        ReloadWeaponInput();
    }

    public void Switch(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            NextWeapon();
        }
    }

    private void NextWeapon()
    {
        currentWeaponIndex++;
        if(currentWeaponIndex > player.weapons.Count)
        {
            currentWeaponIndex = 1;
        }

        SetWeaponByIndex(currentWeaponIndex);
    }

    private void ReloadWeaponInput()
    {
        Weapon activeWeapon = player.activeWeapon.GetActiveWeapon();

        if (activeWeapon.isReloading) return;

        //Come back
        if (activeWeapon.weaponRemainingAmmo < activeWeapon.weaponDetails.weaponMagazineCapacity 
            && !activeWeapon.weaponDetails.hasUnlimitedAmmo) return;

        if(activeWeapon.weaponMagRemainingAmmo >= activeWeapon.weaponDetails.weaponMagazineCapacity) return;

        if (GetComponent<PlayerInput>().actions.FindAction("Reload").IsPressed())
        {
            player.reloadWeaponEvent.CallReloadWeaponEvent(player.activeWeapon.GetActiveWeapon(), 0);
        }

    }

    private void FireWeaponInput(Vector3 weaponDirection, float weaponAngleDegrees, float playerAngleDegrees, Direction playerAimDirection)
    {
        if (GetComponent<PlayerInput>().actions.FindAction("Attack").IsPressed())
        {
            player.fireWeaponEvent.CallFireWeaponEvent(true, fireInputPreviousFrame, playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
            fireInputPreviousFrame = true;
        }
        else
        {
            fireInputPreviousFrame = false;
        }
    }

    private void AimWeaponInput(out Vector3 weaponDirection, out Direction playerAimDirection, out float weaponAngleDegrees, out float playerAngleDegrees)
    {
        Vector3 cursorWorldPosition = Utils.GetCursorWorldPosition();

        weaponDirection = cursorWorldPosition - player.activeWeapon.GetShootPosition();

        Vector3 playerDirection = cursorWorldPosition - player.handPivot.position;

        if(Mathf.Abs(weaponDirection.x) < 2 || Mathf.Abs(weaponDirection.y) < 2)
        {
            weaponDirection = playerDirection;
        }

        weaponAngleDegrees = Utils.GetAngleFromVector(weaponDirection);

        playerAngleDegrees = Utils.GetAngleFromVector(playerDirection);

        playerAimDirection = Utils.GetDirectionFromAngle(playerAngleDegrees);

        player.aimWeaponEvent.CallAimWeaponEvent(playerAimDirection, weaponAngleDegrees, weaponAngleDegrees, weaponDirection);       
    }
}
