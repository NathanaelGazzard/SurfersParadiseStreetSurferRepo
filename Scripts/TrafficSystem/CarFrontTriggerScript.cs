using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFrontTriggerScript : MonoBehaviour
{
    CarScript carScript;
    Transform carInFront;
    // Start is called before the first frame update
    void Start()
    {
        carScript = transform.parent.GetComponent<CarScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (carInFront == null && other.CompareTag("Car"))
        {
            if (carScript.CheckIfTailgating(other.transform))
            {
                carInFront = other.transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == carInFront)
        {
            carInFront = null;
        }
        {
            if (carScript.CheckIfTailgating(other.transform))
            {
                carInFront = other.transform;
            }
        }
    }
}
