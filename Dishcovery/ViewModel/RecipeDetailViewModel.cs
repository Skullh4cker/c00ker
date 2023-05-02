namespace Dishcovery.ViewModel;

[QueryProperty("Recipe", "Recipe")]
public partial class RecipeDetailViewModel : BaseViewModel
{
    public Recipe recipe;
    public Recipe Recipe
    {
        get => recipe;
        set
        {
            if (recipe == value)
                return;

            recipe = value;
            OnPropertyChanged();
        }
    }
    public RecipeDetailViewModel(){ }
}
