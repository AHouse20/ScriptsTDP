using UnityEngine;
using UnityEngine.InputSystem;

public class CursorWorldSpace : MonoBehaviour
{
    private void Update()
    {
        transform.position = Utils.GetCursorWorldPosition();
    }
}
