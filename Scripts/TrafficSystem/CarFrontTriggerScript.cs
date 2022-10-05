using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFrontTriggerScript : MonoBehaviour
{
    CarScript carScript;
    int carsInFront = 0;



    void Start()
    {
        carScript = transform.parent.GetComponent<CarScript>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car") && other.transform != transform.parent)
        {
            carsInFront++;
            carScript.isTailgaiting = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car") && other.transform != transform.parent)
        {
            carsInFront--;
            if (carsInFront == 0)
            {
                carScript.isTailgaiting = false;
            }
        }
    }
}
