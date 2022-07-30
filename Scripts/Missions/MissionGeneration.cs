using UnityEngine;

// Will need some sort of file to store all mission data
public class MissionGeneration
{
    // for now, stored here

    string mode;
    string[] possibleItems = { "Item_1", "Item_2", "Item_3" };

    public MissionGeneration(string m)
    {
        mode = m;
        Debug.Log("Construcotr");
    }
    public void createMission(Vector3 position)
    {

    }
}
