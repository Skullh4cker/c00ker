namespace Dishcovery;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(RecipeDetailViewModel recipeViewModel)
	{
		InitializeComponent();
		BindingContext = recipeViewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}