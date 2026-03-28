
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

    // Novo: Mapeamento de combinações para habilidades
    public Dictionary<string, List<string>> KeyCombos { get; set; } = new();

    [JsonIgnore]
    public static string ConfigPath => Path.Combine(AppContext.BaseDirectory, "config.json");

    [JsonIgnore]
    public static string KeyCombosPath => Path.Combine(AppContext.BaseDirectory, "config_keys.json");

    public void Save()
    {
        var dir = Path.GetDirectoryName(ConfigPath);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ConfigPath, json);
    }

    // Novo: Carregar combinações de teclas do arquivo config_keys.json
    public void LoadKeyCombos()
    {
        try
        {
            if (File.Exists(KeyCombosPath))
            {
                var json = File.ReadAllText(KeyCombosPath);
                var dict = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
                if (dict != null)
                    KeyCombos = dict;
            }
        }
        catch { }
    }

    public static AppConfig Load()
    {
        try
        {
            if (File.Exists(ConfigPath))
            {
                var json = File.ReadAllText(ConfigPath);
                var config = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
                config.LoadKeyCombos();
                return config;
            }
        }
        catch { }
        var cfg = new AppConfig();
        cfg.LoadKeyCombos();
        return cfg;
    }
}
