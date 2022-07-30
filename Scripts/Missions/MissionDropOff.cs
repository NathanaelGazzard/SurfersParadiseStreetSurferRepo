using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class MissionDropOff : MonoBehaviour, IInteractable
{
    // A way to turn off interactions with

    Material mat;
    bool isDropOffPoint = false;
    string interactableText = ""; // not a valid drop off

    // Equivalent to dropping off a mission
    public void Interact()
    {
        mat.color = new Color(Random.value, Random.value, Random.value);
    }
    public string GetDescription()
    {
        return interactableText;
    }

    private void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }
    public bool GetIsDropOffPoint() { return isDropOffPoint; }
    public void SetDropOff(bool b) { isDropOffPoint = b; }
    public void SetMissionIntText(string s) { interactableText = s; }

    public void ResetDropOffPoint()
    {
        mat.SetColor("_Color", Color.grey);
        isDropOffPoint = false;
        interactableText = "";
        Debug.Log("Drop off point: " + isDropOffPoint);
    }
    public bool IsInteractable()
    {
        return !(interactableText == "");
    }

    public void SetAsDropOff()
    {
        isDropOffPoint = true;
        mat.SetColor("_Color", Color.green);
        // Do stuff with location such as adding to mini map etc
    }
}
