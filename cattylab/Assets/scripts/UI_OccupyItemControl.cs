using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OccupyItemControl : MonoBehaviour {

	public string EntityName;
	public string Misc;
	public Image EntityImage;
	public Sprite EntitySprite;
	public Button ActionButton;
	public Text EntityNameText;
	public Text MiscText;
	public string Type;
	public int orderInList;
	public UI_Control MainController;
	
	
	
	// Use this for initialization
	void Start () {
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
		MainController.RemoveInOccupy(orderInList, Type);
		Die();
	}

	public void Die(){
		Debug.Log("IMDED");
		Destroy(gameObject);
	}
}
