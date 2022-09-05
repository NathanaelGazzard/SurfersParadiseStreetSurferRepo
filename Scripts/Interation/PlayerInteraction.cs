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

    // Current Mission UI
    public GameObject missionUI;
    public TextMeshProUGUI missionTitle;
    public TextMeshProUGUI missionLoc;
    public TextMeshProUGUI missionReward;

    // Available Mission UI
    // Will have side menu for all missions

    public Text missionTxt;

    bool missionUIOpen = false;
    bool isOnMission = false;
    int curMissionID = -1;
    bool isMissionPickedUp = false;

    private void Start()
    {
        missionUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Exit menu");
        } else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (missionUIOpen)
            {
                Debug.Log("Closing side menu");
                Cursor.visible = false;
                missionUIOpen = false;
                Cursor.lockState = CursorLockMode.Confined;
            } else
            {
                Debug.Log("Open Mission menu");
                Cursor.visible = true;
                missionUIOpen = true;
                Cursor.lockState = CursorLockMode.None;
            }
            missionUI.SetActive(missionUIOpen);
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
                            interactionText.text = "This is the correct mission!";
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                Debug.Log("Pickedup mission");
                                isMissionPickedUp = true; 
                                point.Interact();
                            }
                        } else
                        {
                            interactionText.text = "This is the incorrect mission!";
                        }
                    }
                } else if (!point.IsPickUp() && isMissionPickedUp) // drop off point or a mission that is not yet active
                {
                    if (point.GetID() == this.curMissionID.ToString()) // is a dropoff
                    {
                        interactionText.text = "Drop off mission!";
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            Debug.Log("Mission dropped off");
                            isMissionPickedUp = false;
                            point.Interact();
                        }
                    } else
                    {
                        Debug.Log("Not a drop off point");
                    }
                } else
                {
                    //Debug.Log("Nothing is going on");
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
