using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;

namespace KeyforgeDota;

public partial class HotkeyCaptureBox : UserControl
{
    public static readonly StyledProperty<string> HotkeyProperty =
        AvaloniaProperty.Register<HotkeyCaptureBox, string>(nameof(Hotkey), defaultValue: "");

    public string Hotkey
    {
        get => GetValue(HotkeyProperty);
        set => SetValue(HotkeyProperty, value);
    }

    public HotkeyCaptureBox()
    {
        InitializeComponent();
        this.AddHandler(KeyDownEvent, OnKeyDown, handledEventsToo: true);
        this.PointerPressed += OnPointerPressed;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private bool _capturing = false;
    private HashSet<Key> _pressedKeys = new();

    // Atualiza o visual quando a propriedade Hotkey muda externamente (ex: binding)
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == HotkeyProperty && !_capturing)
            UpdateVisuals();
    }

    // Garante display correto ao ser inserido na árvore visual pelo ItemsControl
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        var textBlock = this.FindControl<TextBlock>("HotkeyText");
        var border = this.FindControl<Border>("MainBorder");
        bool hasHotkey = !string.IsNullOrWhiteSpace(Hotkey);
        if (textBlock != null)
        {
            textBlock.Text = hasHotkey ? Hotkey : "Pressione...";
            textBlock.Foreground = Avalonia.Media.Brushes.White;
        }
        if (border != null)
            border.Background = hasHotkey ? Avalonia.Media.Brushes.ForestGreen : Avalonia.Media.Brushes.DimGray;
    }

    private void OnPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        _capturing = true;
        _pressedKeys.Clear();
        var textBlock = this.FindControl<TextBlock>("HotkeyText");
        var border = this.FindControl<Border>("MainBorder");
        if (textBlock != null)
        {
            textBlock.Text = "Aguardando...";
            textBlock.Foreground = Avalonia.Media.Brushes.Gold;
        }
        if (border != null)
            border.Background = Avalonia.Media.Brushes.DimGray;
        Focus();
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (!_capturing) return;
        _pressedKeys.Add(e.Key);
        Hotkey = FormatHotkey(_pressedKeys);
        var textBlock = this.FindControl<TextBlock>("HotkeyText");
        var border = this.FindControl<Border>("MainBorder");
        if (textBlock != null)
        {
            textBlock.Text = Hotkey;
            textBlock.Foreground = Avalonia.Media.Brushes.White;
        }
        if (border != null)
            border.Background = Avalonia.Media.Brushes.ForestGreen;
        e.Handled = true;
        if (e.Key == Key.Enter || e.Key == Key.Tab)
        {
            _capturing = false;
            _pressedKeys.Clear();
        }
    }

    private string FormatHotkey(HashSet<Key> keys)
    {
        var list = new List<string>();
        if (keys.Contains(Key.Space)) list.Add("space");
        if (keys.Contains(Key.LeftCtrl) || keys.Contains(Key.RightCtrl)) list.Add("ctrl");
        if (keys.Contains(Key.LeftAlt) || keys.Contains(Key.RightAlt)) list.Add("alt");
        if (keys.Contains(Key.LeftShift) || keys.Contains(Key.RightShift)) list.Add("shift");
        foreach (var k in keys)
        {
            if (k >= Key.A && k <= Key.Z) list.Add(k.ToString().ToLower());
            if (k >= Key.D0 && k <= Key.D9) list.Add(((char)('0' + (k - Key.D0))).ToString());
        }
        return string.Join("+", list);
    }
}
