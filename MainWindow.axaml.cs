using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Runtime.InteropServices;
using System;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace KeyforgeDota;

public partial class MainWindow : Window
{
    // Configuração dinâmica das teclas
    public string WindowName { get; set; } = "Dota 2";
    public string QuasKey { get; set; } = "q";
    public string WexKey { get; set; } = "w";
    public string ExortKey { get; set; } = "e";
    public string InvokeKey { get; set; } = "r";
    public string FirstSpellKey { get; set; } = "d";
    public string SecondSpellKey { get; set; } = "f";

    // Os campos dos controles são gerados automaticamente pelo Avalonia

    public MainWindow()
    {
        InitializeComponent();
        AttachControls();
        AttachEvents();
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
    }

    private void AttachEvents()
    {
        this.FindControl<Button>("ColdSnapBtn").Click += async (_, __) => await RunComboAsync(CastColdSnap);
        this.FindControl<Button>("EMPBtn").Click += async (_, __) => await RunComboAsync(CastEMP);
        this.FindControl<Button>("SunStrikeBtn").Click += async (_, __) => await RunComboAsync(CastSunStrike);
        this.FindControl<Button>("TornadoBtn").Click += async (_, __) => await RunComboAsync(CastTornado);
        this.FindControl<Button>("ChaosMeteorBtn").Click += async (_, __) => await RunComboAsync(CastChaosMeteor);
        this.FindControl<Button>("DeafeningBlastBtn").Click += async (_, __) => await RunComboAsync(CastDeafeningBlast);
        this.FindControl<Button>("IceWallBtn").Click += async (_, __) => await RunComboAsync(CastIceWall);
        this.FindControl<Button>("GhostWalkBtn").Click += async (_, __) => await RunComboAsync(CastGhostWalk);
        this.FindControl<Button>("PanicGhostWalkBtn").Click += async (_, __) => await RunComboAsync(CastPanicGhostWalk);
        this.FindControl<Button>("AlacrityBtn").Click += async (_, __) => await RunComboAsync(CastAlacrity);
        this.FindControl<Button>("ForgeSpiritBtn").Click += async (_, __) => await RunComboAsync(CastForgeSpirit);
    }

    private void UpdateConfigFromUI()
    {
        if (WindowNameBox != null) WindowName = WindowNameBox.Text ?? "Dota 2";
        if (QuasKeyBox != null) QuasKey = QuasKeyBox.Text ?? "q";
        if (WexKeyBox != null) WexKey = WexKeyBox.Text ?? "w";
        if (ExortKeyBox != null) ExortKey = ExortKeyBox.Text ?? "e";
        if (InvokeKeyBox != null) InvokeKey = InvokeKeyBox.Text ?? "r";
        if (FirstSpellKeyBox != null) FirstSpellKey = FirstSpellKeyBox.Text ?? "d";
        if (SecondSpellKeyBox != null) SecondSpellKey = SecondSpellKeyBox.Text ?? "f";
    }

    private async Task RunComboAsync(Func<IntPtr, Task> comboFunc)
    {
        UpdateConfigFromUI();
        SetStatus("Buscando janela...");
        var hWnd = FindWindowByTitle(WindowName);
        if (hWnd == IntPtr.Zero)
        {
            SetStatus("Janela não encontrada", error: true);
            return;
        }
        SetStatus("Executando combo...");
        await comboFunc(hWnd);
        SetStatus("Pronto");
    }

    private void SetStatus(string text, bool error = false)
    {
        if (StatusText != null)
        {
            StatusText.Text = text;
            StatusText.Foreground = error ? Avalonia.Media.Brushes.OrangeRed : Avalonia.Media.Brushes.LimeGreen;
        }
    }

    // ========== COMBOS ==========
    private async Task CastColdSnap(IntPtr hWnd)
    {
        await AllQuas(hWnd);
        await SendKey(hWnd, InvokeKey);
        await AllWex(hWnd);
    }
    private async Task CastEMP(IntPtr hWnd)
    {
        await AllWex(hWnd);
        await SendKey(hWnd, InvokeKey);
        await AllWex(hWnd);
    }
    private async Task CastSunStrike(IntPtr hWnd)
    {
        await AllExort(hWnd);
        await SendKey(hWnd, InvokeKey);
        await AllWex(hWnd);
    }
    private async Task CastTornado(IntPtr hWnd)
    {
        await SendKey(hWnd, WexKey);
        await SendKey(hWnd, WexKey);
        await SendKey(hWnd, QuasKey);
        await SendKey(hWnd, InvokeKey);
        await AllWex(hWnd);
    }
    private async Task CastChaosMeteor(IntPtr hWnd)
    {
        await SendKey(hWnd, ExortKey);
        await SendKey(hWnd, ExortKey);
        await SendKey(hWnd, WexKey);
        await SendKey(hWnd, InvokeKey);
        await AllWex(hWnd);
    }
    private async Task CastDeafeningBlast(IntPtr hWnd)
    {
        await SendKey(hWnd, QuasKey);
        await SendKey(hWnd, WexKey);
        await SendKey(hWnd, ExortKey);
        await SendKey(hWnd, InvokeKey);
        await AllWex(hWnd);
    }
    private async Task CastIceWall(IntPtr hWnd)
    {
        await SendKey(hWnd, QuasKey);
        await SendKey(hWnd, QuasKey);
        await SendKey(hWnd, ExortKey);
        await SendKey(hWnd, InvokeKey);
        await AllWex(hWnd);
    }
    private async Task CastGhostWalk(IntPtr hWnd)
    {
        await SendKey(hWnd, QuasKey);
        await SendKey(hWnd, QuasKey);
        await SendKey(hWnd, WexKey);
        await SendKey(hWnd, InvokeKey);
        await AllWex(hWnd);
    }
    private async Task CastPanicGhostWalk(IntPtr hWnd)
    {
        await CastGhostWalk(hWnd);
        await SendKey(hWnd, FirstSpellKey);
    }
    private async Task CastAlacrity(IntPtr hWnd)
    {
        await SendKey(hWnd, WexKey);
        await SendKey(hWnd, WexKey);
        await SendKey(hWnd, ExortKey);
        await SendKey(hWnd, InvokeKey);
        await AllWex(hWnd);
    }
    private async Task CastForgeSpirit(IntPtr hWnd)
    {
        await SendKey(hWnd, ExortKey);
        await SendKey(hWnd, ExortKey);
        await SendKey(hWnd, QuasKey);
        await SendKey(hWnd, InvokeKey);
        await AllWex(hWnd);
    }

    // ========== SEQUÊNCIAS AUXILIARES ==========
    private async Task AllQuas(IntPtr hWnd)
    {
        await SendKey(hWnd, QuasKey);
        await SendKey(hWnd, QuasKey);
        await SendKey(hWnd, QuasKey);
    }
    private async Task AllWex(IntPtr hWnd)
    {
        await SendKey(hWnd, WexKey);
        await SendKey(hWnd, WexKey);
        await SendKey(hWnd, WexKey);
    }
    private async Task AllExort(IntPtr hWnd)
    {
        await SendKey(hWnd, ExortKey);
        await SendKey(hWnd, ExortKey);
        await SendKey(hWnd, ExortKey);
    }

    // ========== ENVIO DE TECLAS ==========
    private async Task SendKey(IntPtr hWnd, string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return;
        SendKeyToWindow(hWnd, key);
        await Task.Delay(20); // Pequeno delay entre teclas
    }

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