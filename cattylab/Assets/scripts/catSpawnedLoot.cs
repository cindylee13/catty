using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catSpawnedLoot : MonoBehaviour {
	Rigidbody2D _rb;
	Vector2 _origin;
	Animator _anim;
	// Use this for initialization
	void Start () {
		_anim = GetComponent<Animator>();
		_origin = gameObject.transform.position;
		_rb = GetComponent<Rigidbody2D>();
		Vector2 velo = new Vector2(Random.Range(-2f, 2f), Random.Range(5f, 7f));
		_rb.velocity = velo;
		StartCoroutine(DestroySelf());
	}
	
	// Update is called once per frame
	void Update () {
		if(gameObject.transform.position.y <= _origin.y){
			//Debug.Log("Bounce");
			//_rb.AddForce(new Vector2(0, -0.5f * (_rb.velocity.y)));
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, _origin.y, gameObject.transform.position.z);
			_rb.velocity = new Vector2(0.7f * _rb.velocity.x, -0.7f * (_rb.velocity.y));
		}
	}

	IEnumerator DestroySelf(){
		yield return new WaitForSeconds(2.5f);
		_anim.SetBool("dying", true);
		yield return new WaitForSeconds(3f);
		Destroy(gameObject);
	}
}
