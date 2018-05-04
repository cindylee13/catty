using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class EventWithMessage : UnityEvent<string>{}
public class playerStateControl : MonoBehaviour {

    private saveData overallData;
	public cattyLabDictionaty cattyLabDictionaty;

	//-----------
	//  EVENTS
	//-----------

	public UnityEvent OnMoneyChanged, OnGameInitialize;
	public EventWithMessage EventNotifier;


	// Use this for initialization
	void Start () {
		//load savefile on startup
		overallData = new saveData();
		Debug.Log("init");
		if(!overallData.loadfile()){
			overallData.set(gameData.init);
			overallData.saveFile();
		}
		//init events
		if(EventNotifier == null) EventNotifier = new EventWithMessage();
		if(OnMoneyChanged == null) OnMoneyChanged = new UnityEvent();
		if(OnGameInitialize == null) OnGameInitialize = new UnityEvent();





		OnGameInitialize.Invoke();
	}

	void onApplicationQuit(){
		Debug.Log("aye");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//-----------
	// FUNCTIONS
	//-----------

	public long money{
		get{
			return overallData.gameData.money;
		}
	}

	public List<cat> Ownedcats{
		get{
			List<cat> tmp = new List<cat>();

			foreach(cat c in overallData.gameData.ownedCats){
				tmp.Add(c);
			}

			return tmp;
		}
	}

	void changeMoney(int amount){
		overallData.gameData.money += amount;
		OnMoneyChanged.Invoke();
		string message;
		if(amount>=0){
			message = "$" + amount  + " of money gained!";
		}else{
			message = "$" + (amount*-1) + " of money deducted...";
		}
		EventNotifier.Invoke(message);
	}

	
	

	
}

