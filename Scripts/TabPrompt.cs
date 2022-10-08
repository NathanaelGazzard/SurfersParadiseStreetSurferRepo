using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabPrompt : MonoBehaviour
{
    [SerializeField] GameObject promptText;
    float delayLength = 2.5f;



    // Start is called before the first frame update
    void Start()
    {
        Invoke("EnablePrompt", delayLength);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Destroy(gameObject);
        }
    }


    void EnablePrompt()
    {
        promptText.SetActive(true);
    }
}
