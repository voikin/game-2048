using System.Text.Json;
using game_2048.DataLayer.dtos;

namespace game_2048.DataLayer;

public class DataAccess
{
    private string DbPath { get; set; } = "";

    public DataAccess(string dbPath = "records.json")
    {
        DbPath = dbPath;
    }

    public List<PlayerRecordDTO> GetRecords()
    {
        if (File.Exists(DbPath))
        {
            string json = File.ReadAllText(DbPath);
            return (JsonSerializer.Deserialize<List<PlayerRecordDTO>>(json) ?? new List<PlayerRecordDTO>())
                .OrderByDescending(player => player.HighScore)
                .ToList();
        }
        else
        {
            return new List<PlayerRecordDTO>();
        }
    }

    public PlayerRecordDTO GetRecordByName(string name) => 
        GetRecords().Find(user => user.Name == name) ?? new PlayerRecordDTO(name);


    public void CreateOrUpdateRecord(string name, int highScore)
    {
        var users = GetRecords();
        var user = users.Find(usr => usr.Name == name);
        if (user == null)
        {
            users.Add(new PlayerRecordDTO(name, highScore));
        }
        else
        {
            user.HighScore = highScore;
        }

        SaveRecords(users);
    }


    void SaveRecords(List<PlayerRecordDTO> records)
    {
        string json = JsonSerializer.Serialize(records);
        File.WriteAllText(DbPath, json);
    }
}

// TODO: Грамотно обработать исключение System.IO.IOException которое вызывается при попытке записи/чтения в файл, который занят другим процессом.  