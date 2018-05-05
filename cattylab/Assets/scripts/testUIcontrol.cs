using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class testUIcontrol : MonoBehaviour {
	public playerStateControl overallStats;
	public Button moneeee, saveBtn, resetBtn, addCatBtn;
	public Text moneyText, EventText, ownedCatText;
	public cattyLabDictionaty CLD;



	// Use this for initialization
	void Start () {
		moneeee.onClick.AddListener(MoneyBtnTask);
		saveBtn.onClick.AddListener(SaveBtnTask);
		resetBtn.onClick.AddListener(ResetBtnTask);
		addCatBtn.onClick.AddListener(AddCatBtnTask);
		overallStats.EventNotifier.AddListener(ChangeEventText);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//------------
	//Button tasks
	//------------

	void MoneyBtnTask(){
		int amount = Random.Range(-50,100);
		overallStats.SendMessage("changeMoney", amount);
	}

	void SaveBtnTask(){
		overallStats.SendMessage("SaveGameData");
	}

	void ResetBtnTask(){
		overallStats.SendMessage("ResetGameData");
	}

	void AddCatBtnTask(){
		int catid = Random.Range(0,3);
		Debug.Log("Adding cat id:" + catid);
		overallStats.CatControl(catid,1,CatControlType.count);
	}

	//------------
	//Change Text
	//------------

	void ChangeMoneyText(){
		moneyText.text = "$" + overallStats.money ;
	}
	void ChangeEventText(string text){
		EventText.text = text;
	}


	void ChangeOwnedCatText(){
		string outputText = "";
		cat[] cats = overallStats.Ownedcats.ToArray();
		outputText = string.Format("{0}  all:{1} avaliable:{2}", CLD.GetCatName(cats[0].id), cats[0].count, cats[0].avaliable);
		for(int i=1;i<cats.Length;i++){
			outputText += string.Format("\n {0}  all:{1} avaliable:{2}", CLD.GetCatName(cats[i].id), cats[i].count, cats[i].avaliable);
		}
		ownedCatText.text = outputText;
	}



}
