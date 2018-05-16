using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ListItemCtrl : MonoBehaviour{

	public string EntityName;
	public string Misc;
	public Image EntityImage;
	public Sprite EntitySprite;
	public Button ActionButton;
	public Text EntityNameText;
	public Text MiscText;
	public int orderInList;
	public UI_Control MainController;
	
	
	
	// Use this for initialization
	void Start () {
		if(EntityNameText != null)
		EntityNameText.text = EntityName;
		if(MiscText != null)
		MiscText.text = Misc;
		if(EntitySprite != null)EntityImage.sprite = EntitySprite;
		ActionButton.onClick.AddListener(ClickedActionButton);
		if(Misc == "X0"){
			ActionButton.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
			
	}	

	void ClickedActionButton(){
		MainController.CraftingItemClicked(orderInList);
	}

	public void Die(){
		Debug.Log("IMDED");
		Destroy(gameObject);
	}
}
