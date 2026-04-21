using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDetails_", menuName = "Scriptable Objects/AmmoDetails")]
public class AmmoDetailsSO : ScriptableObject
{
    public string ammoName;
    public bool isPlayerAmmo;

    public Sprite ammoSprite;
    public GameObject[] ammoPrefabs;
    public Material ammoMaterial;
    public bool isDelayed = false;
    [ShowIf("isDelayed", true)]
    public float ammoDelayTime = 0f;
    [ShowIf("isDelayed", true)]
    public Material ammoDelayMaterial;

    public int ammoDamage = 1;
    public float ammoSpeedMin = 20f;
    public float ammoSpeedMax = 20f;
    public float ammoRange = 20f;
    public float ammoRotationSpeed = 0f;

    public float ammoSpreadMin = 0f;
    public float ammoSpreadMax = 0f;

    public int ammoSpawnCountMin = 1;
    public int ammoSpawnCountMax = 1;
    public float ammoSpawnIntervalMin = 0f;
    public float ammoSpawnIntervalMax = 0f;

    public bool hasTrail = false;
    [ShowIf("hasTrail", true)]
    public float ammoTrailTime = 3f;
    [ShowIf("hasTrail", true)]
    public Material ammoTrailMaterial;
    [ShowIf("hasTrail", true)]
    [Range(0,1)]
    public float ammoTrailStartWidth;
    [ShowIf("hasTrail", true)]
    [Range(0, 1)]
    public float ammoTrailEndWidth;
}
