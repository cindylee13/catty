using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkingCats : MonoBehaviour {

	public float _walkingInterval = 0.5f;
	public spriteFinder _spriteFinder;
	public int _catID;
	public GameObject _loot;
	private SpriteRenderer _sr;
	private Animator _anim;
	private Rigidbody2D _rb;
	private bool isWalking = false;

	// Use this for initialization
	void Start () {
		if(_spriteFinder == null){
			Debug.Log("Finder not found, finding finder");
			GameObject.Find("AssetFinder").GetComponent<spriteFinder>();
		}
		_anim = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
		_sr = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
		_rb = GetComponent<Rigidbody2D>();
		_sr.sprite = _spriteFinder.findSpriteByEntityID(_catID, "cat");	
		if(Random.Range(0,3) > 1){
			_sr.flipX = true;
		}
		StartCoroutine(RTD_ifWalking());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator RTD_ifWalking(){
		while(true){
			yield return new WaitForSeconds(_walkingInterval);
			if(Random.Range(0,100) <= 20){
				StartCoroutine(startWalking());
			}
		}
	}

	IEnumerator startWalking(){
		if(isWalking) yield break;
		isWalking = true;
		int sec = Random.Range(1,3);
		_anim.SetBool("isMoving", true);
		Vector2 velo = new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f));
		if(velo.x < 0){
			_sr.flipX = false;
		}else{
			_sr.flipX = true;
		}
		_rb.velocity = velo;
		//reset after time up
		yield return new WaitForSeconds(sec);
		_anim.SetBool("isMoving", false);
		_rb.velocity = Vector2.zero;
		isWalking = false;
	}

	void OnSelected(){
		Debug.Log("Meow");
		_anim.Play("cr_cat_touched");
		if(Random.Range(0,10) < 3)
		Instantiate(_loot, this.gameObject.transform.GetChild(1).transform.position, new Quaternion());
	}
}
