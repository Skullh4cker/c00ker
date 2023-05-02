using System.Net.Http.Json;

namespace Dishcovery.Services;

public class RecipeService
{
    HttpClient httpClient;

    public RecipeService()
    {
        httpClient = new HttpClient();
    }

    List<Recipe> recipeList = new List<Recipe>();

    public async Task<List<Recipe>> GetRecipes()
    {
        if (recipeList?.Count > 0)
            return recipeList;

        var url = "https://raw.githubusercontent.com/Skullh4cker/database/main/main1000menu.json";
        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            recipeList = await response.Content.ReadFromJsonAsync<List<Recipe>>();
        }
        return recipeList;
    }
}
