using HeadpatDungeon.Models;
using System.Collections.Generic;

namespace HeadpatDungeon.Contracts
{
    public interface IRecipeContainer
    {
        Dictionary<string, Recipe> GetRecipesList();
        void LoadRecipes();
    }
}