using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class testUIcontrol : MonoBehaviour {
	public playerStateControl overallStats;
	public Button moneeee, saveBtn, resetBtn, addCatBtn, addItemBtn, addRecipeBtn, resetRecipeBtn, submitRecipeBtn;
	public Text moneyText, EventText, ownedCatText, ownedItemText, recipeCostText, recipeText, recipeETCText;
	public Dropdown recipeDropdown;
	private List<Dropdown.OptionData> recipeOptions;
	public cattyLabDictionaty CLD;
	private List<int> catOccupied, itemOccupied;
	private List<Ientity> recipeOptionData;

	// Use this for initialization
	void Start () {
		recipeOptionData = new List<Ientity>();
		catOccupied = new List<int>();
		itemOccupied = new List<int>();
		recipeOptions = new List<Dropdown.OptionData>();
		moneeee.onClick.AddListener(MoneyBtnTask);
		saveBtn.onClick.AddListener(SaveBtnTask);
		resetBtn.onClick.AddListener(ResetBtnTask);
		addCatBtn.onClick.AddListener(AddCatBtnTask);
		addItemBtn.onClick.AddListener(AddItemBtnTask);
		overallStats.EventNotifier.AddListener(ChangeEventText);
		addRecipeBtn.onClick.AddListener(AddRecipeBtnTask);
		resetRecipeBtn.onClick.AddListener(resetRecipeBtnTask);
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
	}

	void resetRecipeBtnTask(){
		ResetRecipe();
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
			outputText += string.Format("\n{0}  all:{1} avaliable:{2}", CLD.GetCatName(cats[i].id), cats[i].count, cats[i].avaliable);
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

	void GetRecipeDropdownList(){
		recipeOptionData.Clear();
		recipeOptions.Clear();
		recipeDropdown.ClearOptions();
		recipeOptions.Add(new Dropdown.OptionData("Select"));
		foreach(cat c in overallStats.Ownedcats){
			int remains = c.avaliable - matchingEntityInList(catOccupied, c.id);
			if(remains > 0){
				Dropdown.OptionData r_option = new Dropdown.OptionData();
				r_option.text = CLD.GetCatName(c.id) + "  (" + remains + ") ";
				recipeOptions.Add(r_option);
				recipeOptionData.Add(CLD.GetCatData(c.id));
			}
		}
		foreach(item i in overallStats.OwnedItems){
			int remains = i.count - matchingEntityInList(catOccupied, i.id);
			if(remains > 0){
				Dropdown.OptionData r_option = new Dropdown.OptionData();
				r_option.text = CLD.GetItemName(i.id) + "  (" + (i.count - matchingEntityInList(catOccupied, i.id)) + ") ";
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

	public void ResetRecipe(){
		recipeCostText.text = "----";
		recipeETCText.text = "----";
		recipeText.text = "----";
		submitRecipeBtn.interactable = false;
		ResetOccupyList();
	}

	public void SubmitRecipe(){
		recipeData s_recipe = CLD.FindRecipeResultData(catOccupied.ToArray(), itemOccupied.ToArray());
		overallStats.SendMessage("SubmitRecipe", s_recipe.id);
	}

	private int matchingEntityInList(List<int> a, int id){
		int matching = 0;
		foreach(int data in a){
			if(data==id) matching++;
		}
		return matching;
	}

	private void craftingStarted(){
		addRecipeBtn.interactable = false;
		resetRecipeBtn.interactable = false;
		submitRecipeBtn.interactable = false;
		recipeCostText.text = "CRAFTING...";
	}

	
}
