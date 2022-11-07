using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D enemyBody;
    [SerializeField] float moveSpeed = 1f;
    BoxCollider2D enemyView;
    
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemyView = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        enemyBody.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        moveSpeed = -moveSpeed;
        FlipEnemySprite();
    }

    void FlipEnemySprite()
    {
        transform.localScale = new Vector2 (-(Mathf.Sign(enemyBody.velocity.x)),1f);
    }
}
