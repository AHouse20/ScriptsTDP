
using UnityEngine;

[DisallowMultipleComponent]
public class Ammo : MonoBehaviour, IFirable
{
    [Tooltip("Use a child object for trail")]
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private GameObject hitEffectPrefab;

    private float ammoRange = 0f;
    private float ammoSpeed;
    private Vector3 fireDirectionVector;
    private float fireDirectionAngle;
    private SpriteRenderer spriteRenderer;
    private AmmoDetailsSO ammoDetails;
    private float ammoDelayTimer;
    private bool isAmmoMaterialSet = false;
    private bool overrideAmmoMovement = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(ammoDelayTimer > 0)
        {
            ammoDelayTimer -= Time.deltaTime;
            return;
        }
        else if(!isAmmoMaterialSet)
        {
            SetAmmoMaterial(ammoDetails.ammoMaterial);
            isAmmoMaterialSet=true;
        }

        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;

        transform.position += distanceVector;

        ammoRange -= distanceVector.magnitude;

        if(ammoRange < 0f)
        {
            DisableAmmo();
        }
    }

    private void DisableAmmo()
    {
        if(hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, this.transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }

    private void SetAmmoMaterial(Material ammoMaterial)
    {
        spriteRenderer.material = ammoMaterial;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DisableAmmo();
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overrideAmmoMovement = false)
    {
        this.ammoDetails = ammoDetails;
        SetFireDirection(ammoDetails, aimAngle, weaponAngle, weaponAimDirectionVector);
        spriteRenderer.sprite = ammoDetails.ammoSprite;
        if (ammoDetails.ammoDelayTime > 0f)
        {
            ammoDelayTimer = ammoDetails.ammoDelayTime;
            SetAmmoMaterial(ammoDetails.ammoDelayMaterial);
            isAmmoMaterialSet = false;
        }
        else
        {
            ammoDelayTimer = 0f;
            SetAmmoMaterial(ammoDetails.ammoMaterial);
            isAmmoMaterialSet = true;
        }
        ammoRange = ammoDetails.ammoRange;
        this.ammoSpeed = ammoSpeed;
        this.overrideAmmoMovement = overrideAmmoMovement;
        gameObject.SetActive(true);

        ///

        if (ammoDetails.hasTrail)
        {
            trailRenderer.gameObject.SetActive(true);
            trailRenderer.emitting = true;
            trailRenderer.material = ammoDetails.ammoTrailMaterial;
            trailRenderer.startWidth = ammoDetails.ammoTrailStartWidth;
            trailRenderer.endWidth = ammoDetails.ammoTrailEndWidth;
            trailRenderer.time = ammoDetails.ammoTrailTime;
        }
        else
        {
            trailRenderer.emitting = false;
            trailRenderer.gameObject.SetActive(false);
        }
    }

    private void SetFireDirection(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAngle, Vector3 weaponAimDirectionVector)
    {
        float randomSpread = Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);
        int spreadToggle = Random.Range(0,2) * 2 - 1;

        if(weaponAimDirectionVector.magnitude < Settings.useAimAngleRange)
        {
            fireDirectionAngle = aimAngle;
        }
        else
        {
            fireDirectionAngle = weaponAngle;
        }

        fireDirectionAngle += spreadToggle * randomSpread;
        transform.eulerAngles = new Vector3(0f,0f,fireDirectionAngle);
        fireDirectionVector = Utils.GetVectorFromAngle(fireDirectionAngle);

    }
}
