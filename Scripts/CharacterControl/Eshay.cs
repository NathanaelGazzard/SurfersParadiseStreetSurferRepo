using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eshay : MonoBehaviour
{
    public enum AngerFSM { Passive, Frustrated, Aggressive }

    public enum Range { Lower, Upper, Far }

    AngerFSM curState;
    Range curRange;

    GameObject player;
    [SerializeField] float angle = 10.0f;
    private Rigidbody _rigidbody; // The rigidbody of the chaser
    
    // Tiered agression system
    float distanceToPlayer;
    float lowerRange = 10.0f;
    float upperRange = 30.0f;
    float outOfRange = 45.0f;
    int numOfWarnings = 2;
    float timeSinceWarning = 0.0f;
    bool wasInClose = false;

    // Timing
    float elapsedTime = 0.0f;
    float lowerTime = 0.0f;
    float upperTime = 0.0f;

    void Start()
    {
        curState = AngerFSM.Passive;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // May need a function that resets some functions every few seconds

        LookAtPlayer();
        UpdateTimes();
        CalculateNewAngerFSM();
        switch(curState)
        {
            case AngerFSM.Passive: PassiveBehaviour();break;
            case AngerFSM.Frustrated: FrustratedBehaviour(); break;
            case AngerFSM.Aggressive: AggressiveBehaviour(); break;
        }
    }

    void UpdateTimes()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //elapsedTime += Time.deltaTime;

        if (distanceToPlayer < lowerRange)
        {
            timeSinceWarning += Time.deltaTime;
            lowerTime += Time.deltaTime;
            wasInClose = true;
            //if (IsPlayerLooking()) lowerTime += 5.0f;
        } else lowerTime -= Time.deltaTime;
        if (distanceToPlayer < upperRange)
        {
            timeSinceWarning += Time.deltaTime;
            upperTime += Time.deltaTime;
            //if (IsPlayerLooking()) upperTime += 2.0f;
        } else upperTime -= Time.deltaTime;
        if (distanceToPlayer < outOfRange) elapsedTime += Time.deltaTime;
    }

    void CalculateNewAngerFSM()
    {
        // riding past
       if (lowerTime < 5.0f && distanceToPlayer > lowerRange && wasInClose)
        {
            Debug.Log("Piss off");
        } 

       // Eventually no matter lower or upper, eshay will get pissed off
       if (numOfWarnings == 0)
        {
            curState = AngerFSM.Aggressive;
        }
    }

    void PassiveBehaviour()
    {
        // general behaviour -> Called once, will be some sort of functionality in seperate script (For pedestrians walking about)
    }
    void FrustratedBehaviour()
    {
        //ChasePlayer(walkSpeed);
        Debug.Log("Frustrated");
    }
    void AggressiveBehaviour()
    {
        if (distanceToPlayer > outOfRange)
        {
            Debug.Log("Piss off ya bastard");
            curState = AngerFSM.Frustrated;
        }
        //ChasePlayer(sprintSpeed);
        Debug.Log("Aggressive");
    }

    // general behaviour
    void LookAtPlayer()
    {
        _rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), 3.0f * Time.deltaTime));
        
    }

    void ChasePlayer(float speed)
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    bool IsPlayerLooking()
    {
        return (Vector3.Angle(player.transform.forward, transform.position - player.transform.position) < angle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lowerRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, upperRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, outOfRange);
    }
}
