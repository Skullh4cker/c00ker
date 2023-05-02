using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Dishcovery.Services;

public static class Search
{
    public static int GetIndexOf(string str, int from, List<Recipe> recipes)
    {
        for (int i = from; i < recipes.Count; ++i)
        {
            if (recipes[i].Name.ToLower().Contains(str.ToLower()))
                return i;
        }
        return -1;
    }

    public static List<Recipe> GetValidRecipes(string query, List<Recipe> Recipes)
    {
        List<Recipe> validRecipes = new List<Recipe>();
        int i = -1;
        bool exists = true;
        while (exists)
        {
            i = Search.GetIndexOf(query, i + 1, Recipes);
            if (i == -1)
                exists = false;
            else
                validRecipes.Add(Recipes[i]);
        }
        return validRecipes;
    }
}
