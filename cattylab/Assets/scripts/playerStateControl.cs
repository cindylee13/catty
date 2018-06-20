using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class EventWithMessage : UnityEvent<string> { }


[System.Serializable]
public class EventWithID : UnityEvent<int, int> { }
public class playerStateControl : MonoBehaviour
{

    private saveData overallData;
    public cattyLabDictionaty CLD;

    //-----------
    //  EVENTS
    //-----------

    public UnityEvent OnMoneyChanged, OnGameInitialize, OnCatDataChaged, OnItemDataChanged, OnCraftingStarted,
    OnCraftingEnded, OnGroupDataChanged, OnExploreStarted, OnExploreEnded, OnLevelDataChanged;
    public EventWithMessage EventNotifier;
    public EventWithID EventNotifierRich;


    // Use this for initialization
    IEnumerator Start()
    {
        if (!CLD.IsReady)
        {
            yield return 0;
        }
        //load savefile on startup
        overallData = new saveData();
        overallData.gameData = new gameData(gameData.init);
        //Debug.Log("init");
        if (!overallData.loadfile())
        {
            overallData.set(gameData.init);
            overallData.saveFile();
        }

        //Debug.Log(JsonUtility.ToJson(overallData.gameData,true));
        //init events
        if (EventNotifier == null) EventNotifier = new EventWithMessage();
        if (EventNotifierRich == null) EventNotifierRich = new EventWithID();
        if (OnMoneyChanged == null) OnMoneyChanged = new UnityEvent();
        if (OnGameInitialize == null) OnGameInitialize = new UnityEvent();
        if (OnCatDataChaged == null) OnCatDataChaged = new UnityEvent();
        if (OnItemDataChanged == null) OnItemDataChanged = new UnityEvent();
        if (OnCraftingStarted == null) OnCraftingStarted = new UnityEvent();
        if (OnCraftingEnded == null) OnCraftingEnded = new UnityEvent();
        if (OnGroupDataChanged == null) OnGroupDataChanged = new UnityEvent();
        if (OnExploreEnded == null) OnExploreEnded = new UnityEvent();
        if (OnExploreStarted == null) OnExploreStarted = new UnityEvent();
        if (OnLevelDataChanged == null) OnLevelDataChanged = new UnityEvent();
        OnGameInitialize.AddListener(OnGameInitializehandler);

        OnGameInitialize.Invoke();
    }

    void onApplicationQuit()
    {
        Debug.Log("aye");
    }

    // Update is called once per frame
    void Update()
    {

    }

    //-----------
    // METHODS
    //-----------

    private void OnGameInitializehandler()
    { // invoke every data event
        StopCoroutine("StartCraftingClock");
        OnCatDataChaged.Invoke();
        OnMoneyChanged.Invoke();
        OnItemDataChanged.Invoke();
        OnGroupDataChanged.Invoke();
        if (overallData.gameData.isCrafting)
        {
            StartCoroutine(StartCraftingClock());
        }
        foreach (exploreGroups eG in overallData.gameData.exploreGroups)
        {
            StartCoroutine(StartExploreClock(eG));
        }
    }

    public void ResetGameData()
    {
        overallData.set(gameData.init);
        OnGameInitialize.Invoke();
        EventNotifier.Invoke("Data Reset");
    }

    public void SaveGameData()
    {
        overallData.saveFile();
        EventNotifier.Invoke("Data Saved");
    }


    public bool isCrafting
    {
        get
        {
            return overallData.gameData.isCrafting;
        }
    }

    public double craftETC
    {
        get
        {
            return overallData.gameData.craftETC;
        }
    }

    public long money
    {
        get
        {
            return overallData.gameData.money;
        }
    }

    public int groupCount
    {
        get
        {
            return overallData.gameData.exploreGroups.Count;
        }
    }

    public int maxGroupCount
    {
        get
        {
            return overallData.gameData.maxGroupCount;
        }
        set
        {
            overallData.gameData.maxGroupCount = value;
            Debug.Log(overallData.gameData.maxGroupCount);
            OnGroupDataChanged.Invoke();
        }
    }

    public int maxGroupPplCount
    {
        get
        {
            return overallData.gameData.maxGroupPplCount;
        }
        set
        {
            overallData.gameData.maxGroupPplCount = value;
            OnGroupDataChanged.Invoke();
        }
    }

    public int unlockScore
    {
        get
        {
            return overallData.gameData.unlockScore;
        }
        private set
        {
            EventNotifier.Invoke("找到了新的區域!");
            overallData.gameData.unlockScore = value;
        }
    }

    public bool canSendGroup
    {
        get
        {
            return groupCount < maxGroupCount;
        }
    }

    public List<cat> Ownedcats
    {
        get
        {
            if (overallData == null) return null;
            else return overallData.gameData.ownedCats;
        }
    }

