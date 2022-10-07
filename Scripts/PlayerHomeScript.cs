using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHomeScript : MonoBehaviour
{
    [SerializeField] GameObject[] items = new GameObject[8];
    [SerializeField] GameManagerScript gameManagerRef;



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (gameManagerRef.wishlistItems[i].isPurchased)
            {
                items[i].SetActive(true);
            }
        }
    }
}
