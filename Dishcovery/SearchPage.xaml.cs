namespace Dishcovery;

public partial class SearchPage : ContentPage
{
	public SearchPage(RecipesViewModel recipesViewModel)
	{
		InitializeComponent();
		BindingContext = recipesViewModel;
    }

    private async void SearchButton_Clicked(object sender, EventArgs e)
    {
        searchEntry.IsEnabled = false;
        searchEntry.IsEnabled = true;
        string request = searchEntry.Text;
        await ((RecipesViewModel)(this.BindingContext)).GetRecipesAsync(request);
    }
}