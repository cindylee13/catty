using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EncControl : MonoBehaviour {

	public UI_GeneralListControl _list;
	public UI_Control _overallControl;
	private List<ListItemData> _LIDList;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void refreshList(){
		if(_LIDList == null){
			_LIDList = new List<ListItemData>();
		}else{
			_LIDList.Clear();
		}
		List<catData> allCats = new List<catData>(_overallControl.CLD.GetAllCats());
		List<itemData> allItems = new List<itemData>(_overallControl.CLD.GetAllItems());
		foreach(catData cD in allCats){
			Debug.Log(cD.id);
			ListItemData lid = new ListItemData();
			lid.EntityID = cD.id;
			lid.EntityType = "cat";
			lid.EntityName = _overallControl.CLD.GetCatName(cD.id);
			lid.MiscData = cD.description;
			_LIDList.Add(lid);
		}
		foreach(itemData iD in allItems){
			ListItemData lid = new ListItemData();
			lid.EntityID = iD.id;
			lid.EntityType = "item";
			lid.EntityName = _overallControl.CLD.GetItemName(iD.id);
			lid.MiscData = iD.description;
			_LIDList.Add(lid);
		}
		_list.listItemData = _LIDList;
	}

}
