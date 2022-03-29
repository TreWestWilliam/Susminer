using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundingForwarder : MonoBehaviour
{
    public TestAI ai;

    void OnTriggerExit2D(Collider2D col) 
    {
        if (col.gameObject.tag == "Block")
            ai.isGrounded = false;
    }
    void OnTriggerStay2D(Collider2D col) 
    {
        if (col.gameObject.tag == "Block")
            ai.isGrounded = true;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Block")
            ai.isGrounded = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
