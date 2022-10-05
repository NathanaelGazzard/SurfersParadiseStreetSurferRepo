using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    // >>> add acceleration

    float speedLimit = 25;
    public float speed = 25f;

    public bool isTailgaiting = false;

    float minSafeDist = 0.1f;

    Transform destinationNode;

    bool givingWay = false;

    float timeStopped = 0;

    [SerializeField] GameObject[] carModels;
    Transform carModel;
    [SerializeField] Transform rotTarg;

    [SerializeField] float turnSpeed;
    Vector3 lookTarg;

    bool isStuck = false;

    bool playerInRange = false;

    Transform playerRef;




    void Start()
    {
        int c = Random.Range(0, carModels.Length);
        carModel = GameObject.Instantiate(carModels[c], transform).transform;
        carModel.localPosition = Vector3.zero;

        rotTarg.LookAt(lookTarg);
        carModel.rotation = rotTarg.rotation;

        destinationNode = GetComponentInParent<CarSpawnScript>().GetStartNode();
        transform.position = destinationNode.position;
        
        destinationNode = destinationNode.GetComponent<TrafficNodeScript>().ChooseCarDestination();
    }


    void Update()
    {
        if (isStuck)
        {
            transform.position += Vector3.up * 15 * Time.deltaTime;
            timeStopped += Time.deltaTime;
            if(timeStopped > 3)
            {
                GetComponentInParent<CarSpawnScript>().carsLeftToSpawn++;
                Destroy(gameObject);
            }
        }
        else if (givingWay)
        {
            givingWay = !destinationNode.GetComponent<TrafficNodeScript>().SafeToTurn();
        }
        else
        {
            if (isTailgaiting)
            {
                speed = 0;
                timeStopped += Time.deltaTime;
                if(timeStopped > 8)
                {
                    isStuck = true;
                    timeStopped = 0;
                    carModel.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                timeStopped = 0;
                speed = speedLimit;
            }

            transform.position = Vector3.MoveTowards(transform.position, destinationNode.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, destinationNode.position) < minSafeDist)
            {
                RequestNewNode();
            }

            rotTarg.LookAt(lookTarg);
            carModel.rotation = Quaternion.RotateTowards(carModel.rotation, rotTarg.rotation, turnSpeed * Time.deltaTime);
        }
    }

    void RequestNewNode()
    {
        destinationNode = destinationNode.GetComponent<TrafficNodeScript>().ChooseCarDestination();
        lookTarg = new(destinationNode.position.x, transform.position.y, destinationNode.position.z);
        speedLimit = destinationNode.GetComponent<TrafficNodeScript>().speedLimit;

        if (!destinationNode.GetComponent<TrafficNodeScript>().mustGiveWay)
        {
            givingWay = true;
        }
    }
}
