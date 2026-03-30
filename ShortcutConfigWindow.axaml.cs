using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.ObjectModel;
using System.Linq;

namespace KeyforgeDota;

public partial class ShortcutConfigWindow : Window
{
    public ObservableCollection<AbilityShortcut> Shortcuts { get; set; }
    private AppConfig _config;

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
        var grid = this.FindControl<DataGrid>("ShortcutGrid");
        grid.ItemsSource = Shortcuts;
        // Adiciona coluna de habilidade via código para evitar erro de binding compilado
        grid.Columns.Insert(0, new Avalonia.Controls.DataGridTextColumn {
            Header = "Habilidade",
            Binding = new Avalonia.Data.Binding("Ability"),
            IsReadOnly = true,
            Width = new Avalonia.Controls.DataGridLength(1, Avalonia.Controls.DataGridLengthUnitType.Star)
        });
        var btnSalvar = this.FindControl<Button>("BtnSalvar");
        btnSalvar.Click += BtnSalvar_Click;
        var btnCancelar = this.FindControl<Button>("BtnCancelar");
        btnCancelar.Click += (_, __) => Close();
    }

    private void BtnSalvar_Click(object? sender, RoutedEventArgs e)
    {
        // Atualiza config com os atalhos editados
        foreach (var s in Shortcuts)
        {
            _config.KeyCombos[s.Ability] = new System.Collections.Generic.List<string>
            {
                s.Shortcut1,
                s.Shortcut2
            }.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        }
        _config.Save();
        Close();
    }
}

public class AbilityShortcut
{
    public string Ability { get; set; } = string.Empty;
    public string Shortcut1 { get; set; } = string.Empty;
    public string Shortcut2 { get; set; } = string.Empty;
}
