using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerSave",menuName ="My Asset/Create Player Save",order = 1)]
public class createSave : ScriptableObject {

    public int money;
    public int maxGropuCount;
    public int maxCats;
    public int maxGroupPplCount;
    public int unlockScore;
    public gameEntities[] gameEntities;
    public exploreGroups[] exploreGroups;

}

public struct gameEntities
{
    public bool isItem;
    public int id;
    public int count;
}

public struct exploreGroups
{
    public int[] crews;
    public bool isOut;
    public double backTime;
    public int destination;
}