using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class MissionRecieval : MonoBehaviour, IInteractable
{
    public enum MODE
    {
        Random, Story
    };

    public GameObject[] missionDropOffPoints;

    public string GetDescription()
    {
        return "Ayo this a mission homie, take it! Dont get caught";
    }

    public bool GetIsDropOffPoint()
    {
        throw new NotImplementedException();
    }

    // Mission recievals equivalent interact is taking a mission
    public void Interact()
    {
        // Dialog with person giving mission
        GameObject missionDropOff = missionDropOffPoints[Random.Range(0, missionDropOffPoints.Length)];
        MissionDropOff dropOffPoint = missionDropOff.GetComponent<MissionDropOff>();
        Debug.Log(missionDropOff.transform.position);

        MissionGeneration mission = new MissionGeneration();
        mission.createMission(missionDropOff.transform.position);

        dropOffPoint.SetAsDropOff();
        dropOffPoint.SetMissionIntText("Mission drop off");
    }

    public bool IsInteractable()
    {
        throw new NotImplementedException();
    }


}
