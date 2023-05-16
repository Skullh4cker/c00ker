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
    RequestFilter RequestFilter { get; set; } = new RequestFilter();
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
        AddIngredientCommand = new Command(async () =>
        {
            try
            {
                if (CheckIngredient())
                {
                    var ing = Ingredients.ToList().Find(x => x.Name.ToLower() == IngredientName.ToLower());

                    if (ing.Name.ToLower() == IngredientName.ToLower() && (IngredientQuantity != "" && IngredientQuantity != null) && (IngredientMeasurement != "" && IngredientMeasurement != null))
                    {
                        var ingr = new IngredientView(ing.Name, IngredientImportance, double.Parse(IngredientQuantity), IngredientMeasurement, ing.ID, ing.GramsInPce, ing.GramsInCup);
                        Requests.AvailableIngredients.Add(ingr);
                        RequestFilter.RequiredIngredients.Add(ingr);
                    }
                    else if (ing.Name.ToLower() == IngredientName.ToLower())
                    {
                        var ingr = new IngredientView(ing.Name, IngredientImportance, ing.ID, ing.GramsInPce, ing.GramsInCup);
                        Requests.AvailableIngredients.Add(ingr);
                        RequestFilter.RequiredIngredients.Add(ingr);
                    }
                }
                IngredientQuantity = "";
                IngredientName = "";
                IngredientMeasurement = "";
                IngredientMeasurements.Clear();
                IngredientsHints.Clear();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", ex.Message, "Ok");
            }
        });

        BanIngredientCommand = new Command(async () =>
        {
            try
            {
                var ing = Ingredients.ToList().Find(x => x.Name.ToLower() == BannedIngredientName.ToLower());
                if (ing.Name.ToLower() == BannedIngredientName.ToLower())
                {
                    Requests.ProhibitedIngredients.Add(new IngredientView(ing.Name, ing.ID));
                    RequestFilter.IgnoredIngredients.Add(new IngredientView(ing.Name, ing.ID));
                }
                BannedIngredientName = "";
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Something wrong, i can feel it", ex.Message, "Ok");
            }
        });
        AddTagCommand = new Command(async () =>
        {
            try
            {
                var tag = Tags.ToList().Find(x => x.Name.ToLower() == TagName.ToLower());
                if (tag.Name.ToLower() == TagName.ToLower())
                {
                    Requests.DesiredTags.Add(new Tag(tag.Name));
                    RequestFilter.WantedTags.Add(new Tag(tag.Name));
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Something wrong, i can feel it", ex.Message, "Ok");
            }
            TagName = "";
        });
        AddProhibitedTagCommand = new Command(async () =>
        {
            try
            {
                var tag = Tags.ToList().Find(x => x.Name.ToLower() == TagName.ToLower());
                if (tag.Name.ToLower() == TagName.ToLower())
                {
                    Requests.ProhibitedTags.Add(new Tag(tag.Name));
                    RequestFilter.UnwantedTags.Add(new Tag(tag.Name));
                }
                TagName = "";
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Something wrong, i can feel it", ex.Message, "Ok");
            }
        });
        ClearAllTabsCommand = new Command(() =>
        {
            Requests.AvailableIngredients.Clear();
            Requests.ProhibitedIngredients.Clear();
            Requests.DesiredTags.Clear();
            Requests.ProhibitedTags.Clear();
            RequestFilter.RequiredIngredients.Clear();
            RequestFilter.IgnoredIngredients.Clear();
            RequestFilter.WantedTags.Clear();
            RequestFilter.UnwantedTags.Clear();
        });
        DeleteIngredientTabCommand = new Command((object obj) =>
        {
            var content = obj as IngredientView;
            Requests.AvailableIngredients.Remove(content);
            RequestFilter.RequiredIngredients.Remove(RequestFilter.RequiredIngredients.Find(x => x.ID == content.ID));
        });
        DeleteProhibitedIngredientTabCommand = new Command((object obj) =>
        {
            var content = obj as IngredientView;
            Requests.ProhibitedIngredients.Remove(content);
            RequestFilter.IgnoredIngredients.Remove(RequestFilter.IgnoredIngredients.Find(x => x.ID == content.ID));
        });
        DeleteTagTabCommand = new Command((object obj) =>
        {
            var content = obj as Tag;
            Requests.DesiredTags.Remove(content);
            RequestFilter.WantedTags.Remove(RequestFilter.WantedTags.Find(x => x.Name == content.Name));
        });
        DeleteProhibitedTagTabCommand = new Command((object obj) =>
        {
            var content = obj as Tag;
            Requests.ProhibitedTags.Remove(content);
            RequestFilter.UnwantedTags.Remove(RequestFilter.UnwantedTags.Find(x => x.Name == content.Name));
        });
        SearchCommand = new Command(async () =>
        {
            if(IsBusy) return;
            try
            {
                IsBusy = true;
                var test = await recipeService.GetRecipes();
                if(Recipes.Count != 0)
                    Recipes.Clear();

                test = await Search.ApplyRequestFilter(RequestFilter, test);
                foreach (var item in test)
                {
                    if(Recipes.Count<40)
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
    public async Task GetRecipesAsync(string str)
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var recipes = await recipeService.GetRecipes();

            if (Recipes.Count != 0)
                Recipes.Clear();
            //var validRecipes = Search.GetValidRecipes(str, recipes);
            //foreach (var validRecipe in validRecipes)
            //{
            //    Recipes.Add(validRecipe);
            //}
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
        if (Requests.AvailableIngredients.Any(x => x.Name.ToLower() == str.ToLower()) || Requests.ProhibitedIngredients.Any(x => x.Name.ToLower() == str.ToLower()))
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
        if (Requests.DesiredTags.Any(x => x.Name.ToLower() == str.ToLower()) || Requests.ProhibitedTags.Any(x => x.Name.ToLower() == str.ToLower()))
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
        if(searchedName != null)
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
