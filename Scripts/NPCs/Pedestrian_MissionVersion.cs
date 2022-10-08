using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pedestrian_MissionVersion : MonoBehaviour
{
    [SerializeField] GameObject[] pedestrianModels; // each pedestrian contains all possible pedestrian models
    int modelToUse; // this is will be assigned via a random function and will determine which of the pedestrian models will be activated

    // the pedestrian animations have have blend trees for both walking and running. The following variables are assigned via random at
    // runtime so that each pedestrian will have a slightly different walk and run animation
    float walkType;
    float runType;

    Animator modelAnimator;

    NavMeshAgent myNavAgent;

    Vector3 destination;

    [SerializeField] public GameObject playerRef; // reference to player

    // this var is used for delays via the DelayCheck function
    float delayTimer = 0;

    int state = -1; // set up enum for the states (Walking, resting, knocked, chasing, frozen)

    [SerializeField] Transform[] loopableDestinations; // a set of destinations that this NPC will loop through rather than relying on randomness to pick their destination
    int currentDestination = 0;

    bool isTalking = false;

    AudioSource npcAudiosource;

    [SerializeField] AudioClip[] greetingAudioClips;
    [SerializeField] AudioClip[] pickupAudioClips;
    [SerializeField] AudioClip[] dropoffDeliveryClips;

    bool isPickupAudio;

    PlayerInteraction pi;

    void Start()
    {
        myNavAgent = GetComponent<NavMeshAgent>();

        destination = loopableDestinations[currentDestination].position;
        myNavAgent.SetDestination(destination);

        modelToUse = Random.Range(0, pedestrianModels.Length);
        pedestrianModels[modelToUse].SetActive(true);
        walkType = Random.Range(0, 1);
        runType = Random.Range(0, 1);

        modelAnimator = pedestrianModels[modelToUse].GetComponent<Animator>();
        modelAnimator.SetFloat("WalkType", walkType);
        modelAnimator.SetFloat("RunType", runType);

        npcAudiosource = GetComponent<AudioSource>();
        pi = playerRef.GetComponent<PlayerInteraction>();
    }



    void Update()
    {
        switch (state)
        {
            case 2:
                IsTalking();
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
        float distanceToPlayer = Vector3.Distance(playerRef.transform.position, transform.position);
        if (distanceToPlayer < 5.0f)
        {
            pi.ShowInteraction(gameObject.GetComponent<MissionInteractable>());
            //StartInteraction(true);
        } 
    }



    void NewDestination()
    {
        //currentDestination++;
        currentDestination = Random.Range(0, loopableDestinations.Length);
        if (currentDestination == loopableDestinations.Length)
        {
            currentDestination = 0;
        }

        NavMeshHit hit;

        if (NavMesh.SamplePosition(loopableDestinations[currentDestination].position, out hit, 1.0f, NavMesh.AllAreas))
        {
            destination = hit.position;
        }
        destination = loopableDestinations[currentDestination].position;
        myNavAgent.SetDestination(destination);
    }



    void IsTalking()
    {
        if (!isTalking)
        {
            modelAnimator.SetTrigger("StopTalking");
            if (isPickupAudio)
            {
                // picks a random audio clip from the pickup audio clips
                int clipToPlay = Random.Range(0, pickupAudioClips.Length);
                npcAudiosource.PlayOneShot(pickupAudioClips[clipToPlay]);
            }
            else
            {
                // picks a random audio clip from the dropoff audio clips
                int clipToPlay = Random.Range(0, dropoffDeliveryClips.Length);
                npcAudiosource.PlayOneShot(dropoffDeliveryClips[clipToPlay]);
            }
        }
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


    void Walking()
    {
        if (Vector3.Distance(transform.position, destination) < 1)
        {
            state = 1;
        }
    }


    public void StartInteraction(bool isPickup)
    {
        state = 2;
        modelAnimator.SetTrigger("CheckPhone"); // a stand trigger
        isTalking = true;

        // picks a random audio clip from the greeting audio clips
        int clipToPlay = Random.Range(0, greetingAudioClips.Length);
        npcAudiosource.PlayOneShot(greetingAudioClips[clipToPlay]);

        // this is set so that at the end of the interaction, the npc can give the appropriate farewell to the player
        isPickupAudio = isPickup;
    }

    public void EndInteraction()
    {
        isTalking = false;
    }


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

