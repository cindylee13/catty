using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openBag : MonoBehaviour {

    //0 = bag closed
    //1 = bag opened
    public GameObject _craftingList;
    int _openOrClose = 2;
    public double y = 0;
    public double speed = 0.5;
    public playerStateControl overallStats;
    public GameObject cat;
    // Use this for initialization
    void Start () {
        overallStats.CatControl(1, 1, CatControlType.count);

    }

    // Update is called once per frame
    void Update () {
		if(_openOrClose == 1 && y != 0)
        {
            _craftingList.transform.position += new Vector3(0, (float)-speed, 0);
            y -= speed;
        }
        else if(_openOrClose == 0 && y != 0)
        {
            _craftingList.transform.position += new Vector3(0, (float)speed, 0);
            y += speed;
        }
    }
    void OnSelected()
    {
        cat = Instantiate(this.cat);
        cat.transform.parent = _craftingList.transform;
        cat.transform.localPosition =new Vector3(-4,-8,-5);
        if (_openOrClose == 0 || _openOrClose == 2)
        {
            y = _craftingList.transform.position.y - 10.5;
            _openOrClose = 1;
        }
        else
        {
            y =_craftingList.transform.position.y - 19;
            _openOrClose = 0;
        }
    }
    
}
