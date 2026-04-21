using UnityEngine;

[CreateAssetMenu(fileName = "CurrentPlayer_", menuName = "Scriptable Objects/CurrentPlayer")]
public class CurrentPlayerSO : ScriptableObject
{
    public PlayerDetailsSO playerDetails;
    public string playerName;
}
