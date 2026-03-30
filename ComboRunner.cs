using System;
using System.Threading.Tasks;

namespace KeyforgeDota
{
    public class ComboRunner
    {
        private readonly AppConfig _config;

        public ComboRunner(AppConfig config)
        {
            _config = config;
        }

        // ========== COMBOS PRINCIPAIS ==========
        public async Task CastColdSnap(IntPtr hWnd)
        {
            await AllQuas(hWnd);
            await SendKey(hWnd, _config.InvokeKey);
            await AllWex(hWnd);
        }
        public async Task CastEMP(IntPtr hWnd)
        {
            await AllWex(hWnd);
            await SendKey(hWnd, _config.InvokeKey);
            await AllWex(hWnd);
        }
        public async Task CastSunStrike(IntPtr hWnd)
        {
            await AllExort(hWnd);
            await SendKey(hWnd, _config.InvokeKey);
            await AllWex(hWnd);
        }
        public async Task CastTornado(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.InvokeKey);
            await AllWex(hWnd);
        }
        public async Task CastChaosMeteor(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.InvokeKey);
            await AllWex(hWnd);
        }
        public async Task CastDeafeningBlast(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.InvokeKey);
            await AllWex(hWnd);
        }
        public async Task CastIceWall(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.InvokeKey);
            await AllWex(hWnd);
        }
        public async Task CastGhostWalk(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.InvokeKey);
            await AllWex(hWnd);
        }
        public async Task CastPanicGhostWalk(IntPtr hWnd)
        {
            await CastGhostWalk(hWnd);
            await SendKey(hWnd, _config.FirstSpellKey);
            await AllWex(hWnd);
        }
        public async Task CastAlacrity(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.InvokeKey);
            await AllWex(hWnd);
        }
        public async Task CastForgeSpirit(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.InvokeKey);
            await AllWex(hWnd);
        }

        // ========== SEQUÊNCIAS AUXILIARES ==========
        private async Task AllQuas(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.QuasKey);
        }
        private async Task AllWex(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
        }
        private async Task AllExort(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.ExortKey);
        }

        // ========== ENVIO DE TECLAS DIRETO (WIN32) ==========
        private async Task SendKey(IntPtr hWnd, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return;
            SendKeyToWindow(hWnd, key);
            await Task.Delay(20); // Pequeno delay entre teclas
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
            if (string.IsNullOrEmpty(key)) return 0;
            char c = char.ToLowerInvariant(key[0]);
            if (c >= 'a' && c <= 'z') return c - 'a' + 0x41;
            if (c >= '0' && c <= '9') return c - '0' + 0x30;
            return 0;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;
    }
}
