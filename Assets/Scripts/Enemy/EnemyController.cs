using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Manages an enemy object and their properties/behaviours

    public Rigidbody2D rigidBody;



    // *** General
    [Header("General: ")]
    public int healthPoints = 200;
    public float walkSpeed; // How fast the enemy walks
    private Vector3 walkDirection; // The direction the enemy walks in
    public SpriteRenderer spriteBody; // The enemy sprite's body
    public GameObject deathSprite;
    public int deathScream; // Sound to play after death
    public GameObject hitParticle;
    public int coinsDrop; // Chance of giving coins after killed
    public float coinsDropRate; // Rate of dropping
    // *** Hurt effects
    private float hitTime = .2f; // How long the hit effect lasts
    private float hitCounter; // Countdown timer of hit

    // *** Behaviours - Enemies can perform different behaviours like patrol, rush, defend, shoot from range, summon objects
    // Patrol
    [Header("Random Patrol: ")]
    public bool canRandomPatrol; // Does the enemy "randomly" patrol the room
    public float patrolLength; // How long the enemy patrols the room
    public float patrolPause; // How long the enemy pauses between a patrol
    private float patrolCounter; // Keeps track of patrols
    private float patrolPauseCounter; // Tracks pauseCounter
    private Vector3 patrolDirection; // Position to move
    [Header("Route Patrol")]
    public bool canRoutePatrol; // Does the enemy patrol using a route
    public Transform[] route; // A route contains points which the enemy can move to
    private int currentPoint; // The current point that the enemy is on
    // Offensive 
    [Header("Attacking: ")]
    public bool isOffensive; // Is the enemy offensive (charges at player)
    public float chaseDistance; // The distance to the player that triggers the chase
    // Defensive 
    [Header("Defensive: ")]
    public bool isDefensive; // Is the enemy defensive (defends position)
    public float defenseDistance; // The distance that triggers the enemy to defend
    // Ranged
    [Header("Ranged: ")]
    public bool isRanged; // Is the enemy ranged
    public int bulletSound; // Sound of bullet
    public Transform bulletPoint; // The place where the bullet originates from
    public GameObject bullet;  // The bullet object
    public float fireRange; // The range from where the enemy shoots from
    public float fireRate; // The rate of fire 
    private float fireRateCounter; // Keeps track of firing rate
    // Summoner
    [Header("Summon: ")]
    public bool isSummoner; // Is the enemy ranged
    public int summonSound;
    public Transform summonPoint; // The place where the summon object spawns
    public GameObject summonObject;  // The summoned object
    public int summonLimit; // How many objects that can be summoned
    public float summonRange; // The range from which causes the summonRange
    public float summonRate;  // The summon rate
    private float summonCounter; // Keeps track of summonCounter
    private List<GameObject> summonedObjectsList = new List<GameObject>(); // Tracks summonedObjects in a list


    // *** Animations
    public Animator animate;

    // Start is called before the first frame update
    void Start()
    {
        if (canRandomPatrol == true)
        {
            patrolPauseCounter = Random.Range(patrolPause * .95f, patrolPause * 1.35f);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (spriteBody.isVisible && PlayerController.player.gameObject.activeInHierarchy) // If the spriteBody is not visible then the enemy behaviours are not performed
        {   // Also checks if the player is still in hierarchy or not
            // Compares position of enemy and player to chaseDistance (is Player in range?)

            walkDirection = Vector3.zero; // Reset to zero as default state if nothing happens


            // Charge at player (isOffensive)
            if (Vector3.Distance(transform.position, PlayerController.player.transform.position) < chaseDistance && isOffensive) // Chase the player if the distance and isOffensive are met
            {
                walkDirection = PlayerController.player.transform.position - transform.position;
                if (PlayerController.player.transform.position.x < transform.position.x) // Face based on the position of the player
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
            else
            {
                if (canRoutePatrol == true)
                {
                    walkDirection = route[currentPoint].position - transform.position; // Move towards the selected point in the route

                    if (route[currentPoint].position.x < transform.position.x) // Face sprite based on the xAxis position
                    {
                        transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1f, 1f, 1f);
                    }
                

                    if (Vector3.Distance(transform.position, route[currentPoint].position) < .1f) // Checks for enemy current route position
                    {
                        currentPoint = currentPoint + 1; // Increase it
                        if (currentPoint >= route.Length) // Checks if the end of the route length has been reached
                        {
                            currentPoint = 0; // Reset it
                        }
                    }
                }

                if (canRandomPatrol == true)
                {
                    if (patrolCounter > 0) // Enemy actively patrolling
                    {
                        patrolCounter = patrolCounter - Time.deltaTime;

                        if (patrolDirection.x < transform.position.x)
                        {
                            transform.localScale = new Vector3(1f, 1f, 1f);
                        }
                        else
                        {

                            transform.localScale = new Vector3(-1f, 1f, 1f);
                        }

                        // Move the enemy
                        walkDirection = patrolDirection;


                        if (patrolCounter <= 0)
                        {
                            patrolPauseCounter = Random.Range(patrolPause * .95f, patrolPause * 1.35f); // Randomly make the enemy pause 

                        }

                    }

                    if (patrolPauseCounter > 0)
                    {
                        patrolPauseCounter = patrolPauseCounter - Time.deltaTime;

                        if (patrolPauseCounter <= 0)
                        {
                            patrolCounter = Random.Range(patrolLength * .95f, patrolLength * 1.35f);

                            patrolDirection = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0f); // The new direction to move
                        }
                    }
                }
            }

            // Defend from player (isDefensive)
            if (Vector3.Distance(transform.position, PlayerController.player.transform.position) < defenseDistance && isDefensive)
            {
                walkDirection = transform.position - PlayerController.player.transform.position;
                if (PlayerController.player.transform.position.x < transform.position.x) // Face based on the position of the player
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }

 
            /*else
            {
            walkDirection = Vector3.zero; // Reset to zero if player is out of range
            } */



        walkDirection.Normalize(); // Make walkspeed consistent even if walking diagonally
        rigidBody.velocity = walkDirection * walkSpeed;

        // *** For ranged enemies
        if (isRanged == true && Vector3.Distance(transform.position, PlayerController.player.transform.position) < fireRange) // Checks if the enemy pos and player pos is less than fireRange
        {
            fireRateCounter = fireRateCounter - Time.deltaTime; // Count backwards

            if (fireRateCounter <= 0) // Reset the counter if it gets to zero or less
            {
                fireRateCounter = fireRate;
                Instantiate(bullet, bulletPoint.position, bulletPoint.rotation); // Create a new bullet at the bulletPoint
            }
        }

        if (isSummoner == true && Vector3.Distance(transform.position, PlayerController.player.transform.position) < summonRange)
            {
                summonCounter = summonCounter - Time.deltaTime; // Count backwards

                if (summonCounter <= 0) // Reset the counter if it gets to zero or less
                {
                    if (summonedObjectsList.Count < summonLimit) // Check limit against list count  
                    {
                        summonCounter = summonRate; // Reset counter
                        summonedObjectsList.Add(Instantiate(summonObject, summonPoint.position, summonPoint.rotation)); // Create new object and add reference to list
                    }
                    else // Otherwise check loop for any missing (destroyed) objects
                    {
                        for (int i = 0; i < summonedObjectsList.Count; i++)
                        {
                            if (summonedObjectsList[i] == null)
                            {
                                summonedObjectsList.RemoveAt(i); // Removes missing objects so new ones can be created
                            }
                        }
                    }

                }
            }

        // ** Manages hurt effects for enemies
        if (hitCounter > 0)
            {
                hitCounter = hitCounter - Time.deltaTime;
                if (hitCounter <= 0)
                {
                    spriteBody.color = new Color(1f, 1f, 1f, 1f);
                }

            }


        // *** Animations
        if (walkDirection != Vector3.zero) // Sets the animation to walking if the enemy is walking or not
        {
            animate.SetBool("isWalking", true);
        }
        else
        {
            animate.SetBool("isWalking", false);
        }


    } else
        {
            rigidBody.velocity = Vector2.zero; // Stops the enemy from moving
        }

    }

    public void HitEnemy(int bulletDamage) // Executes when the player hits an enemy
    {
        


            healthPoints = healthPoints - bulletDamage;

            if (isOffensive == true) // Check if enemy has attacking behaviour
            {
                chaseDistance = 30; // Once the player hits an enemy, make the enemy aggressive forever
            }

            if (isDefensive == true)
            {
                defenseDistance = 30; 
            }

            if (isSummoner == true)
            {
            summonRange = 30;
            }
            
            spriteBody.color = new Color(0.87f, 0.27f, 0.27f, 1f); // Add a hurt effect
            hitCounter = hitTime; // Start the counter which will be used to tell how long the effect lasts
            Instantiate(hitParticle, transform.position, transform.rotation); // Create a hit particle 
            AudioController.audioManager.playSoundEffect(3);
            if (healthPoints <= 0) // Remove the enemy after hp is <= 0
            {
                float chance = Random.Range(0, 100);
                AudioController.audioManager.playSoundEffect(deathScream);
                if (chance < coinsDropRate)
                {
                    LevelManagement.manager.getCoins(coinsDrop);
                    // Display UI
                }


                Destroy(gameObject);
                GameObject obj = Instantiate(deathSprite, transform.position, transform.rotation); // Create a death sprite
                if (PlayerController.player.transform.position.x < transform.position.x) // Make the death sprite face based on the position of the player
                {
                    obj.transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else
                {
                    obj.transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }

        }


    
}
