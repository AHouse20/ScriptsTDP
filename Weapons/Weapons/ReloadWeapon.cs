using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(WeaponReloadedEvent))]
[RequireComponent(typeof(SetActiveWeaponEvent))]

[DisallowMultipleComponent]
public class ReloadWeapon : MonoBehaviour
{
    private ReloadWeaponEvent reloadWeaponEvent;
    private WeaponReloadedEvent weaponReloadedEvent;
    private SetActiveWeaponEvent setActiveWeaponEvent;
    private Coroutine reloadWeaponCo;

    private void Awake()
    {
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
        setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
    }

    private void OnEnable()
    {
        reloadWeaponEvent.OnReloadWeapon += ReloadWeaponEvent_OnReloadWeapon;

        setActiveWeaponEvent.OnSetActiveWeapon += SetActiveWeaponEvent_OnSetActiveWeapon;
    }

    private void OnDisable()
    {
        reloadWeaponEvent.OnReloadWeapon -= ReloadWeaponEvent_OnReloadWeapon;

        setActiveWeaponEvent.OnSetActiveWeapon -= SetActiveWeaponEvent_OnSetActiveWeapon;
    }

    private void ReloadWeaponEvent_OnReloadWeapon(ReloadWeaponEvent @event, ReloadWeaponEventArgs args)
    {
        StartReloadWeapon(args);
    }

    private void StartReloadWeapon(ReloadWeaponEventArgs args)
    {

        if(reloadWeaponCo != null)
        {
            StopCoroutine(reloadWeaponCo);
        }

        reloadWeaponCo = StartCoroutine(ReloadWeaponRoutine(args.weapon, args.topUpPercent));
    }

    private IEnumerator ReloadWeaponRoutine(Weapon weapon, int topUpPercent)
    {
        weapon.isReloading = true;
        AudioManager.Instance.PlayOneShot(weapon.weaponDetails.reloadSoundEvent, transform.position);

        while (weapon.weaponReloadTimer < weapon.weaponDetails.weaponReloadTime)
        {
            Debug.Log(weapon.weaponReloadTimer + " / " + weapon.weaponDetails.weaponReloadTime);
            weapon.weaponReloadTimer += Time.deltaTime;
            yield return null;
        }

        if (topUpPercent != 0)
        {
            int ammoIncrease = Mathf.RoundToInt((weapon.weaponDetails.weaponMaxAmmo * topUpPercent) / 100f);

            int totalAmmo = weapon.weaponRemainingAmmo + ammoIncrease;
            if (totalAmmo > weapon.weaponDetails.weaponMaxAmmo)
            {
                weapon.weaponRemainingAmmo = weapon.weaponDetails.weaponMaxAmmo;
            }
            else
            {
                weapon.weaponRemainingAmmo = totalAmmo;
            }
        }
        if (weapon.weaponDetails.hasUnlimitedAmmo)
        {
            weapon.weaponMagRemainingAmmo = weapon.weaponDetails.weaponMagazineCapacity;
        }
        else if (weapon.weaponRemainingAmmo >= weapon.weaponDetails.weaponMagazineCapacity)
        {
            weapon.weaponMagRemainingAmmo = weapon.weaponDetails.weaponMagazineCapacity;
        }
        else
        {
            weapon.weaponMagRemainingAmmo = weapon.weaponRemainingAmmo;
        }

        weapon.weaponReloadTimer = 0f;
        weapon.isReloading = false;
        weaponReloadedEvent.CallWeaponReloadedEvent(weapon);
    }
    private void SetActiveWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent @event, SetActiveWeaponEventArgs args)
    {
        if (args.weapon.isReloading)
        {
            if(reloadWeaponCo != null)
            {
                StopCoroutine(reloadWeaponCo);
            }

            reloadWeaponCo = StartCoroutine(ReloadWeaponRoutine(args.weapon, 0));
        }
    }

}
