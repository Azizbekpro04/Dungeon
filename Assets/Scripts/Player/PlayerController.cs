using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Manages the player object such as movement, available guns, animations etc

    // *** PlayerController
    public static PlayerController player; // Set for all playerControllers

    [HideInInspector]
    public bool isActivated = true; // Is the player's controls activated
    // *** Movement
    public float walkSpeed; // How fast the player walks
    private Vector2 walkInput;

    // *** Rest detect TODO
    private float restStart;
    private float restLength;

    // *** 2D Physics
    public Rigidbody2D rigidBody;

    // *** Weapon reference
    public Transform weapon;

    // *** Animations
    public Animator animate;

    public SpriteRenderer bodySprite;

    // *** Jumping ability
    [Header("Jumping: ")]
    private float activeWalkSpeed; // The current walking speed
    public float jumpSpeed = 6f; // How fast can the player jump
    public float jumpLength = .5f; // How long the jump should be
    [HideInInspector]
    public float jumpCooldown = 1f; // Wait until the next jump
    public float jumpInvincibility = .6f;
    [HideInInspector] // don't display the below variable in inspector
    public float jumpCounter; 
    private float jumpCooldownCounter;

    public List<PlayerGun> gunsList = new List<PlayerGun>();
    [HideInInspector]
    public int currentGun;
    private void Awake() // Executes as object exists in world
    {
        player = this; // Makes one single version of the instance, set player to this
        DontDestroyOnLoad(gameObject); // Does not destroy the Player object when a new Scene loads 
    }

    // Start is called before the first frame update
    void Start() 
    {
        // gameCamera = Camera.main; // set to Camera.main at the start of level **FOR TESTING**
        activeWalkSpeed = walkSpeed;

        UserInterfaceController.UIcontroller.currentGunImage.sprite = gunsList[currentGun].gunImage; // Get the current gun image from the gunsList and display in UI
        UserInterfaceController.UIcontroller.currentGunText.text = gunsList[currentGun].gunName; // Display gun text in UI
    }

    // Update is called once per frame
    void Update() 
    {

        if (isActivated && LevelManagement.manager.isPaused == false) // If the player's controls are activated then do the following...
        {
            // *** Movement 
            walkInput.x = Input.GetAxisRaw("Horizontal"); // MoveInput x element is set to the Input.GetAxis horizontal control
            walkInput.y = Input.GetAxisRaw("Vertical"); // MoveInput y element is set to the Input.GetAxis vertical control
            walkInput.Normalize(); // Makes the walking speed consistent even if player moves diagonally
                                   // transform.position = transform.position + new Vector3(walkInput.x * Time.deltaTime * walkSpeed, walkInput.y * Time.deltaTime * walkSpeed, 0f); // updates the current pos using x,y values
                                   // walkInput.x * Time.Delta allows consistent walking at different frames, walkSpeed makes it faster 
            rigidBody.velocity = walkInput * activeWalkSpeed;


            // *** Mouse Inputs
            Vector3 mousePosition = Input.mousePosition; // Gets the current position of the mouse on-screen
            Vector3 screenPointer = CameraController.cameraController.mainCamera.WorldToScreenPoint(transform.localPosition); // The current position of the player on-screen

            // Check what direction the player is facing - Right or Left
            if (mousePosition.x > screenPointer.x)
            {
                transform.localScale = new Vector3(1f, 1f, 1f); // Changes the scale of objects based on facing direction
                weapon.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                weapon.localScale = new Vector3(-1f, -1f, 1f);
            }


            // *** Rotate weapon based on mouse position
            // Change the rotation of the weapon
            Vector2 offset = new Vector2(mousePosition.x - screenPointer.x, mousePosition.y - screenPointer.y); // How far away we are from the two points
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg; // Gives the degree of the angle that the gun is pointing to
                                                                           // https://answers.unity.com/questions/1032673/how-to-get-0-360-degree-from-two-points.html
                                                                           // 2d ranged shooting https://www.youtube.com/watch?v=bY4Hr2x05p8
            weapon.rotation = Quaternion.Euler(0, 0, angle); // Converts z to new vector3 value


            /* // *** Firing bullets with Mousebutton (move to PlayerGun script)
            if (Input.GetMouseButtonDown(0)) // Make a bullet appear single click if left MB is pressed
            {
                Instantiate(bulletObject, bulletPoint.position, bulletPoint.rotation); // Create new bullet object
                fireRateCounter = fireRate; // Reset the counter
            }
            if (Input.GetMouseButton(0)) // Holding down the mouse button fire multiple shots
            {
                fireRateCounter = fireRateCounter - Time.deltaTime; // Count downwards
                if (fireRateCounter <= 0) // When it reaches zero, fire a bullet
                {
                    Instantiate(bulletObject, bulletPoint.position, bulletPoint.rotation); // Create a new bullet at the bulletPoint
                    fireRateCounter = fireRate; // Reset the counter
                }
            } */


            // *** Change weapon based on gunsList
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (gunsList.Count > 0)
                {
                    currentGun = currentGun + 1;
                    if (currentGun >= gunsList.Count) // Change the current gun back to the first one in the list
                    {
                        currentGun = 0;
                    }

                    changeGun();
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (gunsList.Count > 0)
                {
                    if (currentGun == 0)
                    {
                        currentGun = gunsList.Count - 1; // If the current gun is the first one, then move the current one back to the last element
                    }
                    else
                    {
                        currentGun = currentGun - 1;
                    }
                    changeGun();

                }
            }


            // *** Player jump ability
            if (Input.GetKey(KeyCode.LeftShift)) // Check if the left shift key is pressed, is jumpCooldown
            {
                if (jumpCooldownCounter <= 0 && jumpCounter <= 0) // Checks if the jump ability is ready to be executed
                {
                    activeWalkSpeed = jumpSpeed; // Set the current walking speed to jumpSpeed 
                    jumpCounter = jumpLength; // Begin the jumpCounter
                    animate.SetTrigger("Jump"); // Perform the jump animation when the ability is triggered
                    PlayerHealthController.playerHealth.makeInvincible(jumpInvincibility);
                    UserInterfaceController.UIcontroller.invincible.SetActive(true); // Enable the animated text
                    AudioController.audioManager.playSoundEffect(9); // Play the fire sound effect
                    Time.timeScale = 0.6f; // Slow down time
                    
                }

            }

            if (jumpCounter > 0) // Check if the jumpCounter has been activated
            {
                jumpCounter = jumpCounter - Time.deltaTime;
                if (jumpCounter <= 0) // Check if the jumpCounter has run out, then deactivate it
                {
                    activeWalkSpeed = walkSpeed; // Set the active back to normal speed
                    UserInterfaceController.UIcontroller.invincible.SetActive(false); // Disable animated text
                    Time.timeScale = 1f;

                    jumpCooldownCounter = jumpCooldown; // Start counting once the jump ends
                }
            }

            if (jumpCooldownCounter > 0) // Checks if jumpCooldown is active, then make it count down
            {
                jumpCooldownCounter = jumpCooldownCounter - Time.deltaTime;

            }


            // *** Animations
            if (walkInput != Vector2.zero) // Sets the animation to walking if the player presses the walking buttons
            {
                animate.SetBool("isWalking", true);
            }
            else
            {
                animate.SetBool("isWalking", false);
            }
        }
        else
        {
            rigidBody.velocity = Vector2.zero; // Stop moving the player if their controls are not activated anymore
            animate.SetBool("isWalking", false);
        }

    }



    public void changeGun() // Allows the player to change the gun if it is in their inventory
    {
        for (int i = 0; i < gunsList.Count; i++)
        {
            gunsList[i].gameObject.SetActive(false); // Set all guns active to false
        }
        UserInterfaceController.UIcontroller.currentGunImage.sprite = gunsList[currentGun].gunImage; // Get the current gun image from the gunsList and display in UI
        UserInterfaceController.UIcontroller.currentGunText.text = gunsList[currentGun].gunName; // Get current gun text and display in UI
        // Play gun change sound
        gunsList[currentGun].gameObject.SetActive(true); // Set the current changed gun to true
    }
}
