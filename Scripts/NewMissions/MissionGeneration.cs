using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should be attached to the player and the player script will call this function
// generate the missions add it to the players list

public class MissionGeneration : MonoBehaviour
{
    Mission currentMission;
    // Points can be pickup or drop off, each mission point will have some description about them
    // Since this is story mission, it will be there name etc
    [SerializeField] GameObject[] missionPoints;
    List<int> pointsTaken = new List<int>();

    [SerializeField] GameObject[] rndMissionPickUpPoints;
    List<Mission> storyMissions = new List<Mission>();
    protected int numerOfMissions = 3;

    public void Start()
    {
        // 3 story missions
        for (int i = 0; i < numerOfMissions; i++)
        {
            GenerateStoryMissionLocations();
        }
        SetMission(1);
    }

    public void SetMission(int x)
    {
        // Will become user selection (testing cur)
        gameObject.GetComponent<PlayerInteraction>().SendMessage("SetCurMissionID", x);
        Debug.Log("Set mission to: " + x.ToString());
    }

    public void GenerateStoryMissionLocations()
    {
        // Distance function implemented later on

        MissionInteractable pickUp = generatePointLocation(missionPoints);
        MissionInteractable dropOff = generatePointLocation(missionPoints, "SetAsDropOff");
        Debug.Log("Created mission, id: " + (storyMissions.Count).ToString());

        // some randomised function to generate these values
        int reward = 1000;
        string item = "Some item";
        // not as random, as we can use the MissionInteraction values to keep location and people the same 
        // for future speed runners, know where to go when we mention person x at location y
        string delInst = "Deliver " + item + pickUp.GetLocation();
        Mission generatedMission = new Mission(reward, item, delInst);
        storyMissions.Add(generatedMission);
    }

    public void GenerateRandomMissions()
    {
        // Go through all random locations maybe only select a few
        // then make those ones actually missions
        // replayabiity as they are randomly chosen
        // each pickup will also have a specific tag to identify certain aspects
        // although random they do have some customisability added ie;
        // a rndMissionPickUpPoint can be suspicious shrooms or rnd weed
    }

    MissionInteractable generatePointLocation(GameObject[] points, string call="SetAsPickUp")
    {
        int x;
        while (true)
        {
            x = Random.Range(0, points.Length);
            if (pointsTaken.IndexOf(x) < 0) 
            {
                break;
            }
        }
        pointsTaken.Add(x);

        MissionInteractable point = points[x].GetComponentInChildren<MissionInteractable>();
        // Will need a mission number, which will be index based
        // but when set will be able to define which points are linked
        // adds to both pick/drop, before it gets added to Missionpoints in GenerateStoryMissionLocations()
        point.SendMessage(call, (storyMissions.Count).ToString());
        return point;
    }
}
