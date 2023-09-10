using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Monster1 : MonoBehaviour //this is a script for a monster that walks on platform left and right. When it detects player in specified range, it stops and shoots. This monster appears in LVL2 in paxo adventures.
{
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] private GameObject m1Bullet_Prefab; //bullet prefab for this monster

    private bool movingRight = true;
    private bool finished_waiting = true; //did monster finish waiting on the edge of the platform
    private bool isShooting = false;

    [SerializeField] private float detectRange; //range for player detection
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundDectRange; //range to check for end of platform

    [SerializeField] Transform castPoint; //wall check and ground check point (different ways of detection for wall and ground)
    [SerializeField] Transform shootPoint;
    [SerializeField] Transform player; //player object
    float nextTimeToFire = 0;
    [SerializeField] private float FireRate;

    [SerializeField] bool isHittingWall = false;
    const float wallCheckRadius = 0.2f;
    [SerializeField] LayerMask detectLayer; //layer for walls (for example tilemap layer)

    private AudioSource src; //audiosource for sounds, attached to monster
    public AudioClip clp; //sound for firing the bullet

    void Start()
    {
        src = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void wallCheck()
    {
        isHittingWall = false;


        Collider2D[] colliders = Physics2D.OverlapCircleAll(castPoint.position, wallCheckRadius, detectLayer); //wall detection if object in specified layer has overlapted with circle around castPoint
        if (colliders.Length > 0)
        {
            isHittingWall = true;
        }
    }

    void Update()
    {

        RaycastHit2D groundInfo = Physics2D.Raycast(castPoint.position, Vector2.down, groundDectRange); //raycast ground check from castPoint

        if (groundInfo.collider == false) //when ground is no longer detected, wait and then rotate
        {
            StartCoroutine(coRoutine(3f));
            Rotation();

        }
        if (isHittingWall) //when wall is detected wait and rotate (even if edge of platform is not detected)
        {
            StartCoroutine(coRoutine(3f));
            Rotation();
            isHittingWall = false;

        }

        

        if(Mathf.Abs(player.position.x - transform.position.x) < detectRange && Mathf.Abs(player.position.y - transform.position.y) < 0.5) //checks if player is detected
        {
            if (player.position.x < transform.position.x) //rotate towards player
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else if(player.position.x >= transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }

            Shoot(); //keep shooting and rotating
            if (Time.time > nextTimeToFire) //firerate
            {
                nextTimeToFire = Time.time + 1 / FireRate;
                BulletSpawn();
            }
        }
        else
        {
            isShooting = false; //stop shooting
        }


        if (Mathf.Abs(rb.velocity.x) > 0.1) //animation
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    private void FixedUpdate() //monster movement
    {
        if (movingRight == true && finished_waiting == true  && isShooting == false)
        {
            rb.velocity = new Vector2(2f, 0f);
        }
        else if (movingRight == false && finished_waiting == true && isShooting == false)
        {
            rb.velocity = new Vector2(-2f, 0f);
        }
        wallCheck();

    }

    void Shoot() //monster stops moving when it is shooting
    {
        isShooting = true;
        rb.velocity = new Vector2(0, 0);
    }

    private void BulletSpawn()
    {
        Instantiate(m1Bullet_Prefab, shootPoint.position, shootPoint.rotation); //script is attached to m1Bullet_Prefab and it gives a speed to a bullet
        src.PlayOneShot(clp);
    }

    IEnumerator coRoutine(float waitTime) //wait at the edge of platform
    {
        finished_waiting = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(waitTime);
        finished_waiting = true;
    }


    private void Rotation() //monster rotation
    {
        if (movingRight == true)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            movingRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            movingRight = true;
        }
    }

}

