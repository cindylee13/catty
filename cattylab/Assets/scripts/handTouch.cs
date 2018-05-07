using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class handTouch : MonoBehaviour {
    public Vector2 h_sceenpos_x = new Vector2(); //紀錄手指觸碰位置的xy座標
    public Vector2 h_sceenpos_y = new Vector2();
    public Vector2 tapCount = new Vector2();  //短時間內的點擊次數(可能用的到)
    public Text t_phase; //目前觸控點的狀態

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.touchCount > 0) //觸控點大於0 = 有觸控
        {
            for(int i = 0; i < Input.touchCount; i++)
            {
                h_sceenpos_x[i] = Input.GetTouch(i).position.x;
                h_sceenpos_y[i] = Input.GetTouch(i).position.y;
                tapCount[i] = Input.GetTouch(i).tapCount;
                //t_phase = Input.GetTouch(i).phase.ToString();
            }
            
        }

    }
}
