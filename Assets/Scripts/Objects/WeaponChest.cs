using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChest : MonoBehaviour
{
    public GunCollect[] gunsList; // Potential guns that can be found from chest

    public SpriteRenderer sprite; // Reference to sprite
    public Sprite openedChest; // Sprite of the chest when it is opened

    public GameObject prompt; // The text prompt message
    private bool triggered; // Checks if prompt message was triggered
    public Transform spawnArea; // Place to spawn 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered == true)
        {
            if (Input.GetKeyDown(KeyCode.Space)) // Check if Spacebar was pressed
            {
                int randomPick = Random.Range(0, gunsList.Length); // Select a gun from list at random

                Instantiate(gunsList[randomPick], spawnArea.position, spawnArea.rotation); // Instantiate the gun at the spawnArea
                AudioController.audioManager.playSoundEffect(12);
                sprite.sprite = openedChest; // Change the sprite to openedChest

                // Play sound here

                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

            }
        }

        if (sprite.sprite == openedChest)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * 2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && sprite.sprite != openedChest) // Check if collision was player
        {
            prompt.SetActive(true); // Enable prompt message
            triggered = true; 

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && sprite.sprite != openedChest || collision.tag == "Player" && sprite.sprite == openedChest) // Check if collision was player
        {
            prompt.SetActive(false); // Disable prompt message
            triggered = false;
        }
    }
}
