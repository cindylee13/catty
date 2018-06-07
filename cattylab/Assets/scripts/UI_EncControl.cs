using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EncControl : MonoBehaviour
{

    public UI_GeneralListControl _list;
    public UI_Control _overallControl;
    private List<ListItemData> _LIDList;
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
        if (_LIDList == null)
        {
            _LIDList = new List<ListItemData>();
        }
        else
        {
            _LIDList.Clear();
        }
        List<catData> allCats = new List<catData>(_overallControl.CLD.GetAllCats());
        List<itemData> allItems = new List<itemData>(_overallControl.CLD.GetAllItems());
        foreach (catData cD in allCats)
        {
            try
            {
                //Debug.Log(cD.ent_id);
                ListItemData lid = new ListItemData();
                lid.EntityID = cD.ent_id;
                lid.EntityType = "cat";
                lid.EntityName = _overallControl.CLD.GetCatName(cD.ent_id) + GetRarityStars(cD.rarity);
                lid.MiscData = cD.description;
                _LIDList.Add(lid);
            }
            catch
            {

            }
        }
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
        _list.listItemData = _LIDList;
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
