using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class UI_GeneralListControl : MonoBehaviour
{
    public GameObject _listItem;
    public string ItemClickedFunction = "ItemClicked";
    public GameObject _list;
    public float margin = 1000;
    public spriteFinder _spriteFinder;
    private List<UI_GeneralListItemControl> _listItemObjects = new List<UI_GeneralListItemControl>();
    private List<ListItemData> _listItemData = new List<ListItemData>();
    private Transform _listOriginalPosition;
    private bool _isMoving = false;

    public GameObject _Controller;

    // Use this for initialization
    void Start()
    {
        //_listOriginalPosition = transform;
        if (_spriteFinder == null)
        {
            _spriteFinder = GameObject.Find("AssetFinder").GetComponent<spriteFinder>();
        }

    }
    // Update is called once per frame
    void Update()
    {
        //DestroyAllItems();
        for (int i = 0; i < _listItemData.Count; i++)
        {
            UI_GeneralListItemControl nowItem = _listItemObjects[i];
            nowItem.EntityName = _listItemData[i].EntityName;
            nowItem.Misc = _listItemData[i].MiscData;
        }

    }

    public List<ListItemData> listItemData
    {
        get
        {
            return _listItemData;
        }
        set
        {
            _listItemData.Clear();
            _listItemData = new List<ListItemData>(value);
            RefreshItems();
        }
    }

    public void RefreshItems()
    {
        DestroyAllItems();
        int Count = 0;
        //Debug.Log(_listItemData.Count);
        foreach (ListItemData lid in _listItemData)
        {
            GameObject item;
            item = Instantiate(_listItem, _list.transform, false) as GameObject;
            //item.transform.localPosition = new Vector3(0, offset, 0);
            item.name = lid.EntityID.ToString();
			UI_GeneralListItemControl itemCtrl = item.GetComponent<UI_GeneralListItemControl>();
			itemCtrl.EntityID = lid.EntityID;
			itemCtrl.count = lid.count;
            itemCtrl.EntityName = lid.EntityName;
            itemCtrl.Misc = lid.MiscData;
            itemCtrl.EntitySprite = _spriteFinder.findSpriteByEntityID(lid.EntityID, lid.EntityType);
            itemCtrl.orderInList = Count;
            itemCtrl.ButtonEnabled = lid.Interable;
            if (itemCtrl.ActionButton != null)
                itemCtrl.ActionButton.onClick.AddListener(delegate
                {
                    ListItemClickedAction(itemCtrl.orderInList);
                });
            _listItemObjects.Add(itemCtrl);
            Count++;
        }
    }

    public List<UI_GeneralListItemControl> ItemControllers
    {
        get
        {
            return _listItemObjects;
        }
    }



    void DestroyAllItems()
    {
        foreach (UI_GeneralListItemControl go in _listItemObjects)
        {
            go.Die();
        }
        _listItemObjects.Clear();
    }

    public Transform ListOrigin
    {
        get
        {
            return _listOriginalPosition;
        }
    }

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        set
        {
            _isMoving = value;
        }
    }

    private void ListItemClickedAction(int orederInList)
    {
        _Controller.SendMessage(ItemClickedFunction, orederInList);
    }


}
