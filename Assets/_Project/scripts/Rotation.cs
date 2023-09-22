using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] Transform RotationHandler;
    void Update()
    {
        float X = RotationHandler.up.x;
        float Z = RotationHandler.up.z;
        var _targetRotation = Mathf.Atan2(X, Z) * Mathf.Rad2Deg;
        Debug.Log(RotationHandler.rotation.eulerAngles.x);
        Debug.Log(RotationHandler.rotation.eulerAngles.z);
        float YAngle = _targetRotation - RotationHandler.rotation.eulerAngles.y;
        transform.localRotation = Quaternion.Euler(0.0f, YAngle, 0.0f);

    }
}
