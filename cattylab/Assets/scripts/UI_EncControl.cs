using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EncControl : MonoBehaviour
{

    public UI_GeneralListControl listCat, listItem;
    public Text collected;
    public UI_Control _overallControl;
    private List<ListItemData> _LIDList = null;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void refreshList()
    {
        //refreshItemList();
        refreshCatList();
    }

    public void refreshItemList()
    {
        if (_LIDList == null)
        {
            _LIDList = new List<ListItemData>();
        }
        else
        {
            _LIDList.Clear();
        }
        List<itemData> allItems = new List<itemData>(_overallControl.CLD.GetAllItems());

        foreach (itemData iD in allItems)
        {
            try
            {
                ListItemData lid = new ListItemData();
                lid.EntityID = iD.ent_id;
                lid.EntityType = "item";
                lid.EntityName = _overallControl.CLD.GetItemName(iD.ent_id) + GetRarityStars(iD.rarity);
                lid.MiscData = "合成物品";
                _LIDList.Add(lid);
            }
            catch
            {

            }
        }
        listItem.listItemData = _LIDList;
    }

    public void refreshCatList()
    {//sometimes I really want myself to die, I'm not suicidal, I just want to test my limit before I break down.
        if (_LIDList == null)
        {
            CreateCatList();
        }
        else
        {
            List<cat> ownedCats = _overallControl.GetCats();
            int ownedCatCount = ownedCats.Count;
            int allCatsCount = _LIDList.Count;
            collected.text = ownedCatCount + "/" + allCatsCount;
            List<UI_GeneralListItemControl> itemsInList = listCat.ItemControllers;
            foreach (UI_GeneralListItemControl lid in itemsInList)
            {
                foreach (cat c in ownedCats)
                {
                    if (c.ent_id == lid.EntityID)
                    {
                        //Debug.Log("ENC REFRESH " + c.ent_id);
                        lid.count = c.count;
                        try{
                        lid.gameObject.GetComponent<UI_EncItemControl>().Check();
                        }catch{
                            Debug.LogError("How Lemon is a fucking idiot");
                        }
                    }
                }
            }
        }
        //gotta find a way to access created list item damn
    }

    void CreateCatList()
    {
        _LIDList = new List<ListItemData>();

        List<catData> allCats = _overallControl.CLD.GetAllCats();
        foreach (catData cD in allCats)
        {
            try
            {
                //Debug.Log(cD.ent_id);
                ListItemData lid = new ListItemData();
                lid.EntityID = cD.ent_id;
                lid.EntityType = "cat";
                //lid.EntityName = _overallControl.CLD.GetCatName(cD.ent_id) + GetRarityStars(cD.rarity);
                lid.MiscData = GetRarityStars(_overallControl.CLD.GetCatData(cD.ent_id).rarity);
                lid.count = 0;
                _LIDList.Add(lid);
            }
            catch
            {

            }
        }

        listCat.listItemData = _LIDList;

        List<cat> ownedCats = _overallControl.GetCats();
        int ownedCatCount = ownedCats.Count;
        int allCatsCount = _LIDList.Count;
        collected.text = ownedCatCount + "/" + allCatsCount;
        List<UI_GeneralListItemControl> itemsInList = listCat.ItemControllers;
        foreach (UI_GeneralListItemControl lid in itemsInList)
        {
            foreach (cat c in ownedCats)
            {
                if (c.ent_id == lid.EntityID)
                {
                    //Debug.Log("ENC REFRESH " + c.ent_id);
                    lid.count = c.count;
                    //lid.gameObject.GetComponent<UI_EncItemControl>().Check();
                }
            }
            lid.gameObject.GetComponent<UI_EncItemControl>().mainController = _overallControl;
        }

    }

    private string GetRarityStars(int rarity)
    {
        string output = "<color=#e5c110>";
        for (int i = 0; i < rarity; i++)
        {
            output += "★";
        }
        output += "</color>";
        return output;
    }


}
