
using UnityEngine;

public static class Settings
{
    #region
    public static int aimRight = Animator.StringToHash("AimRight");
    public static int aimLeft = Animator.StringToHash("AimLeft");
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int isWalking = Animator.StringToHash("isWalking");
    #endregion

    #region
    public const float useAimAngleRange = 0.5f; 
    #endregion
}
