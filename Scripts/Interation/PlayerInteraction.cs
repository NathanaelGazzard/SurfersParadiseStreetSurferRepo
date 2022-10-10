using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] public GameObject compassRef;
    public float interationDistance = 2f;

    // interaction UI
    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;

    MissionGeneration msRef;

    bool missionUIOpen = false;
    bool isOnMission = false;
    int curMissionID = -1;
    bool isMissionPickedUp = false;


    private void Start()
    {
        msRef = gameObject.GetComponent<MissionGeneration>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (missionUIOpen)
            {
                Cursor.visible = false;
                missionUIOpen = false;
                msRef.CloseMissionMenu();
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.visible = true;
                missionUIOpen = true;
                msRef.OpenMissionMenu();
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    void InRange(string id)
    {
        MissionInteractable obj = GameObject.FindGameObjectWithTag(id).GetComponentInChildren<MissionInteractable>();
        interactionText.text = obj.GetDescription();
        interactionUI.SetActive(true);
    }

    public void ShowInteraction(MissionInteractable point)
    {
        if (!isOnMission)
        {
            interactionUI.SetActive(false);
            return;
        }
        if (point.IsPickUp())
        {
            if (point.GetID() == this.curMissionID.ToString()) // point at which we need to pick this mission up
            {
                interactionText.text = "Pick mission up";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isMissionPickedUp = true;
                    point.Interact();
                    isOnMission = true;
                    compassRef.GetComponent<CompassScript>().RemoveWaypoint(msRef.GetCurMission().GetPickT(), 0);
                    compassRef.GetComponent<CompassScript>().AddWaypoint(msRef.GetCurMission().GetDropT());
                    Vector3 competitorTarg = msRef.GetCurMission().GetDropT().position;
                    GameObject.FindGameObjectWithTag("CompetitorObjects").GetComponent<SpawnCompetitor>().InitiateCompetitor(competitorTarg);
                }
            }
            interactionUI.SetActive(!isMissionPickedUp);
        }
        else if (!point.IsPickUp() && isMissionPickedUp) // drop off point or a mission that is not yet active
        {
            if (point.GetID() == this.curMissionID.ToString()) // is a dropoff
            {
                interactionText.text = "Drop mission off";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isMissionPickedUp = false;
                    point.Interact();
                    msRef.CompleteMission();
                    isOnMission = false;
                    compassRef.GetComponent<CompassScript>().RemoveWaypoint(msRef.GetCurMission().GetDropT(), 0);
                    Destroy(GameObject.FindGameObjectWithTag("Competitor"));
                    GameObject.FindGameObjectWithTag("CompetitorNotification").GetComponent<CompetitorNotification>().ResetContainer();
                }
            }
            interactionUI.SetActive(isMissionPickedUp);
        }
    }
    public void HideInteractionBar()
    {
        interactionUI.SetActive(isMissionPickedUp);
    }

    void SetCurMissionID(int id)
    {
        this.isOnMission = true;
        this.curMissionID = id;
    }
}