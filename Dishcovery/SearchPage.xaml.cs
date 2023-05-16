using System.Data;
namespace Dishcovery;

public partial class SearchPage : ContentPage
{
    public SearchPage(RecipesViewModel recipesViewModel)
    {
        InitializeComponent();
        BindingContext = recipesViewModel;
        ImportanceSlider.Value = 0.8;
        CheckEntries();
        CheckTagEntry();
    }
    private void AddIngredientButton_Clicked(object sender, EventArgs e)
    {
        loveSearchEntry.IsEnabled = false; loveSearchEntry.IsEnabled = true;
        quantityEntry.IsEnabled = false; quantityEntry.IsEnabled = true;
        CheckEntries();
        //string request = loveSearchEntry.Text;
        //await ((RecipesViewModel)(this.BindingContext)).GetRecipesAsync(request);
    }
    private void BanIngredientButton_Clicked(object sender, EventArgs e)
    {
        hateIngredientEntry.IsEnabled = false; hateIngredientEntry.IsEnabled = true;
        CheckEntries();
    }
    private void AddTagButton_Clicked(object sender, EventArgs e)
    {
        tagEntry.IsEnabled = false; tagEntry.IsEnabled = true;
    }
    private void loveSearchEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        HintsCollectionView.IsVisible = true; HintsCollectionView2.IsVisible = false;
        ((RecipesViewModel)(this.BindingContext)).GetIngredientHints(loveSearchEntry.Text);
        CheckLoveSearchEntry();
    }
    private void hateIngredientEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        HintsCollectionView.IsVisible = false; HintsCollectionView2.IsVisible = true;
        ((RecipesViewModel)(this.BindingContext)).GetIngredientHints(hateIngredientEntry.Text);
        CheckHateIngredientEntry();
    }
    private void CursorEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        ((RecipesViewModel)(this.BindingContext)).GetTagHints(tagEntry.Text);
        CheckTagEntry();
    }
    private void CheckLoveSearchEntry()
    {
        bool checker = ((RecipesViewModel)(this.BindingContext)).CheckIngredient();
        AddIngredientButton.IsEnabled = checker;
    }
    private void CheckHateIngredientEntry()
    {
        bool checker = ((RecipesViewModel)(this.BindingContext)).CheckIngredientName(hateIngredientEntry.Text);
        BanIngredientButton.IsEnabled = checker;
    }
    private void CheckTagEntry()
    {
        bool checker = ((RecipesViewModel)(this.BindingContext)).CheckTagName(tagEntry.Text);
        AddTagButton.IsEnabled = checker;
        AddProhibitedTagButton.IsEnabled = checker;
    }
    private void ItemSelected(object sender, SelectionChangedEventArgs e)
    {
        string previous = (e.PreviousSelection.FirstOrDefault() as DataIngredient)?.Name;
        string current = (e.CurrentSelection.FirstOrDefault() as DataIngredient)?.Name;
        ((RecipesViewModel)(this.BindingContext)).ConfirmDesiredIngredient(current);
        HintsCollectionView.SelectedItem = previous;
    }

    private void HintsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string previous = (e.PreviousSelection.FirstOrDefault() as DataIngredient)?.Name;
        string current = (e.CurrentSelection.FirstOrDefault() as DataIngredient)?.Name;
        ((RecipesViewModel)(this.BindingContext)).ConfirmProhibitedIngredient(current);
        HintsCollectionView2.SelectedItem = previous;
    }

    private void TagsHintsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string previous = (e.PreviousSelection.FirstOrDefault() as Tag)?.Name;
        string current = (e.CurrentSelection.FirstOrDefault() as Tag)?.Name;
        ((RecipesViewModel)(this.BindingContext)).ConfirmTag(current);
        TagsHintsCollectionView.SelectedItem = previous;
    }
    private void CheckEntries()
    {
        CheckLoveSearchEntry();
        CheckHateIngredientEntry();
    }
}