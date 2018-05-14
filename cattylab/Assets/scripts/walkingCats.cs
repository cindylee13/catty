using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkingCats : MonoBehaviour {

	public spriteFinder _spriteFinder;
	public int catID;
	private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();
		sr.sprite = _spriteFinder.findSpriteByEntityID(catID, "cat");	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