    public List<item> OwnedItems
    {
        get
        {
            if (overallData == null) return null;
            else return overallData.gameData.ownedItems;
        }
    }

    public exploreGroups GetGroupData(int index)
    {
        return overallData.gameData.exploreGroups[index];
    }

    public exploredLevels GetExploredLevelData(int levelID)
    {
        foreach (exploredLevels eL in overallData.gameData.exploredLevels)
        {
            if (eL.id == levelID)
            {
                return eL;
            }
        }
        return null;
    }

    public bool SendGroup(int[] crew, int levelID)
    {
        exploreGroups eG = new exploreGroups();
        eG.crews = crew;
        eG.destination = levelID;
        eG.groupName = "Group " + (groupCount + 1);
        eG.ETC = ConvertToUnixTimestamp(System.DateTime.Now) + CLD.GetLevelByID(levelID).distance;
        if (!canSendGroup)
        {
            Debug.LogError("CAN'T SEND MORE GROUP FUCKER");
            return false;
        }
        int count = 0;

        for (int i = 0; i < eG.crews.Length; i++)
        {
            if (i == 0 || eG.crews[i] == eG.crews[i - 1])
            {
                count++;
            }
            else
            {
                CatControl(eG.crews[i - 1], -count, CatControlType.avaliable);
                count = 1;
            }
            if (i + 1 == eG.crews.Length)
            {
                CatControl(eG.crews[i], -count, CatControlType.avaliable);
            }
        }
        overallData.gameData.exploreGroups.Add(eG);
        StartCoroutine(StartExploreClock(eG));
        //Debug.Log("Explore Started");
        EventNotifier.Invoke("貓貓們出發了!");
        return true;
    }

    public bool CatControl(int id, int amount, CatControlType type)
    {
        int current = 0;
        bool success = false, found = false;
        foreach (cat cat in overallData.gameData.ownedCats)
        {
            if (cat.ent_id == id)
            {
                found = true;
                if (type == CatControlType.count)
                {
                    int tmp = cat.count;
                    tmp += amount;
                    if (tmp < 0)
                    {
                        Debug.LogError("Removed too much cats! must be exact 0 to remove cat type");
                        success = false;
                        break;
                    }
                    else if (tmp == 0)
                    {
                        Debug.Log("Removing Cat Type");
                        overallData.gameData.ownedCats.RemoveAt(current);
                        current--;
                        success = true;
                        break;
                    }
                    cat.count += amount;
                    cat.avaliable += amount;
                    success = true;
                    break;
                }
                else
                {
                    int tmp = cat.avaliable;
                    tmp += amount;
                    if (tmp < 0)
                    {
                        Debug.LogError("Not Enough Cats to occupy!(<0)");
                        success = false;
                        break;
                    }
                    else if (tmp > cat.count)
                    {
                        Debug.LogError("Not Enough Cats to occupy!(>cat count)");
                        success = false;
                        break;
                    }
                    else
                    {
                        cat.avaliable = tmp;
                        success = true;
                        break;
                    }
                }
            }
            current++;
        }
        if (!found && amount > 0)
        {
            if (type == CatControlType.avaliable)
            {
                Debug.LogError("Cat Not Found!");
                success = false;
            }
            else
            {
                Debug.Log("Cat not found, adding cat");

                for (int i = 0; i < overallData.gameData.ownedCats.Count; i++)
                {
                    if (overallData.gameData.ownedCats[i].ent_id > id)
                    {
                        Debug.Log("Inserting cat to " + i);
                        overallData.gameData.ownedCats.Insert(i, new cat(id, amount, amount));
                        success = true;
                        break;
                    }
                }
                if (!success)
                {
                    Debug.Log("Adding cat to the end of the list");
                    overallData.gameData.ownedCats.Add(new cat(id, amount, amount));
                    success = true;
                }
                EventNotifierRich.Invoke(id, 0);
            }
        }
        if (success) OnCatDataChaged.Invoke();
        else Debug.LogError("U DON FKED UP ID:" + id + " AMOUNT:" + amount + " TYPE:" + type);
        System.GC.Collect();
        return success;
    }



    public bool ItemControl(int id, int amount)
    {
        bool success = false, found = false;
        int current = 0;
        foreach (item nowItem in overallData.gameData.ownedItems)
        {
            if (nowItem.ent_id == id)
            {
                found = true;
                if (nowItem.count + amount < 0)
                {
                    Debug.LogError("Must Be exact 0 to remove this item type!");
                    success = false;
                    break;
                }
                else if (nowItem.count + amount == 0)
                {
                    Debug.Log("Removing Item Type");
                    overallData.gameData.ownedItems.RemoveAt(current);
                    success = true;
                    break;
                }
                else
                {
                    nowItem.count = nowItem.count + amount;
                    success = true;
                    break;
                }
            }
        }
        if (!found && amount > 0)
        {
            Debug.Log("Item not found, adding item");
            for (int i = 0; i < overallData.gameData.ownedItems.Count; i++)
            {
                if (overallData.gameData.ownedItems[i].ent_id > id)
                {
                    overallData.gameData.ownedItems.Insert(i, new item(id, amount));
                    success = true;
                    break;
                }
            }
            if (!success)
            {
                overallData.gameData.ownedItems.Add(new item(id, amount));
                success = true;
            }
        }
        if (success) OnItemDataChanged.Invoke();
        else Debug.LogError("U DON FKED UP");
        return success;
    }


