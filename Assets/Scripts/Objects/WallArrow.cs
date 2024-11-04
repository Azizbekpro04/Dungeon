using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallArrow : MonoBehaviour
{
    // Creates a projectile that is fired when player enters the trigger area

    public Transform bulletPoint; // The place where the bullet originates from
    public GameObject bullet;  // The bullet object
    public float fireRange; // The range from where the enemy shoots from
    public float fireRate; // The rate of fire 
    private float fireRateCounter; // Keeps track of firing rate
    private bool triggered; // Checks whether player is in the area
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered) // Checks if the enemy pos and player pos is less than fireRange
        {
            fireRateCounter = fireRateCounter - Time.deltaTime; // Count backwards

            if (fireRateCounter <= 0) // Reset the counter if it gets to zero or less
            {
                AudioController.audioManager.playSoundEffect(16); // Play sound
                fireRateCounter = fireRate;
                Instantiate(bullet, bulletPoint.position, bulletPoint.rotation); // Create a new bullet at the bulletPoint
                
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            triggered = true;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            triggered = false;
        }
    }
}
