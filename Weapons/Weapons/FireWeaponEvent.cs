using System;
using UnityEngine;

[DisallowMultipleComponent]
public class FireWeaponEvent : MonoBehaviour
{
    public event Action<FireWeaponEvent,  FireWeaponEventArgs> OnFireWeapon;

    public void CallFireWeaponEvent(bool fire, bool firePreviousFrame, Direction aimDir, float aimAngle, float weaponAimAngle, Vector3 weaponAimDir)
    {
        OnFireWeapon?.Invoke(this, new FireWeaponEventArgs()
        {
            fire = fire,
            firePreviousFrame = firePreviousFrame,
            aimDirection = aimDir,
            aimAngle = aimAngle,
            weaponAimAngle = weaponAimAngle,
            weaponAimDirection = weaponAimDir
        });
    }
}

public class FireWeaponEventArgs : EventArgs
{
    public bool fire;
    public bool firePreviousFrame;
    public Direction aimDirection;
    public float aimAngle;
    public float weaponAimAngle;
    public Vector3 weaponAimDirection;
}