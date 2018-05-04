using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class testUIcontrol : MonoBehaviour {
	public playerStateControl overallStats;
	public Button moneeee;
	public Text moneyText, EventText, ownedCatText;
	public cattyLabDictionaty CLD;



	// Use this for initialization
	void Start () {
		moneeee.onClick.AddListener(MoneyBtnTask);
		overallStats.EventNotifier.AddListener(ChangeEventText);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(CLD.GetCatName(0));
	}

	void MoneyBtnTask(){
		int amount = Random.Range(-50,100);
		overallStats.SendMessage("changeMoney", amount);
	}

	void ChangeMoneyText(){
		moneyText.text = "$" + overallStats.money ;
	}
	void ChangeEventText(string text){
		EventText.text = text;
	}

	void ChangeOwnedCatText(){
		string outputText = "";
		cat[] cats = overallStats.Ownedcats.ToArray();
		Debug.Log(overallStats.money);
		outputText = string.Format("{0}  all:{1} avaliable:{2}", CLD.GetCatName(1), 2, 3);
		for(int i=1;i<cats.Length;i++){
			outputText += string.Format("<br>{0}  all:{1} avaliable:{2}", CLD.GetCatName(cats[i].id), cats[i].count, cats[i].avaliable);
		}
		ownedCatText.text = outputText;
	}



}
