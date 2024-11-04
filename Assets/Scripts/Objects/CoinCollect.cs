using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{

    public int value; // Value of the coin

    public float itemCollectDelay; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (itemCollectDelay > 0)
        {
            itemCollectDelay = itemCollectDelay - Time.deltaTime; // Add delay 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && itemCollectDelay <= 0) // Checks if the item is a small shield potion
        {
            LevelManagement.manager.getCoins(value);
            // play sound here
            Destroy(gameObject); // Remove the object after item collected
        }
    }
}
