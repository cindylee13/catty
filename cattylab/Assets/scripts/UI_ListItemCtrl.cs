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
	private Image Esr;
	
	// Use this for initialization
	void Start () {
		EntityNameText = gameObject.transform.Find("BG/EntityTitle").GetComponent<Text>();
		MiscText = gameObject.transform.Find("BG/Misc").GetComponent<Text>();
		Esr = gameObject.transform.Find("BG/EntitySprite").GetComponent<Image>();
		EntityNameText.text = EntityName;
		MiscText.text = Misc;
		if(EntitySprite != null)Esr.sprite = EntitySprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
