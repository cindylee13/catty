using UnityEngine;
using System.Collections;
  
public class moveMap : MonoBehaviour
{
    public float moveSpeed = 10; // 設定移動速度    
    void Update()
    {
        // 按住左键的時動作   
        if (Input.GetMouseButton(0))
        {
            
            float h = Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;
            float v = Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime;
            // 設定移動時y軸不改變
           
            this.transform.Translate(h, 0, v, Space.World);
        }
    }
}