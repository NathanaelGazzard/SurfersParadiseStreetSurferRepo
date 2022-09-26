using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplinePlayerTracker : MonoBehaviour
{
    [SerializeField]
    public SplineLine splineScript;
    public Transform playerPos;
    //private Transform transformThisObj;
    private Vector3 closestNode;

    private void Start(){
        //transformThisObj = transform;
        if (playerPos == null){
            playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        }
        Debug.Log("playerPos: " + playerPos.position.ToString() + " = " + playerPos.position.GetType().ToString());
        //test(playerPos.position);


    }
    /*
    private void test(Vector3 testpos){
        closestNode = splineScript.getPlayerPosition(testpos);
        Debug.Log("test result: " + closestNode.ToString());
    }
    */
    

    private void Update(){
        transform.position = splineScript.getPlayerPosition(playerPos.position);
    }
    
}
