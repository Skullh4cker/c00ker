using System.Windows.Input;

namespace Dishcovery.ViewModel;

public partial class IngredientFilterViewModel : BaseViewModel
{
    public ObservableCollection<Ingredient> Ingredients { get; } = new();

    public IngredientFilterViewModel()
    {

    }
    public void AddRecipe()
    { 

    }
}
