using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ListCtrl : MonoBehaviour {
	public GameObject _listItem;
	public GameObject _list;
	public float margin = 1000;
	public spriteFinder _spriteFinder;
	private List<GameObject> _listItemObjects = new List<GameObject>();
	public List<ListItemData> _listItemData = new List<ListItemData>();
	private Transform _listOriginalPosition;

	// Use this for initialization
	void Start () {
		//_listOriginalPosition = transform;
		if(_spriteFinder == null){
			_spriteFinder = GameObject.Find("AssetFinder").GetComponent<spriteFinder>();
		}
		float offset = 0;
		for(int i = 0; i < 10;i++){
			GameObject item;
			item = Instantiate(_listItem,_list.transform, false) as GameObject;
			//item.transform.localPosition = new Vector3(0, offset, 0);
			item.GetComponent<UI_ListItemCtrl>().EntityName = string.Format("{0} asdfasdad", i);
			item.GetComponent<UI_ListItemCtrl>().EntitySprite = _spriteFinder.findSpriteByEntityID(i, "item");
			_listItemObjects.Add(item);
			offset -= margin;
		}
	}	
	// Update is called once per frame
	void Update () {
		//DestroyAllItems();
	}

	void ShowItems(){
		DestroyAllItems();
		float offset = 0;
		foreach(ListItemData lid in _listItemData){
			GameObject item;
			item = Instantiate(_listItem,_list.transform, false) as GameObject;
			//item.transform.localPosition = new Vector3(0, offset, 0);
			item.GetComponent<UI_ListItemCtrl>().EntityName = lid.EntityName;
			item.GetComponent<UI_ListItemCtrl>().Misc = lid.MiscData;
			item.GetComponent<UI_ListItemCtrl>().EntitySprite = _spriteFinder.findSpriteByEntityID(lid.EntityID, lid.EntityType);
			_listItemObjects.Add(item);
			offset -= margin;
		}
	}

	void DestroyAllItems(){
		foreach(GameObject go in _listItemObjects){
			Destroy(go.gameObject);
		}
		_listItemObjects.Clear();
	}

	public Transform ListOrigin{
		get{
			return _listOriginalPosition;
		}
	}


}

public class ListItemData{
	public string EntityName;
	public string MiscData;
	public string EntityType;
	public int EntityID;

}
