using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(PlayerControl))]
[RequireComponent(typeof(Idle))]
[RequireComponent(typeof(IdleEvent))]
[RequireComponent(typeof(MovementByVelocityEvent))]
[RequireComponent(typeof(MovementByVelocity))]
[RequireComponent(typeof(AimWeapon))]
[RequireComponent(typeof(AimWeaponEvent))]
[RequireComponent(typeof(AnimatePlayer))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(SetActiveWeaponEvent))]
[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(FireWeapon))]
[RequireComponent(typeof(WeaponFiredEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(WeaponReloadedEvent))]
[RequireComponent(typeof(ReloadWeapon))]
[RequireComponent(typeof(StudioEventEmitter))]

[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private PlayerDetailsSO tempPlayerDetails;

    private PlayerInput input;
    [HideInInspector] public bool alive = true;

    [HideInInspector] public StudioEventEmitter emitter;
    [HideInInspector] public SetActiveWeaponEvent setActiveWeaponEvent;
    [HideInInspector] public FireWeaponEvent fireWeaponEvent;
    [HideInInspector] public WeaponFiredEvent weaponFiredEvent;
    [HideInInspector] public ReloadWeaponEvent reloadWeaponEvent;
    [HideInInspector] public WeaponReloadedEvent weaponReloadedEvent;
    [HideInInspector] public ActiveWeapon activeWeapon;
    [HideInInspector] public PlayerDetailsSO playerDetails;
    [HideInInspector] public Health health;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;
    [HideInInspector] public IdleEvent idleEvent;
    [HideInInspector] public MovementByVelocityEvent movementByVelocityEvent;
    [HideInInspector] public AimWeaponEvent aimWeaponEvent;
    public Transform handPivot;

    public List<Weapon> weapons = new List<Weapon>();

    private void Awake()
    {
        emitter = GetComponent<StudioEventEmitter>();
        setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
        activeWeapon = GetComponent<ActiveWeapon>();
        health = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        idleEvent = GetComponent<IdleEvent>();
        movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
        aimWeaponEvent = GetComponent<AimWeaponEvent>();
    }

    private void Start()
    {
        Initialize(tempPlayerDetails);
    }
    public void Initialize(PlayerDetailsSO playerDetails)
    {
        this.playerDetails = playerDetails;
        CreateStartingWeapons();
        SetPlayerHealth();
    }

    private void CreateStartingWeapons()
    {
        weapons.Clear();
        foreach (WeaponDetailsSO weaponDetails in playerDetails.startingWeapons)
        {
            AddWeapon(weaponDetails);
        }
    }

    private Weapon AddWeapon(WeaponDetailsSO weaponDetails)
    {
        Weapon weapon = new Weapon()
        {
            weaponDetails = weaponDetails,
            weaponReloadTimer = 0f,
            weaponMagRemainingAmmo = weaponDetails.weaponMagazineCapacity,
            weaponRemainingAmmo = weaponDetails.weaponMaxAmmo,
            isReloading = false
        };
        weapons.Add(weapon);
        weapon.weaponListPosition = weapons.Count;
        setActiveWeaponEvent.CallSetActiveWeaponEvent(weapon);
        return weapon;
    }

    private void SetPlayerHealth()
    {
        health.SetStartingHealth(playerDetails.playerMaxHealth);
    }
    public void SwitchWeapon(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && alive)
        {
            animator.SetTrigger("Die");
            alive = false;
            rb.Sleep();
            handPivot.gameObject.SetActive(false);
            Invoke("PostDeath", 0.75f);
        }
    }
    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && alive)
        {
            animator.SetTrigger("Die");
            alive = false;
            rb.Sleep();
            handPivot.gameObject.SetActive(false);
            Invoke("PostDeath", 0.75f);
        }
    }

    IEnumerator PostDeath()
    {
        GetComponent<AimWeapon>().enabled = false;
        GetComponent<ShadowCaster2D>().enabled = false;
        return null;
    }
    
    private void Update()
    {
        if(rb.linearVelocity.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
   
}
