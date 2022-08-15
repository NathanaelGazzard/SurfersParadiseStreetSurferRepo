using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerInteraction : MonoBehaviour
{
    public Camera mainCam;
    public float interationDistance = 2f;

    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;
    public bool missionStatus = false;

    // Update is called once per frame
    void Update()
    {
        InteractionRay();
    }

    void InteractionRay()
    {
        Ray ray = mainCam.ViewportPointToRay(Vector3.one / 2f);
        RaycastHit hit;

        bool hitSomething = false;
        if (Physics.Raycast(ray, out hit, interationDistance))
        {
            if (hit.collider.GetComponent<MissionDropOff>() != null) // mission pick up interactable
            {
                hitSomething = true;
                MissionDropOff interactable = hit.collider.GetComponent<MissionDropOff>();
                // Might be a place to eventually interact, but not now
                if (!interactable.IsInteractable()) return;

                if (interactable.GetIsDropOffPoint()) // if a valid drop off point for current misison
                {
                    interactionText.text = interactable.GetDescription();
                    // Handle the actions
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // get mission
                        missionStatus = false;
                        interactable.Interact();
                        interactable.SetDropOff(false);
                        interactionText.text = "";
                    }
                }
                else
                {
                    interactionText.text = "Not a valid drop off";
                }
            } else if  (hit.collider.GetComponent<MissionDropOff>() != null) {
                hitSomething = true;
                MissionRecieval interactable = hit.collider.GetComponent<MissionRecieval>();
                if (missionStatus)
                {
                    interactionText.text = "You are already on a mission";
                }
                else
                {
                    interactionText.text = interactable.GetDescription();
                    // Handle the actions
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // get mission
                        missionStatus = true;
                        interactable.Interact();
                    }
                }
            } else // for now is a mission recieval interactable
            {
                Debug.Log("Interacting with eashy");
            }
        }
        interactionUI.SetActive(hitSomething);
    }
}
