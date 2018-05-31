using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cattyLabDictionaty : MonoBehaviour{

	private string rawEntity = "",rawRecipes = "",rawLevels = "",rawSettings ="";
	private entityCollection entityCollection = new entityCollection();
	private recipeCollection recipeCollection = new recipeCollection();
	private levelCollection levelCollection = new levelCollection();
	private CLGameSettings gameSettings = new CLGameSettings();
	private bool _ready = false;
	public TextAsset EntitieText, RecipeText, LevelText, SettingsText;

	public cattyLabDictionaty(){
	}
	void init(){
		if(!_ready){
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

	
	private void SortEntity<T>(ref T[] arr) where T : Ientity, new()
	{
		List<T> tmp = new List<T>();
		Debug.Log(typeof(T).ToString() + arr.Length);
		for(int i = 0;i<arr.Length;i++){
			//Debug.Log(arr[i].ent_id);
			if(i==0 || arr[i].ent_id > tmp[tmp.Count-1].ent_id){
				tmp.Add(arr[i]);
			}else{
				int cc;
				for(cc = tmp.Count;cc>=0&&tmp[cc-1].ent_id>arr[i].ent_id;cc--);
				tmp.Insert(cc, arr[i]);
			}
		}
		int count = 0;
		while(count < tmp.Count){
			if(tmp[count].ent_id > count){
				//Debug.Log("aye");
				tmp.Insert(count, new T());
			}
			count++;
		}
		arr = tmp.ToArray();
		
	}

	public bool IsReady{
		get{
			return _ready;
		}
	}

	void Start(){
		Debug.Log("CLD START");
		init();

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
			tmp.id = entityCollection.cats[id].ent_id;
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

	public List<catData> GetAllCats(){
		List<catData> output = new List<catData>();
		for(int i = 0 ; i < entityCollection.cats.Length ; i++){
			try{
				catData tmp = new catData();
				tmp.id = entityCollection.cats[i].ent_id;
				tmp.name = entityCollection.cats[i].name;
				tmp.level = entityCollection.cats[i].level;
				tmp.price = entityCollection.cats[i].price;
				tmp.description = entityCollection.cats[i].description;
				output.Add(tmp);
			}catch{

			}
		}
		return output;
	}

	public string GetItemName(int id){
		try{
			return entityCollection.items[entityCollection.items[id].ent_id].name;
		}catch{
			return "NOTHING!!!!";
		}
	}

	public List<itemData> GetAllItems(){
		List<itemData> output = new List<itemData>();
		for(int i = 0 ; i < entityCollection.items.Length ; i++){
			try{
				itemData tmp = new itemData();
				tmp.id = entityCollection.items[i].ent_id;
				tmp.name = entityCollection.items[i].name;
				tmp.rarity = entityCollection.items[i].rarity;
				tmp.price = entityCollection.items[i].price;
				tmp.description = entityCollection.items[i].description;
				output.Add(tmp);
			}catch{

			}
		}
		return output;
	}

	public itemData GetItemData(int id){
		itemData tmp = new itemData();
		try{
			tmp.id = entityCollection.items[id].ent_id;
			tmp.name = entityCollection.items[id].name;
			tmp.rarity = entityCollection.items[id].rarity;
			tmp.price = entityCollection.items[id].price;
			tmp.description = entityCollection.items[id].description;
		}catch(Exception e){
			tmp.id = id;
			tmp.name = "NOTHING!!!!";
			tmp.rarity = -1;
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
			try{
				if(cats.SequenceEqual(rD.cats) && items.SequenceEqual(rD.items)){
					return rD;
				}
			}catch{

			}
		}
		return null;
	}

	public recipeData FindRecipeResultData(List<int> cats, List<int> items){
		return FindRecipeResultData(cats.ToArray(), items.ToArray());
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

	public double GetPossibleBonus()
	{
		return gameSettings.possibleBonus;
	}
}

[Serializable]
public class entityCollection
{
	public catData[] cats = new catData[0];
	public itemData[] items = new itemData[0];
}

[Serializable]
public class recipeCollection
{
	public recipeData[] recipes = new recipeData[0];
}

[Serializable]
public class levelCollection
{
	public levels[] levels = new levels[0];
}

[Serializable]
public class CLGameSettings
{
	public int maxGroupCount = 0;
	public double possibleBonus = 0;
}