using Dishcovery.Controls;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;
namespace Dishcovery;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("NunitoSans-Regular.ttf", "NunitoSansRegular");
				fonts.AddFont("NunitoSans-Bold.ttf", "NunitoSansBold");
				fonts.AddFont("NunitoSans-ExtraBold.ttf", "NunitoSansExtraBold");
				fonts.AddFont("NunitoSans-SemiBold.ttf", "NunitoSansSemiBold");
			})
			.ConfigureLifecycleEvents(events =>
			{
#if ANDROID
				events.AddAndroid(android => android.OnCreate((activity, bundle) => MakeStatusBarTranslucent(activity)));

				static void MakeStatusBarTranslucent(Android.App.Activity activity)
				{
					activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
				}
#endif
			})
			.ConfigureMauiHandlers(handlers =>
			{
				handlers.AddHandler(typeof(CursorEntry), typeof(CursorEntryHandler));
			});

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("Cursor", (handler, view) =>
		{
			if (view is CursorEntry)
			{
#if ANDROID
				handler.PlatformView.Background=null;
				handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif IOS || MACCATALYST
				handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
				handler.PlatformView.Layer.BorderWidth = 0;
				handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
				handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
			}
		});

		builder.Services.AddSingleton<RecipeService>();
		builder.Services.AddSingleton<RecipesViewModel>();
		builder.Services.AddTransient<RecipeDetailViewModel>();
        builder.Services.AddSingleton<SearchPage>();
        builder.Services.AddTransient<DetailsPage>();
        return builder.Build();
	}
}
