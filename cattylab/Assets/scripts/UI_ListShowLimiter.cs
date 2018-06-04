using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ListShowLimiter : MonoBehaviour
{

    public GameObject target;
    private Transform targetPosition;
    public float topPosition, bottomPosition;
    // Use this for initialization
    void Start()
    {
        targetPosition = transform;
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 tmp  = Camera.main.WorldToViewportPoint(targetPosition.position);
        if (tmp.y > topPosition || tmp.y < bottomPosition)
        {
            if (target.activeSelf)
                target.SetActive(false);
        }
        else
        {
            if (!target.activeSelf)
                target.SetActive(true);
        }
    }
}
