using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Threading.Tasks;

namespace KeyforgeDota;

public static class AlertPopup
{
    public static async Task Show(Window owner, string message, bool success = true, int durationMs = 1800)
    {
        var popup = new Window
        {
            Width = 320,
            Height = 60,
            CanResize = false,
            ShowInTaskbar = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Background = success ? Brushes.DarkGreen : Brushes.DarkRed,
            Topmost = true,
            SystemDecorations = SystemDecorations.None,
            Content = new Border
            {
                Background = Brushes.Transparent,
                Padding = new Avalonia.Thickness(0),
                Child = new TextBlock
                {
                    Text = message,
                    Foreground = Brushes.White,
                    FontWeight = FontWeight.Bold,
                    FontSize = 18,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    TextAlignment = Avalonia.Media.TextAlignment.Center
                }
            }
        };
        popup.Show(owner);
        await Task.Delay(durationMs);
        popup.Close();
    }
}
