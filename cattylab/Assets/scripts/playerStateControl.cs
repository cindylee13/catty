using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStateControl : MonoBehaviour {

    private saveData overallData;


	// Use this for initialization
	void Start () {
		overallData = new saveData();
		Debug.Log("init");
		if(!overallData.loadfile()){
			overallData.set(gameData.init());
			overallData.saveFile();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
