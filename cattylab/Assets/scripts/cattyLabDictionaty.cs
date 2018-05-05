using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cattyLabDictionaty : MonoBehaviour{

	private string rawEntity;
	private entityCollection entities = new entityCollection();

	public cattyLabDictionaty(){
		start();
	}

	void start(){
		rawEntity = ReadString("Assets/gameData/entities.json");
		entities = JsonUtility.FromJson<entityCollection>(rawEntity);
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

	string ReadString(string path){
		string output;
		StreamReader reader = new StreamReader(path);
		output = reader.ReadToEnd();
		reader.Close();
		return output;
	}


}

[Serializable]
public class entityCollection
{
	public catData[] cats;
	public itemData[] items;
}
