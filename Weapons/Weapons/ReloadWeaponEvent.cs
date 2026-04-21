using JetBrains.Annotations;
using System;
using UnityEngine;


[DisallowMultipleComponent]
public class ReloadWeaponEvent : MonoBehaviour
{
   public event Action<ReloadWeaponEvent, ReloadWeaponEventArgs> OnReloadWeapon;

    public void CallReloadWeaponEvent(Weapon weapon, int topUpPercent)
    {
        OnReloadWeapon?.Invoke(this, new ReloadWeaponEventArgs() { weapon = weapon, topUpPercent = topUpPercent });
    }
}

public class ReloadWeaponEventArgs : EventArgs
{
    public Weapon weapon;
    public int topUpPercent;
}
