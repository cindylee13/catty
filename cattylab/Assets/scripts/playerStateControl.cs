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

	public UnityEvent OnMoneyChanged, OnGameInitialize, OnCatDataChaged;
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
		if(OnCatDataChaged == null) OnCatDataChaged = new UnityEvent();



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

	public void ResetGameData(){
		overallData.set(gameData.init);
		OnGameInitialize.Invoke();
		EventNotifier.Invoke("Data Reset");
	}

	public void SaveGameData(){
		overallData.saveFile();
		EventNotifier.Invoke("Data Saved");
	}

	public long money{
		get{
			return overallData.gameData.money;
		}
	}

	public int maxGroupCount{
		get{
			return overallData.gameData.maxGroupCount;
		}
	}

	public int maxGroupPplCount{
		get{
			return overallData.gameData.maxGroupPplCount;
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

	public bool CatControl(int id, int amount, CatControlType type){
		int current = 0;
		bool success = false;
		foreach(cat cat in overallData.gameData.ownedCats){
			if(cat.id == id){
				if(type == CatControlType.count){
					int tmp = cat.count;
					tmp += amount;
					if(tmp<0){
						Debug.LogError("Removed too much cats! must be exact 0 to remove cat type");
						success = false;
						break;
					}else if(tmp == 0){
						Debug.Log("Removing Cat Type");
						overallData.gameData.ownedCats.RemoveAt(current);
						current--;
						success = true;
						break;
					}
					cat.count += amount;
					cat.avaliable += amount;
					success = true;
					break;
				}else{
					int tmp = cat.avaliable;
					tmp += amount;
					if(tmp < 0){
						Debug.LogError("Not Enough Cats to occupy!(<0)");
						success = false;
						break;
					}else if(tmp > cat.count){
						Debug.LogError("Not Enough Cats to occupy!(>cat count)");
						success = false;
						break;
					}else{
						cat.avaliable = tmp;
						success = true;
						break;
					}
				}
			}
			current++;
		}
		if(!success){
			if(type == CatControlType.avaliable){
				Debug.LogError("Cat Not Found!");
				success = false;
			}else{
				Debug.Log("Cat not found, adding cat");

				for(int i = 0;i<overallData.gameData.ownedCats.Count;i++){
					if(overallData.gameData.ownedCats[i].id > id){
						Debug.Log("Inserting cat to " + i);
						overallData.gameData.ownedCats.Insert(i,new cat(id, amount, amount));
						success = true;
						break;
					}
				}
				if(!success){
					Debug.Log("Adding cat to the end of the list");
					overallData.gameData.ownedCats.Add(new cat(id, amount, amount));
					success = true;
				}

			}
		}
			if(success) OnCatDataChaged.Invoke();
			else Debug.LogError("U DON FKED UP");
			return success;
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

public enum CatControlType
{
	count,
	avaliable
}