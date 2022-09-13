using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficNodeScript : MonoBehaviour
{
    [SerializeField] float speedLimit = 10;

    public Transform[] possibleNodes = new Transform[1]; //all the possible nodes a car can go to once it reaches this one


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public Transform RequestNextNode()
    {
        Transform nextNode = possibleNodes[Random.Range(0, possibleNodes.Length)];
        return nextNode;
    }

    public float GetSpeedLimit()
    {
        return speedLimit;
    }
}
