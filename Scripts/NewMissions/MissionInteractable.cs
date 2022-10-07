using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    General class which is attached to missions
 */
public class MissionInteractable : MonoBehaviour, IInteractable
{
    bool isPickUp = false; // will det if drop or pick
    bool isInteractable = false;
    protected string missionID;

    [SerializeField] string personName;
    [SerializeField] string location;

    protected Transform playerTransform;

    public void Start()
    {
        missionID = "-1";
    }

    private void Update()
    {
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
        
    }

    void DropOffSequence()
    {
        
    }
}