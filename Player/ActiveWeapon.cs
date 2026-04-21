using FMODUnity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SetActiveWeaponEvent))]
[DisallowMultipleComponent]
public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;
    [SerializeField] private PolygonCollider2D weaponPolygonCollider;
    [SerializeField] private Transform weaponShootPosition;
    [SerializeField] private Transform weaponEffectPosition;
    [SerializeField] private SpriteRenderer offHand;

    private SetActiveWeaponEvent setWeaponEvent;
    private Weapon activeWeapon;

    private void Awake()
    {
        setWeaponEvent = GetComponent<SetActiveWeaponEvent>();
    }

    private void OnEnable()
    {
        setWeaponEvent.OnSetActiveWeapon += SetActiveWeaponEvent_OnSetActiveWeapon;
    }

    private void OnDisable()
    {
        setWeaponEvent.OnSetActiveWeapon -= SetActiveWeaponEvent_OnSetActiveWeapon;
    }


    private void SetActiveWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent @event, SetActiveWeaponEventArgs args)
    {
        SetWeapon(args.weapon);
    }

    private void SetWeapon(Weapon weapon)
    {
        activeWeapon = weapon;
        weaponSpriteRenderer.sprite = activeWeapon.weaponDetails.weaponSprite;
        if(weaponPolygonCollider != null && weaponSpriteRenderer.sprite != null)
        {
            List<Vector2> spritePhysicsShapePoints = new List<Vector2>();
            weaponSpriteRenderer.sprite.GetPhysicsShape(0, spritePhysicsShapePoints);
            weaponPolygonCollider.points = spritePhysicsShapePoints.ToArray();
        }
        if (weapon.weaponDetails.isTwoHanded)
        {
            offHand.enabled = true;
            offHand.sprite = GetComponent<Player>().playerDetails.handSprite;
            offHand.transform.localPosition = weapon.weaponDetails.offHandPosition;
        }
        else
        {
            offHand.enabled = false;
        }
            weaponShootPosition.localPosition = activeWeapon.weaponDetails.weaponShootPosition;
    }
    public AmmoDetailsSO GetCurrentAmmo()
    {
        return activeWeapon.weaponDetails.weaponAmmo;
    }

    public Weapon GetActiveWeapon()
    {
        return activeWeapon;
    }

    public Vector3 GetShootPosition()
    {
        return weaponShootPosition.position;
    }
    public Vector3 GetEffectPosition()
    {
        return weaponEffectPosition.position;
    }

    public void RemoveActiveWeapon()
    {
        activeWeapon = null;
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        Utils.ValidationCheckNull(this, nameof(weaponSpriteRenderer),weaponSpriteRenderer);
        Utils.ValidationCheckNull(this, nameof(weaponPolygonCollider), weaponPolygonCollider);
        Utils.ValidationCheckNull(this, nameof(weaponShootPosition), weaponShootPosition);
        Utils.ValidationCheckNull(this, nameof(weaponEffectPosition), weaponEffectPosition);
    }
#endif
    #endregion
}
