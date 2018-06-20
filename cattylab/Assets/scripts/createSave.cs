using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class saveData
{
    public gameData gameData;

    public void saveFile()
    {
        /*
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
        */
        string tmp;
        if (gameData != null)
        {
            tmp = JsonUtility.ToJson(gameData);
            PlayerPrefs.SetString("SAVEDATAS", tmp);
        }
        else
        {
            Debug.LogError("Load savedata first!");
        }
    }

    public bool loadfile()
    {
        /*
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
        gameData data = null;
        try{
            data = (gameData)bf.Deserialize(file);
            file.Close();
        }catch{
            Debug.Log("File Structure Changed: ");
            file.Close();
            return false;
        }
        set(data);
       
        return true;
        */
        string tmp = PlayerPrefs.GetString("SAVEDATAS", "NOT_FOUND");
        if (tmp == "NOT_FOUND")
        {
            gameData = gameData.init;
        }
        else
        {
            //Debug.Log(tmp);
            gameData = JsonUtility.FromJson<gameData>(tmp);
        }
        return true;

    }

    public void set(gameData data)
    {
        try
        {
            gameData = new gameData(data);
        }
        catch
        {
            gameData = gameData.init;
        }
    }
}