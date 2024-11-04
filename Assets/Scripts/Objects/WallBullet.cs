using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBullet : MonoBehaviour
{
    public float bulletSpeed; // Speed of the bullet
    private Vector3 direction; // The direction the bullet travels 
    public GameObject bulletParticle; // Reference to particle object
    public bool inverted; // Facing position of the wall
    // Start is called before the first frame update
    void Start()
    {
        if (inverted == false) // Which way the arrow aims towards up or down
        {
            direction = new Vector3(transform.position.x, transform.position.y-20) - transform.position;
            direction.Normalize();
        }
        else
        {
            direction = new Vector3(transform.position.x, transform.position.y-20) - transform.position;
            direction.Normalize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + direction * bulletSpeed * Time.deltaTime;
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
