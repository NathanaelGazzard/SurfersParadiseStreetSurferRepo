using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDistCheckScript : MonoBehaviour
{
    [SerializeField] Transform playerRef;
    [SerializeField] GameObject toggleObj;
    float interactDist = 4;
    [SerializeField] CompassScript compassScript;


    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(compassScript.GetWaypointLocation().position, playerRef.position) > interactDist)
        {
            toggleObj.SetActive(false);
        }
        else
        {
            toggleObj.SetActive(true);
        }
    }
}
