using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public GameObject ball; //gameobject that flower shoots (in this case ball prefab that deals damage)
    public int shootforce; //strength applied to force the bullets (balls) are shot
    private  Animator anim; //animator
    public Transform shootpoint; //the point from which the balls are fired
    public GameObject[] balls; //array of multiple balls
    private float n = Mathf.PI / 6; //30 degree angle
    private bool canshoot = true; 
    private Vector2 delta_vector; //aditional vector applied on top of main vector so that balls fire with some randomness


    void Start()
    {
        anim = GetComponent<Animator>(); //animation is made with only 2 frames. Plant is sligtly shorter when IsShot is true.
        balls = new GameObject[5];
    }
    IEnumerator Routine (float WaitTime) //what happens when player enters trigger circle of flower
    {
        canshoot = false;
        anim.SetBool("IsShot", true);
        yield return new WaitForSeconds(WaitTime);
        anim.SetBool("IsShot", false);
        canshoot = true;
        Firing();
    }

    private void Firing() //launching balls in multiple directions
    {
        for (int i = 0; i <= 4; i++)
        {
            float delta = Random.Range(n, 5 * n);
            delta_vector = new Vector2(Mathf.Cos(delta), Mathf.Sin(delta)); //smaller delta vector is made
            balls[i] = Instantiate(ball, shootpoint.position, shootpoint.rotation); //"i" ball in array is instantiated at shootpoint
            Rigidbody2D rb = balls[i].GetComponent<Rigidbody2D>(); //temporary rigidbody component for ball "i"
            rb.AddForce(new Vector2(Mathf.Cos((i + 1) * n), Mathf.Sin((i + 1) * n)) * shootforce + delta_vector * 15); //main vectors are made with 30°, 60°, 90°, 120°, 150° angle. Intensity is adjusted with shootforce and delta vector is added on top of main vector for more natural look
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) //Player is tagged Player and Routine starts if it enters trigger collider
    {

        if (collision.gameObject.tag == "Player")
        {
            if (canshoot)
            {
                StartCoroutine(Routine(1));
            }
        }
    }
}
