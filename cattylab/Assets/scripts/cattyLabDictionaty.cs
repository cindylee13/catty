using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cattyLabDictionaty : MonoBehaviour{

	private string rawEntity,rawRecipes,rawLevels;
	private entityCollection entityCollection = new entityCollection();
	private recipeCollection recipeCollection = new recipeCollection();
	private levelCollection levelCollection = new levelCollection();

	public cattyLabDictionaty(){
		start();
	}

	void start(){
		rawEntity = ReadString("Assets/gameData/entities.json");
		rawRecipes = ReadString("Assets/gameData/recipes.json");
		rawLevels = ReadString("Assets/gameData/levels.json");
		entityCollection = JsonUtility.FromJson<entityCollection>(rawEntity);
		recipeCollection = JsonUtility.FromJson<recipeCollection>(rawRecipes);
		levelCollection = JsonUtility.FromJson<levelCollection>(rawLevels);

	}

	public string GetCatName(int id){
		try{
			return entityCollection.cats[id].name;
		}catch{
			return "ERRORCAT";
		}
	}

	public catData GetCatData(int id){
		catData tmp = new catData();
		try{
			tmp.id = id;
			tmp.name = entityCollection.cats[id].name;
			tmp.level = entityCollection.cats[id].level;
			tmp.price = entityCollection.cats[id].price;
			tmp.description = entityCollection.cats[id].description;
		}catch(Exception e){
			tmp.id = id;
			tmp.name = "ERRORCAT";
			tmp.level = -1;
			tmp.price = -1;
			tmp.description = "U DON FKED UP:\n" + e.Message;
		}
		return tmp;
	}

	public string GetItemName(int id){
		try{
			return entityCollection.items[id].name;
		}catch{
			return "NOTHING!!!!";
		}
	}

	public itemData GetItemData(int id){
		itemData tmp = new itemData();
		try{
			tmp.id = id;
			tmp.name = entityCollection.items[id].name;
			tmp.level = entityCollection.items[id].level;
			tmp.price = entityCollection.items[id].price;
			tmp.description = entityCollection.items[id].description;
		}catch(Exception e){
			tmp.id = id;
			tmp.name = "NOTHING!!!!";
			tmp.level = -1;
			tmp.price = -1;
			tmp.description = "U DON FKED UP:<br>" + e.Message;
		}
		return tmp;
	}

	string ReadString(string path){
		string output;
		StreamReader reader = new StreamReader(path);
		output = reader.ReadToEnd();
		reader.Close();
		return output;
	}

	public recipeData FindRecipeResultData(int[] cats, int[] items)
	{
		Array.Sort(cats);
		Array.Sort(items);
		foreach(recipeData rD in recipeCollection.recipes){
			if(cats.SequenceEqual(rD.cats) && items.SequenceEqual(rD.items)){
				return rD;
			}
		}
		return null;
	}

	public double GetRecipeTime(int id){
		return (double) recipeCollection.recipes[id].time;
	}

	public int GetRecipeCost(int id){
		return recipeCollection.recipes[id].cost;
	}

	public recipeData GetRecipeByID(int id){
		return recipeCollection.recipes[id];
	}

	public Ientity GetEntityByRecipeID(int id){
		recipeData rD = recipeCollection.recipes[id];
		if(rD.type=="cat"){
			return GetCatData(rD.r_id);
		}else if(rD.type=="item"){
			return GetItemData(rD.r_id);
		}
		return null;
	}

	public levels GetLevelByID(int id){
		levels lv = levelCollection.levels[id];
		return lv;
	}

	public string GetLevelNameByID(int id){
		return 	levelCollection.levels[id].name;
	}

	public int GetTotalLevelCount(){
		return levelCollection.levels.Length;
	}

	public int GetTotalCatCount(){
		return entityCollection.cats.Length;
	}
	public 	int GetTotalItemCount(){
		return entityCollection.items.Length;
	}
}

[Serializable]
public class entityCollection
{
	public catData[] cats;
	public itemData[] items;
}

[Serializable]
public class recipeCollection
{
	public recipeData[] recipes;
}

[Serializable]
public class levelCollection
{
	public levels[] levels;
}
