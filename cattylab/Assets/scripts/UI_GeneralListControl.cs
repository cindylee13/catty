using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class UI_GeneralListControl : MonoBehaviour {
	public GameObject _listItem;
	public string ItemClickedFunction = "ItemClicked";
	public GameObject _list;
	public float margin = 1000;
	public spriteFinder _spriteFinder;
	private List<GameObject> _listItemObjects = new List<GameObject>();
	private List<ListItemData> _listItemData = new List<ListItemData>();
	private Transform _listOriginalPosition;
	private bool _isMoving = false;

	public GameObject _Controller;

	// Use this for initialization
	void Start () {
		//_listOriginalPosition = transform;
		if(_spriteFinder == null){
			_spriteFinder = GameObject.Find("AssetFinder").GetComponent<spriteFinder>();
		}
		
	}	
	// Update is called once per frame
	void Update () {
		//DestroyAllItems();
		for(int i = 0;i<_listItemData.Count;i++){
			UI_GeneralListItemControl nowItem = _listItemObjects[i].GetComponent<UI_GeneralListItemControl>();
			nowItem.EntityName = _listItemData[i].EntityName;
			nowItem.Misc = _listItemData[i].MiscData;
		}
		
	}

	public List<ListItemData> listItemData{
		get{
			return _listItemData;
		}
		set{
			_listItemData.Clear();
			_listItemData = new List<ListItemData>(value);
			RefreshItems();
		}
	}

	public void RefreshItems(){
		DestroyAllItems();
		int Count = 0;
		Debug.Log(_listItemData.Count);
		foreach(ListItemData lid in _listItemData){
			GameObject item;
			item = Instantiate(_listItem,_list.transform, false) as GameObject;
			//item.transform.localPosition = new Vector3(0, offset, 0);
			item.GetComponent<UI_GeneralListItemControl>().EntityName = lid.EntityName;
			item.GetComponent<UI_GeneralListItemControl>().Misc = lid.MiscData;
			item.GetComponent<UI_GeneralListItemControl>().EntitySprite = _spriteFinder.findSpriteByEntityID(lid.EntityID, lid.EntityType);
			item.GetComponent<UI_GeneralListItemControl>().orderInList = Count;
			item.GetComponent<UI_GeneralListItemControl>().ButtonEnabled = lid.Interable;
			if(item.GetComponent<UI_GeneralListItemControl>().ActionButton != null)
			item.GetComponent<UI_GeneralListItemControl>().ActionButton.onClick.AddListener(delegate{
				ListItemClickedAction(item.GetComponent<UI_GeneralListItemControl>().orderInList);
			});
			_listItemObjects.Add(item);
			Count++;
		}
	}

	void DestroyAllItems(){
		foreach(GameObject go in _listItemObjects){
			go.GetComponent<UI_GeneralListItemControl>().Die();
		}
		_listItemObjects.Clear();
	}

	public Transform ListOrigin{
		get{
			return _listOriginalPosition;
		}
	}

	public bool IsMoving{
		get{
			return _isMoving;
		}
		set{
			_isMoving = value;
		}
	}

	private void ListItemClickedAction(int orederInList){
		_Controller.SendMessage(ItemClickedFunction, orederInList);
	}


}
