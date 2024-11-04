using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public string levelName; // The name of scene to load
    // public string levelStage;

    public float waitCountdown = 2f;
    public GameObject continueButton;
    public GameObject exitButton;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;

        Destroy(PlayerController.player.gameObject); // Destroy object after complete screen displayed
    }

    // Update is called once per frame
    void Update()
    {
        if (waitCountdown > 0) // Add a short countdown before displaying buttons
        {
            waitCountdown = waitCountdown - Time.deltaTime; 
            if (waitCountdown <= 0)
            {
                continueButton.SetActive(true);
                exitButton.SetActive(true);
            }
            else
            {
                if (Input.anyKeyDown) // Once countdown is over, any key pressed or buttons pressed will navigate to next scene
                {
                    continueLevel();
                }
            }
        }
    }

    public void continueLevel() // Save playerstats and load scene
    {
        PlayerPrefs.SetInt("playerCoins", DataManager.data.playerCoins); // Save coins based on the player's current playerCoins after completing dungeon 
        SceneManager.LoadScene(levelName);

    }

    public void exitLevel() // Load the level
    {
        SceneManager.LoadScene("MainMenu");

    }
}
