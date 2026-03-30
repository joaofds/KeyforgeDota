using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Layout;

namespace KeyforgeDota;

public partial class ShortcutConfigWindow : Window
{
    public ObservableCollection<AbilityShortcut> Shortcuts { get; set; }
    private AppConfig _config;

    public delegate void ConfigSavedHandler();
    public event ConfigSavedHandler? OnConfigSaved;

    public ShortcutConfigWindow(AppConfig config)
    {
        InitializeComponent();
        _config = config;
        Shortcuts = new ObservableCollection<AbilityShortcut>(
            config.KeyCombos.Select(kv => new AbilityShortcut
            {
                Ability = kv.Key,
                Shortcut1 = kv.Value.Count > 0 ? kv.Value[0] : string.Empty,
                Shortcut2 = kv.Value.Count > 1 ? kv.Value[1] : string.Empty
            })
        );
        DataContext = this;
        var stack = this.FindControl<StackPanel>("ShortcutStack")!;
        stack.Children.Clear();
        foreach (var shortcut in Shortcuts)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 16, Margin = new Avalonia.Thickness(0, 0, 0, 8) };
            row.Children.Add(new TextBlock
            {
                Text = shortcut.Ability,
                Width = 140,
                Foreground = Avalonia.Media.Brushes.White,
                FontWeight = Avalonia.Media.FontWeight.SemiBold,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
            });
            var hotkey1 = new HotkeyCaptureBox { Width = 120 };
            hotkey1.Hotkey = shortcut.Shortcut1;
            hotkey1.PropertyChanged += (s, e) => { if (e.Property == HotkeyCaptureBox.HotkeyProperty) shortcut.Shortcut1 = hotkey1.Hotkey; };
            row.Children.Add(hotkey1);
            var hotkey2 = new HotkeyCaptureBox { Width = 120 };
            hotkey2.Hotkey = shortcut.Shortcut2;
            hotkey2.PropertyChanged += (s, e) => { if (e.Property == HotkeyCaptureBox.HotkeyProperty) shortcut.Shortcut2 = hotkey2.Hotkey; };
            row.Children.Add(hotkey2);
            stack.Children.Add(row);
        }
        var btnSalvar = this.FindControl<Button>("BtnSalvar")!;
        btnSalvar.Click += BtnSalvar_Click;
        var btnCancelar = this.FindControl<Button>("BtnCancelar")!;
        btnCancelar.Click += (_, __) => Close();
        var btnFechar = this.FindControl<Button>("BtnFechar")!;
        btnFechar.Click += (_, __) => Close();

        // Ao criar cada HotkeyCaptureBox, garantir que o texto inicial reflete o atalho salvo
        foreach (var child in stack.Children)
        {
            if (child is StackPanel row)
            {
                foreach (var ctrl in row.Children)
                {
                    if (ctrl is HotkeyCaptureBox hotkeyBox)
                    {
                        var textBlock = hotkeyBox.FindControl<TextBlock>("HotkeyText");
                        if (textBlock != null)
                        {
                            textBlock.Text = string.IsNullOrWhiteSpace(hotkeyBox.Hotkey) ? "Pressione..." : hotkeyBox.Hotkey;
                            textBlock.Foreground = Avalonia.Media.Brushes.White;
                        }
                        var border = hotkeyBox.FindControl<Border>("MainBorder");
                        if (border != null)
                            border.Background = string.IsNullOrWhiteSpace(hotkeyBox.Hotkey) ? Avalonia.Media.Brushes.DimGray : Avalonia.Media.Brushes.ForestGreen;
                    }
                }
            }
        }
    }

    private void AtualizarListaAtalhosSalvos()
    {
        // Método agora não faz nada (mantido para compatibilidade)
    }

    private void BtnSalvar_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            foreach (var s in Shortcuts)
            {
                _config.KeyCombos[s.Ability] = new System.Collections.Generic.List<string>
                {
                    s.Shortcut1,
                    s.Shortcut2
                }.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            }
            _config.Save();
            AtualizarListaAtalhosSalvos();
            // Feedback popup de sucesso
            _ = AlertPopup.Show(this, "Configurações salvas com sucesso!", true);
            OnConfigSaved?.Invoke();
        }
        catch (System.Exception ex)
        {
            // Feedback popup de erro
            _ = AlertPopup.Show(this, $"Erro ao salvar: {ex.Message}", false);
        }
    }
}

public class AbilityShortcut
{
    public string Ability { get; set; } = string.Empty;
    public string Shortcut1 { get; set; } = string.Empty;
    public string Shortcut2 { get; set; } = string.Empty;
}
