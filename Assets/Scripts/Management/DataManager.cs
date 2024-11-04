using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Manages data between scenes
    public static DataManager data;

    public int currentHP; 
    public int maxHP;
    public int currentShield;
    public int maxShield;
    public int playerCoins;

    //public PlayerController currentPlayer; // Keeps track of the current selected character
    //public GameObject position; // The position to spawn the currentPlayer
    // Start is called before the first frame update


    private void Awake()
    {
        data = this;
    }

    void Start()
    {
        // PlayerController hero = Instantiate(currentPlayer, position.transform.position, currentPlayer.transform.rotation);
        // CameraController.cameraController.targetPosition = hero.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadCoins()
    {
        playerCoins = PlayerPrefs.GetInt("playerCoins", 0); // Get the number of coins, default val is 0
    }
}
