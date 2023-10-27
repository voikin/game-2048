using System.Text.Json;
using game_2048.DataLayer.dtos;
using game_2048.LogicLayer.Models;

namespace game_2048.DataLayer;

public class SessionDataAccess
{
    private string DbPath { get; set; }

    public SessionDataAccess()
    {
        DbPath = Path.GetTempPath();
    }

    public List<string> GetSessionNames()
    {
        var sessionsNames = Directory.GetFiles(Path.GetTempPath(), "*.session", SearchOption.TopDirectoryOnly);
        if (sessionsNames.Length == 0) return new List<string>();
        List<string> names = new();
        foreach (var name in sessionsNames)
        {
            names.Add(name.Split('/').Last().Split('.')[0]);
        }

        return names;
    }
    
    public GameDeck GetSession(string name)
    {
        string fileName = Path.Join(DbPath, name);
        string json = File.ReadAllText(fileName);
        return JsonSerializer.Deserialize<GameDeck>(json) ?? new GameDeck();

    }

    public void SaveSession(GameDeck deck, string name)
    {
        string json = JsonSerializer.Serialize(deck);
        File.WriteAllText(Path.Join(DbPath, name), json);
    }
}