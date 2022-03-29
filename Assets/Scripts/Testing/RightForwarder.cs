using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightForwarder : MonoBehaviour
{

    public TestAI ai;
    public Collider2D lastCollided;

    void OnTriggerExit2D(Collider2D col)
    {
        ai.rightTrigger = false;
        lastCollided = col;
    }
    void OnTriggerStay2D(Collider2D col)
    {
        ai.rightTrigger = true;
        lastCollided = col;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        ai.rightTrigger = true;
        lastCollided = col;
    }


    // is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
