using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class gameData
{
    public long money;
    public int maxGroupCount;
    public int maxCats;
    public int maxGroupPplCount;
    public int unlockScore;
    public List<cat> ownedCats;
    public List<item> ownedItems;
    public List<exploreGroups> exploreGroups;
    public bool isCrafting;
    public int craftID;
    public double craftETC;
    public gameSettings gameSettings;
    public gameData(long moneyIn, int maxG, int maxC, int maxGPC, int uS, cat[] oC, item[] oI, exploreGroups[] eG,bool iC, int cID, double cETC, gameSettings gS)
    {
        money = moneyIn;
        maxGroupCount = maxG;
        maxCats = maxC;
        maxGroupPplCount = maxGPC;
        unlockScore = uS;
        ownedCats = new List<cat>(oC);
        ownedItems = new List<item>(oI);
        exploreGroups = new List<exploreGroups>(eG);
        isCrafting = iC;
        craftID = cID;
        craftETC = cETC;
        gameSettings = gS;
    }

    public gameData(gameData data){
        money = data.money;
        maxGroupCount = data.maxGroupCount;
        maxCats = data.maxCats;
        maxGroupPplCount = data.maxGroupCount;
        unlockScore = data.unlockScore;
        ownedCats = new List<cat>(data.ownedCats);
        ownedItems = new List<item>(data.ownedItems);
        exploreGroups = new List<exploreGroups>(data.exploreGroups);
        isCrafting = data.isCrafting;
        craftID = data.craftID;
        craftETC = data.craftETC;
    }

    public static gameData init{
        get{
            return new gameData(1000,1,20,3,0,new cat[1]{new cat(0,1,1)},new item[0],new exploreGroups[0],false,-1,0, new gameSettings());
        }
    }
}


[System.Serializable]
public struct item
{
    public int id{
        get;
        set;
    }
    public int count;
}

[System.Serializable]
public class cat
{
    public cat(){}

    public cat(int iid, int a, int cc){
        id = iid;
        avaliable = a;
        count = cc;
    }

    public int id{
        get;
        set;
    }
    public int avaliable;
    public int count;
}



[System.Serializable]
public class catData
{
    public int id{
        get;
        set;
    }
    public string name;
    public int level;
    public int price;
    public string description;
}

[System.Serializable]
public struct itemData
{
    public int id{
        get;
        set;
    }
    public string name;
    public int level;
    public int price;
    public string description;
}

[System.Serializable]
public struct exploreGroups
{
    public string groupName;
    public int[] crews; //ids of each cat
    public bool isOut;
    public double backTime;
    public int destination;
}

public struct levels
{
    public int id;
    public string name;
    public int distance;
    public int rate;
    public int unlockScore;
}

[System.Serializable]
public class gameSettings
{
    public bool pure = true;
    public int maxGroupCount;
}