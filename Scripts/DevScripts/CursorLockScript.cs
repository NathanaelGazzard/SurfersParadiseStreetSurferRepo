using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLockScript : MonoBehaviour
{
    //place this script on any object in a scene to ensure the cursor is locked at the start (this function is already inlcuded in the games manager script so this script is only for scenes that do not contain it)


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
