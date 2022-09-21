using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedActivationScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ActivateObjects", 3);
    }



    void ActivateObjects()
    {
        foreach (Transform objectToActivate in transform)
        {
            objectToActivate.gameObject.SetActive(true);
        }
    }
}
