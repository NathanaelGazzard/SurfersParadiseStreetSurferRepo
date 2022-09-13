using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCamScript : MonoBehaviour
{
    [SerializeField] Transform ragdollRef;
    Vector3 offset = new Vector3(0, 4, 0);
    [SerializeField] float moveAwaySpeed = 2;


    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = ragdollRef.position + offset;

        offset.y = Mathf.Clamp(offset.y += moveAwaySpeed * Time.deltaTime, 4, 50);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + moveAwaySpeed * Time.deltaTime, transform.eulerAngles.z);
    }
}
