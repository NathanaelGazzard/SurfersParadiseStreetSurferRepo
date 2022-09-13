using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnPointScript : MonoBehaviour
{
    [SerializeField] GameObject[] carPrefabs;


    // Start is called before the first frame update
    void Start()
    {
        int carToSpawn = Random.Range(0, carPrefabs.Length);
        GameObject newCar = GameObject.Instantiate(carPrefabs[carToSpawn]);
        Transform initNode = GetComponentInParent<TrafficNodeScript>().RequestNextNode();
        newCar.GetComponent<CarScript>().SetInitialDestination(initNode);
        newCar.transform.position = transform.position;
        newCar.transform.eulerAngles = transform.parent.eulerAngles;
    }
}