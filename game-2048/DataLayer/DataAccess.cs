using System.Text.Json;
using game_2048.DataLayer.dtos;

namespace game_2048.DataLayer;

public class DataAccess
{
    private string DbPath { get; set; }

    public DataAccess(string dbPath = "records.json")
    {
        DbPath = Path.GetTempPath() + dbPath;
    }

    public List<PlayerRecordDto> GetRecords()
    {
        if (File.Exists(DbPath))
        {
            var json = File.ReadAllText(DbPath);
            return (JsonSerializer.Deserialize<List<PlayerRecordDto>>(json) ?? new List<PlayerRecordDto>())
                .OrderByDescending(player => player.HighScore)
                .ToList();
        }
        else
        {
            return new List<PlayerRecordDto>();
        }
    }

    public PlayerRecordDto GetRecordByName(string name) => 
        GetRecords().Find(user => user.Name == name) ?? new PlayerRecordDto(name);


    public void CreateOrUpdateRecord(string name, int highScore)
    {
        var users = GetRecords();
        var user = users.Find(usr => usr.Name == name);
        if (user == null)
        {
            users.Add(new PlayerRecordDto(name, highScore));
        }
        else
        {
            user.HighScore = highScore;
        }

        SaveRecords(users);
    }


    void SaveRecords(List<PlayerRecordDto> records)
    {
        string json = JsonSerializer.Serialize(records);
        File.WriteAllText(DbPath, json);
    }
}