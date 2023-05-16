using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;
using System.Xml.Linq;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.NetworkInformation;

namespace Dishcovery.ViewModel;

public partial class RecipesViewModel : BaseViewModel
{
    public RequestFilter RequestFilter { get; set; } = new RequestFilter();
    RecipeService recipeService;
    //public DataIngredient SearchedIngredient { get; set; }
    public ObservableCollection<Recipe> Recipes { get; } = new();
    public ObservableCollection<DataIngredient> Ingredients { get; set; } = new();
    public ObservableCollection<DataIngredient> IngredientsHints { get; set; } = new();
    public ObservableCollection<Tag> Tags { get; set; } = new();
    public ObservableCollection<Tag> TagsHints { get; set; } = new();
    public ObservableCollection<string> IngredientMeasurements { get; } = new();
    public ICommand BanIngredientCommand { get; set; }
    public ICommand AddIngredientCommand { get; set; }
    public ICommand AddTagCommand { get; set; }
    public ICommand AddProhibitedTagCommand { get; set; }
    public ICommand DeleteIngredientTabCommand { get; set; }
    public ICommand DeleteProhibitedIngredientTabCommand { get; set; }
    public ICommand DeleteTagTabCommand { get; set; }
    public ICommand DeleteProhibitedTagTabCommand { get; set; }
    public ICommand ClearAllTabsCommand { get; set; }
    public ICommand SearchCommand { get; set; }
    public Request Requests { get; set; } = new Request();

