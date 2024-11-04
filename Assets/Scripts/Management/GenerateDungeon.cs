using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GenerateDungeon : MonoBehaviour
{

    // Manages the creation and implementation of a randomly generated dungeon that contains a set of room placements and their layouts

    public GameObject placeRoom; // Base placement of a room
    public int distToFinish; // Number of rooms to finish
    public bool hasShop; // Does the dungeon level contain a shop
    public int minDistShop;
    public int maxDistShop;
    // For minimap display
    public Color start; // Starting Room Color
    public Color end; // End Room Color
    public Color shop; // Shop Color 
    public Transform generatePoint; // Position to generate the room

    public RoomLayouts roomLayouts; // Room layouts that a room can have

    public enum RoomDirection // Directions of where to place rooms
    {
        top,
        right,
        left,
        bottom,
    };

    public RoomDirection selected; // The direction that was selected

    public float xAxis = 18f; // Offsets for placing rooms
    public float yAxis = 10f;
    public LayerMask roomLayer; // The layer of the room
    private GameObject lastRoom; // Reference to the last room of the dungeon, the room which the player needs to complete to finish level
    private GameObject shopRoom; // The shop room 
    private List<GameObject> roomList = new List<GameObject>(); // List of rooms in the dungeon
    private List<GameObject> createdLayouts = new List<GameObject>(); // Reference to room layouts that have been built

    // RoomMiddle references
    public RoomMiddle[] middlesList; // List of template room middles to choose from
    public RoomMiddle middleStart; // The starting room
    public RoomMiddle middleEnd; // The ending room 
    public RoomMiddle middleShop; // The shop   


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(placeRoom, generatePoint.position, generatePoint.rotation).GetComponent<SpriteRenderer>().color = start; // Give the starting room a specific color

        selected = (RoomDirection)Random.Range(0, 4); // Cast random number into the direction of RoomDirection
        moveGeneratePoint(); // Move the generate room point to another direction

        for (int i = 0; i < distToFinish; i++) // Count up to rooms limit
        {
            GameObject createdRoom = Instantiate(placeRoom, generatePoint.position, generatePoint.rotation); // Create a new room

            roomList.Add(createdRoom); // Add the createdRoom to the list of rooms in the dungeon


            if (i + 1 == distToFinish) // Check if room limit has been reached
            {
                createdRoom.GetComponent<SpriteRenderer>().color = end; // Make the last room the end room color
                lastRoom = createdRoom;
                roomList.RemoveAt(roomList.Count - 1); // Remove the lastRoom from the list (Separates lastroom from others)
            }

            selected = (RoomDirection)Random.Range(0, 4); // Cast random number into the direction of RoomDirection
            moveGeneratePoint(); // Move the generate room position somewhere else

            while (Physics2D.OverlapCircle(generatePoint.position, .2f, roomLayer)) // While a room is already there in generatePoint then move it to a open space
            {
                moveGeneratePoint();
            }


        }

        if (hasShop == true) // Find a location to create a shop for the dungeon level
        {
            int pickLocation = Random.Range(minDistShop, maxDistShop); // Pick a location to spawn the shop based on minDistance and maxDistance
                                                                       // (Location picked is always minDist or maxDistShop -1)

            shopRoom = roomList[pickLocation]; // The location for the shop
            roomList.RemoveAt(pickLocation); // Remove the shopRoom from the createdLayouts list
            shopRoom.GetComponent<SpriteRenderer>().color = shop; // Assign shop color to shopRoom sprite


        }

        // Create room layouts 
        createRoomLayout(Vector3.zero);
        for (int i = 0; i < roomList.Count; i++)
        {
            createRoomLayout(roomList[i].transform.position); // Create a layout for each room in the list
        }
        createRoomLayout(lastRoom.transform.position); // Create a layout for last room

        if (hasShop == true)
        {
            createRoomLayout(shopRoom.transform.position);
        }


        // Assign middles to the room layouts
        for (int i = 0; i < createdLayouts.Count; i++)
        {
            bool middleStartEndAssigned = false; // Tracks if middleStart/middleEnd was assigned

            if (createdLayouts[i].transform.position == Vector3.zero) // Is roomLayout at start position then its starting room and assign to it
            {
                Instantiate(middleStart, createdLayouts[i].transform.position, transform.rotation).room = createdLayouts[i].GetComponent<RoomController>();
                middleStartEndAssigned = true; 
            }

            if (createdLayouts[i].transform.position == lastRoom.transform.position) // Is the roomLayout the last room, if it is then assign to it
            {
                Instantiate(middleEnd, createdLayouts[i].transform.position, transform.rotation).room = createdLayouts[i].GetComponent<RoomController>();
                middleStartEndAssigned = true;
            }

            if (hasShop == true)
            {
                if (createdLayouts[i].transform.position == shopRoom.transform.position) 
                {
                    // Create a shop at the position if the roomLayout is at shopRoom's position
                    Instantiate(middleShop, createdLayouts[i].transform.position, transform.rotation).room = createdLayouts[i].GetComponent<RoomController>();
                    middleStartEndAssigned = true;
                }
            }

            Random.InitState(System.DateTime.Now.Millisecond); // The random seed will always be different every time
            int randomInt = Random.Range(0, middlesList.Length); // Randomly pick a middle and assign to roomLayout
            

            if (middleStartEndAssigned == false) // If middleStart was not assigned, then generate a random middle
            {
                Instantiate(middlesList[randomInt], createdLayouts[i].transform.position, transform.rotation).room = createdLayouts[i].GetComponent<RoomController>();
                // Assigns a middle to a roomLayout and determines whether the room should be left unlocked/closed
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        /* For testing generated dungeons 
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        */
    }

    public void createRoomLayout(Vector3 position) // Creates a room layout for each room
    {
        // Checks for rooms that may exist in the specified position
        bool roomUp = Physics2D.OverlapCircle(position + new Vector3(0f, yAxis, 0f), 2f, roomLayer);
        bool roomDown = Physics2D.OverlapCircle(position + new Vector3(0f, -yAxis, 0f), 2f, roomLayer);
        bool roomRight = Physics2D.OverlapCircle(position + new Vector3(xAxis, 0f, 0f), 2f, roomLayer);
        bool roomLeft = Physics2D.OverlapCircle(position + new Vector3(-xAxis, 0f, 0f), 2f, roomLayer);

        int count = 0; // Tracks how many directions have been counted
        if (roomUp)
        {
            count = count + 1;
        }
        if (roomDown)
        {
            count = count + 1;
        }
        if (roomRight)
        {
            count = count + 1;
        }
        if (roomLeft)
        {
            count = count + 1;
        }

        switch (count)
        {
            case 1: // Checks how many directions were counted, creates a layout based on it

                if (roomUp == true) 
                {
                    createdLayouts.Add(Instantiate(roomLayouts.up, position, transform.rotation)); // Add the created Layout to the list
                }
                if (roomDown == true)
                {
                    createdLayouts.Add(Instantiate(roomLayouts.down, position, transform.rotation));
                }
                if (roomRight == true)
                {
                    createdLayouts.Add(Instantiate(roomLayouts.right, position, transform.rotation)); 
                }
                if (roomLeft == true)
                {
                    createdLayouts.Add(Instantiate(roomLayouts.left, position, transform.rotation)); 
                }
                break;

            case 2:

                if (roomUp == true && roomDown == true)
                {
                    createdLayouts.Add(Instantiate(roomLayouts.upDown, position, transform.rotation)); 
                }
                if (roomUp == true && roomRight == true)
                {
                    createdLayouts.Add(Instantiate(roomLayouts.upRight, position, transform.rotation)); 
                }
                if (roomLeft == true && roomRight == true)
                {
                    createdLayouts.Add(Instantiate(roomLayouts.leftRight, position, transform.rotation));
                }
                if (roomRight == true && roomDown == true)
                {
                    createdLayouts.Add(Instantiate(roomLayouts.rightDown, position, transform.rotation)); 
                }
                if (roomDown == true && roomLeft == true)
                {
                    createdLayouts.Add(Instantiate(roomLayouts.downLeft, position, transform.rotation)); 
                }
                if (roomLeft == true && roomUp == true)
                {
                    createdLayouts.Add(Instantiate(roomLayouts.leftUp, position, transform.rotation)); 
                }
                break;

            case 3:

                if (roomUp == true && roomRight == true && roomDown == true)
                {
                    createdLayouts.Add(Instantiate(roomLayouts.upRightDown, position, transform.rotation));
                }
                if (roomRight == true && roomDown == true && roomLeft == true)
                {
                    createdLayouts.Add(Instantiate(roomLayouts.rightDownLeft, position, transform.rotation));
                }
                if (roomDown == true && roomLeft == true && roomUp == true)
                {
                    createdLayouts.Add(Instantiate(roomLayouts.downLeftUp, position, transform.rotation));
                }
                if (roomLeft == true && roomUp == true && roomRight == true)
                {
                    createdLayouts.Add(Instantiate(roomLayouts.leftUpRight, position, transform.rotation));
                }
                break;

            case 4:

                if (roomUp == true && roomLeft == true && roomRight == true && roomDown == true)
                {
                    createdLayouts.Add(Instantiate(roomLayouts.upLeftRightDown, position, transform.rotation));
                }

                break;
        }


    }

    public void moveGeneratePoint()
    {
        switch (selected) // Check which direction is selected
        {
            case RoomDirection.top:
                // Move the generatePoint using yAxis.
                generatePoint.position = generatePoint.position + new Vector3(0f, yAxis, 0f);
                break;
            case RoomDirection.bottom:
                // Move the generatePoint using -yAxis.
                generatePoint.position = generatePoint.position + new Vector3(0f, -yAxis, 0f);
                break;
            case RoomDirection.left:
                generatePoint.position = generatePoint.position + new Vector3(-xAxis, 0f, 0f);
                // Move the generatePoint using -xAxis.
                break;
            case RoomDirection.right:
                generatePoint.position = generatePoint.position + new Vector3(xAxis, 0f, 0f);
                // Move the generatePoint using xAxis.
                break;
        }
    }
}


[System.Serializable] // Unity processes as data object and displays in Inspector
public class RoomLayouts // All possible room layouts that a room can have
{
    // One exits room layout
    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;

    // Two exits room layout
    public GameObject upDown;
    public GameObject leftRight;
    public GameObject upRight;
    public GameObject rightDown;
    public GameObject downLeft;
    public GameObject leftUp;

    // Three exits room layout
    public GameObject upRightDown;
    public GameObject rightDownLeft;
    public GameObject downLeftUp;
    public GameObject leftUpRight;

    // Four exits room layout
    public GameObject upLeftRightDown;


}
