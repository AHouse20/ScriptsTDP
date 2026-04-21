using UnityEngine;

[RequireComponent (typeof(AimWeaponEvent))]
[RequireComponent (typeof (SpriteRenderer))]
[DisallowMultipleComponent]
public class AimWeapon : MonoBehaviour
{
    [SerializeField] private Transform handPivot;
    private SpriteRenderer spriteRenderer;

    private AimWeaponEvent aimWeaponEvent;

    private void Awake()
    {
        aimWeaponEvent = GetComponent<AimWeaponEvent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;
    }

    private void OnDisable()
    {
        aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;
    }
    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        Aim(aimWeaponEventArgs.aimDirection, aimWeaponEventArgs.aimAngle);
    }

    private void Aim(Direction aimDirection, float aimAngle)
    {
        handPivot.eulerAngles = new Vector3(0f, 0f, aimAngle);
        switch (aimDirection)
        {
            case Direction.Left:
                handPivot.localScale = new Vector3(1f, -1f, 0f);
                spriteRenderer.flipX = true;
                break;
            case Direction.Right:
                handPivot.localScale = new Vector3(1f, 1f, 0f);
                spriteRenderer.flipX = false;
                break;
            default:
                break;
        }
    }
}