    public RecipesViewModel(RecipeService recipeService)
    {
        this.recipeService = recipeService;
        GetIngredientsAsync();
        GetTagsAsync();
        AddIngredientCommand = new Command(() =>
        {
            if (CheckIngredient())
            {
                var ing = Ingredients.ToList().Find(x => x.Name.ToLower() == IngredientName.ToLower());

                if (ing.Name.ToLower() == IngredientName.ToLower() && (IngredientQuantity != "" && IngredientQuantity != null) && (IngredientMeasurement != "" && IngredientMeasurement != null))
                {
                    var ingr = new IngredientView(ing.Name, IngredientImportance, double.Parse(IngredientQuantity), IngredientMeasurement, ing.ID, ing.GramsInPce, ing.GramsInCup);
                    Requests.AvailableIngredients.Add(ingr);
                    RequestFilter.RequireIngredient(ingr);
                }
                else if (ing.Name.ToLower() == IngredientName.ToLower())
                {
                    var ingr = new IngredientView(ing.Name, IngredientImportance, ing.ID, ing.GramsInPce, ing.GramsInCup);
                    Requests.AvailableIngredients.Add(ingr);
                    RequestFilter.RequireIngredient(ingr);
                }
            }
            IngredientQuantity = "";
            IngredientName = "";
            IngredientMeasurement = "";
            IngredientMeasurements.Clear();
            IngredientsHints.Clear();
        });
        BanIngredientCommand = new Command(() =>
        {
            var ing = Ingredients.ToList().Find(x => x.Name.ToLower() == BannedIngredientName.ToLower());
            if (ing.Name.ToLower() == BannedIngredientName.ToLower())
            {
                RequestFilter.DisableIngredient(new IngredientView(ing.Name, ing.ID));
            }
            BannedIngredientName = "";
        });
        AddTagCommand = new Command(() =>
        {
            var tag = Tags.ToList().Find(x => x.Name.ToLower() == TagName.ToLower());
            if (tag.Name.ToLower() == TagName.ToLower())
            {
                RequestFilter.RequireTag(new Tag(tag.Name));
            }
            TagName = "";
        });
        AddProhibitedTagCommand = new Command(() =>
        {
            var tag = Tags.ToList().Find(x => x.Name.ToLower() == TagName.ToLower());
            if (tag.Name.ToLower() == TagName.ToLower())
            {
                RequestFilter.UnwantTag(new Tag(tag.Name));
            }
            TagName = "";
        });
        ClearAllTabsCommand = new Command(() =>
        {
            RequestFilter.ClearAll();
        });
        DeleteIngredientTabCommand = new Command((object obj) =>
        {
            var content = obj as IngredientView;
            RequestFilter.UnrequireIngredient(content);
        });
        DeleteProhibitedIngredientTabCommand = new Command((object obj) =>
        {
            var content = obj as IngredientView;
            RequestFilter.EnableIngredient(content);
        });
        DeleteTagTabCommand = new Command((object obj) =>
        {
            var content = obj as Tag;
            RequestFilter.RemoveWantedTag(content);
        });
        DeleteProhibitedTagTabCommand = new Command((object obj) =>
        {
            var content = obj as Tag;
            RequestFilter.RemoveUnwantedTag(content);
        });
        SearchCommand = new Command(async () =>
        {
            await GetRecipesAsync();
        });
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
    public async Task GetRecipesAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var allRecipes = await recipeService.GetRecipes();
            if (Recipes.Count != 0)
                Recipes.Clear();

            allRecipes = await Search.ApplyRequestFilter(RequestFilter, allRecipes);
            foreach (var item in allRecipes)
            {
                if (Recipes.Count < 40)
                    Recipes.Add(item);
            }
            IsBusy = false;
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
    public bool CheckIngredient()
    {
        if (CheckIngredientName(IngredientName))
        {
            if (!IngredientMeasurements.Contains("гр"))
                IngredientMeasurements.Add("гр");
            if(!IngredientMeasurements.Contains("кг"))
                IngredientMeasurements.Add("кг");
            var ing = Ingredients.ToList().Find(x => x.Name.ToLower() == IngredientName.ToLower());
            if (ing.Name.ToLower() == IngredientName.ToLower())
            {
                if (ing.GramsInPce != 0)
                {
                    if (!IngredientMeasurements.Contains("шт."))
                        IngredientMeasurements.Add("шт.");
                }
                if (ing.GramsInCup != 0)
                {
                    if (!IngredientMeasurements.Contains("мл"))
                        IngredientMeasurements.Add("мл");
                }
            }
            return true;
        }
        IngredientMeasurements.Clear();
        return false;
    }
    public bool CheckIngredientName(string str)
    {
        if (str == null)
        {
            str = "";
            return false;
        }
        if (RequestFilter.RequiredIngredients.Any(x => x.Name.ToLower() == str.ToLower()) || RequestFilter.IgnoredIngredients.Any(x => x.Name.ToLower() == str.ToLower()))
        {
            return false;
        }
        return Ingredients.Any(x => x.Name.ToLower() == str.ToLower());
    }
    public bool CheckTagName(string str)
    {
        if (str == null)
        {
            str = "";
            return false;
        }
        if (RequestFilter.WantedTags.Any(x => x.Name.ToLower() == str.ToLower()) || RequestFilter.UnwantedTags.Any(x => x.Name.ToLower() == str.ToLower()))
        {
            return false;
        }
        return Tags.Any(x => x.Name.ToLower() == str.ToLower());
    }
    public async void GetTagsAsync()
    {
        var tagsList = await recipeService.GetTags();
        foreach (var tag in tagsList)
        {
            Tags.Add(tag);
        }
    }
    public async void GetIngredientsAsync()
    {
        var ingList = await recipeService.GetIngredients();
        foreach (var ingr in ingList)
        {
            Ingredients.Add(ingr);
        }
    }
    public void GetTagHints(string str)
    {

        if (str != "" && Tags.Any(x => x.Name.ToLower().Contains(str.ToLower())))
        {
            TagsHints.Clear();
            var tags = Tags.ToList().FindAll(x => x.Name.ToLower().Contains(str.ToLower()));
            foreach (var tag in tags)
            {
                if (TagsHints.Count < 20)
                    TagsHints.Add(tag);
            }
        }
        else if (str == "" || !Tags.Any(x => x.Name.ToLower().Contains(str.ToLower())))
            TagsHints.Clear();
    }
    public void GetIngredientHints(string str)
    {
        if (str != "" && Ingredients.Any(x => x.Name.ToLower().Contains(str.ToLower())))
        {
            IngredientsHints.Clear();
            var test = Ingredients.ToList().FindAll(x => x.Name.ToLower().Contains(str.ToLower()));
            foreach (var ingr in test)
            {
                if (IngredientsHints.Count < 20)
                    IngredientsHints.Add(ingr);
            }
        }
        else if (str == "" || !Ingredients.Any(x => x.Name.ToLower().Contains(str.ToLower())))
            IngredientsHints.Clear();
    }
    public void ConfirmDesiredIngredient(string searchedName)
    {
            if (searchedName != null)
            {
                IngredientName = searchedName;
                IngredientsHints.Clear();
                CheckIngredient();
            }
    }
    public void ConfirmProhibitedIngredient(string searchedName)
    {
        if (searchedName != null)
        {
            BannedIngredientName = searchedName;
            IngredientsHints.Clear();
        }
    }
    public void ConfirmTag(string searchedName)
    {
        if (searchedName != null)
        {
            TagName = searchedName;
            TagsHints.Clear();
        }
    }
}
