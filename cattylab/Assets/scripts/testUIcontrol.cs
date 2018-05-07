using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class testUIcontrol : MonoBehaviour {
	public playerStateControl overallStats;
	public Button moneeee, saveBtn, resetBtn, addCatBtn, addItemBtn, addRecipeBtn, resetRecipeBtn, submitRecipeBtn, addCrewBtn;
	public Text moneyText, EventText, ownedCatText, ownedItemText, recipeCostText, recipeText, recipeETCText, crewReadyText, gcText, mgpcText;
	public Dropdown recipeDropdown, crewDropdown, levelDropdown;
	private List<Dropdown.OptionData> recipeOptions, crewOptions, levelOptions;
	public cattyLabDictionaty CLD;
	private List<int> catOccupied, itemOccupied, crewOptionData, crewReady, levelOptionData;
	private List<Ientity> recipeOptionData;

	// Use this for initialization
	void Start () {
		recipeOptionData = new List<Ientity>();
		crewOptionData = new List<int>();
		catOccupied = new List<int>();
		itemOccupied = new List<int>();
		levelOptionData = new List<int>();
		levelOptions = new List<Dropdown.OptionData>();
		recipeOptions = new List<Dropdown.OptionData>();
		crewOptions = new List<Dropdown.OptionData>();
		crewReady = new List<int>();
		moneeee.onClick.AddListener(MoneyBtnTask);
		saveBtn.onClick.AddListener(SaveBtnTask);
		resetBtn.onClick.AddListener(ResetBtnTask);
		addCatBtn.onClick.AddListener(AddCatBtnTask);
		addItemBtn.onClick.AddListener(AddItemBtnTask);
		overallStats.EventNotifier.AddListener(ChangeEventText);
		addRecipeBtn.onClick.AddListener(AddRecipeBtnTask);
		resetRecipeBtn.onClick.AddListener(resetRecipeBtnTask);
		submitRecipeBtn.onClick.AddListener(SubmitRecipeBtnTask);
		addCrewBtn.onClick.AddListener(AddCrewBtnTask);

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

	void AddItemBtnTask(){
		int itemId = Random.Range(0,3);
		Debug.Log("Adding Item id:" + itemId);
		overallStats.ItemControl(itemId, 1);
	}

	void AddCrewBtnTask(){
		InsertCatToReadyList(crewDropdown.value);
		crewReadyText.text = "";
		foreach(int cid in crewReady){
			crewReadyText.text += cid + " ";
		}
		mgpcText.text = string.Format("({0}/{1})", crewReady.Count, overallStats.maxGroupPplCount);
		addCrewBtn.interactable = overallStats.maxGroupPplCount > crewReady.Count;
	}

	void AddRecipeBtnTask(){
		InsertEntityToOccupyList(recipeDropdown.value);
		recipeData possibleRecipe = CLD.FindRecipeResultData(catOccupied.ToArray(), itemOccupied.ToArray());
		recipeText.text = "Cat:";
		foreach(int i in catOccupied){
			recipeText.text += i + " ";
		}
		recipeText.text += " Item:";
		foreach(int i in itemOccupied){
			recipeText.text += i + " ";
		}
		if(possibleRecipe != null){
			recipeCostText.text ="Cost: " + possibleRecipe.cost;
			recipeETCText.text = possibleRecipe.time + " Sec";
		}else{
			recipeCostText.text ="Not Found";
			recipeETCText.text = "Not Found";
		}
		submitRecipeBtn.interactable = possibleRecipe != null && possibleRecipe.cost < overallStats.money;
		GetRecipeDropdownList();
	}

	void resetRecipeBtnTask(){
		ResetRecipe();
	}

	void SubmitRecipeBtnTask(){
		SubmitRecipe();
	}

	//------------
	//Change Text
	//------------

	void ChangeGCText(){
		gcText.text = overallStats.groupCount +"/" + overallStats.maxGroupCount;
		mgpcText.text ="(" + overallStats.maxGroupPplCount + ")";
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
		outputText = string.Format("{0}: {1}  all:{2} avaliable:{3}",cats[0].id, CLD.GetCatName(cats[0].id), cats[0].count, cats[0].avaliable);
		for(int i=1;i<cats.Length;i++){
			outputText += string.Format("\n{0}: {1}  all:{2} avaliable:{3}",cats[i].id, CLD.GetCatName(cats[i].id), cats[i].count, cats[i].avaliable);
		}
		ownedCatText.text = outputText;
	}

	void ChangeOwnedItemText(){
		string outputText = "";
		item[] items = overallStats.OwnedItems.ToArray();
		for(int i=0;i<items.Length;i++){
			if(i>0) outputText +="\n";
			outputText += string.Format("{0}  Owned:{1}", CLD.GetItemName(items[i].id), items[i].count);
		}
		ownedItemText.text = outputText;
	}

	void GetLevelDropdownList(){
		levelOptionData.Clear();
		levelOptions.Clear();
		levelDropdown.ClearOptions();
		levelOptions.Add(new Dropdown.OptionData("Select"));
		for(int i = 0 ; i < CLD.GetTotalLevelCount(); i++){
			levels levels = CLD.GetLevelByID(i);
			if(levels.avaliable(overallStats.unlockScore)){
				Dropdown.OptionData l_option = new Dropdown.OptionData();
				l_option.text = levels.name;
				levelOptions.Add(l_option);
				levelOptionData.Add(levels.id);
			}
		}
		foreach(Dropdown.OptionData message in levelOptions){
			levelDropdown.options.Add(message);
		}
		levelDropdown.value = 0;
		levelDropdown.RefreshShownValue();
	}

	void GetRecipeDropdownList(){
		recipeOptionData.Clear();
		recipeOptions.Clear();
		recipeDropdown.ClearOptions();
		recipeOptions.Add(new Dropdown.OptionData("Select"));
		foreach(cat c in overallStats.Ownedcats){
			int remains = c.avaliable - MatchingEntityInList(catOccupied, c.id);
			if(remains > 0){
				Dropdown.OptionData r_option = new Dropdown.OptionData();
				r_option.text = CLD.GetCatName(c.id) + "  (" + remains + ") ";
				recipeOptions.Add(r_option);
				recipeOptionData.Add(CLD.GetCatData(c.id));
			}
		}
		foreach(item i in overallStats.OwnedItems){
			int remains = i.count - MatchingEntityInList(itemOccupied, i.id);
			if(remains > 0){
				Dropdown.OptionData r_option = new Dropdown.OptionData();
				r_option.text = CLD.GetItemName(i.id) + "  (" + (i.count - MatchingEntityInList(catOccupied, i.id)) + ") ";
				recipeOptions.Add(r_option);
				recipeOptionData.Add(CLD.GetItemData(i.id));
			}
		}	
		foreach(Dropdown.OptionData message in recipeOptions){
			recipeDropdown.options.Add(message);
		}
		recipeDropdown.value = 0;
		recipeDropdown.RefreshShownValue();
	}

	void GetCrewDropdownList(){
		crewOptions.Clear();
		crewDropdown.ClearOptions();
		crewOptions.Add(new Dropdown.OptionData("Select"));
		foreach(cat c in overallStats.Ownedcats){
			int remains = c.avaliable - MatchingEntityInList(crewReady, c.id);
			if(remains > 0){
				Dropdown.OptionData c_option = new Dropdown.OptionData();
				c_option.text = CLD.GetCatName(c.id) + "  (" + remains + ") ";
				crewOptions.Add(c_option);
				crewOptionData.Add(c.id);
			}
		}
		foreach(Dropdown.OptionData message in crewOptions){
			crewDropdown.options.Add(message);
		}
		crewDropdown.value = 0;
		crewDropdown.RefreshShownValue();

	}

	private void InsertCatToReadyList(int valueInOptions){
		if(valueInOptions==0) return;
		crewReady.Add(crewOptionData[valueInOptions - 1]);
		GetCrewDropdownList();
	}

	private void InsertEntityToOccupyList(int valueInOptions){
		if(valueInOptions == 0) return;
		if(recipeOptionData[valueInOptions-1].GetType() == typeof(catData)){
			Debug.Log("Cat");
			catOccupied.Add(recipeOptionData[valueInOptions-1].id);
		}else{
			Debug.Log("Item");
			itemOccupied.Add(recipeOptionData[valueInOptions-1].id);
		}
		GetRecipeDropdownList();// reset the dropdown list
	}

	public void ResetOccupyList(){
		catOccupied.Clear();
		itemOccupied.Clear();
		GetRecipeDropdownList();// reset the dropdown list
	}

	public void ResetReadyList(){
		crewReady.Clear();
		GetCrewDropdownList();
	}

	public void ResetRecipe(){
		recipeCostText.text = "----";
		recipeETCText.text = "----";
		recipeText.text = "----";
		submitRecipeBtn.interactable = false;
		ResetOccupyList();
	}

	public void SubmitRecipe(){
		recipeData s_recipe = CLD.FindRecipeResultData(catOccupied.ToArray(), itemOccupied.ToArray());
		ResetOccupyList();
		ResetReadyList();
		overallStats.SendMessage("SubmitRecipe", s_recipe.id);
	}

	private int MatchingEntityInList(List<int> a, int id){
		int matching = 0;
		foreach(int data in a){
			if(data==id) matching++;
		}
		return matching;
	}

	private void CraftingStarted(){
		addRecipeBtn.interactable = false;
		resetRecipeBtn.interactable = false;
		submitRecipeBtn.interactable = false;
		recipeCostText.text = "CRAFTING...";
		StartCoroutine(craftingCountDownClock());
	}

	private void CraftingEnded(){
		addRecipeBtn.interactable = true;
		resetRecipeBtn.interactable = true;
		ResetRecipe();
	}

	IEnumerator craftingCountDownClock(){
		while(overallStats.isCrafting){
			recipeETCText.text = (int)(overallStats.craftETC - ConvertToUnixTimestamp(System.DateTime.Now)) + "Secs";
			yield return new WaitForSeconds(1f);
		}
	}

	double ConvertToUnixTimestamp(System.DateTime date){
		System.DateTime st = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
 	    System.TimeSpan diff = date - st;
	    return System.Math.Floor(diff.TotalSeconds);
	}
}
