using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace KeyforgeDota;

public partial class ShortcutConfigWindow : Window
{
    public ObservableCollection<AbilityShortcut> Shortcuts { get; set; }
    private readonly AppConfig _config;

    public delegate void ConfigSavedHandler();
    public event ConfigSavedHandler? OnConfigSaved;

    public ShortcutConfigWindow(AppConfig config)
    {
        _config = config;
        Shortcuts = new ObservableCollection<AbilityShortcut>(
            config.KeyCombos.Select(kv => new AbilityShortcut
            {
                Ability   = kv.Key,
                Shortcut1 = kv.Value.Count > 0 ? kv.Value[0] : string.Empty,
                Shortcut2 = kv.Value.Count > 1 ? kv.Value[1] : string.Empty
            })
        );
        DataContext = this;
        InitializeComponent();

        this.FindControl<Button>("BtnSalvar")!.Click   += BtnSalvar_Click;
        this.FindControl<Button>("BtnCancelar")!.Click += (_, _) => Close();
    }

    private void BtnSalvar_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            foreach (var s in Shortcuts)
            {
                _config.KeyCombos[s.Ability] = new List<string> { s.Shortcut1, s.Shortcut2 }
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();
            }
            _config.Save();
            _ = AlertPopup.Show(this, "Configurações salvas com sucesso!", true);
            OnConfigSaved?.Invoke();
        }
        catch (System.Exception ex)
        {
            _ = AlertPopup.Show(this, $"Erro ao salvar: {ex.Message}", false);
        }
    }
}

public class AbilityShortcut : INotifyPropertyChanged
{
    private static readonly Dictionary<string, string> DisplayNames = new()
    {
        { "coldsnap",      "Cold Snap"       },
        { "emp",           "EMP"             },
        { "sunstrike",     "Sun Strike"      },
        { "tornado",       "Tornado"         },
        { "chaosmeteor",   "Chaos Meteor"    },
        { "deafeningblast","Deafening Blast" },
        { "icewall",       "Ice Wall"        },
        { "ghostwalk",     "Ghost Walk"      },
        { "panicghostwalk","Panic Ghost Walk"},
        { "alacrity",      "Alacrity"        },
        { "forgespirit",   "Forge Spirit"    },
    };

    private string _shortcut1 = string.Empty;
    private string _shortcut2 = string.Empty;

    public string Ability { get; set; } = string.Empty;

    public string DisplayName =>
        DisplayNames.TryGetValue(Ability, out var name) ? name : Ability;

    public string Shortcut1
    {
        get => _shortcut1;
        set { _shortcut1 = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Shortcut1))); }
    }

    public string Shortcut2
    {
        get => _shortcut2;
        set { _shortcut2 = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Shortcut2))); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
