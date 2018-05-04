using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class gameData
{
    public int money;
    public int maxGroupCount;
    public int maxCats;
    public int maxGroupPplCount;
    public int unlockScore;
    public gameEntities[] gameEntities;
    public exploreGroups[] exploreGroups;
    public bool isCrafting;
    public int craftID;
    public double craftETC;

    public gameData(int moneyInt, int maxG, int maxC, int maxGPC, int uS, gameEntities[] gE, exploreGroups[] eG,bool iC, int cID, double cETC)
    {
        money = moneyInt;
        maxGroupCount = maxG;
        maxCats = maxC;
        maxGroupPplCount = maxGPC;
        unlockScore = uS;
        gameEntities = (gameEntities[])gE.Clone();
        exploreGroups = (exploreGroups[])eG.Clone();
        isCrafting = iC;
        craftID = cID;
        craftETC = cETC;
    }

    public gameData(gameData data){
        money = data.money;
        maxGroupCount = data.maxGroupCount;
        maxCats = data.maxCats;
        maxGroupPplCount = data.maxGroupCount;
        unlockScore = data.unlockScore;
        gameEntities = (gameEntities[])data.gameEntities.Clone();
        exploreGroups = (exploreGroups[])data.exploreGroups.Clone();
        isCrafting = data.isCrafting;
        craftID = data.craftID;
        craftETC = data.craftETC;
    }

    public static gameData init(){
        return new gameData(1000,1,20,3,0,new gameEntities[0],new exploreGroups[0],false,-1,0);
    }
}

public struct gameEntities
{
    public bool isItem; //true=item  false=cat
    public int id;
    public int count;
}

public struct exploreGroups
{
    public string groupName;
    public int[] crews; //ids of each cat
    public bool isOut;
    public double backTime;
    public int destination;
}