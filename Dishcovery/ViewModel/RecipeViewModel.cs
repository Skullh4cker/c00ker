using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Dishcovery.ViewModel;

public partial class RecipesViewModel : BaseViewModel
{
    RecipeService recipeService;
    public ObservableCollection<Recipe> Recipes { get; } = new();

    public RecipesViewModel(RecipeService recipeService)
    {
        this.recipeService = recipeService;
    }

    [RelayCommand]
    public async Task GoToDetailsAsync(Recipe recipe)
    {
        if (recipe is null)
            return;

        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true,
            new Dictionary<string, object>
            {
                {"Recipe", recipe}
            });
    }

    public async Task GetRecipesAsync(string str)
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var recipes = await recipeService.GetRecipes();

            if (Recipes.Count != 0)
                Recipes.Clear();
            var validRecipes = Search.GetValidRecipes(str, recipes);

            foreach (var validRecipe in validRecipes)
            {
                Recipes.Add(validRecipe);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Something wrong, i can feel it", ex.Message, "Ok");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
