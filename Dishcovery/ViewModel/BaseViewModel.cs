namespace Dishcovery.ViewModel;

public partial class BaseViewModel : INotifyPropertyChanged
{
    string ingredientName;
    string bannedIngredientName;
    string ingredientQuantity;
    string ingredientMeasurement;
    double ingredientImportance;
    string tagName;
    bool isBusy;
    string title;
    public bool IsBusy
    {
        get => isBusy;
        set
        {
            if (isBusy == value)
                return;

            isBusy = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(isNotBusy));
        }
    }
    public string IngredientName
    {
        get => ingredientName;
        set
        {
            if (ingredientName != value)
            {
                ingredientName = value;
                OnPropertyChanged();
            }
        }
    }
    public string BannedIngredientName
    {
        get => bannedIngredientName;
        set
        {
            if (bannedIngredientName != value)
            {
                bannedIngredientName = value;
                OnPropertyChanged();
            }
        }
    }
    public string IngredientQuantity
    {
        get => ingredientQuantity;
        set
        {
            if (ingredientQuantity != value)
            {
                ingredientQuantity = value;
                OnPropertyChanged();
            }
        }
    }
    public string IngredientMeasurement
    {
        get => ingredientMeasurement;
        set
        {
            if (ingredientMeasurement != value)
            {
                ingredientMeasurement = value;
                OnPropertyChanged();
            }
        }
    }
    public double IngredientImportance
    {
        get => ingredientImportance;
        set
        {
            if (ingredientImportance != value)
            {
                ingredientImportance = value;
                OnPropertyChanged();
            }
        }
    }
    public string TagName
    {
        get => tagName;
        set
        {
            if (tagName != value)
            {
                tagName = value;
                OnPropertyChanged();
            }
        }
    }
    public string Title
    {
        get => title;
        set
        {
            if (title == value)
                return;

            title = value;
            OnPropertyChanged();
        }
    }
    public bool isNotBusy => !isBusy;

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
