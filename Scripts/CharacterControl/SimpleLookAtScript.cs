using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLookAtScript : MonoBehaviour
{    public float GetRotation(Transform targetTransform)
    {
        Vector3 lookTarg = new(targetTransform.position.x, transform.position.y, targetTransform.position.z);
        transform.LookAt(lookTarg);
        return transform.eulerAngles.y;
    }
}
