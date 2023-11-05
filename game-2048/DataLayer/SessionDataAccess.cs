using System.Text.Json;

namespace game_2048.DataLayer;

public class SessionDataAccess
{
    private string DbPath { get; } = Path.GetTempPath();

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
    
    public int[][] GetSession(string name)
    {
        var fileName = Path.Join(DbPath, $"{name}.session");
        var json = File.ReadAllText(fileName);
        File.Delete(fileName);
        return JsonSerializer.Deserialize<int[][]>(json) ?? new int[4][];

    }

    public void SaveSession(int[][] deck, string name)
    {
        var json = JsonSerializer.Serialize(deck);
        File.WriteAllText(Path.Join(DbPath, $"{name}.session"), json);
    }
}