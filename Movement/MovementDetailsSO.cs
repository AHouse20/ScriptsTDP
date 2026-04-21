using UnityEngine;

[CreateAssetMenu(fileName = "MovementDetails_", menuName = "Scriptable Objects/MovementDetails")]
public class MovementDetailsSO : ScriptableObject
{
    public float minMoveSpeed = 0f;
    public float maxMoveSpeed = 0f;

    public float GetMoveSpeed()
    {
        if(minMoveSpeed == maxMoveSpeed)
        {
            return minMoveSpeed;
        }
        else
        {
            return Random.Range(minMoveSpeed, maxMoveSpeed);
        }
    }

    private void OnValidate()
    {
        Utils.ValidationCheckPositiveRange(this, nameof(minMoveSpeed), minMoveSpeed, nameof(maxMoveSpeed), maxMoveSpeed, false);
    }
}
