using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ListItemCtrl : MonoBehaviour{

	public string EntityName;
	public string Misc;
	public Sprite EntitySprite;
	public Button ActionButton;
	private Text EntityNameText;
	private Text MiscText;
	private Image Esr;
	
	
	// Use this for initialization
	void Start () {
		if(EntityNameText != null)
		EntityNameText = gameObject.transform.Find("BG/EntityTitle").GetComponent<Text>();
		if(MiscText != null)
		MiscText = gameObject.transform.Find("BG/Misc").GetComponent<Text>();
		Esr = gameObject.transform.Find("BG/EntitySprite").GetComponent<Image>();
		if(EntityNameText != null)
		EntityNameText.text = EntityName;
		if(MiscText != null)
		MiscText.text = Misc;
		if(EntitySprite != null)Esr.sprite = EntitySprite;
	}
	
	// Update is called once per frame
	void Update () {
			
	}	
}
