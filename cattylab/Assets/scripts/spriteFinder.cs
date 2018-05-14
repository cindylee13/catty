using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteFinder : MonoBehaviour {

	public Sprite _notFound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Sprite findSpriteByEntityID(int id, string type){
		Sprite foundSprite;
		string path = "";
		switch(type){
			case "cat":
			path = string.Format("Entities/c_{0}", id);
			break;
			case "item":
			path = string.Format("Entities/i_{0}", id);
			break;
		}
		
		foundSprite = Resources.Load<Sprite>(path);
		Debug.Log(foundSprite);
		if(foundSprite == null) return _notFound;
		else return foundSprite;
	}
}