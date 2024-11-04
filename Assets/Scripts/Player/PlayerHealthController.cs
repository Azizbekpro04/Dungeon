using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    // Manages the player's health and shield and updates the UI after each change

    public static PlayerHealthController playerHealth;

    public int currentHP; // Keep track of current health
    public int maxHP; // Maximum health of a player

    public int currentShield; // Current shield
    public int maxShield; // Maximum shield


    public float hitInvincibilityTime = 1f; // How much invisibility the player should have 
    private float invincibilityCounter; // How long to remain invicible for



    private void Awake()
    {
        playerHealth = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        maxShield = DataManager.data.maxShield; // Retrieves player stats from dataManager 
        currentShield = DataManager.data.currentShield;
        maxHP = DataManager.data.maxHP;
        currentHP = DataManager.data.currentHP;

        // currentHP = maxHP;

        UserInterfaceController.UIcontroller.HPBar.maxValue = maxHP; // Sets maxValue of the HPBar to maxHP of player
        UserInterfaceController.UIcontroller.HPBar.value = currentHP; // Sets value to current health of player
        UserInterfaceController.UIcontroller.HPText.text = currentHP + "/" + maxHP;

        // currentShield = maxShield;
        UserInterfaceController.UIcontroller.shieldBar.maxValue = maxShield;
        UserInterfaceController.UIcontroller.shieldBar.value = currentShield;
        UserInterfaceController.UIcontroller.shieldText.text = currentShield + "/" + maxShield;

    }

    // Update is called once per frame
    void Update()
    {
        if (invincibilityCounter > 0) // Count down the timer if the player is invincible
        {
            invincibilityCounter = invincibilityCounter - Time.deltaTime;

            if (invincibilityCounter <= 0) // Means if  the invicibility time is over, reset the body of the player back
            {
                PlayerController.player.bodySprite.color = new Color(1f, 1f, 1f, 1f);
                
            }
        }
    }

    public void hitPlayer() // Damage the player and update the health after each hit
    {

        if (invincibilityCounter <= 0) // If the invicibility time is over then hurt the player
        {


            if (currentShield > 0)
            {
                invincibilityCounter = hitInvincibilityTime; // Reset the time

                currentShield = currentShield - 1;

                PlayerController.player.animate.SetTrigger("Hurt"); // Play the hurt animation
                AudioController.audioManager.playSoundEffect(10); // Play the hurt sound
                PlayerController.player.bodySprite.color = new Color(0.87f, 0.27f, 0.27f, 1f); // Adds a hurt effect to the player
                UserInterfaceController.UIcontroller.shieldBar.value = currentShield; // Updates the values
                UserInterfaceController.UIcontroller.shieldText.text = currentShield + "/" + maxShield;
            }
            else
            {
                invincibilityCounter = hitInvincibilityTime;
                currentHP = currentHP - 1;

                if (currentHP <= 0) // 
                {
                    PlayerController.player.gameObject.SetActive(false);
                    // Play death sound
                    CustomCursor.customCursor.OnDisable();
                    UserInterfaceController.UIcontroller.deathUI.SetActive(true);

                }
                PlayerController.player.animate.SetTrigger("Hurt"); // Play the hurt animation
                AudioController.audioManager.playSoundEffect(10); // Play the hurt sound
                PlayerController.player.bodySprite.color = new Color(0.87f, 0.27f, 0.27f, 1f);
                UserInterfaceController.UIcontroller.HPBar.value = currentHP; // Update values after each hit
                UserInterfaceController.UIcontroller.HPText.text = currentHP + "/" + maxHP;
            }
        }
    }

    public void regenPlayer(int healAmount, string itemName) // Regens player's health or shield 
    {
        if (itemName == "ItemShieldSmall(Clone)" || itemName == "ItemShieldLarge(Clone)" || itemName == "ShopItemShield") // Checks what the item is 
        {
            currentShield = currentShield + healAmount; // Heal the player based on amount
            if (currentShield > maxShield) // If the current shield is greater than make it the same as max
            {
                currentShield = maxShield;
            }
            UserInterfaceController.UIcontroller.shieldBar.value = currentShield; // Updates the values
            UserInterfaceController.UIcontroller.shieldText.text = currentShield + "/" + maxShield;
            // return true;
        }
        
        if (itemName == "ItemHealthSmall(Clone)" || itemName == "ItemHealthLarge(Clone)" || itemName == "ShopItemHealth")
        {
            currentHP = currentHP + healAmount;
            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
            UserInterfaceController.UIcontroller.HPBar.value = currentHP; 
            UserInterfaceController.UIcontroller.HPText.text = currentHP + "/" + maxHP;
            // return true;
        }
        // return false;

    }

    public void makeInvincible(float len) // Make the player invincible with the length 
    {
        invincibilityCounter = len; 
    }

    public void upgradeMaxHP(int value)
    {
        maxHP = maxHP + value;
        currentHP = maxHP;
        UserInterfaceController.UIcontroller.HPBar.maxValue = maxHP; // increases Slider value
        UserInterfaceController.UIcontroller.HPText.text = currentHP + "/" + maxHP;
        UserInterfaceController.UIcontroller.HPBar.value = currentHP;
        UserInterfaceController.UIcontroller.HPText.text = currentHP + "/" + maxHP;
    }


}
