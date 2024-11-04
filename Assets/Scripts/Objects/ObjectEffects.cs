using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEffects : MonoBehaviour
{
    // Manages object's effects when they are interacted with
    public SpriteRenderer sprite;
    public float speed = 4f;
    private Vector3 direction;
    public float stopSpeed = 6f; // 
    public float lifespan = 4f;
    public float fadeTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        direction.x = Random.Range(-speed, speed);
        direction.y = Random.Range(-speed, speed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + direction * Time.deltaTime;

        direction = Vector3.Lerp(direction, Vector3.zero, stopSpeed * Time.deltaTime);  // Slows down pieces 

        lifespan = lifespan - Time.deltaTime; // Count down
        if (lifespan < 0)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.MoveTowards(sprite.color.a, 0f, fadeTime * Time.deltaTime));
            if (sprite.color.a == 0f) // If a is 0, then it has faded so destroy it
            {
                Destroy(gameObject);
            }
            
        }


    }
}
