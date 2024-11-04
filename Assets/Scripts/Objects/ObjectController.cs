using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    // Handles objects in the rooms and their interactions
    public GameObject[] objectPieces; // For objects that have multiple pieces
    public int maxObjectPieces = 4;
    public bool droppable; // Does this object drop items
    public GameObject[] droppableItems; // The items that it drops
    public float dropChance; // How likely does an item drop


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (PlayerController.player.jumpCounter > 0) // If the character is jumping then the object can be destroyed
            {
                objectDrops(Random.Range(3, maxObjectPieces)); //
            }
        }
        if (collision.gameObject.tag == "Enemy")
        {
            objectDrops(Random.Range(3, maxObjectPieces)); // 
        }



    }

    public void objectDrops(int dropRate) // 
    {
        Destroy(gameObject);
        // Play sound
        AudioController.audioManager.playSoundEffect(1);
        for (int i = 0; i < dropRate; i++) // Loop and select at random the object pieces and instantiate them
        {
            int selectRandom = Random.Range(0, objectPieces.Length); // Select a piece at random

            Instantiate(objectPieces[selectRandom], transform.position, transform.rotation); // Instantiate it after crate destroyed
        }

        // Drop item
        if (droppable == true)
        {
            float generateNumber = Random.Range(0f, 100f);

            if (generateNumber < dropChance)
            {
                int pickItem = Random.Range(0, droppableItems.Length);

                Instantiate(droppableItems[pickItem], transform.position, transform.rotation);
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") // Check if the object is a crate
        {
            if (PlayerController.player.jumpCounter > 0) // If the character is jumping then the object can be destroyed
            {
                Destroy(gameObject);

                // Show effects
                int dropRate = Random.Range(1, maxObjectPieces); // Random number of object pieces to drop

                for (int i = 0; i < dropRate; i++) // Loop and select at random the object pieces and instantiate them
                {
                    int selectRandom = Random.Range(0, objectPieces.Length); // Select a piece at random

                    Instantiate(objectPieces[selectRandom], transform.position, transform.rotation); // Instantiate it after crate destroyed
                }

                // Drop item
                if (droppable == true)
                {
                    float generateNumber = Random.Range(0f, 100f); 
                    
                    if (generateNumber < dropChance)
                    {
                        int pickItem = Random.Range(0, droppableItems.Length);

                        Instantiate(droppableItems[pickItem], transform.position, transform.rotation);
                    }
                }
                
            }
            

            
        }
        if (collision.gameObject.tag == "Bullet")
        {
            objectDrops(Random.Range(3, maxObjectPieces)); // 
        }
    }
}
