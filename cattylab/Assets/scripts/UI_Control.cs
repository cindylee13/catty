using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour {
	
	public cattyLabDictionaty CLD;
	public playerStateControl overallData;
	public List<Text> _moneyText;
	public List<Text> _exploreTimeText;
	public UI_ListCtrl _CraftingList;
	public UI_OccupyListControl _OccupyList;
	public MainUI_GroupCounterControl _GCC;
	public List<int> _catOccupied, _itemOccupied;
	private List<ListItemData> _CraftlidList, _OccupylidList;

	// Use this for initialization
	void Start () {
		_CraftlidList = new List<ListItemData>();
		_catOccupied = new List<int>();
		_itemOccupied = new List<int>();
		ResetCraftingOccupy();
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
	}

	void ResetCraftingOccupy(){
		_CraftlidList.Clear();
		_catOccupied.Clear();
		_itemOccupied.Clear();
		RefreshCraftingList();
	}

	public void CraftingItemClicked(int orderInList){
		
		Debug.Log("Yee Haw "+orderInList);
		if(_catOccupied.Count + _itemOccupied.Count > 8) return;
		if(_CraftlidList[orderInList].EntityType == "cat"){
			_catOccupied.Add(_CraftlidList[orderInList].EntityID);
		}else{
			_itemOccupied.Add(_CraftlidList[orderInList].EntityID);
		}

		RefreshCraftingList();
	}

	private int MatchingEntityInList(List<int> a, int id){
		int matching = 0;
		foreach(int data in a){
			if(data==id) matching++;
		}
		return matching;
	}

	void ChangeGroupCount(){
		
	}
}
