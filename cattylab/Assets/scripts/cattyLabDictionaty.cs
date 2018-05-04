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
		Debug.Log(rawEntity);
		entities = JsonUtility.FromJson<entityCollection>(rawEntity);
	}

	public string GetCatName(int id){
		return entities.cats[id].name;
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
