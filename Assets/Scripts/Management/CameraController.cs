using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Handles the camera movement

    public Camera mainCamera; // Main game camera
    public Camera fullMapCamera; // Camera of the map
    private bool isFullMapActive; 
    public static CameraController cameraController;

    public float cameraSpeed; // How fast the camera moves
    public int cameraZoom = 10; // The zoom of the camera
    
    public Transform targetPosition; // The position where the camera is
    // Start is called before the first frame update



    private void Awake()
    {
        cameraController = this;
    }

    void Start()
    {
        // Camera.main.orthographicSize = 5;
    }

    // Update is called once per frame
    void Update()
    {
        // var scroll = Input.GetAxis("Mouse ScrollWheel");
        /*if (scroll < 0f)
        {
            Camera.main.orthographicSize = Camera.main.orthographicSize + 15 * Time.deltaTime;
            if (Camera.main.orthographicSize > 6)
            {
                Camera.main.orthographicSize = 6; // Max size
            }
        }
        /* 
         * For debugging purposes
         * if (scroll > 0f)
        {
            Camera.main.orthographicSize = Camera.main.orthographicSize - 15 * Time.deltaTime;
            if (Camera.main.orthographicSize < 4)
            {
                Camera.main.orthographicSize = 4; // Min size 
            }
        } */

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isFullMapActive == false) // If the fullmap camera is not open then open it
            {
                openFullMap();
            }
            else
            {
                closeFullMap();
                UserInterfaceController.UIcontroller.mapIcon.enabled = true; // Enable mapIcon
            }
        }

        if (targetPosition != null) // if target is not empty then move the camera otherwise don't move it
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.position.x, targetPosition.position.y, transform.position.z), cameraSpeed * Time.deltaTime);
        }
        
    }

    public void openFullMap()
    {
        if (fullMapCamera == null)  // Stops errors if the fullMapCamera is not in use
        {
            print("fullMapCamera does not exist");
        }
        else if (LevelManagement.manager.isPaused == false) // Should only work if the game is not paused
        {
            isFullMapActive = true; // Set to true as fullMap camera is active
            fullMapCamera.enabled = true; // Enable full map camera
            mainCamera.enabled = false; // Disable mainCamera
            UserInterfaceController.UIcontroller.minimap.SetActive(false); // Make minimap inactive
            UserInterfaceController.UIcontroller.mapIcon.enabled = false; // Turn off the mapIcon
            CustomCursor.customCursor.OnDisable(); // Disables the aiming cursor
            UserInterfaceController.UIcontroller.minimapBackground.SetActive(false); 
            PlayerController.player.isActivated = false; // When fullmap is opened, player controls will be disabled
            Time.timeScale = 0f; // Freeze time so that nothing else moves
        }

    }

    public void closeFullMap()
    {
        if (fullMapCamera == null) // Stops errors if the fullMapCamera is not in use
        {
            print("fullMapCamera does not exist");
        }
        else if (LevelManagement.manager.isPaused == false)
        {
            isFullMapActive = false; // Set to false as fullMap camera is inactive
            fullMapCamera.enabled = false; // Disable full map camera
            mainCamera.enabled = true; // Enable mainCamera
            UserInterfaceController.UIcontroller.minimap.SetActive(true); // Activate minimap
            UserInterfaceController.UIcontroller.mapIcon.enabled = true; // Turn off the mapIcon
            CustomCursor.customCursor.SetCursor(); // Enables the aiming cursor
            UserInterfaceController.UIcontroller.minimapBackground.SetActive(true);
            PlayerController.player.isActivated = true; // Enable player controls when fullmap is closed
            Time.timeScale = 1f; // Freeze time so that nothing else moves
        }
    }

    public void changeCameraToRoom(Transform roomPosition) // Moves camera to the new position
    {
        targetPosition = roomPosition;
    }
}
