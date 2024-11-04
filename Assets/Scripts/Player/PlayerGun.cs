using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    // Handles player gun 
    [Header("Gun Details: ")]
    public string gunName; // Name of the gun
    public Sprite gunImage; // Gun image displayed in UI
    public int gunSound; // The sound of the gun when fired
    public GameObject bulletObject; // The bullet to fire
    public Transform[] bulletPoint;  // The position to fire from
    public float fireRate; // The rate of fire for a weapon
    private float fireRateCounter; // Keeps track of fireRate counts


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.player.isActivated && LevelManagement.manager.isPaused == false)
        {
            if (fireRateCounter > 0)
            {
                fireRateCounter = fireRateCounter - Time.deltaTime; // Limits how many bullets can be fired
            } 
            
            else
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) // Make a bullet appear single click if left MB is pressed or holding it down
                {
                    AudioController.audioManager.playSoundEffect(gunSound);
                    Instantiate(bulletObject, bulletPoint[0].position, bulletPoint[0].rotation); // Create new bullet object
                    if (bulletPoint.Length > 1) // If gun has multiple bullet points then fire them
                    {
                        for (int i = 1; i < bulletPoint.Length; i++)
                        {
                            Instantiate(bulletObject, bulletPoint[i].position, bulletPoint[i].rotation);
                        }
                    }

                    fireRateCounter = fireRate; // Reset the counter
                }
                /* if (Input.GetMouseButton(0)) // Holding down the mouse button fire multiple shots
                {
                    fireRateCounter = fireRateCounter - Time.deltaTime; // Count downwards
                    if (fireRateCounter <= 0) // When it reaches zero, fire a bullet
                    {
                        Instantiate(bulletObject, bulletPoint.position, bulletPoint.rotation); // Create a new bullet at the bulletPoint
                        fireRateCounter = fireRate; // Reset the counter
                    }
                } */
            }
        }
    }
}
