using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArrowBehaviour : MonoBehaviour
{
    Rigidbody2D arrowBody;
    [SerializeField] Vector2 arrowSpeed = new Vector2 (1f, 0f);
    PlayerManager player;
    float xSpeed;
    void Start()
    {
        arrowBody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerManager>();
        xSpeed = player.transform.localScale.x * arrowSpeed.x;
    }

    void Update()
    {
        arrowBody.velocity = new Vector2 (xSpeed,0f);  
        FlipArrowSprite();
    }

    void FlipArrowSprite()
    {
        transform.localScale = new Vector2 (Mathf.Sign(arrowBody.velocity.x),1f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }   
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        Destroy(gameObject);    
    }
}
