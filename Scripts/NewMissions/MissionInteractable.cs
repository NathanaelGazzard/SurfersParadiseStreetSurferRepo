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
        mat = GetComponent<Renderer>().material;
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
        }
        else
        {
            // some voice line to say, no misisons for ya
            Debug.Log("Not active mission point yet, or ever");
        }

    }
    public string GetDescription()
    {
        return GetPointTypeString() + " with name: " + personName;
    }
    public string Location()
    {
        return this.location;
    }
    public bool IsInteractable()
    {
        return this.isInteractable;
    }

    // Setters

    void SetAsPickUp(string missionID)
    {
        // turn into FSM, PickUp or DropOff
        isPickUp = true;
        this.missionID = missionID;
        SetAsInteractable();
    }

    void SetAsDropOff(string missionID)
    {
        this.missionID = missionID;
        SetAsInteractable();
    }

    void SetAsInteractable()
    {
        this.isInteractable = true;
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

    public string GetID() { return this.missionID; }

    // General functions

    void PickUpSequence()
    {
        mat.SetColor("_Color", Color.yellow);
    }

    void DropOffSequence()
    {
        mat.SetColor("_Color", Color.cyan);
    }
}