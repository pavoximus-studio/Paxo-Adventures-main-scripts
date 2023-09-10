using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outgrowth : MonoBehaviour
{
    //this is alien plant that explodes when player touches it

    public float impactfield;
    public float force;
    public LayerMask whattohit;
    public GameObject explosionprefab; //explosion with particles (prefab)
    [SerializeField] private int Damage;

    private GameObject M_player; //empty with audiosource
    AudioSource src;
    public AudioClip clp;
    private void Start()
    {
        M_player = GameObject.Find("Music_Player");
        src = M_player.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Explosion();
            Player_Health.player_hp -= Damage;
            src.PlayOneShot(clp);
        }
    }

    private void Explosion() //pushes objects away from it (objects in whattohit layer)
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, impactfield, whattohit);
        foreach(Collider2D obj in objects)
        {
            Vector2 dir = obj.transform.position - transform.position;
            obj.GetComponent<Rigidbody2D>().AddForce(dir * force);
        }
        Instantiate(explosionprefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected() //visually shows impactfield
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, impactfield);
    }
}
