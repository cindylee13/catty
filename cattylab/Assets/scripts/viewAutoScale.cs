using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class viewAutoScale : MonoBehaviour {

    public float baseWidth = 411;
    public float baseHeight = 731;
    public float baseOrthographicSize = 5; //檢視空間的一半大小
    public Camera camera;

    // Use this for initialization
    void Start () {
        float newOrthographicSize = (float)Screen.height / (float)Screen.width * this.baseWidth / this.baseHeight * this.baseOrthographicSize;
        camera.orthographicSize = Mathf.Max(newOrthographicSize, this.baseOrthographicSize);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
