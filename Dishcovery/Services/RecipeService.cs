using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dishcovery.Services;

public class RecipeService
{
    HttpClient httpClient;

    public RecipeService()
    {
        httpClient = new HttpClient();
    }

    List<Recipe> recipeList = new List<Recipe>();
    List<DataIngredient> ingredientList = new List<DataIngredient>();
    List<Tag> tagList = new List<Tag>();

    public async Task<List<Recipe>> GetRecipes()
    {
        if (recipeList?.Count > 0)
            return recipeList;

        string filename = "recipes1000menu[part 1].json";

        using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);
        using StreamReader reader = new StreamReader(fileStream);
        string jsonRecipes = await reader.ReadToEndAsync();
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonRecipes));
        var recipeList1 = await JsonSerializer.DeserializeAsync<List<Recipe>>(stream);

        filename = "recipes1000menu[part 2].json";

        using Stream fileStream2 = await FileSystem.Current.OpenAppPackageFileAsync(filename);
        using StreamReader reader2 = new StreamReader(fileStream2);
        jsonRecipes = await reader2.ReadToEndAsync();
        stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonRecipes));
        var recipeList2 = await JsonSerializer.DeserializeAsync<List<Recipe>>(stream);

        recipeList.AddRange(recipeList1);
        recipeList.AddRange(recipeList2);
        return recipeList;
    }
    public async Task<List<DataIngredient>> GetIngredients()
    {
        if (ingredientList?.Count > 0)
            return ingredientList;

        string filename = "ingredients1000menu.json";

        using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);
        using StreamReader reader = new StreamReader(fileStream);
        string jsonIngredients = await reader.ReadToEndAsync();
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonIngredients));
        ingredientList = await JsonSerializer.DeserializeAsync<List<DataIngredient>>(stream);
        return ingredientList;
    }
    public async Task<List<Tag>> GetTags()
    {
        if (tagList?.Count > 0)
            return tagList;

        string filename = "tagsNames.txt";

        using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);
        using StreamReader reader = new StreamReader(fileStream);
        while (!reader.EndOfStream)
        {
            tagList.Add(new Tag(await reader.ReadLineAsync()));
        }
        //string allTags = await reader.ReadToEndAsync();
        //var tags = allTags.Split('\r');
        //foreach (var tag in tags)
        //{
        //    tagList.Add(new Tag(tag));
        //}
        return tagList;
    }
}
