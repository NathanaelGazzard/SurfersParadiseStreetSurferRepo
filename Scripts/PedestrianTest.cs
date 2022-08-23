using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianTest : MonoBehaviour
{
    // >>> figure out way to constrain actor to a limited distance from their spawn location


    Vector3 destination;
    [SerializeField] NavMeshAgent myNavAgent;
    [SerializeField] float minWalkDist;
    [SerializeField] float maxWalkDist;

    float phoneBreakTimer = 0;
    float phoneBreakLength;
    int state = 0; //set up enum for the states (Walking, resting, knocked, chasing, frozen)


    // Start is called before the first frame update
    void Start()
    {
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
        phoneBreakTimer += Time.deltaTime;
        if (phoneBreakTimer > phoneBreakLength)
        {
            NewDestination();
            state = 0;
        }
    }


    void KnockedOver()
    {

    }


    void ChasePlayer()
    {

    }


    void Walking()
    {
        if (Vector3.Distance(transform.position, destination) < 2)
        {
            phoneBreakTimer = 0;
            phoneBreakLength = Random.Range(3, 9);
            state = 1;
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
