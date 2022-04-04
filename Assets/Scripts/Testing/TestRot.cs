using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRot : MonoBehaviour
{
    public Camera gameCamera;
    public Vector3 mousePos;

    Quaternion m_MyQuaternion;
    //float m_Speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_MyQuaternion = new Quaternion();
    }

    // Update is called once per frame
    void Update()
    {

        //Fetch the mouse's position
        mousePos = Input.mousePosition;
        //Fix how far into the Scene the mouse should be
        mousePos.z = 50.0f;
        //Transform the mouse position into world space
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        //Set the Quaternion rotation from the GameObject's position to the mouse position
        m_MyQuaternion.SetFromToRotation(transform.position, mousePos);
        //Move the GameObject towards the mouse position
        //transform.position = Vector3.Lerp(transform.position, mousePos, m_Speed * Time.deltaTime);
        //Rotate the GameObject towards the mouse position
        transform.rotation = new Quaternion (0, 0, (m_MyQuaternion * transform.rotation)[2], 1);



    }
}
