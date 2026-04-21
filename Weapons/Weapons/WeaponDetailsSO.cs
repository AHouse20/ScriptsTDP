using UnityEngine;
using Sirenix.OdinInspector;
using FMODUnity;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/WeaponDetails")]
public class WeaponDetailsSO : ScriptableObject
{
    public string weaponName;
    public Sprite weaponSprite;

    public EventReference shootSoundEvent;
    public EventReference reloadSoundEvent;

    public Vector3 weaponShootPosition;
    public Vector3 weaponEjectPosition;
    public bool isTwoHanded;
    [ShowIf("isTwoHanded")]
    public Vector3 offHandPosition;

    public AmmoDetailsSO weaponAmmo;

    public bool hasUnlimitedAmmo = false;
    [HideIf("hasUnlimitedAmmo", true)]
    public int weaponMaxAmmo = 120;

    public bool hasUnlimitedMagazine = false;
    [HideIf("hasUnlimitedMagazine", true)]
    public int weaponMagazineCapacity = 6;
    [HideIf("hasUnlimitedMagazine", true)]
    public float weaponReloadTime = 0.8f;

    public float weaponFireRate = 0.2f;
    public bool semiAutomatic = false;

    public bool hasChargeUp = false;
    [ShowIf("hasChargeUp", true)]
    public float weaponChargeDelay = 0f;
    [ShowIf("hasChargeUp", true)]
    public bool chargePerShot = false;
    [ShowIf("hasChargeUp", true)]
    public bool fireOnRelease = false;
}
