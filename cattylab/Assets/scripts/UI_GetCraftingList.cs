using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GetCraftingList : MonoBehaviour {

	public GameObject _list;
	public Transform _listTo;
	public float speed = 10;
	private Transform _startMarker, _nowPosition;
	private float _startTime, _journeyLength;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnSelected(){
		if(_list.GetComponent<UI_ListCtrl>().IsMoving) return;
		else _list.GetComponent<UI_ListCtrl>().IsMoving = true;
		StartCoroutine(Move());
	}

	IEnumerator Move(){
		while(!V3Equal(_list.transform.position, _listTo.position)){
			_list.transform.position = Vector3.Lerp(_list.transform.position, _listTo.position, speed/100);
			yield return new WaitForSeconds(0.01f);
		}
		_list.transform.position = _listTo.position;
		_list.GetComponent<UI_ListCtrl>().IsMoving = false;
	}

	public void OnReleased(){

	}
	private bool V3Equal(Vector3 a, Vector3 b){
        return Vector3.SqrMagnitude(a - b) < 0.001;
    }
}
