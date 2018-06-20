using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EncItemControl : MonoBehaviour
{

    private UI_GeneralListItemControl itemController;
    private bool isKnown = false;
    public UI_Control mainController;
    public GameObject ItemImage, notFoundImage;
    public Button Activate;
    // Use this for initialization
    void Start()
    {
        itemController = GetComponent<UI_GeneralListItemControl>();
		Activate.onClick.AddListener(seeDetail);
        Check();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Check()
    {
        Debug.Log("checking " + itemController);
        if (itemController.count <= 0)
        {
            ItemImage.SetActive(false);
            notFoundImage.SetActive(true);
            isKnown = false;
        }
        else
        {
            ItemImage.SetActive(true);
            notFoundImage.SetActive(false);
            isKnown = true;
        }
    }

    public void seeDetail()
    {
        RichMessage rm = new RichMessage();
        if (isKnown)
        {
            rm.Image = itemController.EntitySprite;
            rm.title = mainController.CLD.GetCatData(itemController.EntityID).name;
            rm.rarity = itemController.Misc;
            rm.misc = mainController.CLD.GetCatData(itemController.EntityID).description;
        }
        else
        {
			rm.Image = notFoundImage.GetComponent<Image>().sprite;
			rm.title = mainController.CLD.GetCatData(itemController.EntityID).name;
            rm.rarity = itemController.Misc;
            rm.misc = "...?";
		}
		//Debug.Log(mainController);
		mainController.RichEventMessage(rm);
    }

}
