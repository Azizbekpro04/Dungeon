using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // The level to play
    public string levelName;

    // For displaying screens in Main Menu
    public GameObject mainScreen;
    public GameObject helpScreen;

    // Buttons for turning on/off sound effects
    public GameObject soundActiveButton;
    public GameObject soundInactiveButton;

    public GameObject newGamePrompt; // Warning prompt
    public GameObject deletedGamePrompt; // Displays once game data was deleted 
    public PlayerChanger[] characterList; // List of characters

    public AudioSource audioMusic;
    // Handles Main Menu UI
    // Start is called before the first frame update
    void Start()
    {
        if (AudioListener.volume == 0) // If the volume was already muted then display the correct icon
        {
            soundActiveButton.SetActive(false);
            soundInactiveButton.SetActive(true);
        }
        else
        {
            soundActiveButton.SetActive(true);
            soundInactiveButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void newGame() // Load the level
    {
        SceneManager.LoadScene(levelName);

    }

    public void soundTrigger() // Turn off/on sound effects
    {
        if (AudioListener.volume == 1) // If the audio source is already running then turn it off
        {
            audioMusic.enabled = false;
            AudioListener.volume = 0; // Set global volume to 0
            soundActiveButton.SetActive(false);
            soundInactiveButton.SetActive(true);
        }
        else
        {
            audioMusic.enabled = true;
            AudioListener.volume = 1;
            soundActiveButton.SetActive(true);
            soundInactiveButton.SetActive(false);
        }


    }

    public void overwriteSave() // Opens the panel which has the prompt whether to restart data
    {
        newGamePrompt.SetActive(true); // Display the prompt
    }

    public void confirmDelete() 
    {
        newGamePrompt.SetActive(false);
        foreach (PlayerChanger character in characterList) // Deletes data 
        {
            PlayerPrefs.SetInt(character.playableCharacter.name, 0); // Delete save by setting value to 0 of all chars
        }
        PlayerPrefs.SetInt("playerCoins", 0); // Erase playerCoins
        deletedGamePrompt.SetActive(true);
        // newGame(); // Executes new game
    }

    public void closeDeletedPrompt() // Disables the prompt onclick
    {
        deletedGamePrompt.SetActive(false);
    }

    public void cancelNewgame()
    {
        newGamePrompt.SetActive(false);
    }

    public void exitHelpScreen() // When the player presses back button
    {
        helpScreen.SetActive(false);
        mainScreen.SetActive(true);
    }

    public void exitGame() // Quit the game
    {
        print("Quit app");
        Application.Quit();
    }

    public void help() // Activate the help screen and deactivate the main screen
    {
        mainScreen.SetActive(false);
        helpScreen.SetActive(true);
    }
}
