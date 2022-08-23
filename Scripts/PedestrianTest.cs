using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianTest : MonoBehaviour
{
    // >>> figure out way to constrain actor to a limited distance from their spawn location

    Transform playerRef;
    Vector3 destination;
    [SerializeField] NavMeshAgent myNavAgent;
    [SerializeField] float minWalkDist;
    [SerializeField] float maxWalkDist;

    float chaseDist = 10f;

    float defaultWalkSpeed;

    float delayTimer = 0;
    float delayLength;
    int state = 0; //set up enum for the states (Walking, resting, knocked, chasing, frozen)

    bool hasBeenHitByPlayer = false; //this bool is here to use if I want AI memory. Eg, if the player has previously hit them, the AI might yell if they get close and may even start chasing again


    // Start is called before the first frame update
    void Start()
    {
        defaultWalkSpeed = myNavAgent.speed;
        NewDestination();
        myNavAgent.SetDestination(destination);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 3:
                ChasePlayer();
                break;
            case 2:
                KnockedOver();
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
        float range = Random.Range(minWalkDist, maxWalkDist);
        while (destination == Vector3.zero)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                destination = hit.position;
            }
        }
        myNavAgent.SetDestination(destination);
    }

    void PhoneBreak()
    {
        print("waiting");
        if (DelayCheck())
        {
            NewDestination();
            state = 0;
        }
    }


    void KnockedOver()
    {
        if (DelayCheck())
        {
            //start chase animation
            delayLength = 1;//this will determine how frequently the pedestrian will recalculate their path. The lower the value, the more accurately it will track the player but at the cost of performance
            myNavAgent.speed = 5;
            state = 3;
        }
    }


    void ChasePlayer()
    {
        if (DelayCheck())
        {
            delayTimer = 0;
            myNavAgent.SetDestination(playerRef.position);
        }

        float distToPlayer = Vector3.Distance(transform.position, playerRef.position);

        if (distToPlayer > chaseDist)
        {
            myNavAgent.speed = defaultWalkSpeed;
            myNavAgent.SetDestination(destination);
            state = 0;
        }else if(distToPlayer < 1)
        {
            delayTimer = 0;
            delayLength = 1.5f;
            //play punch anim from here (not from punching function)
            PunchingPlayer();
        }
    }




    void Walking()
    {
        if (Vector3.Distance(transform.position, destination) < 2)
        {
            delayTimer = 0;
            delayLength = Random.Range(3, 9);
            state = 1;
        }
    }


    void PunchingPlayer()
    {
        if (DelayCheck())
        {
            myNavAgent.speed = defaultWalkSpeed;
            myNavAgent.SetDestination(destination);
            state = 0;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasBeenHitByPlayer = true;
            playerRef = other.transform;
            //play knocked over anim
            delayLength = 3; //make this the length of the knocked over anim
            delayTimer = 0;
        }
    }

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


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minWalkDist);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxWalkDist);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(myNavAgent.destination, 2);
    }
}
