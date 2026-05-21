
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace KeyforgeDota;

public class AppConfig
{
    public string WindowName { get; set; } = "Dota 2";
    public string QuasKey { get; set; } = "q";
    public string WexKey { get; set; } = "w";
    public string ExortKey { get; set; } = "e";
    public string InvokeKey { get; set; } = "r";
    public string FirstSpellKey { get; set; } = "d";
    public string SecondSpellKey { get; set; } = "f";

    public Dictionary<string, List<string>> KeyCombos { get; set; } = new();

    [JsonIgnore]
    public static readonly string[] DefaultAbilities =
    {
        "coldsnap", "emp", "sunstrike", "tornado", "chaosmeteor", "deafeningblast",
        "icewall", "ghostwalk", "panicghostwalk", "alacrity", "forgespirit"
    };

    [JsonIgnore]
    public static string ConfigPath => Path.Combine(AppContext.BaseDirectory, "config.json");

    [JsonIgnore]
    public static string? LastLoadError { get; private set; }

    public void Save()
    {
        var dir = Path.GetDirectoryName(ConfigPath);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ConfigPath, json);
    }


    public static AppConfig Load()
    {
        try
        {
            AppConfig config;
            if (File.Exists(ConfigPath))
            {
                var json = File.ReadAllText(ConfigPath);
                config = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            }
            else
            {
                config = new AppConfig();
            }

            foreach (var hab in DefaultAbilities)
            {
                if (!config.KeyCombos.ContainsKey(hab))
                    config.KeyCombos[hab] = new List<string>();
            }
            return config;
        }
        catch (Exception ex)
        {
            LastLoadError = ex.Message;
            File.WriteAllText(
                Path.Combine(AppContext.BaseDirectory, "config_error.log"),
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Erro ao carregar config.json:\n{ex}");
        }
        // fallback
        var fallback = new AppConfig();
        foreach (var hab in DefaultAbilities)
            fallback.KeyCombos[hab] = new List<string>();
        return fallback;
    }
}
