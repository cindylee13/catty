using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIAutoScale : MonoBehaviour {
    public float baseWidth = 411;
    public float baseHeight = 731;

    private float baseAspect;   //開發時定義的基礎解析度長寬比
    private float targetAspect; //實際顯示畫面的長寬比
    private float m03; //實際畫面上，GUI的x軸起始點為從左邊算來第幾個像素(pixel)
    private float m13; //實際畫面上，GUI的y軸起始點為從上方算來第幾個像素(pixel)
    private float m33; //x軸及 y軸等比縮放比例的反比，也就是說，數值變大為等比縮小，數值變小則等比
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
