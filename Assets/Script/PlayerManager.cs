using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{  
    Vector2 moveInput;
    Rigidbody2D playerBody;
    CapsuleCollider2D playerCollider;
    BoxCollider2D playerFeetCollider;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 1f;
    [SerializeField] Vector2 hitReaction = new Vector2(10f, 10f);
    Animator playerAnimator;
    Vector2 playerVelocity;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;
    public bool playerIsAlive = true;
    


    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>(); 
    }

    
    void Update()
    {
        if (!playerIsAlive)
        {
            return;
        }
        Run();
        FlipSprite();
        ClimbingLadder();
        JumpAnimation();
        PlayerOnHit();
    }

    void OnMove(InputValue value)
    {
        if (!playerIsAlive)
        {
            return;
        }
           moveInput = value.Get<Vector2>(); 
    }
    
    void OnJump(InputValue value)
    {
        if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform")) || !playerIsAlive)
            {
                    return;
            }
        playerBody.velocity += new Vector2(0f, jumpSpeed);
        
    }
    void JumpAnimation()
    {
         if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            playerAnimator.SetBool("isJumping", true);
        }
        else
        {
            playerAnimator.StopPlayback();
            playerAnimator.SetBool("isJumping", false);
        }

    }
    void Run()
    {
        if (!playerIsAlive)
        {
            return;
        }

        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerBody.velocity.y);
        playerBody.velocity = playerVelocity;
        bool playerIsRunning = Mathf.Abs(playerBody.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", playerIsRunning);
    }
    void ClimbingLadder()
    {
        if(!playerBody.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            //disable control "w" here
            playerAnimator.SetBool("isClimbing", false);
            playerBody.gravityScale = 4;
            playerAnimator.StopPlayback();
            return;
        }
        //enable control "w" here
        playerAnimator.SetBool("isClimbing", true);
        Vector2 climbVelocity = new Vector2(playerBody.velocity.x, moveInput.y * climbSpeed);
        playerBody.velocity = climbVelocity;
        playerBody.gravityScale = 0;
        bool playerIsClimbing = Mathf.Abs(playerBody.velocity.y) > Mathf.Epsilon;
        if (playerIsClimbing)
        {
            playerAnimator.StopPlayback();
        }
        else
        {
            playerAnimator.StartPlayback();
        }
    }
    void FlipSprite()
    {
        bool playerHasMovement = Mathf.Abs(playerBody.velocity.x) > Mathf.Epsilon;
        if (playerHasMovement && playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            transform.localScale = new Vector2 (Mathf.Sign(playerBody.velocity.x),1f);
        }
    }
    void PlayerOnHit()
    {
        if(playerBody.IsTouchingLayers(LayerMask.GetMask("Enemy","Hazards")))
        {
            playerBody.velocity = hitReaction;
            playerIsAlive = false;
            playerAnimator.SetTrigger("isHitted");
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
    void OnFire(InputValue value)
    {
        if (!playerIsAlive)
        {
            return;
        }
        Instantiate(arrow, bow.position, transform.rotation);
        playerAnimator.SetTrigger("isFiring");
    }
}