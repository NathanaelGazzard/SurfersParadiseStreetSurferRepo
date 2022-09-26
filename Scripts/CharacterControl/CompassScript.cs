using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CompassScript : MonoBehaviour
{
    List<Transform> waypoints = new List<Transform>();
    List<Image> waypointMarkers = new List<Image>();

    [SerializeField] Image compassBackground; // the strip image that will be the backing for the compass
    [SerializeField] Image waypointIcon; // the image that will be used for waypoints

    float compassHalfWidth;

    [SerializeField] Transform playerRef; // reference the camera arm

    [SerializeField] SimpleLookAtScript lookAtRef; // an object placed as a child of the camera arm with the SimpleLookAtScript attached




    void Start()
    {
        compassHalfWidth = compassBackground.GetComponent<RectTransform>().sizeDelta.x / 2;
    }




    void Update()
    {
        // only runs the code if we actually have waypoints
        if (waypoints.Count != 0)
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                // makes the look at object rotate to look at the waypoint then returns the y rotation (this is the y rotation the camera would need to be looking straight at the waypoint)
                float lookAtAngle = lookAtRef.GetRotation(waypoints[i]);

                // adjusts the planned rotation angle based on the direction the camera is looking
                float markerAngle = lookAtAngle - playerRef.eulerAngles.y;

                // this ensures that we only ever get a value between -180 and 180 (the number wraps around once the rotation is higher than 180
                if(markerAngle > 180)
                {
                    markerAngle = -(360 - markerAngle);
                }

                // sets the position of the respective waypoint marker along the compass base. anything that is further than directly left or right of the player (so, behind them at all) will stay at the relevent end of the compass without moving off it. If the waypoint ends up passing from left to right or vice-versa whilst behind the player, it's marker will warp to the other end of the compass
                waypointMarkers[i].GetComponent<RectTransform>().anchoredPosition = new(compassHalfWidth * Mathf.Clamp(markerAngle, -90, 90) / 90, 0);
            }
        }
    }



    // call this function anytime a new waypoint needs to be added to the list
    public void AddWaypoint(Transform newWaypoint)
    {
        waypoints.Add(newWaypoint);
        Image newMarker = Instantiate(waypointIcon, compassBackground.transform);
        waypointMarkers.Add(newMarker);
    }



    // call this function anytime a new waypoint needs to be removed from the list
    public void RemoveWaypoint(Transform oldWaypoint)
    {
        int indexOf = waypoints.IndexOf(oldWaypoint);
        waypoints.RemoveAt(indexOf);
        waypointMarkers.RemoveAt(indexOf);
    }
}
