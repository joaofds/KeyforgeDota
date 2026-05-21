using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia.Threading;
using System.Collections.Generic;

namespace KeyforgeDota;


public partial class MainWindow : Window
{
    private AppConfig Config;
    // Novo: Mapeamento reverso para busca rápida
    private Dictionary<string, string> ComboToAbility = new();
    private KeyboardHookWin? _keyboardHook;
    private bool _isEnabled = true; // Estado global do app

    public MainWindow()
    {
        InitializeComponent();
        Config = AppConfig.Load();
        BuildComboToAbilityMap();
        AttachControls();
        _comboRunner = new ComboRunner(Config);
        AttachEvents();
        LoadConfigToUI();

        if (AppConfig.LastLoadError != null)
            SetStatus($"Config corrompida, usando padrões. Veja config_error.log", error: true);

        // Botão de configuração de atalhos
        this.FindControl<Button>("OpenShortcutConfigBtn").Click += OpenShortcutConfigBtn_Click;

        // Inicia hook global de teclado (WinAPI)
        try
        {
            _keyboardHook = new KeyboardHookWin();
            _keyboardHook.OnComboPressed += KeyboardHook_OnComboPressed;
        }
        catch (InvalidOperationException ex)
        {
            SetStatus($"Hook de teclado falhou: {ex.Message}", error: true);
        }
    }

    private void OpenShortcutConfigBtn_Click(object? sender, RoutedEventArgs e)
    {
        var win = new ShortcutConfigWindow(Config);
        win.OnConfigSaved += () => {
            Config = AppConfig.Load();
            BuildComboToAbilityMap();
            _comboRunner = new ComboRunner(Config);
        };
        win.ShowDialog(this);
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _keyboardHook?.Dispose();
    }

    // Handler para combos globais

    private void KeyboardHook_OnComboPressed(HashSet<int> pressedKeys)
    {
        // VK_PAUSE = 0x13 = 19
        if (pressedKeys.Contains(0x13))
        {
            _isEnabled = !_isEnabled;
            SetStatus(_isEnabled ? "Atalhos ativados (Pause)" : "Atalhos desativados (Pause)", error: !_isEnabled);
            return;
        }
        if (!_isEnabled)
            return;
        var combo = KeysToComboString(pressedKeys);
        var ability = GetAbilityForCombo(combo);
        if (ability != null)
        {
            SetStatus($"Combo detectado: {combo} → {ability}");
            _ = DispararHabilidade(ability);
        }
    }

    // Converte conjunto de Keys para string de combinação (ex: "space+t")
    private string KeysToComboString(HashSet<int> keys)
    {
        var list = new List<string>();
        if (keys.Contains(0x20)) list.Add("space");
        if (keys.Contains(0x11)) list.Add("ctrl"); // VK_CONTROL
        if (keys.Contains(0x12)) list.Add("alt");  // VK_MENU
        if (keys.Contains(0x10)) list.Add("shift"); // VK_SHIFT
        // Letras A-Z
        for (int vk = 0x41; vk <= 0x5A; vk++)
            if (keys.Contains(vk)) list.Add(((char)vk).ToString().ToLower());
        // Números 0-9
        for (int vk = 0x30; vk <= 0x39; vk++)
            if (keys.Contains(vk)) list.Add(((char)vk).ToString());
        return string.Join("+", list);
    }

    private ComboRunner _comboRunner;

    // Dispara a habilidade correspondente delegando ao ComboRunner
    private async Task DispararHabilidade(string ability)
    {
        var hWnd = FindWindowByTitle(Config.WindowName);
        if (hWnd == IntPtr.Zero)
        {
            SetStatus($"Janela '{Config.WindowName}' não encontrada", error: true);
            return;
        }
        // _comboRunner já é instanciado no construtor

        switch (ability.ToLower())
        {
            case "tornado": await _comboRunner.CastTornado(hWnd); break;
            case "emp": await _comboRunner.CastEMP(hWnd); break;
            case "coldsnap": await _comboRunner.CastColdSnap(hWnd); break;
            case "sunstrike": await _comboRunner.CastSunStrike(hWnd); break;
            case "chaosmeteor": await _comboRunner.CastChaosMeteor(hWnd); break;
            case "deafeningblast": await _comboRunner.CastDeafeningBlast(hWnd); break;
            case "icewall": await _comboRunner.CastIceWall(hWnd); break;
            case "ghostwalk": await _comboRunner.CastGhostWalk(hWnd); break;
            case "panicghostwalk": await _comboRunner.CastPanicGhostWalk(hWnd); break;
            case "alacrity": await _comboRunner.CastAlacrity(hWnd); break;
            case "forgespirit": await _comboRunner.CastForgeSpirit(hWnd); break;
            default:
                SetStatus($"Habilidade '{ability}' não implementada.", error: true);
                break;
        }
    }

