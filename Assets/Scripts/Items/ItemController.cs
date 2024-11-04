using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour

    // Manages different game items within the environment and their interaction with the player

{
    public GameObject item; // Reference to item
    public float itemCollectDelay = .1f; // Delay before the item can be collected
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
        // bool result = false;
        if (collision.tag == "Player" && item.name == "ItemShieldSmall(Clone)" && itemCollectDelay <= 0) // Checks if the item is a small shield potion
        {
            int amount = 5;
            PlayerHealthController.playerHealth.regenPlayer(amount, item.name); // Regen the player by calling PlayerHealthController
            AudioController.audioManager.playSoundEffect(0); // Play the collected sound
            Destroy(gameObject); // Remove the object after item collected
        }
        if (collision.tag == "Player" && item.name == "ItemHealthSmall(Clone)" && itemCollectDelay <= 0)
        {
            int amount = 5;
            PlayerHealthController.playerHealth.regenPlayer(amount, item.name);
            AudioController.audioManager.playSoundEffect(0);
            Destroy(gameObject);
        }
        if (collision.tag == "Player" && item.name == "ItemShieldLarge(Clone)" && itemCollectDelay <= 0)
        {
            int amount = 10;
            PlayerHealthController.playerHealth.regenPlayer(amount, item.name);
            AudioController.audioManager.playSoundEffect(0);
            Destroy(gameObject);
        }
        if (collision.tag == "Player" && item.name == "ItemHealthLarge(Clone)" && itemCollectDelay <= 0)
        {
            int amount = 10;
            PlayerHealthController.playerHealth.regenPlayer(amount, item.name);
            AudioController.audioManager.playSoundEffect(0);
            Destroy(gameObject);
        }
        if (collision.tag == "Player" && item.name == "ItemCoin(Clone)" && itemCollectDelay <= 0)
        {
            // int amount = 1;
        }

    }
}
