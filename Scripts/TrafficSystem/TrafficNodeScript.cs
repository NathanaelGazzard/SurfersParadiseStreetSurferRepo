using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrafficNodeScript : MonoBehaviour
{
    [SerializeField] bool isBranch = false;
    [SerializeField] Transform[] otherTrackNodes;
    TrafficStripScript parentRef;

    Transform[] branchNodes;

    public float speedLimit;

    public bool mustGiveWay = false;
    int carsInZone = 0;


    void Awake()
    {
        if (mustGiveWay || isBranch)
        {
            tag = "SpecTrafficNode";
        }
        else
        {
            tag = "TrafficNode";
        }
    }

    void Start()
    {
        parentRef = transform.parent.GetComponent<TrafficStripScript>();
        if(transform.GetSiblingIndex()+1 >= transform.parent.childCount)
        {
            branchNodes = new Transform[1];
            branchNodes[0] = parentRef.GetEndNode();
        }
        else if (isBranch)
        {
            if(otherTrackNodes == null)
            {
                print("Assign the other track nodes to " + gameObject.name + " Damnit!");
            }
            else
            {
                branchNodes = new Transform[otherTrackNodes.Length + 1];
                branchNodes[0] = parentRef.GetNextNode(transform);
                int i = 1;
                foreach (Transform node in otherTrackNodes)
                {
                    branchNodes[i] = node;
                    i++;
                }
            }
        }
        else
        {
            branchNodes = new Transform[1];
            branchNodes[0] = parentRef.GetNextNode(transform);
        }

        if(mustGiveWay)
        {
            if(GetComponent<BoxCollider>() == null)
            {
                print("No triggers detecting the give way zones for " + gameObject.name);
            }
            else
            {
                GetComponent<BoxCollider>().isTrigger = true;
            }
        }
    }


    public Transform ChooseCarDestination()
    {
        int nodeToChoose = Random.Range(0, branchNodes.Length);
        return branchNodes[nodeToChoose];
    }


    private void OnDrawGizmos()
    {
        if (isBranch)
        {
            foreach(Transform node in otherTrackNodes)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(transform.position, node.position);
            }
        }
        if (mustGiveWay)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 5);
        }
    }

    

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            carsInZone++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            carsInZone--;
        }
    }


    public bool SafeToTurn()
    {
        if(carsInZone == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
