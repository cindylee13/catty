using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour {
	
	public cattyLabDictionaty CLD;
	public playerStateControl overallData;
	public List<Text> _moneyText;
	public List<Text> _exploreTimeText;
	public MainUI_GroupCounterControl _GCC;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ChangeMoneyText(){
		long money = overallData.money;
		foreach(Text tx in _moneyText){
			tx.text = "$" + money;
		}
	}

	void ChangeGroupCount(){
		
	}
}
