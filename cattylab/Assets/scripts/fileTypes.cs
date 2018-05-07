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
    public List<exploredLevels> exploredLevels;
    public bool isCrafting;
    public int craftID;
    public double craftETC;
    public gameSettings gameSettings;
    public gameData(long moneyIn, int maxG, int maxC, int maxGPC, int uS, cat[] oC, item[] oI, exploreGroups[] eG,exploredLevels[] eL,bool iC, int cID, double cETC, gameSettings gS)
    {
        money = moneyIn;
        maxGroupCount = maxG;
        maxCats = maxC;
        maxGroupPplCount = maxGPC;
        unlockScore = uS;
        ownedCats = new List<cat>(oC);
        ownedItems = new List<item>(oI);
        exploreGroups = new List<exploreGroups>(eG);
        exploredLevels = new List<exploredLevels>(eL);
        isCrafting = iC;
        craftID = cID;
        craftETC = cETC;
        gameSettings = gS;
    }

    public gameData(gameData data){
        money = data.money;
        maxGroupCount = data.maxGroupCount;
        maxCats = data.maxCats;
        maxGroupPplCount = data.maxGroupPplCount;
        unlockScore = data.unlockScore;
        ownedCats = new List<cat>(data.ownedCats);
        ownedItems = new List<item>(data.ownedItems);
        exploreGroups = new List<exploreGroups>(data.exploreGroups);
        exploredLevels = new List<exploredLevels>(data.exploredLevels);
        isCrafting = data.isCrafting;
        craftID = data.craftID;
        craftETC = data.craftETC;
    }

    public static gameData init{
        get{
            return new gameData(1000,1,20,3,0,new cat[1]{new cat(0,1,1)},new item[0],new exploreGroups[0], new exploredLevels[0],false,-1,0, new gameSettings());
        }
    }
}

[System.Serializable]
public class exploredLevels{
    public int id;
    public int rate;
}

public interface Ientity{
    int id{
        get;
        set;
    }
}

public interface ILootable:Ientity
{
    int rarity{
        get;
    }
}

[System.Serializable]
public class item:Ientity
{
    public item(){}
    public item(int iid, int icount){
        id = iid;
        count = icount;
    }
    public int id{
        get;
        set;
    }
    public int count;
}

[System.Serializable]
public class cat:Ientity
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
    public bool isAvaliable{
        get{
            return (count - avaliable) > 0;
        }
    }
}


[System.Serializable]
public class catData:Ientity, ILootable
{

    public int id{
        get;
        set;
    }
    public string name;
    public int level;
    public int price;
    public string description;

    public int rarity{
        get{
            return level;
        }
    }
}

[System.Serializable]
public class itemData:Ientity, ILootable
{
    public int id{
        get;
        set;
    }
    public string name;
    public int rarity{
        get;
        set;
    }
    public int price;
    public string description;
}

[System.Serializable]
public class exploreGroups
{
    public string groupName;
    public int[] crews; //ids of each cat
    public double ETC;
    public int destination;

}

[System.Serializable]
public class levels
{
    public int id;
    public string name;
    public int distance;
    public int rate;
    public int unlockScore;
    public int cost;

    public loots[] loots;

    public bool avaliable(int score)
    {
        return score >= unlockScore;
    }
}

[System.Serializable]
public class loots
{   
    public string type;
    public int id;
    public double pos;
}

[System.Serializable]
public class gameSettings
{
    public bool pure = true;
    public int maxGroupCount;
}


[Serializable]
public class recipeData
{
    public int id;
	public int time;
	public List<int> cats;
	public List<int> items;
    public string type;
	public int r_id;
    public int cost;
	
}