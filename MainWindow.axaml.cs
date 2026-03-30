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

    public MainWindow()
    {
        InitializeComponent();
        Config = AppConfig.Load();
        BuildComboToAbilityMap();
        AttachControls();
        _comboRunner = new ComboRunner(Config);
        AttachEvents();
        LoadConfigToUI();

        // Inicia hook global de teclado (WinAPI)
        _keyboardHook = new KeyboardHookWin();
        _keyboardHook.OnComboPressed += KeyboardHook_OnComboPressed;
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _keyboardHook?.Dispose();
    }

    // Handler para combos globais
    private void KeyboardHook_OnComboPressed(HashSet<int> pressedKeys)
    {
        var combo = KeysToComboString(pressedKeys);
        var ability = GetAbilityForCombo(combo);
        if (ability != null)
        {
            Dispatcher.UIThread.Post(async () =>
            {
                SetStatus($"Combo detectado: {combo} → {ability}");
                await DispararHabilidade(ability);
            });
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
        WexKeyBox!.LostFocus += (_, __) => SaveConfigFromUI();
        ExortKeyBox!.LostFocus += (_, __) => SaveConfigFromUI();
        InvokeKeyBox!.LostFocus += (_, __) => SaveConfigFromUI();
    }

    private void AttachEvents()
    {
        this.FindControl<Button>("ColdSnapBtn").Click += async (_, __) => await DispararHabilidade("coldsnap");
        this.FindControl<Button>("EMPBtn").Click += async (_, __) => await DispararHabilidade("emp");
        this.FindControl<Button>("SunStrikeBtn").Click += async (_, __) => await DispararHabilidade("sunstrike");
        this.FindControl<Button>("TornadoBtn").Click += async (_, __) => await DispararHabilidade("tornado");
        this.FindControl<Button>("ChaosMeteorBtn").Click += async (_, __) => await DispararHabilidade("chaosmeteor");
        this.FindControl<Button>("DeafeningBlastBtn").Click += async (_, __) => await DispararHabilidade("deafeningblast");
        this.FindControl<Button>("IceWallBtn").Click += async (_, __) => await DispararHabilidade("icewall");
        this.FindControl<Button>("GhostWalkBtn").Click += async (_, __) => await DispararHabilidade("ghostwalk");
        this.FindControl<Button>("PanicGhostWalkBtn").Click += async (_, __) => await DispararHabilidade("panicghostwalk");
        this.FindControl<Button>("AlacrityBtn").Click += async (_, __) => await DispararHabilidade("alacrity");
        this.FindControl<Button>("ForgeSpiritBtn").Click += async (_, __) => await DispararHabilidade("forgespirit");
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

    private async Task RunComboAsync(Func<IntPtr, Task> comboFunc)
    {
        UpdateConfigFromUI();
        SetStatus("Buscando janela...");
        var hWnd = FindWindowByTitle(Config.WindowName);
        if (hWnd == IntPtr.Zero)
        {
            SetStatus($"Janela '{Config.WindowName}' não encontrada", error: true);
            return;
        }
        // Tenta obter o título real da janela encontrada
        string foundTitle = GetWindowTitle(hWnd);
        SetStatus($"Janela encontrada: '{foundTitle}' (hWnd: 0x{hWnd.ToInt64():X})");
        await Task.Delay(500);
        SetStatus("Executando combo...");
        await comboFunc(hWnd);
        SetStatus("Pronto");
    }
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

    private string GetWindowTitle(IntPtr hWnd)
    {
        var sb = new System.Text.StringBuilder(256);
        GetWindowText(hWnd, sb, sb.Capacity);
        return sb.ToString();
    }

    private void SetStatus(string text, bool error = false)
    {
        if (StatusText != null)
        {
            StatusText.Text = text;
            StatusText.Foreground = error ? Avalonia.Media.Brushes.OrangeRed : Avalonia.Media.Brushes.LimeGreen;
        }
    }

    // Toda a lógica de combos foi movida para ComboRunner

    // ========== WIN32 API ==========
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

    [DllImport("user32.dll")]
    private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    private const uint WM_KEYDOWN = 0x0100;
    private const uint WM_KEYUP = 0x0101;

    private IntPtr FindWindowByTitle(string title)
    {
        return FindWindow(null, title);
    }

    private void SendKeyToWindow(IntPtr hWnd, string key)
    {
        var vk = KeyToVirtualKey(key);
        if (vk == 0) return;
        PostMessage(hWnd, WM_KEYDOWN, (IntPtr)vk, IntPtr.Zero);
        PostMessage(hWnd, WM_KEYUP, (IntPtr)vk, IntPtr.Zero);
    }

    private int KeyToVirtualKey(string key)
    {
        // Suporte básico para letras e números
        if (string.IsNullOrEmpty(key)) return 0;
        char c = char.ToLowerInvariant(key[0]);
        if (c >= 'a' && c <= 'z') return c - 'a' + 0x41;
        if (c >= '0' && c <= '9') return c - '0' + 0x30;
        // Adicione mais teclas se necessário
        return 0;
    }
}