using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class Player_Movement : MonoBehaviour
{
    public GameObject FirePoint; //Empty object to shoot bullets from
    private Rigidbody2D rb; //player rigidbody
    private Animator anim; //animator
    static public float moveSpeed; //used to increase move speed
    private float dirX; //x axis speed, constant velocity
    private bool facingRight = true;
    private Vector3 localScale; //player scale
    static public bool CanMove = true; //this is accessed by other scripts to stop player from moving


    [SerializeField] bool isGrounded = false; //true if player is on ground (layer)
    [SerializeField] Transform groundCheckCollider; //empty object below player (child object to player)
    const float groundCheckRadius = 0.2f; //radius to check for ground layer
    [SerializeField] LayerMask groundLayer; //layers player can jump on

    private AudioSource movement; //audiosource for footstep and jump sounds
    [SerializeField] private AudioClip Footstep1;//not used in this script
    [SerializeField] private AudioClip Footstep2;
    [SerializeField] private AudioClip Jump;

    private void GroundCheck() //checks if player is on ground and is not moving on y axis (is not falling)
    {
        isGrounded = false;


        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0 && rb.velocity.y < 0.1f)
        {
            isGrounded = true;
        }
    }


    private void Start()
    {
        movement = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.localScale;
        moveSpeed = 2.7f;
    }
    
    private void Footstep11() //this function is called on specified frame in looped animation
    {
        movement.PlayOneShot(Footstep1);
    }
    private void Footstep22()
    {
        movement.PlayOneShot(Footstep2);
    }



    private void Update()
    {

        dirX = CrossPlatformInputManager.GetAxis("Horizontal") * moveSpeed; //crossplatforminput manager is used for player movement

        if (CrossPlatformInputManager.GetButtonDown("Jump") && isGrounded && CanMove) //check if can jump
        {
            rb.AddForce(Vector2.up * 550f);
            movement.PlayOneShot(Jump);
        }

        if (Mathf.Abs(dirX) > 0 && rb.velocity.y > -0.2 && rb.velocity.y < 0.2) //checks if player is running and plays animation
        { 
            anim.SetBool("isRunning", true);
        }

        else
            anim.SetBool("isRunning", false);


        if ( rb.velocity.y > -0.2 && rb.velocity.y < 0.2) //jumping and falling animations
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }

        if (rb.velocity.y > 0.2)
            anim.SetBool("isJumping", true);


        if (rb.velocity.y < -0.2)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
        }

    }
    private void FixedUpdate()
    {
        GroundCheck();
        if (CanMove)
        {
            rb.velocity = new Vector2(dirX, rb.velocity.y); //player movement (constant x velocity)
        }
        else
        {
            rb.velocity = new Vector2(0,0);
        }
        
    }


    private void LateUpdate() //used to rotate player properly
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
        {
            localScale.x *= -1;
            FirePoint.transform.Rotate(0f, 180f, 0f);
        }
        transform.localScale = localScale;
    }

}
