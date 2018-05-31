using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopGCControl : MonoBehaviour {

	// Use this for initialization
	public Text _priceText;
	private int _priceValue;
	public int _basePrice = 1500;
	public Button _actionBtn;
	public Color _inactiveTextColor;
	private Color _defaultColor;
	public UI_Control _mainController;
	void Start () {
		
		_defaultColor = _priceText.color;
		_actionBtn.onClick.AddListener(actionBtnClicked);
		
	}
	
	// Update is called once per frame
	void Update () {
		try{
		_priceValue = _basePrice * (int)Mathf.Pow(1.5f,_mainController.overallData.maxGroupCount);
		_priceText.text = "$" + _priceValue;
		_actionBtn.interactable = _priceValue <= _mainController.overallData.money;
		}catch{}
		if(!_actionBtn.interactable){
			_priceText.color = _inactiveTextColor;
		}else{
			_priceText.color = _defaultColor;
		}
	}
	void actionBtnClicked(){
		_mainController.overallData.changeMoney(-_priceValue);
		_mainController.overallData.maxGroupCount = _mainController.overallData.maxGroupCount + 1;
		_priceValue = _basePrice * (int)Mathf.Pow(1.5f,_mainController.overallData.maxGroupCount);
		_priceText.text = "$" + _priceValue;
	}
}
