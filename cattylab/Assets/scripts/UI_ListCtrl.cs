using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ListCtrl : MonoBehaviour {
	public GameObject _listItem;
	public GameObject _list;
	public float margin = 10;
	public spriteFinder _spriteFinder;
	private List<GameObject> _listItemObjects;

	// Use this for initialization
	void Start () {
		if(_spriteFinder == null){
			_spriteFinder = GameObject.Find("AssetFinder").GetComponent<spriteFinder>();
		}
		float offset = 0;
		for(int i = 0; i < 10;i++){
			GameObject item;
			item = Instantiate(_listItem,_list.transform, false) as GameObject;
			item.transform.localPosition = new Vector3(0, offset, 0);
			item.GetComponent<UI_ListItemCtrl>().EntityName = string.Format("{0} asdfasdad", i);
			item.GetComponent<UI_ListItemCtrl>().EntitySprite = _spriteFinder.findSpriteByEntityID(i, "item");
			offset -= margin;
		}
	}	
	// Update is called once per frame
	void Update () {
		
	}


}
