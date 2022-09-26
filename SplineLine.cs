using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineLine : MonoBehaviour
{
    private Vector3[] splineNode;
    private int splineNumber;
    //private Vector3 nodeNearestPlayer;
    
    private void Start(){
        splineNumber = transform.childCount;
        splineNode = new Vector3[splineNumber];

        // add postition of spline nodes to splineNode 

        for (int i = 0; i<splineNumber; i++){
            Transform childTransform = transform.GetChild(i);
            splineNode[i] = childTransform.position;
        }

         for (int i = 1; i< splineNumber; i++){
            Debug.DrawLine(splineNode[i - 1], splineNode[i], Color.red);
        }
    }

    /*****
                THIS IS ONLY FOR TESTING.
                MAKE SURE TO CHANGE CODE STRUCTURE!!
    *****/

    
    public Vector3 getPlayerPosition(Vector3 playerPosition){
        
        int nodeNearestPlayer = findClosestSplineNode(playerPosition);
        
        if (nodeNearestPlayer == 0){
            return findSegment(splineNode[0], splineNode[1], playerPosition);
        }
        else if (nodeNearestPlayer == splineNumber - 1){
            return findSegment(splineNode[splineNumber - 1], splineNode[splineNumber - 2], playerPosition);
        }  
        else {
            Vector3 leftSegment = findSegment(splineNode[nodeNearestPlayer - 1], splineNode[nodeNearestPlayer], playerPosition);
            Vector3 rightSegment = findSegment(splineNode[nodeNearestPlayer + 1], splineNode[nodeNearestPlayer], playerPosition);

            if((playerPosition - leftSegment).sqrMagnitude <= (playerPosition - rightSegment).sqrMagnitude){
                return leftSegment;
            } else {
                return rightSegment;
            }
        }
        
    }

    private int findClosestSplineNode(Vector3 position){
        int nodeNum = -1;
        float distance = 0.0f;

        

        for (int i = 0; i< splineNumber; i++){
            float sqrLen = (splineNode[i] - position).sqrMagnitude;

            if (distance == 0.0f || sqrLen < distance){
                distance = sqrLen;
                nodeNum = i;
            }
        }

        return nodeNum;
        
    }

    
    private Vector3 findSegment(Vector3 node1, Vector3 node2, Vector3 plypos){
        Vector3 node1_plypos = plypos - node1;
        Vector3 nodeDifference = (node2 - node1);
        Vector3 direction = nodeDifference.normalized;

        float distV1 = Vector3.Dot(direction, node1_plypos);

        if (distV1 < 0.0f){
            return node1;
        }
        else if (distV1 * 2 > nodeDifference.sqrMagnitude){
            return node2;
        }
        else{
            Vector3 fromNode1 = direction * distV1;
            return node1 + fromNode1;
        }
    }
    

}
