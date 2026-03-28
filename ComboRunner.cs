using System;
using System.Threading.Tasks;

namespace KeyforgeDota
{
    public class ComboRunner
    {
        private readonly AppConfig _config;
        private readonly Action<string> _log;
        private readonly Action<string> _console;
        private readonly Action<IntPtr, string, ushort> _sendKeyToWindow;

        public ComboRunner(AppConfig config, Action<string> log, Action<string> console, Action<IntPtr, string, ushort> sendKeyToWindow)
        {
            _config = config;
            _log = log;
            _console = console;
            _sendKeyToWindow = sendKeyToWindow;
        }

        private Task SendKey(IntPtr hWnd, string key)
            => KeySender.SendKey(hWnd, key, _log, _console, _sendKeyToWindow);

        public async Task CastColdSnap(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.InvokeKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
        }
        public async Task CastEMP(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.InvokeKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
        }
        public async Task CastSunStrike(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.InvokeKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
        }
        public async Task CastTornado(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.InvokeKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
        }
        public async Task CastChaosMeteor(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.InvokeKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
        }
        public async Task CastDeafeningBlast(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.InvokeKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
        }
        public async Task CastIceWall(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.InvokeKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
        }
        public async Task CastGhostWalk(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.InvokeKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
        }
        public async Task CastPanicGhostWalk(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.InvokeKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.FirstSpellKey);
        }
        public async Task CastAlacrity(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.InvokeKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
        }
        public async Task CastForgeSpirit(IntPtr hWnd)
        {
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.ExortKey);
            await SendKey(hWnd, _config.QuasKey);
            await SendKey(hWnd, _config.InvokeKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
            await SendKey(hWnd, _config.WexKey);
        }
    }
}
