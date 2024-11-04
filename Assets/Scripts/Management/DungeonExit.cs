using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Manage scenes
public class DungeonExit : MonoBehaviour
{
    // Exits a dungeon and moves to new scene

    public string levelName;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") // Load a new scene after the player enters trigger area
        {
            StartCoroutine(LevelManagement.manager.exitDungeon()); // Start the coroutine and load the next level
        }
    }
}
