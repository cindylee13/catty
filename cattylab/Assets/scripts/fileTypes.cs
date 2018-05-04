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
    public gameData(int moneyInt, int maxG, int maxC, int maxGPC, int uS, gameEntities[] gE, exploreGroups[] eG)
    {
        money = moneyInt;
        maxGroupCount = maxG;
        maxCats = maxC;
        maxGroupPplCount = maxGPC;
        unlockScore = uS;
        gameEntities = (gameEntities[])gE.Clone();
        exploreGroups = (exploreGroups[])eG.Clone();
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
    public int[] crews; //ids of each cat
    public bool isOut;
    public double backTime;
    public int destination;
}