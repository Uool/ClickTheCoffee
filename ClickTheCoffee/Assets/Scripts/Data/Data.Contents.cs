using System;
using System.Collections.Generic;


namespace Data
{
    [Serializable]
    public class Stat
    {
        public string engName;  // 이걸 키값으로
        public string korName;
        public string type;
        public int imageNumber;
        public int cost;
        public int level;
        public float totalAmount;
        public bool isLocked;
    }

    [Serializable]
    public class Upgrade
    {
        public string engUpgradeName;  // 이걸 키값으로
        public string korUpgradeName;
        public string info;
        public int cost;
        public int totalLevel;
    }

    [Serializable]
    public class Stuff
    {
        public string engName;  // key
        public string korName;
        public int cost;        // unlock cost
        public int stuffID;
        public List<float> color = new List<float>();
        public bool isLocked;
    }

    [Serializable]
    public class Recipe
    {
        public string engName;  // Key
        public string korName;
        public List<string> stuffList = new List<string>();
        public List<float> amountList = new List<float>();
        public int drinkID;
        public string info = "";
        public int cost;
    }

    [Serializable]
    public class PlayerStat
    {
        public int money = 5000;
        public int day = 1;
        public int menu_coffeeLevel = 1;
        public int menu_TeaLevel = 1;
        public int menu_JuiceLevel = 1;
        public int speed_CoffeeLevel = 0;
        public int speed_TeaLevel = 0;
        public int speed_JuiceLevel = 0;
        public int popLevel = 1;
        public int customers = 0;
    }

    [Serializable]
    public class StuffData : ILoader<string, Data.Stuff>
    {
        public List<Data.Stuff> stuffs = new List<Data.Stuff>();
        public Dictionary<string, Data.Stuff> MakeDict()
        {
            Dictionary<string, Data.Stuff> dict = new Dictionary<string, Data.Stuff>();
            foreach (Data.Stuff stuff in stuffs)
                dict.Add(stuff.engName, stuff);

            return dict;
        }
    }
    public class RecipeData : ILoader<int, Data.Recipe>
    {
        public List<Data.Recipe> recipes = new List<Data.Recipe>();
        public Dictionary<int, Data.Recipe> MakeDict()
        {
            Dictionary<int, Data.Recipe> dict = new Dictionary<int, Data.Recipe>();
            foreach (Data.Recipe recipe in recipes)
                dict.Add(recipe.drinkID, recipe);

            return dict;
        }
    }
}