using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Competitor : MonoBehaviour
{
    
    // [SerializeField]
    // GameObject competitorModel;

    [SerializeField] NavMeshAgent myNavAgent;


    private float startDelay;

    public GameObject spawnPointsContainer;
    private Vector3[] spawnPoints;
    private Vector3 actualSpawnPoint;
    private int spawnPointsNum;

    public GameObject destinationPointsContainer;
    private Vector3[] destinationPoints;
    private Vector3 actualDestination;
    private int destinationPointsNum;

    private float totalDistance;
    private float remainingPercentage;

    public Transform playerScript;
    
    private Vector3 testPos = new Vector3(1732.0f, 11.0f, 2605.0f);

    private void Start(){
        GetAllSpawnPoints();
        GetAllDestinationPoints();
        
        //OnRoadDestination(testPos);
        FindClosestPoint(testPos);
        
        PickSpawnPoint();

        transform.position = actualSpawnPoint;
        totalDistance = Vector3.Distance(transform.position, actualDestination);

        // competitorModel.SetActive();

        //startDelay = 20f;
        myNavAgent.SetDestination(actualDestination);
    }
    
    // find all the postion (Vector3 value) of possible spawn points and store them in spawnPoints list.
    private void GetAllSpawnPoints(){
        spawnPointsNum = spawnPointsContainer.transform.childCount;
        spawnPoints = new Vector3[spawnPointsNum];
        for (int i = 0; i<spawnPointsNum; i++){
            Transform spawnPointPosition = spawnPointsContainer.transform.GetChild(i);
            spawnPoints[i] = spawnPointPosition.position;
        }
    }

    //find all the position of (Vector3 value) of all possible destinatino points and store them in destinationPoints list.
    private void GetAllDestinationPoints(){
        destinationPointsNum = destinationPointsContainer.transform.childCount;
        destinationPoints = new Vector3[destinationPointsNum];
        for (int i = 0; i<destinationPointsNum; i++){
            Transform destinationPointPosition = destinationPointsContainer.transform.GetChild(i);
            destinationPoints[i] = destinationPointPosition.position;
        }
    }


    // call this function and pass it the player mission destination as a parameter. 
    private void OnRoadDestination(Vector3 targetDestination)
    {
        Vector3 roadDestination = Vector3.zero;

            NavMeshHit hit;

            // find the navmesh point closest to the new point
            if (NavMesh.SamplePosition(targetDestination, out hit, 15f, 16))
            {
                roadDestination = hit.position;
            }

        // this is the actual on-road destination for the competitor
        actualDestination =  roadDestination;
    }

    // Since the competitor can only travel on roads, we cannot use the actual client position
    // and need to use substitute goal points to say that the competitor reached the goal.
    // find the destination point that is closest to the position of the client.
    private void FindClosestPoint(Vector3 clientPosition){
        float distanceToGoal = Mathf.Infinity;
        foreach (Vector3 destination in destinationPoints){
           if (Vector3.Distance(destination, clientPosition) < distanceToGoal){
                distanceToGoal = Vector3.Distance(destination, clientPosition);
                actualDestination = destination;
           }
        }
    }  

    // Based on the destination points, find the furthest spawn points from all the possible spawn points.
    // distance can be changed
    private void PickSpawnPoint(){
        float distanceToActualDestination = 0f;
        
        foreach (Vector3 spawnPoint in spawnPoints){
            if (Vector3.Distance(spawnPoint, actualDestination) > distanceToActualDestination){
                distanceToActualDestination = Vector3.Distance(spawnPoint, actualDestination);
                actualSpawnPoint = spawnPoint;
            }
        }
    }
    


    // Update is called once per frame
    void Update()
    {
        /*
        if (Time.deltaTime >= startDelay){
            myNavAgent.SetDestination(actualDestination);
        } else 
        */
        
        if (Vector3.Distance(transform.position, actualDestination) <=  1.0f){
            ReachedFirst();
        }
        remainingPercentage = Vector3.Distance(transform.position, actualDestination) / totalDistance;
    } 

    // 
    public float GetRemainingDistanceInPercentage(){
        return remainingPercentage;
    }

    // if the competitor reaches the destination first
    // simply kills the main character...for now.
    void ReachedFirst(){
        playerScript.GetComponent<CharacterControllerScript>().ModifyHealth(-100);
    }

    void MissionFailed(){
        // for now
        Destroy(gameObject);
        Debug.Log("Competitor destroyed.");
    }





}

// Need to fix:
// Vector3.Distance(transform, actualDestination) can be replaced.
// Competitor should only start moving after startDelay but since Navmesh.SetDestination is defined in the start() it starts moving when the game begins.