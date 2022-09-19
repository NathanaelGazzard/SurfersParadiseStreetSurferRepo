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
    }

}
