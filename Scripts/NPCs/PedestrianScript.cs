using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianScript : MonoBehaviour
{
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

    public Transform playerRef; // reference to player

    float chaseDist = 50f; // the max distance between the pedestrian and player before the pedestrian gives up on chasing the player

    float defaultWalkSpeed; // the navmeshagent's walkspeed

    // this var is used for delays via the DelayCheck function
    float delayTimer = 0;

    int state = 0; // set up enum for the states (Walking, resting, knocked, chasing, frozen)

    bool holdsGrudge = false; // this bool is here to use if we want AI memory. Eg, if the player has previously hit them, the AI might yell if they get too close in future and may even start chasing again

    bool hasPunched = false; // this will be used to prevent the pedestrian from being "hit" by the player the moment their state transitions after punching the player

    AudioSource pedoAudioSource;

    // note: audio for the player getting too close will be played from a seperate game object (below) to save on distance calculations checking for the difference between a close call and an actual hit
    [SerializeField] GameObject proximityBarkObject;
    [SerializeField] AudioClip[] hitAudio;
    [SerializeField] AudioClip[] chaseAudio;
    [SerializeField] AudioClip[] giveUpAudio;


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

        pedoAudioSource = GetComponent<AudioSource>();
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
        if (DelayCheck(4))
        {
            modelAnimator.ResetTrigger("CheckPhone");
            NewDestination();
            state = 0;
        }
    }


    void Knocked()
    {
        if (DelayCheck(1.4f))
        {
            myNavAgent.speed = 5;
            myNavAgent.SetDestination(playerRef.position);

            AudioClip clipToPlay = chaseAudio[Random.Range(0, chaseAudio.Length)];
            pedoAudioSource.PlayOneShot(clipToPlay);

            state = 3;
        }
    }


    void ChasePlayer()
    {
        //checks players position at intervals (length of intervals defined in Knocked() function
        if (DelayCheck(0.2f))
        {
            myNavAgent.SetDestination(playerRef.position);
        }

        float distToPlayer = Vector3.Distance(transform.position, playerRef.position);

        // if the player gets too far away, the pedestrian will stop chasing. If they get close enough, they will punch the player (KO).
        if (distToPlayer > chaseDist)
        {
            modelAnimator.SetTrigger("StopChasing");
            myNavAgent.speed = defaultWalkSpeed;
            myNavAgent.SetDestination(destination);

            AudioClip clipToPlay = giveUpAudio[Random.Range(0, giveUpAudio.Length)];
            pedoAudioSource.PlayOneShot(clipToPlay);

            state = 0;
        }
        else if(distToPlayer < 1.5f)
        {
            modelAnimator.SetTrigger("Punch");
            myNavAgent.speed = defaultWalkSpeed;
            myNavAgent.SetDestination(destination);
            hasPunched = true;
            state = 0;
            Invoke("DelayedAttackOutput", 0.4f);
        }
    }

    
    //this exists so that the player receives damage when the punch anim plays out, rather than being called immidiately
    void DelayedAttackOutput()
    {
        playerRef.GetComponent<CharacterControllerScript>().ModifyHealth(-100);
    }




    void Walking()
    {
        if (Vector3.Distance(transform.position, destination) < 2)
        {
            state = 1;
        }
    }



    // is the player bumps into the pedestrian, they will get real angry
    private void OnTriggerEnter(Collider other)
    {
        //this is when the player initially hits the pedestrian
        if (other.CompareTag("Player") && state < 2 && !hasPunched)
        {
            proximityBarkObject.SetActive(false); // pedestrian will no longer yell about the player getting too close

            AudioClip clipToPlay = hitAudio[Random.Range(0, hitAudio.Length)];
            pedoAudioSource.PlayOneShot(clipToPlay);

            myNavAgent.speed = 0; // cannot move while staggered

            modelAnimator.SetTrigger("Knocked");

            holdsGrudge = true;

            state = 2;
        }
    }


    // this function can be called by an if statement in update and will only activate after a delay the length of delayLength
    bool DelayCheck(float delayLength)
    {
        delayTimer += Time.deltaTime;
        if (delayTimer > delayLength)
        {
            delayTimer = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
}
