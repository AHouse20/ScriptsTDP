using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public static class Utils
{
    public static Camera mainCamera;

    public static Vector3 GetCursorWorldPosition()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        Vector3 cursorScreenPosition = Mouse.current.position.ReadValue();

        cursorScreenPosition.x = Mathf.Clamp(cursorScreenPosition.x , 0f, Screen.width);
        cursorScreenPosition.y = Mathf.Clamp(cursorScreenPosition.y, 0f, Screen.height);

        Vector3 cursorWorldPosition = mainCamera.ScreenToWorldPoint(cursorScreenPosition);
        cursorScreenPosition.z = 0f;
        return cursorWorldPosition;
    }

    public static bool ValidationCheckEmptyString(Object obj, string fieldName, string stringToCheck)
    {
        if(stringToCheck == "")
        {
            Debug.Log(fieldName + " must not be empty on " + obj.name.ToString());
            return true;
        }
        return false;
    }

    public static bool ValidationCheckNull(Object obj, string fieldName, UnityEngine.Object objectToCheck)
    {
        if(objectToCheck == null)
        {
            Debug.Log(fieldName + " must not be null on " + obj.name.ToString());
            return true;
        }
        return false;
    }

    public static bool ValidaionCheckPositive(Object obj, string fieldName, int value, bool isZeroAllowed)
    {
        bool error = false;
        if (isZeroAllowed)
        {
            if(value < 0f)
            {
                Debug.LogWarning(fieldName + " must be >=0 on " + obj.name.ToString());
                error = true;
            }
        }
        else
        {
            if (value <= 0f)
            {
                Debug.LogWarning(fieldName + " must be >0 on " + obj.name.ToString());
                error = true;
            }
        }
        return error;
    }

    public static bool ValidationCheckPositive(Object obj, string fieldName, float value, bool isZeroAllowed)
    {
        bool error = false;
        if (isZeroAllowed)
        {
            if (value < 0f)
            {
                Debug.LogWarning(fieldName + " must be >=0 on " + obj.name.ToString());
                error = true;
            }
        }
        else
        {
            if (value <= 0f)
            {
                Debug.LogWarning(fieldName + " must be >0 on " + obj.name.ToString());
                error = true;
            }
        }
        return error;
    }


    public static bool ValidationCheckPositiveRange(Object obj, string fieldNameMin, float valueMin, string fieldNameMax, float valueMax, bool isZeroAllowed)
    {
        bool error = false;
        if(valueMin > valueMax)
        {
            Debug.LogError(fieldNameMin + " must me <= " + fieldNameMax + " on " + obj.name.ToString());
            error = true;
        }

        if(ValidationCheckPositive(obj, fieldNameMin, valueMin, isZeroAllowed)) { error = true; }

        if (ValidationCheckPositive(obj, fieldNameMax, valueMax, isZeroAllowed)) { error = true; }

        return error;
    }
    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2 (vector.y, vector.x);
        float degrees = radians * Mathf.Rad2Deg;

        return degrees;
    }

    public static Direction GetDirectionFromAngle(float angle)
    {
        if(angle >= -90 && angle <= 90)
        {
            return Direction.Right;
        }
        else
        {
            return Direction.Left;
        }
    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        Vector3 directionVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
        return directionVector;
    }
}
