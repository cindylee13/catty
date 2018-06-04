using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GeneralListItemControl : MonoBehaviour {

	public string EntityName;
	public string Misc;
	public Image EntityImage;
	public Sprite EntitySprite;
	public Button ActionButton;
	public Text EntityNameText;
	public Text MiscText;
	public string Type;
	public int orderInList;
	public bool ButtonEnabled = false;
	
	
	
	// Use this for initialization
	void Start () {
		if(EntitySprite != null)EntityImage.sprite = EntitySprite;
		if(ActionButton != null){
		ActionButton.interactable = ButtonEnabled;
		}
		if(EntityName != null){
			EntityNameText.text = EntityName;
		}
		if(Misc != null){
			MiscText.text = Misc;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
			
	}	

	void OnBecameVisible(){
		gameObject.SetActive(true);
	}

	void OnBecameInvisible(){
		gameObject.SetActive(false);
	}

	public void Die(){
		//Debug.Log("IMDED");
		Destroy(gameObject);
	}
}
