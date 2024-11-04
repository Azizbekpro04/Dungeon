using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    // Checks the type of collision made and damages the player accordingly

    public SpriteRenderer bodySprite; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider) // Triggers when the player touches the object
    {
        if (gameObject.tag == "Enemy" && collider.tag == "Player") // Checks if the collision was with an enemy and player
        {
            PlayerHealthController.playerHealth.hitPlayer();
        }
        else if (gameObject.tag == "Object" && collider.tag == "Player" && ((bodySprite.sprite.name == "spikes_1") || (bodySprite.sprite.name == "spikes_2") || (bodySprite.sprite.name == "spikes_3"))) // If the collision was with the player, then hurt them
        { // Checks if the gameObject was a spike

            PlayerHealthController.playerHealth.hitPlayer();
        }
    }

    private void OnTriggerStay2D(Collider2D collider) // Triggers when the player stays in collision with object
    {
        if (gameObject.tag == "Enemy" && collider.tag == "Player") // Checks if the collision was with an enemy and player
        {
            PlayerHealthController.playerHealth.hitPlayer();
        }
        else if (gameObject.tag == "Enemy" && collider.tag == "Player" && ((bodySprite.sprite.name == "spikes_1") || (bodySprite.sprite.name == "spikes_2") || (bodySprite.sprite.name == "spikes_3"))) // If the collision was with the player, then hurt them
        {
            PlayerHealthController.playerHealth.hitPlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Enemy")
        {

            PlayerHealthController.playerHealth.hitPlayer();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Enemy")
        {

            PlayerHealthController.playerHealth.hitPlayer();
        }
    }
}
