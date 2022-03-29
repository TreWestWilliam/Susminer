using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftForwarder : MonoBehaviour
{
    public TestAI ai;
    public Collider2D lastCollided;
    public Player player;

    void OnTriggerExit2D(Collider2D col)
    {
        ai.leftTrigger = false;
        lastCollided = col;
        if (col.gameObject.tag == "Player") 
        {
            if (col.gameObject.GetComponent(typeof(Player)) != null) 
            {
                player = col.gameObject.GetComponent(typeof(Player)) as Player;
            }
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        ai.leftTrigger = true;
        lastCollided = col;

        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent(typeof(Player)) != null)
            {
                player = col.gameObject.GetComponent(typeof(Player)) as Player;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        ai.leftTrigger = true;
        lastCollided = col;

        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent(typeof(Player)) != null)
            {
                player = col.gameObject.GetComponent(typeof(Player)) as Player;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = new Player();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
