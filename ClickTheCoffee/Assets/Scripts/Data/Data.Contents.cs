using System;
using System.Collections.Generic;


namespace Data
{
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
        public string engName;
        public string korName;
        public List<string> korStuffList = new List<string>();
        public List<string> engStuffList = new List<string>();
        public List<float> amountList = new List<float>();
        public int drinkID;     // Key
        public string info = "";
        public int cost;
        public int unlockCost;
    }

    [Serializable]
    public class PlayerStat
    {
        public int money = 5000;
        public int day = 1;
        // 해금 이름만 있으면 해금되도록?
        public List<string> unlockStuffList = new List<string>();
        public List<string> unlockDrinkList = new List<string>();
        public List<string> buyingDrinkList = new List<string>();
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