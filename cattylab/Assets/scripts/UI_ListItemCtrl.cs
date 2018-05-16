using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ListItemCtrl : MonoBehaviour {

	public string EntityName;
	public string Misc;
	public Sprite EntitySprite;
	private Text EntityNameText;
	private Text MiscText;
	private SpriteRenderer Esr;
	
	// Use this for initialization
	void Start () {
		EntityNameText = gameObject.transform.Find("Canvas/EntityName").GetComponent<Text>();
		MiscText = gameObject.transform.Find("Canvas/EntityStuff").GetComponent<Text>();
		Esr = gameObject.transform.Find("EntitySprite").GetComponent<SpriteRenderer>();
		EntityNameText.text = EntityName;
		MiscText.text = Misc;
		Esr.sprite = EntitySprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
