using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCharacter : MonoBehaviour
{
    private bool triggered;
    public GameObject prompt;
    public PlayerChanger[] characterPicker; // List of characters that can be freed
    private PlayerChanger characterToUnlock; // The character to unlock
    public SpriteRenderer sprite; // Ref to sprite
    // Start is called before the first frame update
    void Start()
    {
        characterToUnlock = characterPicker[Random.Range(0, characterPicker.Length)]; // Randomly select a char from charlist as one to unlock
        sprite.sprite = characterToUnlock.playableCharacter.bodySprite.sprite; // Set the sprite to the characterToUnlock sprite

    } 

    // Update is called once per frame
    void Update()
    {
        if (triggered == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerPrefs.SetInt(characterToUnlock.playableCharacter.name, 1); // Save character name and int value (1) to PlayerPrefs
                
                // Instantiate(characterToUnlock, transform.position, transform.rotation);

                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Activate when player enters the trigger area
    {
        if (collision.tag == "Player")
        {
            triggered = true;
            prompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // Deactivate after the player leaves the trigger area
    {
        if (collision.tag == "Player")
        {
            triggered = false;
            prompt.SetActive(false);
        }
    }
}
