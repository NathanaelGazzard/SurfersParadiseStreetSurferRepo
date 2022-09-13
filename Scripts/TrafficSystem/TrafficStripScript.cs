using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficStripScript : MonoBehaviour
{
    List<Transform> trafficNodes = new List<Transform>();

    void Awake()
    {
        // fills the traffic node list with all the children of this track object
        foreach (Transform child in transform)
        {
            trafficNodes.Add(child);
        }

        // sets the default next node for each node
        for (int i = 0; i < trafficNodes.Count; i++)
        {
            if( i + 1 == trafficNodes.Count)
            {
                trafficNodes[i].GetComponent<TrafficNodeScript>().possibleNodes[0] = trafficNodes[0];
            }
            else
            {
                trafficNodes[i].GetComponent<TrafficNodeScript>().possibleNodes[0] = trafficNodes[i + 1];
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
