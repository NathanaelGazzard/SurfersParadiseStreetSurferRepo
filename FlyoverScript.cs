using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FlyoverScript : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] Transform nodeContainer;
    List<Transform> nodes = new List<Transform>();

    [SerializeField] float speed = 1;
    [SerializeField] float turnSpeed = 0.5f;
    [SerializeField] float precisionDistance = 1f;

    int targetNode;


    void Awake()
    {
        foreach (Transform child in nodeContainer)
        {
            nodes.Add(child);
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            int newIndex = i + 1;
            if (newIndex == nodes.Count)
            {
                newIndex = 0;
            }
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.position = nodes[0].position;
        targetNode = 1;
    }


    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, nodes[targetNode].position) < precisionDistance)
        {
            targetNode++;
            if(targetNode == nodes.Count)
            {
                targetNode = 0;
            }
        }

        rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nodes[targetNode].position - transform.position), turnSpeed * Time.fixedDeltaTime));
        rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }
}

