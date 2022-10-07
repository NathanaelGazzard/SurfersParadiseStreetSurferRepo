using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EshayScript : MonoBehaviour
{
    [SerializeField] GameObject[] eshayModels; // each eshay contains all possible ehsay models and will enable one, rathre than instantiating one
    int modelToUse; // this is will be assigned via a random function and will determine which of the eshay models will be activated

    Animator modelAnimator;

    [SerializeField] NavMeshAgent myNavAgent;

    // max distance a eshay will pick their new point 
    [SerializeField] float maxWalkDist;
    float defaultWalkSpeed; // the navmeshagent's walkspeed
    [SerializeField] float runSpeedMultiplyer = 2;

    Vector3 destination;

    Transform playerRef; // reference to player

    bool playerClose; // once the player is within the trigger this is set to true, allowing for agro checks (dist, angle etc)

    float warningRange = 25;
    float agroRange = 15; // the distance at which the eshays anger will build regardles of whether you're looking at them
    float chaseRange = 8;
    float agroLevel = 0;
    float agroThreshold = 5.5f;
    float playerFOV;
    float chaseDist = 35f; // the max distance between the eshay and player before the eshay gives up on chasing the player

    // this variable is used for delays via the DelayCheck function
    float delayTimer = 0;

    int state = 0; // set up enum for the states (Walking, resting, knocked, chasing, frozen)

    bool holdsGrudge = false; // if the eshay has been agroed before, they will immediately come after the player again in future 

    AudioSource eshayAudioSource;

    [SerializeField] AudioClip[] warningAudio;
    [SerializeField] AudioClip[] agroAudio;
    [SerializeField] AudioClip[] chaseAudio;
    [SerializeField] AudioClip[] giveUpAudio;



    void Start()
    {
        GetComponent<SphereCollider>().radius = warningRange;
        playerFOV = 50;
        defaultWalkSpeed = myNavAgent.speed;
        NewDestination();
        myNavAgent.SetDestination(destination);

        modelToUse = Random.Range(0, eshayModels.Length);
        eshayModels[modelToUse].SetActive(true);

        modelAnimator = eshayModels[modelToUse].GetComponent<Animator>();

        eshayAudioSource = GetComponent<AudioSource>();
    }



    void Update()
    {
        switch (state)
        {
            case 4:
                ChasePlayer();
                break;
            case 3:
                Agro();
                break;
            case 2:
                Warning();
                break;
            case 1:
                SmokeBreak();
                break;
            case 0:
                Walking();
                break;
            default:
                state = 0;
                break;
        }
    }





    void Walking()
    {
        if (Vector3.Distance(transform.position, destination) < 2)
        {
            modelAnimator.SetTrigger("HaveASmoke");
            delayTimer = 0;
            state = 1;
        }
    }


    void SmokeBreak()
    {
        if (DelayCheck(9.3f))
        {
            NewDestination();
            state = 0;
        }
    }


    void Warning()
    {
        Vector3 lookTarg = new Vector3(playerRef.position.x, transform.position.y, playerRef.position.z);
        transform.LookAt(lookTarg);

        float playerLookAngle = Vector3.Angle(playerRef.forward, transform.position - playerRef.position);
        float distToPlayer = Vector3.Distance(transform.position, playerRef.position);

        // this has a bit of redundancy, but I'll get it working first and make it pretty later
        if(distToPlayer < 5) // if the player gets too close, they will immediately agro the Eshay
        {
            agroLevel = agroThreshold;
        }
        else if(playerLookAngle <= playerFOV || distToPlayer <= agroRange)
        {
            if (playerLookAngle <= 15)
            {
                agroLevel += 2 * Time.deltaTime;
            }
            else if(playerLookAngle <= playerFOV)
            {
                agroLevel += Time.deltaTime;
            }

            if (distToPlayer <= agroRange)
            {
                agroLevel += Time.deltaTime;
            }
        }
        else
        {
            // agro level decays if nothing is raising it
            agroLevel -= Time.deltaTime;
        }

        if(agroLevel >= agroThreshold)
        {
            holdsGrudge = true;
            modelAnimator.SetTrigger("BackToWalking");

            AudioClip clipToPlay = agroAudio[Random.Range(0, agroAudio.Length)];
            eshayAudioSource.PlayOneShot(clipToPlay);

            state = 3;
        }
        else if (!playerClose)
        {
            modelAnimator.SetTrigger("BackToWalking");
            NewDestination();

            AudioClip clipToPlay = giveUpAudio[Random.Range(0, giveUpAudio.Length)];
            eshayAudioSource.PlayOneShot(clipToPlay);

            state = 0;
        }
    }


    void Agro()
    {
        if (DelayCheck(0.2f))
        {
            myNavAgent.SetDestination(playerRef.position);
        }

        float distToPlayer = Vector3.Distance(transform.position, playerRef.position);

        // if the player gets too far away, the eshay will stop following. If they get close enough, they will start running.
        if (distToPlayer > chaseDist)
        {
            myNavAgent.SetDestination(destination);


            AudioClip clipToPlay = giveUpAudio[Random.Range(0, giveUpAudio.Length)];
            eshayAudioSource.PlayOneShot(clipToPlay);

            state = 0;
        }
        else if (distToPlayer < chaseRange)
        {
            modelAnimator.SetTrigger("StartRunning");
            myNavAgent.speed = defaultWalkSpeed * runSpeedMultiplyer;

            AudioClip clipToPlay = chaseAudio[Random.Range(0, chaseAudio.Length)];
            eshayAudioSource.PlayOneShot(clipToPlay);

            state = 4;
        }
    }


    void ChasePlayer()
    {
        if (DelayCheck(0.2f))
        {
            myNavAgent.SetDestination(playerRef.position);
        }

        float distToPlayer = Vector3.Distance(transform.position, playerRef.position);

        // if the player gets too far away, the eshay will stop following. If they get close enough, they will shank the player.
        if (distToPlayer > chaseDist)
        {
            modelAnimator.SetTrigger("BackToWalking");
            myNavAgent.speed = defaultWalkSpeed;
            myNavAgent.SetDestination(destination);
            state = 0;
        }
        else if (distToPlayer < 1.8f)
        {
            ShankPlayer();
        }
    }


    void ShankPlayer()
    {
        modelAnimator.SetTrigger("Shank");
        myNavAgent.speed = defaultWalkSpeed;
        myNavAgent.SetDestination(destination);
        state = 0;
        Invoke("DelayedAttackOutput", 0.6f);
    }


    void DelayedAttackOutput()
    {
        // >>> add blood particle system?
        playerRef.GetComponent<CharacterControllerScript>().ModifyHealth(-100);
    }



    // once the player enters the trigger for the Eshay, the eshay will start watching them.
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = true;
            playerRef = other.transform;
            myNavAgent.ResetPath();
            if (holdsGrudge)
            {
                modelAnimator.SetTrigger("StartRunning");

                AudioClip clipToPlay = chaseAudio[Random.Range(0, chaseAudio.Length)];
                eshayAudioSource.PlayOneShot(clipToPlay);

                state = 4;
            }
            else
            {
                modelAnimator.SetTrigger("Agro");

                AudioClip clipToPlay = warningAudio[Random.Range(0, warningAudio.Length)];
                eshayAudioSource.PlayOneShot(clipToPlay);

                state = 2;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = false;
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
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, 41))
            {
                destination = hit.position;
            }
        }
        //set the new destination
        myNavAgent.SetDestination(destination);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, warningRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, agroRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}