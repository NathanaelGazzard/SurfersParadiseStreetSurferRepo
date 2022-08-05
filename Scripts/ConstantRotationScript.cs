using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotationScript : MonoBehaviour
{
    [SerializeField] float xRotSpeed = 0f;
    [SerializeField] float yRotSpeed = 0f;
    [SerializeField] float zRotSpeed = 0f;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(xRotSpeed * Time.deltaTime, yRotSpeed * Time.deltaTime, zRotSpeed * Time.deltaTime);
    }
}
