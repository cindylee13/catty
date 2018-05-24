using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_MapControl : MonoBehaviour {
	//I've decided to assemble group here, fuckers 
	// Use this for initialization
	private List<int> _catOccupied;
	
	private int _selectedLevel;
	public GameObject _groupListObject, _mapObject;
	public UI_GeneralListControl _occupyList;
	public Button _retractBtn, _goBtn;
	public Text _cost, _levelName, _pplCount;
	private List<ListItemData> _catLIDList, _occupyLIDList;
	private UI_GeneralListControl _groupList;
	public float _listTransistionSpeed = 10;
	public UI_Control _overallControl;
	public Transform _originAnchor, _targetAnchor;
	void Start () {
		_occupyList.ItemClickedFunction = "OccupyItemClicked";
		_groupList = _groupListObject.GetComponent<UI_GeneralListControl>();
		_catOccupied = new List<int>();
		_retractBtn.onClick.AddListener(DeactivateCharactorSelection);
		_goBtn.onClick.AddListener(SendGroup);
		foreach(Transform tr in _mapObject.transform){
			Button btn  = tr.GetComponent<Button>();
			btn.onClick.AddListener(delegate{OnButtonPressedAction(tr.gameObject);});
		}
	}
	
	// Update is called once per frame
	void Update () {
		setText();//FUCC U
		_goBtn.interactable = CanSendGroup;
	}
	//-------------------------
	//----INTERFACE CONTROL----
	//-------------------------

	void ActiateCharactorSelection(){	
		RefreshCatList();
		StartCoroutine(Move(_targetAnchor));
	}

	void DeactivateCharactorSelection(){
		ResetOccupyList();
		StartCoroutine(Move(_originAnchor));
	}
	
	void ItemClicked(int orderInList){
		Debug.Log(_catLIDList[orderInList].EntityName);
		AddCatToOccupyList(_catLIDList[orderInList].EntityID);
		RefreshCatList();
	}

	void OccupyItemClicked(int orderInList){
		_catOccupied.RemoveAt(orderInList);
		RefreshCatList();
	}

	//--------------------
	//----DATA CONTROL----
	//--------------------

	void AddCatToOccupyList(int catID){
		_catOccupied.Add(catID);
	}

	void RemoveCatFromOccupyList(int IndexInList){
		_catOccupied.RemoveAt(IndexInList);
	}

	void SendGroup(){
		if(_selectedLevel != -1 && _catOccupied.Count > 0)
		_overallControl.SendGroup(_catOccupied, _selectedLevel);
		DeactivateCharactorSelection();
	}

	void ResetOccupyList(){
		_selectedLevel = -1;
		_catOccupied.Clear();
	}

	void SetLevel(int levelID){
		_selectedLevel = levelID;
		Debug.Log(_selectedLevel + " is pressed asdf");
	}


	void OnButtonPressedAction(GameObject pin){
		ActiateCharactorSelection();
		SetLevel(Int32.Parse(pin.name));
	}

	void RefreshCatList(){
		if(_catLIDList==null){
			_catLIDList = new List<ListItemData>();
		}else{
			_catLIDList.Clear();
		}
		List<cat> cats = _overallControl.GetCats();
		foreach(cat c in cats){
			ListItemData lid = new ListItemData();
			lid.EntityID = c.id;
			lid.EntityType = "cat";
			lid.EntityName = _overallControl.CLD.GetCatName(c.id);
			lid.MiscData = "X" + (c.count - MatchingEntityInList(_catOccupied, c.id)) + "  " + GetRarityStars(_overallControl.CLD.GetCatData(c.id).rarity);
			lid.Interable = (c.count - MatchingEntityInList(_catOccupied, c.id)) > 0 && CanAddCats;
			_catLIDList.Add(lid);
		}
		_groupList.listItemData = _catLIDList;
		RefreshOccupyList();
		setText();
	}

	void RefreshOccupyList(){
		if(_occupyLIDList==null){
			_occupyLIDList = new List<ListItemData>();
		}else{
			_occupyLIDList.Clear();
		}
		foreach(int cID in _catOccupied){
			ListItemData lid = new ListItemData();
			lid.EntityID = cID;
			lid.EntityType = "cat";
			lid.Interable = true;
			_occupyLIDList.Add(lid);
		}
		_occupyList.listItemData = _occupyLIDList;
	}

	void setText(){
		if(_selectedLevel == -1) return;
		levels lvlData = _overallControl.CLD.GetLevelByID(_selectedLevel);
		_levelName.text = lvlData.name;
		_cost.text ="$" + lvlData.cost;
		_pplCount.text = _catOccupied.Count + "/" + _overallControl.maxGroupPplCount;
	}

	bool CanAddCats{
		get{
			return _catOccupied.Count < _overallControl.maxGroupPplCount;
		}
	}

	bool CanSendGroup{
		get{
			return _catOccupied.Count > 0 && _overallControl.groupCount <= _overallControl.maxGroupCount;
		}
	}

	//------
	//FUNCTIONS
	//------

	private int MatchingEntityInList(List<int> a, int id){
		int matching = 0;
		foreach(int data in a){
			if(data==id) matching++;
		}
		return matching;
	}

	IEnumerator Move(Transform listTo){
		if(_groupList.GetComponent<UI_GeneralListControl>().IsMoving) yield break;
		_groupList.GetComponent<UI_GeneralListControl>().IsMoving = true;
		while(!V3Equal(_groupListObject.transform.position, listTo.position)){
			_groupListObject.transform.position = Vector3.Lerp(_groupListObject.transform.position, listTo.position, _listTransistionSpeed/100f);
			yield return new WaitForSeconds(0.01f);
		}
		_groupListObject.transform.position = listTo.position;
		_groupListObject.GetComponent<UI_GeneralListControl>().IsMoving = false;
	}

	private bool V3Equal(Vector3 a, Vector3 b){
        return Vector3.SqrMagnitude(a - b) < 0.001;
    }

	private string GetRarityStars(int rarity){
		string output = "";
		for(int i = 0;i<rarity;i++){
			output += "☆";
		}
		
		return output;
	}


}
