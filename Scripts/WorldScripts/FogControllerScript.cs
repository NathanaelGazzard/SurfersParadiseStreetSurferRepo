using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FogControllerScript : MonoBehaviour
{
    [SerializeField] Transform playerRef;
    [SerializeField] Transform minDistMarker;
    [SerializeField] Transform maxDistMarker;
    [SerializeField] Image fogUI;
    float minFogDist;
    float maxFogDist;
    float fogRange;
    Color fogColor;


    void Start()
    {
        minFogDist = Vector3.Distance(transform.position, minDistMarker.position);
        maxFogDist = Vector3.Distance(transform.position, maxDistMarker.position);
        fogRange = maxFogDist - minFogDist;
        fogColor = fogUI.color;
    }


    // Update is called once per frame
    void Update()
    {
        float playerDist = Vector3.Distance(transform.position, playerRef.position);
        if (playerDist > minFogDist)
        {
            fogColor.a = (playerDist - minFogDist) / fogRange;//bung
            fogUI.color = fogColor;
        }
        else
        {
            fogColor.a = 0;
            fogUI.color = fogColor;
        }
    }
}
