using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    // >>> add acceleration

    float speedLimit = 25;
    public float speed = 25f;
    CarScript matchSpeed;


    float minSafeDist = 0.1f;

    [SerializeField] Transform startNode;

    Transform destinationNode;




    // ------------------------ VISUAL ELEMENTS ------------------------------------------------------------------

    [SerializeField] GameObject[] carModels;
    Transform carModel;
    [SerializeField] Transform rotTarg;

    [SerializeField] float turnSpeed;
    Vector3 lookTarg;








    void Start()
    {
        destinationNode = startNode;
        transform.position = destinationNode.position;

        destinationNode = destinationNode.GetComponent<TrafficNodeScript>().ChooseCarDestination();


        // ------------------------ VISUAL ELEMENTS ------------------------------------------------------------------
        int c = Random.Range(0, carModels.Length);
        carModel = GameObject.Instantiate(carModels[c], transform).transform;
        carModel.localPosition = Vector3.zero;
    }


    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destinationNode.position, speed * Time.deltaTime);
        if(Vector3.Distance(transform.position, destinationNode.position) < minSafeDist)
        {
            destinationNode = destinationNode.GetComponent<TrafficNodeScript>().ChooseCarDestination();
            lookTarg = new(destinationNode.position.x, transform.position.y, destinationNode.position.z);
            speedLimit = destinationNode.GetComponent<TrafficNodeScript>().speedLimit;
        }

        rotTarg.LookAt(lookTarg);
        carModel.rotation = Quaternion.RotateTowards(carModel.rotation, rotTarg.rotation, turnSpeed * Time.deltaTime);

        if(matchSpeed != null)
        {
            speed = matchSpeed.speed;
        }
        else
        {
            speed = speedLimit;
        }
    }


    public bool CheckIfTailgating(Transform carDetected)
    {
        if (carDetected.IsChildOf(transform.parent))
        {
            matchSpeed = carDetected.GetComponent<CarScript>();
            return true;
        }
        else
        {
            return false;
        }
    }
}
