using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGizmoScript : MonoBehaviour
{
    [SerializeField] Color gizCol = Color.yellow;
    [SerializeField] float gizRad = 0.5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizCol;
        Gizmos.DrawSphere(transform.position, gizRad);
    }
}
