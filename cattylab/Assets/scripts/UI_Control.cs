using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour {
	
	public cattyLabDictionaty CLD;
	public playerStateControl overallData;
	public List<Text> _moneyText;
	public List<Text> _craftingTimeText;
	public List<Text> _exploreTimeText;
	public UI_ListCtrl _CraftingList;
	public UI_OccupyListControl _OccupyList;
	public MainUI_GroupCounterControl _GCC;
	public List<int> _catOccupied, _itemOccupied;
	private List<ListItemData> _CraftlidList;
	public Button _StartCraftingBtn;

	// Use this for initialization
	void Start () {
		_CraftlidList = new List<ListItemData>();
		_catOccupied = new List<int>();
		_itemOccupied = new List<int>();
		_StartCraftingBtn.interactable = false;
		_StartCraftingBtn.onClick.AddListener(SendRecipe);
		ResetCraftingOccupy();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SendRecipe(){
		recipeData s_recipe = CLD.FindRecipeResultData(_catOccupied.ToArray(), _itemOccupied.ToArray());
		overallData.SendMessage("SubmitRecipe", s_recipe.id);
		ResetCraftingOccupy();
		StartCoroutine(craftingCountDownClock());

	}

	void ChangeMoneyText(){
		long money = overallData.money;
		foreach(Text tx in _moneyText){
			tx.text = "$" + money;
		}
	}

	void RefreshCraftingList(){
		//create Item data list
		_CraftlidList.Clear();
		foreach(cat c in overallData.Ownedcats){
			ListItemData lid = new ListItemData();
			lid.EntityID = c.id;
			lid.EntityType = "cat";
			lid.MiscData = "X" + (c.count - MatchingEntityInList(_catOccupied, c.id));
			_CraftlidList.Add(lid);
		}
		foreach(item i in overallData.OwnedItems){
			ListItemData lid = new ListItemData();
			lid.EntityID = i.id;
			lid.EntityType = "item";
			lid.MiscData = "X" + (i.count - MatchingEntityInList(_itemOccupied, i.id));
			_CraftlidList.Add(lid);
		}
		_CraftingList.listItemData = _CraftlidList;
		CheckRecipe();
	}

	void RefreshOccupyList(){
		List<ListItemData> occupylidList = new List<ListItemData>();
		foreach(int cid in _catOccupied){
			ListItemData lid = new ListItemData();
			lid.EntityID = cid;
			lid.EntityType = "cat";
			occupylidList.Add(lid);
		} 
		foreach(int iid in _itemOccupied){
			ListItemData lid = new ListItemData();
			lid.EntityID = iid;
			lid.EntityType = "item";
			occupylidList.Add(lid);
		}
		_OccupyList.listItemData = occupylidList;
		CheckRecipe();
	}

	void ResetCraftingOccupy(){
		_CraftlidList.Clear();
		_catOccupied.Clear();
		_itemOccupied.Clear();
		RefreshCraftingList();
		RefreshOccupyList();
	}

	public void CraftingItemClicked(int orderInList){
		
		Debug.Log("Yee Haw "+orderInList);
		if(_catOccupied.Count + _itemOccupied.Count >= 8) return;
		if(_CraftlidList[orderInList].EntityType == "cat"){
			_catOccupied.Add(_CraftlidList[orderInList].EntityID);
		}else{
			_itemOccupied.Add(_CraftlidList[orderInList].EntityID);
		}
		RefreshCraftingList();
		RefreshOccupyList();
	}

	public void RemoveInOccupy(int idInOccupy, string type){
		if(type == "cat"){
			try{
				_catOccupied.RemoveAt(IndexInList(_catOccupied, idInOccupy));
			}catch{

			}
		}else{
			try{
				_itemOccupied.RemoveAt(IndexInList(_itemOccupied, idInOccupy));
			}catch{

			}
		}
		RefreshCraftingList();
		RefreshOccupyList();
	}

	private int IndexInList(List<int> a, int b){
		for(int i = 0; i< a.Count;i++){
			if(a[i] == b) return i;
		}
		return -1;
	}

	private void CheckRecipe(){
		_StartCraftingBtn.interactable = CLD.FindRecipeResultData(_catOccupied, _itemOccupied) != null;
	}

	private int MatchingEntityInList(List<int> a, int id){
		int matching = 0;
		foreach(int data in a){
			if(data==id) matching++;
		}
		return matching;
	}

	IEnumerator craftingCountDownClock(){
		while(overallData.isCrafting){
			foreach(Text t in _craftingTimeText){
				t.text = IntToTime((int)(overallData.craftETC - ConvertToUnixTimestamp(System.DateTime.Now)));
			}
			yield return new WaitForSeconds(1f);
		}
		foreach(Text t in _craftingTimeText){
				t.text = "---";
			}
	}
	double ConvertToUnixTimestamp(System.DateTime date){
		System.DateTime st = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
 	    System.TimeSpan diff = date - st;
	    return System.Math.Floor(diff.TotalSeconds);
	}

	string IntToTime(int seconds){
		string output = "";
		output = string.Format("{0}:{1}:{2}", saveDivide(seconds, 3600) ,saveDivide(seconds, 60) % 60,seconds % 60 );
		return output;
	}

	int saveDivide(int t, int o){
		int result = -1;
		if(o == 0){
			return 0;
		}
		while(t > o * (++result));
		return result;
	}
	void ChangeGroupCount(){
		
	}
}
