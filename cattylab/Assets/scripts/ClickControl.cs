﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
			RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
			if(hit && hit.transform.gameObject.CompareTag("touchable")){
				try{
					hit.collider.SendMessageUpwards("OnSelected");
				}catch{
					Debug.Log("Untouchable");
				}
			}
		}
	}
}
