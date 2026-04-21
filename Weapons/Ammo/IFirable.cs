using UnityEngine;

public interface IFirable
{
    void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overrideAmmoMovement = false);

    GameObject GetGameObject();
}
