using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    public Transform FocusedTransform;
    public float zPos = -15f;

    // Start is called before the first frame update
    void Start()
    {

        if (FocusedTransform == null) {
            FocusedTransform = GameObject.Find("Player").transform;
        } 
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FocusedTransform == null) { return; }

        transform.position = new Vector3(FocusedTransform.position.x, FocusedTransform.position.y, zPos);
    }
}