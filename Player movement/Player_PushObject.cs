using UnityEngine;
using UnityEngine.Events;

public class Player_PushObject : MonoBehaviour
{

    //this script plays push animation when player is pushing some gameobject and is moving

    Animator Main_Animator; //Animator component
    public GameObject Player; //player gameobject
    public Rigidbody2D rb; //Player rigidbody


    [SerializeField] bool isPushing = false;
    [SerializeField] Transform pushCheckCollider; //empty object (child object of player) placed in front of player
    const float pushCheckRadius = 0.1f; //radius to check for objects to push
    [SerializeField] LayerMask pushLayer; //layer of objects player can push

    private void PushCheck() //checks if there are objects to push in circle around empty
    {
        isPushing = false;


        Collider2D[] colliders = Physics2D.OverlapCircleAll(pushCheckCollider.position, pushCheckRadius, pushLayer);
        if (colliders.Length > 0) //if there is at least 1 object to push, isPushing = true
        {
            isPushing = true;
        }
    }
    void Start()
    {
        Main_Animator = Player.GetComponent<Animator>();
    }

    public void Update()
    {
        if (isPushing && Mathf.Abs(rb.velocity.x) > 0.1) //checks if isPushing and if player is moving
        {
            Main_Animator.SetBool("isPushing", true); //looped push animation
        }
        else
        {
            Main_Animator.SetBool("isPushing", false);
        }
    }
    public void FixedUpdate()
    {
        PushCheck();
    }
}


