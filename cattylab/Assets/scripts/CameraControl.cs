using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public GameObject _camera;
    public double x;
    public double speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (x > 10 || x < -10)
        {
            speed = 2.5;
        }
        else if (x > 5 || x < -5)
        {
            speed = 0.8;
        }
        else if (x > 1.5 || x < -1.5)
        {
            speed = 0.4;
        }
        else
        {
            speed = 0.1;
        }
        if (x < 0)
        {
            _camera.transform.position += new Vector3((float)-speed, 0, 0);
            x += speed;
        }
        if (x > 0)
        {
            _camera.transform.position += new Vector3((float)speed, 0, 0);
            x -= speed;
        }
    }
    public void moveCameraToCatLab()
    {
        x = -10.9 - _camera.transform.position.x;
    }
    public void moveCameraToHome()
    {
        x = -0.1 - _camera.transform.position.x;
    }
    public void moveCameraToEN()
    {
        x = -21.8 - _camera.transform.position.x;
    }
    public void moveCameraToMap()
    {
        x = 10.7 - _camera.transform.position.x;
    }
    public void moveCameraToShop()
    {
        x = 21.6 - _camera.transform.position.x;
    }
}
