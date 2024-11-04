using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public GameObject prompt; // To display prompt message when player enters the area
    private bool triggered; // Checks whether the prompt message was triggered so purchase can be made
    public int itemPrice;

    // Shop items available
    public bool healthRegen;


    public bool shieldRegen;


    public bool itemPickup;


    public bool healthUpgrade;
    public int healthUpgradeValue;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered == true)
        {
            if (Input.GetKeyDown(KeyCode.Space)) // If the player presses the spacebar 
            {
                if (LevelManagement.manager.playerCoins >= itemPrice) // Check if they have enough coins to make purchase
                {
                    LevelManagement.manager.useCoins(itemPrice);
                    AudioController.audioManager.playSoundEffect(11); // Play the purchase sound
                    if (healthRegen == true) // Check what kind of item they are trying to buy
                    {
                        PlayerHealthController.playerHealth.regenPlayer(PlayerHealthController.playerHealth.maxHP, "ShopItemHealth"); 
                    }

                    if (shieldRegen == true)
                    {
                        PlayerHealthController.playerHealth.regenPlayer(PlayerHealthController.playerHealth.maxShield, "ShopItemShield");
                    }
                    
                    if (healthUpgrade == true)
                    {
                        PlayerHealthController.playerHealth.upgradeMaxHP(healthUpgradeValue);
                    }

                    gameObject.SetActive(false); // After purchase is complete, disable the item
                    triggered = false; // Disables item purchase forever
                    
                }
                else
                {
                    // Play sound effect
                }
            }
        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            prompt.SetActive(true);
            triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            prompt.SetActive(false);
            triggered = false;
        }
    }
}
