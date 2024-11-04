using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChanger : MonoBehaviour
{
    // Allows the player to change their current character into another character

    private bool triggered; // Checks if the character can be playable
    public GameObject prompt; // The message
    public PlayerController playableCharacter; // Know what character to spawn in game

    public bool unlockable; // Whether the character is an unlockable character or a starting character
    // Start is called before the first frame update
    void Start()
    {
        if (unlockable == true) // Checks whether the character is an unlockable character
        {
            if (PlayerPrefs.HasKey(playableCharacter.name))
            {   // Check if key exists of character
                if (PlayerPrefs.GetInt(playableCharacter.name) == 1) // Get the int value stored in the storedCharacter name. If it's 1 then it is unlocked
                {
                    gameObject.SetActive(true); // Set the obj as active
                }
                else
                {
                    gameObject.SetActive(false); // Otherwise set as inactive
                }
            }
            else
            {
                gameObject.SetActive(false); // Otherwise set as inactive
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered == true)
        {
            if (Input.GetKeyDown(KeyCode.Space)) // Check if spacebar was pressed with triggered active
            {
                Vector3 playerCurrentPosition = PlayerController.player.transform.position; // Get the player's current position
                Destroy(PlayerController.player.gameObject); // Destroy the current player
                PlayerController newCharacter = Instantiate(playableCharacter, playerCurrentPosition, playableCharacter.transform.rotation); // Instantiate the new character to play as
                PlayerController.player = newCharacter; // Assign the newCharacter to be the instance of the PlayerController
                AudioController.audioManager.playSoundEffect(13); // Play character change sound
                // Deactivate the character switcher because they are playing as this character already
                gameObject.SetActive(false);

                // Make camera follow the new character
                CameraController.cameraController.targetPosition = newCharacter.transform;
                //DataManager.data.currentPlayer = newCharacter;

                CharacterManager.manager.currentPlayer = newCharacter; // Set the currentPlayer as the new Character selected
                CharacterManager.manager.activePlayerChanger.gameObject.SetActive(true); // Make the last active character selected as active (Reset characters)
                CharacterManager.manager.activePlayerChanger = this; // This character that was activated will become active and deactivated once another character is selected



            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") // Checks if collision was with player
        {
            triggered = true;
            prompt.SetActive(true); // Display the text
        }
                
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag== "Player") // Check if the player left the trigger area
        {
            triggered = false;
            prompt.SetActive(false); // Hide text
        }
    }
}
