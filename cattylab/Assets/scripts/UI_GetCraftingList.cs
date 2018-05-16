using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GetCraftingList : MonoBehaviour {

	public GameObject _list;
	public Transform _listTo;

	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnSelected(){
		_list.transform.position = _listTo.position;
	}

	public void OnReleased(){

	}
}
