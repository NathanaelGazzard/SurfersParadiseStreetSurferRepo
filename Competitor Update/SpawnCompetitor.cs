using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnCompetitor : MonoBehaviour
{
    // [SerializeField]
    // GameObject competitorModel;
    [SerializeField] GameObject competitorPrefab;

    public GameObject spawnPointsContainer;
    private Vector3[] spawnPoints;
    private Vector3 actualSpawnPoint;
    private int spawnPointsNum;

    public GameObject destinationPointsContainer;
    private Vector3[] destinationPoints;
    private Vector3 actualDestination;
    private int destinationPointsNum;

    private float totalDistance;


    public Vector3 clientPosition = new Vector3(1732.0f, 11.0f, 2605.0f);
    private Quaternion rotation = Quaternion.Euler(0, 0, 0);

    // Start is called before the first frame update
    private void Start(){
        InitiateCompetitor(clientPosition);
    }

    public void InitiateCompetitor(Vector3 goal){
        GetAllSpawnPoints();
        GetAllDestinationPoints();
        
        //OnRoadDestination(testPos);
        FindClosestPoint(goal);

        PickSpawnPoint();
        Instantiate(competitorPrefab, actualSpawnPoint, rotation);
        GameObject.FindGameObjectWithTag("CompetitorNotification").GetComponent<CompetitorNotification>().ShowContainer();
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
            if (UnityEngine.AI.NavMesh.SamplePosition(targetDestination, out hit, 15f, 41))
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
    
    public Vector3 GetSpawnPoint(){
        return actualSpawnPoint;
    }

    public Vector3 GetDestination(){
        return actualDestination;
    }


}
