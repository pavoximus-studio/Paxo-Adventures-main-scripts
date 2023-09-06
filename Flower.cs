using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public GameObject ball;
    public int shootforce;
    private  Animator anim;
    public Transform shootpoint;
    public GameObject[] balls;
    private float n = Mathf.PI / 6;
    public Transform player;
    private bool canshoot = true;
    private Vector2 delta_vector;


    void Start()
    {
        anim = GetComponent<Animator>();
        balls = new GameObject[5];
    }
    IEnumerator Routine (float WaitTime)
    {
        canshoot = false;
        anim.SetBool("IsShot", true);
        yield return new WaitForSeconds(WaitTime);
        anim.SetBool("IsShot", false);
        canshoot = true;
        Firing();
    }

    private void Firing()
    {
        for (int i = 0; i <= 4; i++)
        {
            float delta = Random.Range(n, 5 * n);
            delta_vector = new Vector2(Mathf.Cos(delta), Mathf.Sin(delta));
            balls[i] = Instantiate(ball, shootpoint.position, shootpoint.rotation);
            Rigidbody2D rb = balls[i].GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(Mathf.Cos((i + 1) * n), Mathf.Sin((i + 1) * n)) * shootforce + delta_vector * 15);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
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
