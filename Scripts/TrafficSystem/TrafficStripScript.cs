using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficStripScript : MonoBehaviour
{
    [SerializeField] Color nodeColour = Color.yellow;
    [Range(0.5f, 2.5f)]
    [SerializeField] float nodeRadius = 1;
    [Tooltip("Does this track link one closed loop to another?")]
    [SerializeField] bool isLinkTrack = false;
    [Tooltip("Only assign if isLinkTrack == true")]
    [SerializeField] Transform endNode;



    public Transform GetNextNode(Transform currNode)
    {
        if (currNode == null)
        {
            return transform.GetChild(0);
        }
        else if(currNode.GetSiblingIndex() < transform.childCount - 1)
        {
            return transform.GetChild(currNode.GetSiblingIndex() + 1);
        }
        else
        {
            return transform.GetChild(0);
        }
    }


    private void Awake()
    {
        if (isLinkTrack && endNode == null)
        {
            print("Oi, dipshits, " + gameObject.name + " needs it's endNode assigned!");
        }else if (!isLinkTrack)
        {
            endNode = transform.GetChild(0);
        }
    }

    public Transform GetEndNode()
    {
        return endNode;
    }



    void OnDrawGizmos()
    {
        if(transform.childCount > 0)
        {
            Gizmos.color = nodeColour;
            foreach (Transform t in transform)
            {
                Gizmos.DrawSphere(t.position, nodeRadius);
            }

            for (int i = 0; i < transform.childCount - 1; i++)
            {
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
            }
            if (isLinkTrack)
            {
                Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, endNode.position);
            }
            else
            {
                Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
            }            
        }
    }
}
