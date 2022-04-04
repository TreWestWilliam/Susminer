using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour
{

    public Rigidbody2D myBody;
    public BoxCollider2D myCollider;
    public BoxCollider2D LeftCollider;
    public BoxCollider2D RightCollider;
    public CircleCollider2D alertCollider;
    public CircleCollider2D aggroCollider;
    public GameObject healthBarOne;
    public GameObject healthBarTwo;

    public BoxCollider2D playerCollider;

    public bool isGrounded;
    public bool leftTrigger;
    public bool rightTrigger;
    public bool alertTrigger;
    public bool aggroTrigger;
    public bool isAggro;

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

        if (thisEnemy.getDead()) 
        {
            state = 4;
        }

        healthBar();

        alertTrigger = alertCollider.IsTouching(playerCollider);
        aggroTrigger = aggroCollider.IsTouching(playerCollider);


        if ( (thisEnemy.alwaysAggro && alertTrigger) || thisEnemy.aggro) 
        {
            target = playerCollider.gameObject;
            state = 3;
        }



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

        stateManager();
        
    }

    void stateManager() 
    {
        switch (state) 
        {
            case 0: chooseDirection(); break;
            case 1: roamLeft();break;
            case 2:roamRight();break;
            case 3:chaseTarget();break;
        }
    }

    void chooseDirection() 
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
    void roamLeft() 
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

    void roamRight() 
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

    void chaseTarget() 
    {
        Transform tar = target.transform; // Set Target

        if (tar.position.x - 2 > transform.position.x) // If  must go right
        {
            if (myBody.velocity.x < speed)
            {
                myBody.AddForce(new Vector2(speed, 0)); // Go right
            }
        }
        else if (tar.position.x + 2 < transform.position.x) // Else
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

        if (!aggroTrigger) 
        {
            state = 0;
            thisEnemy.aggro = false;
        }
    }

    void healthBar() 
    {
        if (thisEnemy.showHealth)
        {
            healthBarOne.SetActive(true);
            healthBarTwo.SetActive(true);

            healthBarTwo.transform.localScale = new Vector3( ( thisEnemy.getHP() / thisEnemy.getMaxHP() ) *0.95f , .8f, 1);
        }
        else 
        {
            healthBarOne.SetActive(false);
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
