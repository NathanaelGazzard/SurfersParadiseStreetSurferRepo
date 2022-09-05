using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    General class which is attached to missions
 */
public class MissionInteractable : MonoBehaviour, IInteractable
{
    int reward;
    bool isPickUp = false; // will det if drop or pick
    bool isInteractable = false;
    Material mat;
    protected string missionID;

    [SerializeField] string personName;
    [SerializeField] string location;

    protected Transform playerTransform;
    GameObject playerObj;
    float distanceToPlayer;
    protected float x;
    protected float z;

    public void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObj.transform;
        if (!playerTransform)
        {
            print("No player object");
        }
        missionID = "-1";
        // Can setup location values here as well to help with spreading the mission pick/drop offs
        x = gameObject.transform.position.x;
        z = gameObject.transform.position.z;
    }

    private void Update()
    {
        // can watch out player coming into range, once it does, send a message to the playerInteraction component
        // which will activate the interactoin UI stuff, instead of having to use a camera etc and can be more general area specific

        // Will add some sort of listen function
        // if player is yonks away dont make another Distance check for a estimated time
        // otherwise if its in a med range, reduce it more
        // in a low range check every few seconds...


        // WILL BE WORKING ON LATER...

        //if (this.missionID == "-1") return;
        //distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        //if (distanceToPlayer < 3.0f)
        //{
            //Debug.Log("Player in range");
            //playerObj.GetComponent<PlayerInteraction>().SendMessage("InRange", this.missionID);
        //}
    }

    public void Init()
    {
        
    }

    // IInteractable
    public void Interact()
    {
        // if pick up, do pick up sequence
        if (IsInteractable())
        {
            if (isPickUp)
            {
                PickUpSequence();
            }
            // else, if drop off do drop off sequence
            else
            {
                DropOffSequence();
            }
        } else
        {
            // some voice line to say, no misisons for ya
            Debug.Log("No missions from me mate");
        }
        
    }
    public string GetDescription()
    {
        return GetPointTypeString() + " with id: " + missionID;
    }
    public bool IsInteractable()
    {
        return isInteractable;
    }

    // Setters
    
    void SetAsPickUp(string missionID)
    {
        isPickUp = true;
        isInteractable = true;
        this.missionID = missionID;
        mat.SetColor("_Color", Color.green);
    }

    void SetAsDropOff(string missionID)
    {
        mat.SetColor("_Color", Color.red);
        isInteractable = true;
        this.missionID = missionID;
    }

    // Getters
    public bool IsPickUp()
    {
        return this.isPickUp;
    }

    string GetPointTypeString()
    {
        if (IsPickUp()) return "Pick up";
        return "Drop off";
    }

    public string GetLocation()
    {
        return this.location;
    }

    public string GetID() { return this.missionID;  }

    // General functions

    void PickUpSequence()
    {
        Debug.Log("PickUpSeq");
    }

    void DropOffSequence()
    {
        Debug.Log("DropOffSeq");
    }
}
