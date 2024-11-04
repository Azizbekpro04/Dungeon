using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    // Handles room behaviours and objectives for a specific room

    public bool closeOnEnter; // Room will close when it is entered
    public GameObject[] roomDoors; // Number of doors in the room
    // public bool openOnEnemyKill; // Open doors when enemies are killed

    // public List<GameObject> roomEnemies = new List<GameObject>(); // The enemies that belong to a room

    [HideInInspector]
    public bool isActive; // Check if room is active
    public GameObject mapFog; // The gameObject that hides room from minimap
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (roomEnemies.Count > 0 && isActive && openOnEnemyKill)
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
                foreach (GameObject roomDoor in roomDoors)
                {
                    roomDoor.SetActive(false); // Make all the doors of the room deactive
                    closeOnEnter = false; // Make the rooms doors always open
                }
            }
        } */
    }

    public void UnlockDoors()
    {
        foreach (GameObject roomDoor in roomDoors)
        {
            roomDoor.SetActive(false); // Make all the doors of the room deactive
            closeOnEnter = false; // Make the rooms doors always open
            StartCoroutine(UserInterfaceController.UIcontroller.displayRoomCleared()); // Play the cleared room animation
            AudioController.audioManager.playSoundEffect(15);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CameraController.cameraController.changeCameraToRoom(transform); // When player enters a room, move camera there
            if (closeOnEnter == true)
            {
                foreach(GameObject roomDoor in roomDoors)
                {
                    roomDoor.SetActive(true); // Make all the doors of the room active
                }
            }
            isActive = true; // Setroom as active on enter

            mapFog.SetActive(false); 
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isActive = false;// Make room inactive when the player leaves it
        }
    }
}
