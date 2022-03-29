using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour
{

    public Rigidbody2D myBody;
    public BoxCollider2D myCollider;
    public BoxCollider2D LeftCollider;
    public BoxCollider2D RightCollider;

    public bool isGrounded;
    public bool leftTrigger;
    public bool rightTrigger;

    public LeftForwarder left;
    public RightForwarder right;

    int state = 3;
    float roamTimer = 0;
    public float speed = 1.5f;
    public float jumpPower = 25;
    public GameObject target;

    Enemy thisEnemy;

    public float testhp;

    public Player ply;

    /// <summary>
    ///state 0 = choosing where to go
    ///state 1 = moving left
    ///state 2 = moving right
    ///state 3 = chasing target
    /// </summary>


    // Start is called before the first frame update
    void Start()
    {
        thisEnemy = gameObject.GetComponent(typeof(Enemy)) as Enemy;
        
    }

    // Update is called once per frame
    void Update()
    {
        testhp = thisEnemy.getHP();
        if ((leftTrigger || rightTrigger)) 
        {
            if (leftTrigger)
            {
                if (left.lastCollided.gameObject.GetComponent(typeof(Player)) != null) 
                {
                    ply = left.lastCollided.gameObject.GetComponent(typeof(Player)) as Player;
                    ply.Damage(thisEnemy.enemyDamage);
                }
            }
            if (rightTrigger) 
            {
                if (right.lastCollided.gameObject.GetComponent(typeof(Player)) != null) 
                {
                    ply = right.lastCollided.gameObject.GetComponent(typeof(Player)) as Player;
                    ply.Damage(thisEnemy.enemyDamage);
                }
            }
            if ( ( (right.lastCollided.gameObject.tag == "Block") || (left.lastCollided.gameObject.tag == "Block") ) && isGrounded) 
            {
                jump();
            }
             
        }

        if (state == 0)
        {
            int coinflip = (int)(Random.value * 2);

            if (coinflip == 1)
            {
                state = 1;

            }
            else
            {
                state = 2;

            }
        }
        else if (state == 1)
        {
            if (myBody.velocity.x > (-1 * speed))
                myBody.AddForce(new Vector2((-speed), 0));
            roamTimer += Time.deltaTime;
            if (roamTimer > 3)
            {
                int coinflip = (int)(Random.value * 2);
                if (coinflip == 1)
                {
                    roamTimer = 0;
                }
                else
                {
                    roamTimer = 0;
                    state = 0;
                }
            }
        }
        else if (state == 2)
        {
            if (myBody.velocity.x < speed)
            {
                myBody.AddForce(new Vector2(speed, 0));
            }
            roamTimer += Time.deltaTime;
            if (roamTimer > 3)
            {
                int coinflip = (int)(Random.value * 2);
                if (coinflip == 1)
                {
                    roamTimer = 0;
                }
                else
                {
                    roamTimer = 0;
                    state = 0;
                }
            }
        }
        else if (state == 3)
        {
            Transform tar = target.transform; // Set Target

            if (tar.position.x - 2.5 > transform.position.x) // If  must go right
            {
                if (myBody.velocity.x < speed)
                {
                    myBody.AddForce(new Vector2(speed, 0)); // Go right
                }
            }
            else if (tar.position.x + 2.5 < transform.position.x) // Else
            {
                if (myBody.velocity.x > -speed)
                {
                    myBody.AddForce(new Vector2(-speed, 0)); // go left
                }
            }
            else
            {
                myBody.AddForce(new Vector2(-myBody.velocity.x, -myBody.velocity.y));
            }
        }
        else if (state == 4) 
        {

        }
    }

    void jump() 
    {
        myBody.AddForce(transform.up * jumpPower);
    }
    /*
    void OnCollisionEnter(Collision col) 
    {
        jump();
        Debug.Log("test");
    }
    */
    /*
    void OnTriggerEnter2D(Collider2D col) 
    { 
    
    }
    */
}
