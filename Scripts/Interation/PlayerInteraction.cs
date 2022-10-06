using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] public Camera mainCam;
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
        InteractionRay();
    }

    void InRange(string id)
    {
        MissionInteractable obj = GameObject.FindGameObjectWithTag(id).GetComponentInChildren<MissionInteractable>();
        interactionText.text = obj.GetDescription();
        interactionUI.SetActive(true);
    }

    void InteractionRay()
    {
        Ray ray = mainCam.ViewportPointToRay(Vector3.one / 10f);
        RaycastHit hit;
        bool hitSomething = false;
        if (Physics.Raycast(ray, out hit, interationDistance))
        {
            if (hit.collider.GetComponent<MissionInteractable>())
            {
                hitSomething = true;
                MissionInteractable point = hit.collider.GetComponentInChildren<MissionInteractable>();
                if (point.IsPickUp())
                {
                    if (isOnMission)
                    {
                        if (point.GetID() == this.curMissionID.ToString()) // point at which we need to pick this mission up
                        {
                            interactionText.text = "Pick mission up";
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                isMissionPickedUp = true;
                                point.Interact();
                            }
                        }
                        else
                        {
                            hitSomething = false; // wont show any interaction text so player doesn't know that this is a future spot
                        }
                    }
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
                        }
                    }
                    else
                    {
                        hitSomething = false; // wont show any interaction text so player doesn't know that this is a future spot
                    }
                }
                else
                {
                    Debug.Log("Could be a random mission");
                    hitSomething = false; // wont show any interaction text so player doesn't know that this is a future spot
                }
            }
        }
        interactionUI.SetActive(hitSomething);
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(mainCam.transform.position, interationDistance);
    }

    void SetCurMissionID(int id)
    {
        this.isOnMission = true;
        this.curMissionID = id;
    }
}