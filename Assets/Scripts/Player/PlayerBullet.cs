using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // Manages player's bullets

    // *** Player Bullet behaviours
    public float bulletSpeed = 7.5f; // How fast the bullet travel
    public Rigidbody2D rigidBody;
    public GameObject bulletParticle; // Reference to particle object

    public int bulletDamage = 50;
    public float lifespanTime; // How long the bullet lasts
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.velocity = transform.right * bulletSpeed; // Moves to the right relative to the object's facing position

        if (lifespanTime > 0)
        {
            lifespanTime = lifespanTime - Time.deltaTime;
            if (lifespanTime <= 0)
            {
                Instantiate(bulletParticle, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Executes when one trigger collision enters another collider
    {
        Instantiate(bulletParticle, transform.position, transform.rotation); // Create a bullet particle effect at the current location of the bullet
        Destroy(gameObject); // Destroys the gameObject that this script is attached to

        if (collision.tag == "Enemy") // Checks if the collision was with the tag "Enemy" assigned to it
        {
            collision.GetComponent<EnemyController>().HitEnemy(bulletDamage); // On the other collider, get the enemyController script and execute hitEnemy function
            // enemy knockback
            Rigidbody2D enemy = collision.GetComponent<Rigidbody2D>();
            if (enemy != null)
            {
                StartCoroutine(knockback(enemy));
            }  
            // collision.GetComponent<EnemyController>().spriteBody.color = new Color(0.87f, 0.27f, 0.27f, 1f);
            //StartCoroutine(delay()); // Delays for some time
            // collision.GetComponent<EnemyController>().spriteBody.color = new Color(1f, 1f, 1f, 1f); // resets enemy back to normal 
        }
    }

    private IEnumerator knockback(Rigidbody2D enemy)
    {

        Vector2 forceDirection = enemy.transform.position - transform.position;
        Vector2 force = forceDirection.normalized * 15;
        enemy.velocity = force;
        yield return new WaitForSeconds(.3f);
        enemy.velocity = new Vector2();
    }


    //IEnumerator delay() // Adds a delay
    //{
    //    yield return new WaitForSeconds(10f);
    //}

    private void OnBecameInvisible() // The bullet object is no longer visible (left the screen), so destroy it
    {
        Destroy(gameObject);

    }
}
