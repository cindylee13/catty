using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCamera : MonoBehaviour
{
    public float leftRestriction = -40f;
    public float rightRestriction = 10f;
    public float upRestriction = 30f;
    public float downRestriction = -10f;

    // Use this for initialization
    public int theScreenWidth;
    public int theScreenHeight;
    public int Boundary = -10;
    public int speed = 10;

    void Start()
    {
        theScreenWidth = Screen.width;
        theScreenHeight = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        // Right
        if (Input.mousePosition.x > theScreenWidth - Boundary && transform.position.x <= rightRestriction)
        {
            float posX = transform.position.x;
            posX += speed * Time.deltaTime;
            transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        }
        //Left
        if (Input.mousePosition.x < 0 + Boundary && transform.position.x >= leftRestriction)
        {
            float posX = transform.position.x;
            posX -= speed * Time.deltaTime;
            transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        }

        //Up
        if (Input.mousePosition.y > theScreenHeight - Boundary && transform.position.y <= upRestriction)
        {
            float posY = transform.position.y;
            posY += speed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        }

        //Down
        if (Input.mousePosition.y < 0 + Boundary && transform.position.y >= downRestriction)
        {
            float posY = transform.position.y;
            posY -= speed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        }
    }
}