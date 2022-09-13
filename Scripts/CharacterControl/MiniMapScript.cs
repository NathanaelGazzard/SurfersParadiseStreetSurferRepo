using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    [SerializeField] Vector3 offsetValue;

    [SerializeField] Transform playerCamRootRef;

    [SerializeField] Transform cameraRef;

    float camHeight;


    // Start is called before the first frame update
    void Start()
    {
        transform.position += offsetValue; // sets the duplicate map to its offset location
        camHeight = cameraRef.position.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newCamPos = playerCamRootRef.position + offsetValue;
        newCamPos.y = camHeight;
        cameraRef.position = newCamPos;
        cameraRef.eulerAngles = new Vector3(90, playerCamRootRef.eulerAngles.y, 0);
    }
}
