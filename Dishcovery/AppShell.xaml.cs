namespace Dishcovery;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
        Init();
	}
    private void Init()
    {
        MainTabBar.CurrentItem = search;
    }
}
