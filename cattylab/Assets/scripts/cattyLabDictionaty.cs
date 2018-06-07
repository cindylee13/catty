using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cattyLabDictionaty : MonoBehaviour
{

    private string rawEntity = String.Empty, rawRecipes = String.Empty, rawLevels = String.Empty, rawSettings = String.Empty;
    private entityCollection entityCollection = new entityCollection();
    private recipeCollection recipeCollection = new recipeCollection();
    private levelCollection levelCollection = new levelCollection();
    private CLGameSettings gameSettings = new CLGameSettings();
    private bool _ready = false;
    public TextAsset EntitieText, RecipeText, LevelText, SettingsText;

    public cattyLabDictionaty()
    {
    }
    void init()
    {
        if (!_ready)
        {
            rawEntity = EntitieText.text;
            rawRecipes = RecipeText.text;
            rawLevels = LevelText.text;
            rawSettings = SettingsText.text;
            entityCollection = JsonUtility.FromJson<entityCollection>(rawEntity);
            recipeCollection = JsonUtility.FromJson<recipeCollection>(rawRecipes);
            levelCollection = JsonUtility.FromJson<levelCollection>(rawLevels);
            gameSettings = JsonUtility.FromJson<CLGameSettings>(rawSettings);
            //reset
            SortEntity<catData>(ref entityCollection.cats);
            SortEntity<itemData>(ref entityCollection.items);
            SortEntity<recipeData>(ref recipeCollection.recipes);
            SortEntity<levels>(ref levelCollection.levels);
            _ready = true;
        }
    }


    private void SortEntity<T>(ref List<T> arr) where T : Ientity, new()
    {
        List<T> tmp = new List<T>();
        tmp = arr.OrderBy(o => o.ent_id).ToList();
        int count = 0;
        while (count < tmp.Count)
        {
            if (tmp[count].ent_id > count)
            {
                //Debug.Log("aye");
                tmp.Insert(count, new T());
            }
            count++;
        }
        arr = tmp;

    }

    public bool IsReady
    {
        get
        {
            return _ready;
        }
    }

    void Start()
    {
        Debug.Log("CLD START");
        init();

    }

    public string GetCatName(int id)
    {
        try
        {
            return entityCollection.cats[id].name;
        }
        catch
        {
            return "ERRORCAT";
        }
    }

    public catData GetCatData(int id)
    {
        try
        {
            return entityCollection.cats[id];
        }
        catch (Exception e)
        {
            catData tmp = new catData();
            tmp.id = id;
            tmp.name = "ERRORCAT";
            tmp.level = -1;
            tmp.price = -1;
            tmp.description = "U DON FKED UP:\n" + e.Message;
            return tmp;
        }
    }

    public List<catData> GetAllCats()
    {
        return entityCollection.cats;
    }

    public string GetItemName(int id)
    {
        try
        {
            return entityCollection.items[entityCollection.items[id].ent_id].name;
        }
        catch
        {
            return "NOTHING!!!!";
        }
    }

    public List<itemData> GetAllItems()
    {
        return entityCollection.items;
    }

    public itemData GetItemData(int id)
    {

        try
        {
            return entityCollection.items[id];
        }
        catch (Exception e)
        {
            itemData tmp = new itemData();
            tmp.id = id;
            tmp.name = "NOTHING!!!!";
            tmp.rarity = -1;
            tmp.price = -1;
            tmp.description = "U DON FKED UP:<br>" + e.Message;

            return tmp;
        }
    }

    public recipeData FindRecipeResultData(int[] cats, int[] items)
    {
        Array.Sort(cats);
        Array.Sort(items);
        foreach (recipeData rD in recipeCollection.recipes)
        {
            try
            {
                if (cats.SequenceEqual(rD.cats) && items.SequenceEqual(rD.items))
                {
                    return rD;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("ERROR FINDING RECIPE--- cats:" + cats.Length + " items:" + items.Length);
                Debug.LogError(e.Message);
            }
        }
        return null;
    }

    public recipeData FindRecipeResultData(List<int> cats, List<int> items)
    {
        return FindRecipeResultData(cats.ToArray(), items.ToArray());
    }

    public double GetRecipeTime(int id)
    {
        return (double)recipeCollection.recipes[id].time;
    }

    public int GetRecipeCost(int id)
    {
        return recipeCollection.recipes[id].cost;
    }

    public recipeData GetRecipeByID(int id)
    {
        return recipeCollection.recipes[id];
    }

    public Ientity GetEntityByRecipeID(int id)
    {
        recipeData rD = recipeCollection.recipes[id];
        if (rD.type == "cat")
        {
            return GetCatData(rD.r_id);
        }
        else if (rD.type == "item")
        {
            return GetItemData(rD.r_id);
        }
        return null;
    }

    public levels GetLevelByID(int id)
    {
        levels lv = levelCollection.levels[id];
        return lv;
    }

    public string GetLevelNameByID(int id)
    {
        return levelCollection.levels[id].name;
    }

    public int GetTotalLevelCount()
    {
        return levelCollection.levels.Count;
    }

    public int GetTotalCatCount()
    {
        return entityCollection.cats.Count;
    }
    public int GetTotalItemCount()
    {
        return entityCollection.items.Count;
    }

    public double GetPossibleBonus()
    {
        return gameSettings.possibleBonus;
    }
}

[Serializable]
public class entityCollection
{
    public List<catData> cats = new List<catData>();
    public List<itemData> items = new List<itemData>();
}

[Serializable]
public class recipeCollection
{
    public List<recipeData> recipes = new List<recipeData>();
}

[Serializable]
public class levelCollection
{
    public List<levels> levels = new List<levels>();
}

[Serializable]
public class CLGameSettings
{
    public int maxGroupCount = 0;
    public double possibleBonus = 0;
}