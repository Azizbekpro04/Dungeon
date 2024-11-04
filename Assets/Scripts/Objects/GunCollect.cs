using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCollect : MonoBehaviour
{

    public PlayerGun gun; 
    public float gunCollectDelay = .1f; // Delay before the item can be collected
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gunCollectDelay > 0)
        {
            gunCollectDelay = gunCollectDelay - Time.deltaTime; // Add delay 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && gunCollectDelay <= 0)
        {
            bool duplicateGun = false; // Checks whether the player has the gun already

            for (int i = 0; i < PlayerController.player.gunsList.Count; i++)
            {
                if (gun.gunName == PlayerController.player.gunsList[i].gunName)
                {
                    duplicateGun = true; // If it's the same name then its a duplicate 
                }

            }

            if (duplicateGun == false) // If the gun is not duplicated
            {
                PlayerGun collectedGun = Instantiate(gun);
                collectedGun.transform.parent = PlayerController.player.weapon; // Make the weapon in Player a parent of the collectedGun object
                collectedGun.transform.position = PlayerController.player.weapon.position; // Set to same position as weapon
                collectedGun.transform.localRotation = Quaternion.Euler(Vector3.zero); // Set rotation to 0
                collectedGun.transform.localScale = Vector3.one; // Set scale to 1 (size)

                PlayerController.player.gunsList.Add(collectedGun); // Add the collected to the player's gun list
                PlayerController.player.currentGun = PlayerController.player.gunsList.Count - 1; // change currentGun to the newly added gun
                PlayerController.player.changeGun(); // Update the player and make them equip the newly added gun
            }

            Destroy(gameObject); // Destroy gun pickup after touched
            AudioController.audioManager.playSoundEffect(0);
            // Play sound here
        }
    }

}
