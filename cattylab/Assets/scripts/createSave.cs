using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


/*
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
*/

public class saveData
{
    public gameData gameData;

    public void saveFile()
    {
        string destination = Application.persistentDataPath + "/playerSave";
        string filename = "/save.dat";
        FileStream file;
        if (File.Exists(destination + filename)) {
            Debug.Log("rewriting");
            file = File.OpenWrite(destination + filename);
            }
        else {
            Debug.Log("creating");
            if(!Directory.Exists(destination)){
                Directory.CreateDirectory(destination);
            }
            file = File.Create(destination + filename);
            }

        gameData tmpData = new gameData(gameData);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, tmpData);
        file.Close();
    }

    public bool loadfile()
    {
        Debug.Log("loading file");
        string destination = Application.persistentDataPath + "/playerSave/save.dat";
        FileStream file;
        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.Log("File not found");
            return false;
        }
        BinaryFormatter bf = new BinaryFormatter();
        gameData data = (gameData)bf.Deserialize(file);
        file.Close();
        set(data);
       
        return true;
    }

    public void set(gameData data){
        gameData = new gameData(data);
    }

}