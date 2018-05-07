using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class EventWithMessage : UnityEvent<string>{}
public class playerStateControl : MonoBehaviour {

    private saveData overallData;
	public cattyLabDictionaty CLD;

	//-----------
	//  EVENTS
	//-----------

	public UnityEvent OnMoneyChanged, OnGameInitialize, OnCatDataChaged, OnItemDataChanged, OnCraftingStarted, OnCraftingEnded, OnGroupDataChanged;
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
		if(OnItemDataChanged == null) OnItemDataChanged = new UnityEvent();
		if(OnCraftingStarted == null) OnCraftingStarted = new UnityEvent();
		if(OnCraftingEnded == null) OnCraftingEnded = new UnityEvent();
		if(OnGroupDataChanged == null) OnGroupDataChanged = new UnityEvent();
		OnGameInitialize.AddListener(OnGameInitializehandler);


		OnGameInitialize.Invoke();
	}

	void onApplicationQuit(){
		Debug.Log("aye");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//-----------
	// METHODS
	//-----------

	private void OnGameInitializehandler(){ // invoke every data event
		StopCoroutine("StartCraftingClock");
		OnCatDataChaged.Invoke();
		OnMoneyChanged.Invoke();
		OnItemDataChanged.Invoke();
		OnGroupDataChanged.Invoke();
		if(overallData.gameData.isCrafting){
			OnCraftingStarted.Invoke();
			StartCoroutine(StartCraftingClock());
		}
	}

	public void ResetGameData(){
		overallData.set(gameData.init);
		OnGameInitialize.Invoke();
		EventNotifier.Invoke("Data Reset");
	}

	public void SaveGameData(){
		overallData.saveFile();
		EventNotifier.Invoke("Data Saved");
	}


	public bool isCrafting{
		get{
			return overallData.gameData.isCrafting;
		}
	}

	public double craftETC{
		get{
			return overallData.gameData.craftETC;
		}
	}

	public long money{
		get{
			return overallData.gameData.money;
		}
	}

	public int groupCount{
		get{
			return overallData.gameData.exploreGroups.Count;
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

	public int unlockScore{
		get{
			return overallData.gameData.unlockScore;
		}
	}

	public List<cat> Ownedcats{
		get{
			return overallData.gameData.ownedCats;
		}
	}

	public List<item> OwnedItems{
		get{
			return overallData.gameData.ownedItems;
		}
	}


	public bool CatControl(int id, int amount, CatControlType type){
		int current = 0;
		bool success = false, found = false;
		foreach(cat cat in overallData.gameData.ownedCats){
			if(cat.id == id){
				found = true;
				if(type == CatControlType.count){
					int tmp = cat.count;
					tmp += amount;
					if(tmp < 0){
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
		if(!found && amount > 0){
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


	
	public bool ItemControl(int id, int amount){
		bool success = false, found = false;
		int current = 0;
		foreach(item nowItem in overallData.gameData.ownedItems){
			if(nowItem.id == id){
				found = true;
				if(nowItem.count + amount < 0){
					Debug.LogError("Must Be exact 0 to remove this item type!");
					success = false;
					break;
				}else if(nowItem.count + amount == 0){
					Debug.Log("Removing Item Type");
					overallData.gameData.ownedItems.RemoveAt(current);
					success = true;
					break;
				}else{
					nowItem.count = nowItem.count + amount;
					success = true;
					break;
				}
			}
		}
		if(!found && amount > 0){
				Debug.Log("Item not found, adding item");
				for(int i = 0;i<overallData.gameData.ownedItems.Count;i++){
					if(overallData.gameData.ownedItems[i].id > id){
						overallData.gameData.ownedItems.Insert(i, new item(id, amount));
						success = true;
						break;
					}
				}
				if(!success){
					overallData.gameData.ownedItems.Add(new item(id, amount));
					success = true;
				}
		}
		if(success) OnItemDataChanged.Invoke();
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

	public void SubmitRecipe(int recipe_id){
		if(overallData.gameData.isCrafting){
			Debug.LogError("ALREADY CRAFTING!");
			return;
		}
		Debug.Log("Recipe Received");
		recipeData rD = CLD.GetRecipeByID(recipe_id);
		int catToRemove = 0, itemToRemove = 0;
		changeMoney(rD.cost * -1);
		for(int i = 0; i<rD.cats.Count;i++){
			if(i==0 || rD.cats[i-1] == rD.cats[i]){
				catToRemove++;
			}else{
				CatControl(rD.cats[i-1], -catToRemove, CatControlType.count);
				catToRemove = 1;
			}

			if(i == rD.cats.Count - 1){
				CatControl(rD.cats[i], -catToRemove, CatControlType.count);
			}
		}
		Debug.Log("Cat Removed");
		for(int i = 0; i < rD.items.Count;i++){
			if(i==0 || rD.items[i-1] == rD.items[i]){
				itemToRemove++;
			}else{
				ItemControl(rD.items[i-1], -itemToRemove);
				itemToRemove = 1;
			}
			if(i == rD.items.Count - 1){
				ItemControl(rD.items[i], -itemToRemove);
			}
		}
		Debug.Log("Item Removed");
		overallData.gameData.isCrafting = true;
		overallData.gameData.craftID = recipe_id;
		overallData.gameData.craftETC = ConvertToUnixTimestamp(System.DateTime.Now) + CLD.GetRecipeTime(recipe_id);
		EventNotifier.Invoke("Crafting Start!");
		StartCoroutine(StartCraftingClock());
		OnCatDataChaged.Invoke();
		OnItemDataChanged.Invoke();
		OnCraftingStarted.Invoke();
	}

	void CraftingEnded(){
		overallData.gameData.isCrafting = false;
		Ientity entity = CLD.GetEntityByRecipeID(overallData.gameData.craftID);
		if(entity.GetType() == typeof(catData)){
			Debug.Log("Added Cat:" + ((catData)entity).name + "  ID:" + ((catData)entity).id);
			CatControl(((catData)entity).id,1,CatControlType.count);
			EventNotifier.Invoke("Crafted New Cat: " + ((catData)entity).name);
			OnCatDataChaged.Invoke();
		}else{
			ItemControl(((itemData)entity).id, 1);
			EventNotifier.Invoke("Crafted New Item: " + ((itemData)entity).name);
			OnItemDataChanged.Invoke();
		}
		OnCraftingEnded.Invoke();
	}


	private IEnumerator StartCraftingClock(){
		yield return new WaitForSecondsRealtime((int)(overallData.gameData.craftETC - ConvertToUnixTimestamp(System.DateTime.Now)));
		CraftingEnded();
	}

	double ConvertToUnixTimestamp(System.DateTime date){
		System.DateTime st = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
 	    System.TimeSpan diff = date - st;
	    return System.Math.Floor(diff.TotalSeconds);
	}
}

public enum CatControlType
{
	count,
	avaliable
}