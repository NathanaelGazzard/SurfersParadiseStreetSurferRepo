using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should be attached to the player and the player script will call this function
// generate the missions add it to the players list

public class MissionGeneration : MonoBehaviour
{
    Mission currentMission;
    GameObject[] missionPickUpPoints;
    GameObject[] missionDropOffPoints;

    GameObject[] rndMissionPickUpPoints;
    Mission[] storyMissions;

    public MissionGeneration(string m)
    {
        Debug.Log("Initialised Mission generaiton: " + m);
    }

    public  Mission GenerateStoryMission()
    {
        // On generation, it will communicate with these pick/drop points and make them a mission pick/drop
        GameObject pickUp = generatePointLocation(missionPickUpPoints);
        GameObject dropOff = generatePointLocation(missionDropOffPoints);

        // some randomised function to generate these values
        int r = 1000;
        string item = "Cory Cigaretes";
        string delInst = "Deliver to Cory at x location";
        Mission generatedMission = new Mission(r, item, delInst);
        return generatedMission;
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

    public GameObject generatePointLocation(GameObject[] points)
    {
        return points[Random.Range(0, points.Length)];
    }
}
