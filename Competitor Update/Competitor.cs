using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Competitor : MonoBehaviour
{
    [SerializeField] NavMeshAgent myNavAgent;

    private float startDelay = 5.0f;
    private float delayTimer = 0;

    private float totalDistance;
    private float remainingPercentage;

    
    private Vector3 actualSpawnPoint;
    private Vector3 actualDestination;

    public bool goodsDelivered = false;

    private void Start(){
        actualDestination = GameObject.FindGameObjectWithTag("CompetitorObjects").GetComponent<SpawnCompetitor>().GetDestination();   
        totalDistance = Vector3.Distance(transform.position, actualDestination);
    }

    private bool DelayCheck(float delayLength)
    {
        delayTimer += Time.deltaTime;
        if (delayTimer >= delayLength)
        {
            return true;
        } else {
            return false;
        }
    }


    // Update is called once per frame
    void Update()
    {   
        // starts moving after startDelay(5sec) 
        if (DelayCheck(startDelay)){
            myNavAgent.SetDestination(actualDestination);
        } 

        //
        if (goodsDelivered){
            MissionFailed();
        }

        // if competitor gets to the destination first
        if (Vector3.Distance(transform.position, actualDestination) <=  1.0f){
            CompetitorReachedFirst();
        }
        remainingPercentage = Vector3.Distance(transform.position, actualDestination) / totalDistance;
        
    } 

    // 
    public float GetRemainingDistanceInPercentage(){
        return remainingPercentage;
    }

    public void PlayerReachedFirst(){
        goodsDelivered = true;
    }

    // if the competitor reaches the destination first
    // simply kills the main character...for now.
    private void CompetitorReachedFirst(){
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControllerScript>().ModifyHealth(-100);
        GameObject.FindGameObjectWithTag("CompetitorNotification").GetComponent<CompetitorNotification>().ResetContainer(); 
    }

    private void MissionFailed(){
        // for now
        Destroy(gameObject);
        Debug.Log("Competitor destroyed.");
        GameObject.FindGameObjectWithTag("CompetitorNotification").GetComponent<CompetitorNotification>().ResetContainer(); 
    }

}
