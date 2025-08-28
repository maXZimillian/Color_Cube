using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class GameData
{
    public int currentLevel = 0;
    
    public GameData()
    {
        currentLevel = 0;
    }
}
public class DataSaver
{

    public void Save(int level = -1)
    {
        GameData prevSave = Load();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
          + "/SaveData.dat");
        GameData data = new GameData();

        if (prevSave.currentLevel<level){
            data.currentLevel = level;
        }
        else
        {
            data.currentLevel = prevSave.currentLevel;
        }
        bf.Serialize(file, data);
        file.Close();
    }

    public GameData Load()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = 
              File.Open(Application.persistentDataPath 
              + "/SaveData.dat", FileMode.Open);
            GameData data = new GameData();
            try
            {
                data = (GameData)bf.Deserialize(file);
            }
            catch
            {
            }
            file.Close();
            return data;
        }
        else return new GameData();
    }

        public void Reset()
        {
            if (File.Exists(Application.persistentDataPath 
            + "/SaveData.dat"))
            {
                File.Delete(Application.persistentDataPath 
                + "/SaveData.dat");
            }
        }
}
