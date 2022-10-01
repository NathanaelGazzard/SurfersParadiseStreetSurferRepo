using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// NOTE
// handles all mission related stuff (Will change to Mission instead of Missiongeneration soon

public class MissionGeneration : MonoBehaviour
{
    Mission currentMission;
    PlayerInteraction playerInteraction;

    [SerializeField] GameObject[] missionPoints;
    List<int> pointsTaken = new List<int>();

    [SerializeField] GameObject[] rndMissionPickUpPoints;
    List<Mission> storyMissions = new List<Mission>();
    List<bool> missionStatus = new List<bool>();

    protected int numerOfMissions = 2;
    int completedCount = 0;
    int curSelectedMission = 0;
    bool isOnMission = false;

    // UI
    [SerializeField] GameObject missionUI;
    [SerializeField] TextMeshProUGUI pickupUI;
    [SerializeField] TextMeshProUGUI dropOffUI;
    [SerializeField] TextMeshProUGUI rewardUI;

    [SerializeField] TextMeshProUGUI pickupSelectUI;
    [SerializeField] TextMeshProUGUI dropOffSelectUI;
    [SerializeField] TextMeshProUGUI rewardSelectUI;

    public void Start()
    {
        missionUI.SetActive(false);
        playerInteraction = gameObject.GetComponent<PlayerInteraction>();
    }

    public void GenMissions()
    {
        for (int i = 0; i < 2; i++)
        {
            GenerateStoryMissionLocations();
        }
        Mission cur = this.storyMissions[curSelectedMission];
        ChangeMissionSelectCard(cur);
    }

    public void GenerateStoryMissionLocations()
    {
        // Distance function implemented later on
        // Can be made into one function...
        int pickUp = GeneratePickUpLocation(missionPoints);
        int dropOff = GeneratePickUpLocation(missionPoints, "SetAsDropOff");

        // some randomised function to generate these values
        int reward = 1000;
        // not as random, as we can use the MissionInteraction values to keep location and people the same 
        // for future speed runners, know where to go when we mention person x at location y
        string[] rndItems = { "Ciggies", "Mushrooms", "Drugz" };
        MissionInteractable point = this.missionPoints[pickUp].GetComponentInChildren<MissionInteractable>();
        MissionInteractable point_2 = this.missionPoints[dropOff].GetComponentInChildren<MissionInteractable>();
        Mission generatedMission = new Mission(reward, rndItems[Random.Range(0, rndItems.Length)], point.Location(), point_2.Location(), pickUp, dropOff);
        storyMissions.Add(generatedMission);
        missionStatus.Add(false);
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

    int GeneratePickUpLocation(GameObject[] points, string call = "SetAsPickUp")
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
        return x;
    }

    void ChangeMissionCard(string p, string d, string r)
    {
        pickupUI.text = p; ;
        dropOffUI.text = d;
        rewardUI.text = r;
    }

    void ChangeMissionSelectCard(Mission cur)
    {
        pickupSelectUI.text = cur.GetPickUp();
        dropOffSelectUI.text = cur.GetDropOff();
        rewardSelectUI.text = cur.GetReward().ToString();
    }

    // Mission UI related functions
    public void OpenMissionMenu()
    {
        missionUI.SetActive(true);
    }
    public void CloseMissionMenu()
    {
        missionUI.SetActive(false);
    }

    public void SetMission()
    {
        if (isOnMission) return;
        if (missionStatus[curSelectedMission]) { Debug.Log("Already completed"); return; }
        Debug.Log(curSelectedMission);
        gameObject.GetComponent<PlayerInteraction>().SendMessage("SetCurMissionID", curSelectedMission);
        currentMission = this.storyMissions[curSelectedMission];
        ChangeMissionCard(currentMission.GetPickUp(), currentMission.GetDropOff(), currentMission.GetReward().ToString());
        isOnMission = true;
    }

    public void NextMission()
    {
        if (isOnMission) return;
        curSelectedMission++;
        if (curSelectedMission == this.storyMissions.Count) curSelectedMission = 0;
        Mission cur = this.storyMissions[curSelectedMission];
        ChangeMissionSelectCard(cur);
    }

    public void PrevMission()
    {
        if (isOnMission) return;
        curSelectedMission--;
        if (curSelectedMission == -1) curSelectedMission = this.storyMissions.Count - 1;
        Mission cur = this.storyMissions[curSelectedMission];
        ChangeMissionSelectCard(cur);
    }

    public void CompleteMission()
    {
        isOnMission = false;
        completedCount++;
        ChangeMissionCard("", "", "");
        missionStatus[curSelectedMission] = true;
        curSelectedMission = 0;
        Mission cur = this.storyMissions[curSelectedMission];
        ChangeMissionSelectCard(cur);
        if (completedCount == numerOfMissions)
        {
            Debug.Log("Game completed");
        }
    }
}