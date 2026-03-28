using System;
using System.Threading.Tasks;

namespace KeyforgeDota
{
    public static class KeySender
    {
        public static Task SendKey(IntPtr hWnd, string key, Action<string> log, Action<string> console, Action<IntPtr, string, ushort> sendKeyToWindow)
        {
            if (string.IsNullOrWhiteSpace(key)) return Task.CompletedTask;
            console($"{key.ToUpper()} ");
            log($"[Disparar] KeyDown para '{key}'");
            sendKeyToWindow(hWnd, key, 0); // KeyDown
            System.Threading.Thread.Sleep(10);
            log($"[Disparar] KeyUp para '{key}'");
            sendKeyToWindow(hWnd, key, 1); // KeyUp
            System.Threading.Thread.Sleep(10);
            return Task.CompletedTask;
        }
    }
}
