using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianTest : MonoBehaviour
{
    // >>> figure out way to constrain actor to a limited distance from their spawn location
    // >>> punch is not activating
    // >>> the falling animation makes the character fall through the floor. Maybe raise the model container during this anim?
    [SerializeField] GameObject[] pedestrianModels;
    int modelToUse;
    float walkType;
    float runType;

    Animator modelAnimator;

    Transform playerRef;
    Vector3 destination;
    [SerializeField] NavMeshAgent myNavAgent;
    [SerializeField] float minWalkDist;
    [SerializeField] float maxWalkDist;

    float chaseDist = 50f;

    float defaultWalkSpeed;

    float delayTimer = 0;
    float delayLength;
    int state = 0; //set up enum for the states (Walking, resting, knocked, chasing, frozen)

    bool holdsGrudge = false; //this bool is here to use if I want AI memory. Eg, if the player has previously hit them, the AI might yell if they get close and may even start chasing again


    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        print(state);
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
        modelAnimator.SetTrigger("CheckPhone");
        if (DelayCheck())
        {
            modelAnimator.ResetTrigger("CheckPhone");
            NewDestination();
            state = 0;
        }
    }


    void KnockedOver()
    {
        if (DelayCheck())
        {
            delayTimer = 0;
            delayLength = 0.5f;//this will determine how frequently the pedestrian will recalculate their path. The lower the value, the more accurately it will track the player but at the cost of performance
            myNavAgent.speed = 5;
            state = 3;
        }
    }


    void ChasePlayer()
    {
        //checks players position each second
        if (DelayCheck())
        {
            delayTimer = 0;
            myNavAgent.SetDestination(playerRef.position);
        }


        float distToPlayer = Vector3.Distance(transform.position, playerRef.position);

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
            PunchingPlayer();
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


    void PunchingPlayer()
    {
        myNavAgent.speed = defaultWalkSpeed;
        myNavAgent.SetDestination(destination);
        state = 0;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && state < 2)
        {
            myNavAgent.speed = 0;
            modelAnimator.SetTrigger("FallDown");
            holdsGrudge = true;
            playerRef = other.transform;
            delayLength = 4.9f; //lenght of the fall down and standup anims combined
            delayTimer = 0;
            state = 2;
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
    }
}
