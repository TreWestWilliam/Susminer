using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    Rigidbody2D PlayerBody;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpPower = 200f;

    [SerializeField] bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        transform.position += Move * Time.deltaTime * moveSpeed;

        if (Input.GetButton("Jump") && isGrounded == true) 
        {
            PlayerBody.AddForce(transform.up * jumpPower);
            isGrounded = false;
        }
        
    }

    void OnCollisionEnter2D(Collision2D other) {
        isGrounded = true;
    }
}
