using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Access UI elements of Unity
using UnityEngine.SceneManagement;
public class UserInterfaceController : MonoBehaviour
{
    // Creates a UIcontroller, references UI elements which are used in other scripts and executes UI elements when needed

    public static UserInterfaceController UIcontroller;

    public string mainMenu;
    public string newGame;

    // *** Main User Interface
    public Image pauseButton;
    public Image pauseButtonIcon;
    // The HP Bar properties
    public Slider HPBar;
    public Text HPText;
    // The Shield properties
    public Slider shieldBar;
    public Text shieldText;
    // Player coin
    public Text playerCoins;
    // Gun details
    public Image currentGunImage;
    public Text currentGunText;

    // Minimap
    public Image mapIcon;
    public GameObject minimap;
    public GameObject minimapBackground;

    // Screen when the player is dead
    public GameObject deathUI;

    // Screen when game is paused
    public GameObject pauseUI;

    // Fading image background
    public Image fadeBackground;
    public float fadeTime;
    private bool fadeIn;
    private bool fadeOut;

    //Scene name fade text
    public Text fadeText;
    public float textFadeTime;
    private bool textFadeIn;
    private bool textFadeOut;

    //Invincible Text
    public GameObject invincible;

    //RoomCleared tex
    public GameObject roomCleared;
    public float roomClearedTime;

    
    public void Awake()
    {
        UIcontroller = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        fadeOut = true; // At start, make fade out from black background
        fadeIn = false;

        
        textFadeOut = true;
        textFadeIn = false;

        currentGunImage.sprite = PlayerController.player.gunsList[PlayerController.player.currentGun].gunImage; 
        currentGunText.text = PlayerController.player.gunsList[PlayerController.player.currentGun].gunName;

        // this.StartCoroutine(this.displayRoomCleared()); test
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut == true)
        {
            fadeBackground.color = new Color(fadeBackground.color.r, fadeBackground.color.g, fadeBackground.color.b, Mathf.MoveTowards(fadeBackground.color.a, 0f, fadeTime * Time.deltaTime));
            
            if (fadeBackground.color.a == 0f)
            {
                fadeOut = false; // Set to false once the background has faded
            }
        }

        if (fadeIn == true)
        {
            fadeBackground.color = new Color(fadeBackground.color.r, fadeBackground.color.g, fadeBackground.color.b, Mathf.MoveTowards(fadeBackground.color.a, 1f, fadeTime * Time.deltaTime));
            
            if (fadeBackground.color.a == 1f)
            {
                fadeIn = false; 
            }

        }

        if (textFadeOut == true)
        {
            fadeText.color = new Color(fadeText.color.r, fadeText.color.g, fadeText.color.b, Mathf.MoveTowards(fadeText.color.a, 0f, textFadeTime * Time.deltaTime));

            if (fadeText.color.a == 0f)
            {
                textFadeOut = false; // Set to false once the background has faded
            }
        }

        if (textFadeIn == true)
        {
            fadeText.color = new Color(fadeText.color.r, fadeText.color.g, fadeText.color.b, Mathf.MoveTowards(fadeText.color.a, 1f, textFadeTime * Time.deltaTime));

            if (fadeText.color.a == 1f)
            {
                textFadeIn = false;
            }

        }
    }

    public void loadNewGame() // Loads new game
    {
        Time.timeScale = 1f; // Reset time back to normal when new game is loaded
        SceneManager.LoadScene(newGame);
        Destroy(PlayerController.player.gameObject); // Destroy the player object when new game is made
        CustomCursor.customCursor.SetCursor(); // Set the game cursor
    }

    public void exitGame() // Exits back to main menu
    {
        Time.timeScale = 1f; 

        SceneManager.LoadScene("MainMenu");

        Destroy(PlayerController.player.gameObject); // Destroy when on the main menu
    }

    public void resumeGame() // Resumes game from pause state
    {
        LevelManagement.manager.pauseLevel();
    }

    public void startFadeIn() // Starts the fading in animation
    {
        fadeIn = true;
        fadeOut = false;
    }

    public IEnumerator displayRoomCleared() // Display the roomcleared text once a dungeon room has been cleared of enemies
    {
        roomCleared.SetActive(true); // Enable game object
        var anim = roomCleared.GetComponent<Animator>(); // Animator reference
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime); // Wait until the animation finishes properly
        roomCleared.SetActive(false); // Disable game object
    }
}
