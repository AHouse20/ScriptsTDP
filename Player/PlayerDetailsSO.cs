using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerDetails_", menuName = "Scriptable Objects/PlayerDetails")]
public class PlayerDetailsSO : ScriptableObject
{
    public string playerCharacterName;
    public GameObject playerPrefab;
    public RuntimeAnimatorController runtimeAnimatorController;

    public int playerMaxHealth;

    public WeaponDetailsSO startingWeapon;
    public List<WeaponDetailsSO> startingWeapons;

    public Sprite minimapIcon;
    public Sprite handSprite;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        Utils.ValidationCheckEmptyString(this, nameof(playerCharacterName), playerCharacterName);
        Utils.ValidationCheckNull(this, nameof(playerPrefab), playerPrefab);
        Utils.ValidationCheckPositive(this, nameof (playerMaxHealth), playerMaxHealth, false);
        Utils.ValidationCheckNull(this,nameof(startingWeapon), startingWeapon);
        Utils.ValidationCheckNull(this,nameof (minimapIcon), minimapIcon);
        Utils.ValidationCheckNull(this,nameof(handSprite), handSprite);
        Utils.ValidationCheckNull(this,nameof(runtimeAnimatorController), runtimeAnimatorController);
        /// COME BACK
    }
#endif
    #endregion
}
