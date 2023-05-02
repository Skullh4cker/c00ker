namespace Dishcovery.ViewModel;

public partial class BaseViewModel : INotifyPropertyChanged
{
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
