using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using NewLife.SyncDB.Ava.Views;

namespace NewLife.SyncDB.Ava;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var flTheme = new FluentTheme();
            
        flTheme.Palettes[ThemeVariant.Dark] = FluentColorPalettes.DarkPalettes["Forest"];
        flTheme.Palettes[ThemeVariant.Light] = FluentColorPalettes.LightPalettes["Forest"];
            
        App.Current.Styles.Add(flTheme);
            
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}