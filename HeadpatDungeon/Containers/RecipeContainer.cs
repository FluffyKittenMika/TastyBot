using HeadpatDungeon.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HeadpatDungeon.Contracts;

namespace HeadpatDungeon.Containers
{
    public class RecipeContainer : IRecipeContainer
    {
        private Dictionary<string, Recipe> Recipes = null; //The Crafting lookup

        public Dictionary<string, Recipe> GetRecipesList()
        {
            return Recipes;
        }

        public void LoadRecipes()
        {
            Recipes = new Dictionary<string, Recipe>(StringComparer.OrdinalIgnoreCase);

            LoadItemRecepies(@".\HpDataFiles\crafting_smelting.json", ref Recipes);
        }

        private void LoadItemRecepies(string path, ref Dictionary<string, Recipe> dict)
        {
            string json = File.ReadAllText(path);
            dict = JsonConvert.DeserializeObject<Dictionary<string, Recipe>>(json);
        }
    }
}
