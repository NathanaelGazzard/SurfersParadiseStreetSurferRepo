using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    [SerializeField] Transform destinationNode; // set this with the vehicle spawner (part of node script)
    Rigidbody rb;
    float speed = 10;
    [SerializeField] float baseTurnSpeed = 20;
    float turnSpeed;
    [SerializeField] float turnSpeedPaceModifier = 0.2f;
    float precisionDistance = 1;


    private void Start()
    {
        turnSpeed = baseTurnSpeed;
        speed = destinationNode.GetComponent<TrafficNodeScript>().GetSpeedLimit();
        rb = GetComponent<Rigidbody>();
        rb.rotation = Quaternion.LookRotation(destinationNode.position - transform.position);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, destinationNode.position) < precisionDistance)
        {
            destinationNode = destinationNode.GetComponent<TrafficNodeScript>().RequestNextNode();
            speed = destinationNode.GetComponent<TrafficNodeScript>().GetSpeedLimit();
            if(speed > baseTurnSpeed)
            {
                turnSpeed = baseTurnSpeed + speed * turnSpeedPaceModifier;
            }
            else
            {
                turnSpeed = baseTurnSpeed;
            }
        }
    }



    private void FixedUpdate()
    {


        rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(destinationNode.position - transform.position), turnSpeed * Time.fixedDeltaTime));
        rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }
}
