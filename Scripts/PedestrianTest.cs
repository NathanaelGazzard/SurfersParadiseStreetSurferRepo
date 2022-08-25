using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianTest : MonoBehaviour
{
    // >>> figure out way to constrain actor to a limited distance from their spawn location?

    [SerializeField] GameObject[] pedestrianModels; // each pedestrian contains all possible pedestrian models
    int modelToUse; // this is will be assigned via a random function and will determine which of the pedestrian models will be activated

    // the pedestrian animations have have blend trees for both walking and running. The following variables are assigned via random at
    // runtime so that each pedestrian will have a slightly different walk and run animation
    float walkType; 
    float runType;

    Animator modelAnimator;

    [SerializeField] NavMeshAgent myNavAgent;

    // max distance a pedestrian will pick their new point 
    [SerializeField] float maxWalkDist;

    Vector3 destination;

    public Transform playerRef; // >>> set this up to be assigned by the pedestrian spawner

    float chaseDist = 50f; // the max distance between the pedestrian and player before the pedestrian gives up on chasing the player

    float defaultWalkSpeed; // the navmeshagent's walkspeed

    // these variables are used for delays via the DelayCheck function
    float delayTimer = 0;
    float delayLength;

    int state = 0; // set up enum for the states (Walking, resting, knocked, chasing, frozen)

    bool holdsGrudge = false; // this bool is here to use if we want AI memory. Eg, if the player has previously hit them, the AI might yell if they get too close in future and may even start chasing again



    void Start()
    {
        defaultWalkSpeed = myNavAgent.speed;
        NewDestination();
        myNavAgent.SetDestination(destination);

        modelToUse = Random.Range(0, pedestrianModels.Length);
        pedestrianModels[modelToUse].SetActive(true);
        walkType = Random.Range(0, 1);
        runType = Random.Range(0, 1);

        modelAnimator = pedestrianModels[modelToUse].GetComponent<Animator>();
        modelAnimator.SetFloat("WalkType", walkType);
        modelAnimator.SetFloat("RunType", runType);
    }



    void Update()
    {
        switch (state)
        {
            case 3:
                ChasePlayer();
                break;
            case 2:
                Knocked();
                break;
            case 1:
                PhoneBreak();
                break;
            case 0:
                Walking();
                break;
            default:
                state = 0;
                break;
        }
    }



    void NewDestination()
    {
        destination = Vector3.zero;        

        while (destination == Vector3.zero)
        {
            Vector3 randomPoint;
            NavMeshHit hit;

            // pick a random point within the max walk distance
            randomPoint = transform.position + Random.insideUnitSphere * maxWalkDist;

            // find the navmesh point closest to the new point
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                destination = hit.position;
            }
        }
        //set the new destination
        myNavAgent.SetDestination(destination);
    }



    void PhoneBreak()
    {
        modelAnimator.SetTrigger("CheckPhone");
        if (DelayCheck())
        {
            modelAnimator.ResetTrigger("CheckPhone");
            NewDestination();
            state = 0;
        }
    }


    void Knocked()
    {
        if (DelayCheck())
        {
            delayLength = 0.5f; // this will determine how frequently the pedestrian will recalculate their path. The lower the value, the more accurately it will track the player but at the cost of performance
            delayTimer = delayLength; // this is initialised to the time limit so that it will set the players position immediately then check at intervals
            myNavAgent.speed = 5;
            state = 3;
        }
    }


    void ChasePlayer()
    {
        //checks players position at intervals (length of intervals defined in Knocked() function
        if (DelayCheck())
        {
            delayTimer = 0;
            myNavAgent.SetDestination(playerRef.position);
        }

        float distToPlayer = Vector3.Distance(transform.position, playerRef.position);

        // if the player gets too far away, the pedestrian will stop chasing. If they get close enough, they will punch the player (KO).
        if (distToPlayer > chaseDist)
        {
            modelAnimator.SetTrigger("StopChasing");
            myNavAgent.speed = defaultWalkSpeed;
            myNavAgent.SetDestination(destination);
            state = 0;
        }
        else if(distToPlayer < 1.5f)
        {
            modelAnimator.SetTrigger("Punch");
            delayTimer = 0;
            delayLength = 0.3f;
            playerRef.GetComponent<CharacterControllerScript>().Wasted();
            myNavAgent.speed = defaultWalkSpeed;
            myNavAgent.SetDestination(destination);
            state = 0;
        }
    }




    void Walking()
    {
        if (Vector3.Distance(transform.position, destination) < 2)
        {
            delayTimer = 0;
            delayLength = 4;
            state = 1;
        }
    }



    // is the player bumps into the pedestrian, they will get real angry
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && state < 2)
        {
            myNavAgent.speed = 0; // cannot move while staggered
            modelAnimator.SetTrigger("Knocked");
            holdsGrudge = true;
            delayLength = 1.4f; //length of the knocked animation
            delayTimer = 0;
            state = 2;
        }
    }


    // this function can be called by an if statement in update and will only activate after a delay the length of delayLength
    // (provided delayTimer is reset to zero before this function is repeatedly called)
    bool DelayCheck()
    {
        delayTimer += Time.deltaTime;
        if (delayTimer > delayLength)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
