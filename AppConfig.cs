
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
            if (File.Exists(ConfigPath))
            {
                var json = File.ReadAllText(ConfigPath);
                var config = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
                return config;
            }
        }
        catch { }
        return new AppConfig();
    }
}