    public bool changeMoney(int amount)
    {
        if (overallData.gameData.money + amount < 0)
        {
            EventNotifier.Invoke("罐罐不夠了...");
            return false;
        }
        overallData.gameData.money += amount;
        OnMoneyChanged.Invoke();
        string message;
        if (amount >= 0)
        {
            message = "得到了" + amount + "個罐罐!";
        }
        else
        {
            message = "花掉了" + (amount * -1) + " 個罐罐...";
        }
        EventNotifier.Invoke(message);
        return true;
    }

    public void SubmitRecipe(int recipe_id)
    {
        if (overallData.gameData.isCrafting)
        {
            Debug.LogError("ALREADY CRAFTING!");
            return;
        }
        Debug.Log("Recipe Received");
        recipeData rD = CLD.GetRecipeByID(recipe_id);
        int catToRemove = 0, itemToRemove = 0;
        if (!changeMoney(rD.cost * -1)) return;
        for (int i = 0; i < rD.cats.Count; i++)
        {
            if (i == 0 || rD.cats[i - 1] == rD.cats[i])
            {
                catToRemove++;
            }
            else
            {
                CatControl(rD.cats[i - 1], -catToRemove, CatControlType.count);
                catToRemove = 1;
            }

            if (i == rD.cats.Count - 1)
            {
                CatControl(rD.cats[i], -catToRemove, CatControlType.count);
            }
        }
        Debug.Log("Cat Removed");
        for (int i = 0; i < rD.items.Count; i++)
        {
            if (i == 0 || rD.items[i - 1] == rD.items[i])
            {
                itemToRemove++;
            }
            else
            {
                ItemControl(rD.items[i - 1], -itemToRemove);
                itemToRemove = 1;
            }
            if (i == rD.items.Count - 1)
            {
                ItemControl(rD.items[i], -itemToRemove);
            }
        }
        Debug.Log("Item Removed");
        overallData.gameData.isCrafting = true;
        overallData.gameData.craftID = recipe_id;
        overallData.gameData.craftETC = ConvertToUnixTimestamp(System.DateTime.Now) + CLD.GetRecipeTime(recipe_id);
        EventNotifier.Invoke("開始合成!");
        StartCoroutine(StartCraftingClock());
        OnCatDataChaged.Invoke();
        OnItemDataChanged.Invoke();
    }

    void CraftingEnded()
    {
        overallData.gameData.isCrafting = false;
        Ientity entity = CLD.GetEntityByRecipeID(overallData.gameData.craftID);
        if (entity.GetType() == typeof(catData))
        {
            Debug.Log("Added Cat:" + ((catData)entity).name + "  ID:" + ((catData)entity).ent_id);
            CatControl(((catData)entity).ent_id, 1, CatControlType.count);
            EventNotifier.Invoke("合成了: " + ((catData)entity).name);
            OnCatDataChaged.Invoke();
        }
        else
        {
            ItemControl(((itemData)entity).ent_id, 1);
            EventNotifier.Invoke("合成了: " + ((itemData)entity).name);
            OnItemDataChanged.Invoke();
        }
        OnCraftingEnded.Invoke();
    }

