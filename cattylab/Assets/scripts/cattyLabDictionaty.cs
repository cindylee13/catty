using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cattyLabDictionaty : MonoBehaviour{

	private string rawEntity,rawRecipes;
	private entityCollection entities = new entityCollection();
	private recipeCollection recipes = new recipeCollection();

	public cattyLabDictionaty(){
		start();
	}

	void start(){
		rawEntity = ReadString("Assets/gameData/entities.json");
		rawRecipes = ReadString("Assets/gameData/recipes.json");
		entities = JsonUtility.FromJson<entityCollection>(rawEntity);
		recipes = JsonUtility.FromJson<recipeCollection>(rawRecipes);

	}

	public string GetCatName(int id){
		try{
			return entities.cats[id].name;
		}catch{
			return "ERRORCAT";
		}
	}

	public catData GetCatData(int id){
		catData tmp = new catData();
		try{
			tmp.id = id;
			tmp.name = entities.cats[id].name;
			tmp.level = entities.cats[id].level;
			tmp.price = entities.cats[id].price;
			tmp.description = entities.cats[id].description;
		}catch(Exception e){
			tmp.id = id;
			tmp.name = "ERRORCAT";
			tmp.level = -1;
			tmp.price = -1;
			tmp.description = "U DON FKED UP:<br>" + e.Message;
		}
		return tmp;
	}

	public string GetItemName(int id){
		try{
			return entities.items[id].name;
		}catch{
			return "NOTHING!!!!";
		}
	}

	public itemData GetItemData(int id){
		itemData tmp = new itemData();
		try{
			tmp.id = id;
			tmp.name = entities.items[id].name;
			tmp.level = entities.items[id].level;
			tmp.price = entities.items[id].price;
			tmp.description = entities.items[id].description;
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
		foreach(recipeData rD in recipes.recipes){
			if(cats.SequenceEqual(rD.cats) && items.SequenceEqual(rD.items)){
				return rD;
			}
		}
		return null;
	}

	public Ientity GetEntityByRecipeID(int id){
		recipeData rD = recipes.recipes[id];
		if(rD.type=="cat"){
			return entities.cats[rD.id];
		}else if(rD.type=="item"){
			return entities.items[rD.id];
		}
		return null;
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
