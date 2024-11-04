using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManagement : MonoBehaviour
{
    // Manages the dungeon level such as moving from one level to next, game paused, buying things etc

    public static LevelManagement manager;
    public Transform spawnPoint; // Where to spawn the player

    public float delayTimer = 3f; // The wait 

    public string levelName; // Name of level/scene to progress to 

    public bool isPaused; // Checks if game is paused

    public int playerCoins; // Tracks playerCoins

    private void Awake()
    {
        manager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.player.transform.position = spawnPoint.position; // Spawn Player in spawnPoint position 
        PlayerController.player.isActivated = true; // Activate player controls
        Time.timeScale = 1f; // Make the time back to normal when new game is loaded
        DataManager.data.loadCoins(); // Loads coins
        playerCoins = DataManager.data.playerCoins; // Retrieves player's Coins from data

        UserInterfaceController.UIcontroller.playerCoins.text = playerCoins.ToString(); // Set the coin text to the playerCoins
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Pause the game when key P is pressed
        {
            pauseLevel();
        }
    }

    public void pauseLevel()
    {
        if (!isPaused == true) // If the game isnt paused then pause it
        {
            CustomCursor.customCursor.OnDisable();
            UserInterfaceController.UIcontroller.pauseUI.SetActive(true);
            UserInterfaceController.UIcontroller.pauseButton.enabled = false;
            UserInterfaceController.UIcontroller.pauseButtonIcon.enabled = false;
            isPaused = true;

            pauseTime(0f);
            
        }
        else // Otherwise it is paused so unpause it
        {
            CustomCursor.customCursor.SetCursor();
            resumeLevel();

            isPaused = false;
            UserInterfaceController.UIcontroller.pauseButton.enabled = true;
            UserInterfaceController.UIcontroller.pauseButtonIcon.enabled = true;
            pauseTime(1f);
        }
    }

    public void pauseTime(float value) // Pauses the current time or reactivate time again based on value
    {
        Time.timeScale = value;
    }


    public void resumeLevel() // Resumes level
    {
        UserInterfaceController.UIcontroller.pauseUI.SetActive(false);
    }

    public IEnumerator exitDungeon() // Coroutine
    {
        // AudioManager.instance.PlayVictory();
        AudioController.audioManager.playVictoryMusic();
        PlayerController.player.isActivated = false;

        UserInterfaceController.UIcontroller.startFadeIn(); // Fading background

        yield return new WaitForSeconds(delayTimer);  // Wait for some time
        DataManager.data.playerCoins = playerCoins; // Keeps player data from current state into DataManager so it can be retrieved from new scenes
        DataManager.data.currentHP = PlayerHealthController.playerHealth.currentHP;
        DataManager.data.currentShield = PlayerHealthController.playerHealth.currentShield;
        DataManager.data.maxHP = PlayerHealthController.playerHealth.maxHP;
        DataManager.data.maxShield = PlayerHealthController.playerHealth.maxShield;

        SceneManager.LoadScene(levelName);


    }

    public void getCoins(int value) // Gives the player coins after an event
    {
        playerCoins = playerCoins + value;
        UserInterfaceController.UIcontroller.playerCoins.text = playerCoins.ToString();
    }

    public void useCoins(int value) // Using coins to buy things
    {
        playerCoins = playerCoins - value;

        if (playerCoins < 0) 
        {
            playerCoins = 0;
        }
        UserInterfaceController.UIcontroller.playerCoins.text = playerCoins.ToString();
    }
}