    // Novo: Monta o dicionário reverso para busca rápida
    private void BuildComboToAbilityMap()
    {
        ComboToAbility.Clear();
        foreach (var kv in Config.KeyCombos)
        {
            var ability = kv.Key;
            foreach (var combo in kv.Value)
            {
                var norm = NormalizeCombo(combo);
                if (!string.IsNullOrWhiteSpace(norm))
                    ComboToAbility[norm] = ability;
            }
        }
    }

    // Normaliza a string da combinação (ex: "space+t" -> "space+t")
    private string NormalizeCombo(string combo)
    {
        return string.Join("+", combo.ToLower().Split('+', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }

    // Exemplo de uso: dado uma combinação pressionada, retorna a habilidade
    private string? GetAbilityForCombo(string combo)
    {
        var norm = NormalizeCombo(combo);
        if (ComboToAbility.TryGetValue(norm, out var ability))
            return ability;
        return null;
    }

    private void AttachControls()
    {
        WindowNameBox = this.FindControl<TextBox>("WindowNameBox");
        QuasKeyBox = this.FindControl<TextBox>("QuasKeyBox");
        WexKeyBox = this.FindControl<TextBox>("WexKeyBox");
        ExortKeyBox = this.FindControl<TextBox>("ExortKeyBox");
        InvokeKeyBox = this.FindControl<TextBox>("InvokeKeyBox");
        FirstSpellKeyBox = this.FindControl<TextBox>("FirstSpellKeyBox");
        SecondSpellKeyBox = this.FindControl<TextBox>("SecondSpellKeyBox");
        StatusText = this.FindControl<TextBlock>("StatusText");

        // Eventos para salvar ao alterar
        WindowNameBox!.LostFocus += (_, __) => SaveConfigFromUI();
        QuasKeyBox!.LostFocus += (_, __) => SaveConfigFromUI();
        WexKeyBox!.LostFocus += (_, __) => SaveConfigFromUI();
        ExortKeyBox!.LostFocus += (_, __) => SaveConfigFromUI();
        InvokeKeyBox!.LostFocus += (_, __) => SaveConfigFromUI();
        FirstSpellKeyBox!.LostFocus += (_, __) => SaveConfigFromUI();
        SecondSpellKeyBox!.LostFocus += (_, __) => SaveConfigFromUI();
    }

    private void AttachEvents()
    {
        // Nenhum botão de habilidade para registrar eventos
    }

    private void UpdateConfigFromUI()
    {
        Config.FirstSpellKey = FirstSpellKeyBox?.Text ?? "d";
        Config.SecondSpellKey = SecondSpellKeyBox?.Text ?? "f";
        Config.WindowName = WindowNameBox?.Text ?? "Dota 2";
        Config.QuasKey = QuasKeyBox?.Text ?? "q";
        Config.WexKey = WexKeyBox?.Text ?? "w";
        Config.ExortKey = ExortKeyBox?.Text ?? "e";
        Config.InvokeKey = InvokeKeyBox?.Text ?? "r";
    }

    private void LoadConfigToUI()
    {
        if (FirstSpellKeyBox != null) FirstSpellKeyBox.Text = Config.FirstSpellKey;
        if (SecondSpellKeyBox != null) SecondSpellKeyBox.Text = Config.SecondSpellKey;
        if (WindowNameBox != null) WindowNameBox.Text = Config.WindowName;
        if (QuasKeyBox != null) QuasKeyBox.Text = Config.QuasKey;
        if (WexKeyBox != null) WexKeyBox.Text = Config.WexKey;
        if (ExortKeyBox != null) ExortKeyBox.Text = Config.ExortKey;
        if (InvokeKeyBox != null) InvokeKeyBox.Text = Config.InvokeKey;
    }

    private void SaveConfigFromUI()
    {
        UpdateConfigFromUI();
        Config.Save();
    }

    private void SetStatus(string text, bool error = false)
    {
        if (StatusText != null)
        {
            StatusText.Text = text;
            StatusText.Foreground = error ? Avalonia.Media.Brushes.OrangeRed : Avalonia.Media.Brushes.LimeGreen;
        }
    }

    // ========== WIN32 API ==========
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

    private IntPtr FindWindowByTitle(string title) => FindWindow(null, title);
}