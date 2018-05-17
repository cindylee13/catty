using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OccupyListControl : MonoBehaviour {

	public GameObject _listItem;
	public GameObject _list;
	public float margin = 1000;
	public spriteFinder _spriteFinder;
	private List<GameObject> _listItemObjects = new List<GameObject>();
	private List<ListItemData> _listItemData = new List<ListItemData>();
	public UI_Control _MainControl;

	// Use this for initialization
	void Start () {
		//_listOriginalPosition = transform;
		if(_spriteFinder == null){
			_spriteFinder = GameObject.Find("AssetFinder").GetComponent<spriteFinder>();
		}
		
	}	
	// Update is called once per frame
	void Update () {
		
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
		float offset = 0;
		Debug.Log(_listItemData.Count);
		foreach(ListItemData lid in _listItemData){
			GameObject item;
			item = Instantiate(_listItem,_list.transform, false) as GameObject;
			//item.transform.localPosition = new Vector3(0, offset, 0);
			item.GetComponent<UI_OccupyItemControl>().EntitySprite = _spriteFinder.findSpriteByEntityID(lid.EntityID, lid.EntityType);
			item.GetComponent<UI_OccupyItemControl>().orderInList = lid.EntityID;
			item.GetComponent<UI_OccupyItemControl>().Type = lid.EntityType;
			item.GetComponent<UI_OccupyItemControl>().MainController = _MainControl;
			_listItemObjects.Add(item);
			offset -= margin;
			Count++;
		}
	}

	void DestroyAllItems(){
		foreach(GameObject go in _listItemObjects){
			go.GetComponent<UI_OccupyItemControl>().Die();
		}
		_listItemObjects.Clear();
	}
}