
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(WeaponFiredEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]

[DisallowMultipleComponent]
public class FireWeapon : MonoBehaviour
{
    [SerializeField] Animator handAnim;
    private float chargeTimer = 0f;
    private float fireRateCooldownTimer = 0f;
    private ActiveWeapon activeWeapon;
    private FireWeaponEvent fireWeaponEvent;
    private WeaponFiredEvent weaponFiredEvent;
    private ReloadWeaponEvent reloadWeaponEvent;
    private bool waitingForRelease;

    private void Awake()
    {
        activeWeapon = GetComponent<ActiveWeapon>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
    }

    private void OnEnable()
    {
        fireWeaponEvent.OnFireWeapon += FireWeaponEvent_OnFireWeapon;
    }

    private void OnDisable()
    {
        fireWeaponEvent.OnFireWeapon -= FireWeaponEvent_OnFireWeapon;
    }
    private void FireWeaponEvent_OnFireWeapon(FireWeaponEvent fireWeaponEvent, FireWeaponEventArgs args)
    {
        Fire(args);
    }
    private void Fire(FireWeaponEventArgs args)
    {
        WeaponChargeUp(args);

        if (activeWeapon.GetActiveWeapon().weaponDetails.fireOnRelease && chargeTimer <= 0f)
            StartCoroutine(WaitForRelease(args, true));

        if (args.fire && IsReadyToFire() && !waitingForRelease)
        {
            FireBullet(args);
            if (activeWeapon.GetActiveWeapon().weaponDetails.semiAutomatic)
            {
                StartCoroutine(WaitForRelease(args, false));
            }
        }
    }

    private void FireBullet(FireWeaponEventArgs args)
    {
        if(handAnim != null) { handAnim.SetTrigger("Recoil"); } 
        AudioManager.Instance.PlayOneShot(activeWeapon.GetActiveWeapon().weaponDetails.shootSoundEvent, transform.position);
        FireAmmo(args.aimAngle, args.weaponAimAngle, args.weaponAimDirection);
        ResetCooldownTimer();
        if (activeWeapon.GetActiveWeapon().weaponDetails.chargePerShot)
        {
            ResetChargeTimer();
        }
    }

    private void WeaponChargeUp(FireWeaponEventArgs args)
    {
        if (args.firePreviousFrame)
        {
            chargeTimer -= Time.deltaTime;
        }
        else
        {
            ResetChargeTimer();
        }
    }
    private void ResetChargeTimer()
    {
        chargeTimer = activeWeapon.GetActiveWeapon().weaponDetails.weaponChargeDelay;
    }
    private void ResetCooldownTimer()
    {
        fireRateCooldownTimer = activeWeapon.GetActiveWeapon().weaponDetails.weaponFireRate;
    }

    private void FireAmmo(float aimAngle, float weaponAimAngle, Vector3 weaponAimDirection)
    {
        AmmoDetailsSO currentAmmo = activeWeapon.GetCurrentAmmo();
        if(currentAmmo != null)
        {
            StartCoroutine(FireRoutine(currentAmmo, aimAngle, weaponAimAngle, weaponAimDirection));
        }
    }

    private IEnumerator FireRoutine(AmmoDetailsSO currentAmmo, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirection)
    {
        int ammoCounter = 0;

        int ammoPerShot = Random.Range(currentAmmo.ammoSpawnCountMin, currentAmmo.ammoSpawnCountMax + 1);

        float ammoSpawnInterval;

        /*
        if(ammoPerShot > 1)
        {
            ammoSpawnInterval = Random.Range(currentAmmo.ammoSpawnIntervalMin, currentAmmo.ammoSpawnIntervalMax);
        }
        else
        {
            ammoSpawnInterval = 0f;
        }
        */
        while(ammoCounter < ammoPerShot)
        {
            ammoCounter++;
            ammoSpawnInterval = Random.Range(currentAmmo.ammoSpawnIntervalMin, currentAmmo.ammoSpawnIntervalMax);
            GameObject ammoPrefab = currentAmmo.ammoPrefabs[Random.Range(0, currentAmmo.ammoPrefabs.Length)];

            float ammoSpeed = Random.Range(currentAmmo.ammoSpeedMin, currentAmmo.ammoSpeedMax);

            IFirable ammo = (IFirable)PoolManager.Instance.ReuseComponent(ammoPrefab, activeWeapon.GetShootPosition(), Quaternion.identity);

            ammo.InitialiseAmmo(currentAmmo, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirection);

            //yield return new WaitForSeconds(ammoSpawnInterval);
        }


        if (!activeWeapon.GetActiveWeapon().weaponDetails.hasUnlimitedMagazine)
        {
            activeWeapon.GetActiveWeapon().weaponMagRemainingAmmo--;
        }
        if (!activeWeapon.GetActiveWeapon().weaponDetails.hasUnlimitedAmmo)
        {
            activeWeapon.GetActiveWeapon().weaponRemainingAmmo--;
        }
        Debug.Log("Fired! Remaining ammo: " + activeWeapon.GetActiveWeapon().weaponMagRemainingAmmo);
        weaponFiredEvent.CallWeaponFiredEvent(activeWeapon.GetActiveWeapon());
        yield return null;
    }

    private bool IsReadyToFire()
    {
        if(activeWeapon.GetActiveWeapon().weaponRemainingAmmo <= 0 && !activeWeapon.GetActiveWeapon().weaponDetails.hasUnlimitedAmmo)
            return false;
        
        if(activeWeapon.GetActiveWeapon().isReloading)
            return false;

        if (!activeWeapon.GetActiveWeapon().weaponDetails.hasUnlimitedMagazine && activeWeapon.GetActiveWeapon().weaponMagRemainingAmmo <= 0)
        {
            reloadWeaponEvent.CallReloadWeaponEvent(activeWeapon.GetActiveWeapon(), 0);
            return false;
        }

        if (fireRateCooldownTimer > 0f || chargeTimer > 0f)
            return false;


        return true;
    }

    private IEnumerator WaitForRelease(FireWeaponEventArgs args, bool fireOnRelease)
    {
        waitingForRelease = true;
        while (GetComponent<PlayerInput>().actions.FindAction("Attack").IsPressed())
        {
            Debug.Log("Waiting");
            yield return null;
        }
        Debug.Log("Released");
        waitingForRelease = false;
        if (args.fire && IsReadyToFire() && fireOnRelease)
        {
            FireBullet(args);
        }
        yield return null;
    }
    private void Update()
    {
        fireRateCooldownTimer -= Time.deltaTime;
    }
}
