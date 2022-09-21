using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnScript : MonoBehaviour
{
    [SerializeField] GameObject carPrefab;
    [SerializeField] Transform startNode;
    int carsInSpawnZone = 0;
    bool spawnZoneFree = true;
    float delayTimer = 0;
    public int carsLeftToSpawn = 50;
    

    // Update is called once per frame
    void Update()
    {
        if (DelayCheck(0.75f))
        {
            if (spawnZoneFree)
            {
                Instantiate(carPrefab, transform);
                carsLeftToSpawn--;
            }
        }
    }



    public Transform GetStartNode()
    {
        return startNode;
    }


    bool DelayCheck(float delayLength)
    {
        delayTimer += Time.deltaTime;
        if (delayTimer > delayLength)
        {
            delayTimer = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            carsInSpawnZone++;
            spawnZoneFree = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            carsInSpawnZone--;
            if(carsInSpawnZone == 0)
            {
                spawnZoneFree = true;
            }
        }
    }
}
