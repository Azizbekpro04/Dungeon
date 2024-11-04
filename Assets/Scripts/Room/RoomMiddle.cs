using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMiddle : MonoBehaviour
{
    public List<GameObject> roomEnemies = new List<GameObject>(); // The enemies that belong to a room
    public bool openOnEnemyKill; // Open doors when enemies are killed
    public RoomController room;
    // Start is called before the first frame update 
    void Start()
    {
        if (openOnEnemyKill == true) // Checks to make sure Room knows whether to make doors open or closed on enter
        {   
            room.closeOnEnter = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (roomEnemies.Count > 0 && room.isActive && openOnEnemyKill)
        {
            for (int i = 0; i < roomEnemies.Count; i++)
            {
                if (roomEnemies[i] == null) // Check if the object is missing
                {
                    roomEnemies.RemoveAt(i); // Remove the missing object position
                    i = i - 1; // Remove the enemy, and recheck slot again (accurate)
                }
            }

            if (roomEnemies.Count == 0) // If the enemies are killed then open all doors
            {
                room.UnlockDoors();
            }
        }
    }
}
