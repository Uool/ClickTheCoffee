using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface ILoader<Key,Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<string, Data.Stuff> StuffDict { get; private set; } = new Dictionary<string, Data.Stuff>();
    public Dictionary<int, Data.Recipe> RecipeDict { get; private set; } = new Dictionary<int, Data.Recipe>();
    public Data.PlayerStat Playerdata = new Data.PlayerStat();

    public void Init()
    {
        StuffDict = LoadJson<Data.StuffData, string, Data.Stuff>("StuffData").MakeDict();
        RecipeDict = LoadJson<Data.RecipeData, int, Data.Recipe>("RecipeData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");

        if (null == textAsset)
            Debug.Log($"Data Load failed : {path}");

        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    public void SaveJson<T> (T saveData) where T : Data.PlayerStat
    {
        string jsonData = JsonUtility.ToJson(saveData);
        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        File.WriteAllText(path, jsonData);

        Debug.Log("Save Complete.");
    }

    public void LoadPlayerData()
    {
        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        if (false == File.Exists(path))
        {
            Debug.Log("'PlayerData' doesn't not exist. Make a new file");
            Debug.Log($"File path : {path}");

            File.Create(path).Close();
            string jsonData = JsonUtility.ToJson(Playerdata);
            File.WriteAllText(path, jsonData);
        }
        else
        {
            Debug.Log($"Find Player File / File path : {path}");
            string jsonData = File.ReadAllText(path);
            Playerdata = JsonUtility.FromJson<Data.PlayerStat>(jsonData);
        }

        if (null == Playerdata)
            Debug.Log("playerdata is missing");
    }

    public void DeletePlayerData()
    {
        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        if (false == File.Exists(path))
        {
            Debug.Log($"'PlayerData' doesn't not exist. : {path}");
            return;
        }
        else
        {
            Debug.Log("Delete Player File");
            File.Delete(path);
        }
    }

    public bool CheckSaveFile()
    {
        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        if (true == File.Exists(path))
            return true;

        return false;
    }
}
