using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Manages enemy bullets 

    // *** Enemy bullet behaviours
    public float bulletSpeed; // Speed of the bullet
    private Vector3 direction; // The direction the bullet travels 
    public bool rotate; // Apply rotation to projectile
    public GameObject bulletParticle; // Reference to particle object

    // Start is called before the first frame update
    void Start()
    {
        direction = PlayerController.player.transform.position - transform.position; // Get the current direction of the player
        direction.Normalize(); // Shortens the direction (fastest route)
        // It is called at start so it doesn't chase the player like an enemy, it just goes at the direction of the player at the moment it is fired
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + direction * bulletSpeed * Time.deltaTime; // Make the bullet move towards direction

        if (rotate == true) 
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 1f)); // Makes the bullet rotate
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // When the bullet collides with a player
    {
        Instantiate(bulletParticle, transform.position, transform.rotation); // Create a bullet particle effect at the current location of the bullet
        Destroy(gameObject); // Destroys the gameObject that this script is attached to


        if (collision.tag == "Player")
        {
            PlayerHealthController.playerHealth.hitPlayer();
        }
        Destroy(gameObject);

    }

    private void OnBecameInvisible() // Remove the bullet if it is no longer visible by camera
    {
        Destroy(gameObject);
    }
}