    private void ExploreEnded(int indexInList)
    {
        Debug.Log("Explore Ended");
        exploreGroups eG = overallData.gameData.exploreGroups[indexInList];
        levels lvl = CLD.GetLevelByID(eG.destination);
        List<Ientity> loots = new List<Ientity>(CalculateLoots(lvl, eG.crews));
        bool explored = false;
        int count = 0;
        string lootText = string.Empty;
        //return cats to count
        for (int i = 0; i < eG.crews.Length; i++)
        {
            if (i == 0 || eG.crews[i] == eG.crews[i - 1])
            {
                count++;
            }
            else
            {
                CatControl(eG.crews[i - 1], count, CatControlType.avaliable);
                count = 1;
            }
            if (i + 1 == eG.crews.Length)
            {
                CatControl(eG.crews[i], count, CatControlType.avaliable);
            }
        }
        //adding loots to inventory
        for (int i = 0; i < loots.Count; i++)
        {
			//lootText += CLD.GetCatName(loots[i].ent_id) + "  ";
            Debug.Log("aye" + loots[i].GetType());
            count = 1;
            for (int j = i + 1; j < loots.Count; j++)
            {
                Debug.Log("aye" + loots[j].GetType());
                if (loots[i].GetType() == loots[j].GetType() && loots[i].ent_id == loots[j].ent_id)
                {
                    Debug.Log("DIE");
                    count++;
                    loots.RemoveAt(j);
                    j--;
                }
            }
            if (loots[i].GetType() == typeof(catData))
            {
                CatControl(loots[i].ent_id, count, CatControlType.count);
                lootText += CLD.GetCatName(loots[i].ent_id) + "X" + count + " ";
            }
            else
            {
                ItemControl(loots[i].ent_id, count);
                lootText += CLD.GetItemName(loots[i].ent_id) + "X" + count + " ";
            }
        }
        //setting explored level
        foreach (exploredLevels exLvl in overallData.gameData.exploredLevels)
        {
            if (exLvl.id == lvl.ent_id)
            {
                explored = true;
                if (exLvl.rate + lvl.rate >= 100 && exLvl.rate < 100)
                {
                    unlockScore++;
                }
                exLvl.rate += lvl.rate;
            }
        }
        if (!explored)
        {
            exploredLevels exLvl = new exploredLevels();
            exLvl.id = lvl.ent_id;
            exLvl.rate = lvl.rate;
            overallData.gameData.exploredLevels.Add(exLvl);
            if (exLvl.rate >= 100)
            {
                unlockScore++;
            }
        }

        overallData.gameData.exploreGroups.RemoveAt(indexInList);
        EventNotifier.Invoke("探索完成!");
        OnLevelDataChanged.Invoke();
        OnGroupDataChanged.Invoke();
        OnExploreEnded.Invoke();
        changeMoney(CLD.GetLevelByID(eG.destination).cost);
        if (lootText != string.Empty)
            EventNotifier.Invoke("貓貓們找到了: " + lootText);
        else
            EventNotifier.Invoke("貓貓們什麼都沒有拿回來:3");
    }

    private Ientity[] CalculateLoots(levels level, int[] catIDs)
    {

        List<Ientity> loots = new List<Ientity>();
        List<loots> possibleLoots = new List<loots>(level.loots);
        foreach (int ID in catIDs)
        {
            //Debug.Log("CATIDS: " + ID);
            Ientity bestLoot = null;
            catData cat = CLD.GetCatData(ID);
            foreach (loots lootData in possibleLoots)
            {
                ILootable lootCand;
                if (lootData.type == "cat")
                {
                    lootCand = CLD.GetCatData(lootData.id);
                }
                else
                {
                    lootCand = CLD.GetItemData(lootData.id);
                }
                int dice = (int)(Random.Range(0, 100) + (cat.rarity - lootCand.rarity) * CLD.GetPossibleBonus());

                //Debug.Log("DICE: " + dice);
                if (dice <= lootData.pos)
                {
                    bestLoot = lootCand;
                    break;
                }
            }
            if (bestLoot != null) loots.Add(bestLoot);
        }
		foreach(Ientity ie in loots){
			Debug.Log("IENTITY ID LOOT: " + ie.ent_id);
		}
        return loots.ToArray();
    }


    private IEnumerator StartCraftingClock()
    {
        OnCraftingStarted.Invoke();
        yield return new WaitForSecondsRealtime((int)(overallData.gameData.craftETC - ConvertToUnixTimestamp(System.DateTime.Now)));
        CraftingEnded();
    }

    private IEnumerator StartExploreClock(exploreGroups eG)
    {
        Debug.Log("SEC Started ETC:" + (eG.ETC - ConvertToUnixTimestamp(System.DateTime.Now)));
        OnExploreStarted.Invoke();
        yield return new WaitForSecondsRealtime((int)(eG.ETC - ConvertToUnixTimestamp(System.DateTime.Now)));
        Debug.Log("SEC Ended");
        for (int i = 0; i < overallData.gameData.exploreGroups.Count; i++)
        {
            if (IsSame(overallData.gameData.exploreGroups[i], eG))
            {
                ExploreEnded(i);
            }
        }
    }

    double ConvertToUnixTimestamp(System.DateTime date)
    {
        System.DateTime st = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
        System.TimeSpan diff = date - st;
        return System.Math.Floor(diff.TotalSeconds);
    }

    public bool IsSame(exploreGroups eg1, exploreGroups eg2)
    {
        if (eg1.ETC != eg2.ETC)
        {
            return false;
        }
        if (eg1.crews.Length == eg2.crews.Length)
        {
            for (int i = 0; i < eg1.crews.Length; i++)
            {
                if (eg1.crews[i] != eg2.crews[i])
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
        return eg1.destination == eg2.destination && eg1.groupName == eg2.groupName;
    }
}

public enum CatControlType
{
    count,
    avaliable
}