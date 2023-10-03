using System.Text.Json;
using game_2048.DataLayer.dtos;

namespace game_2048.DataLayer;

public static class DataAccess
{
    private static string DBPath => "records.json";

    public static List<PlayerRecordDTO> GetRecords()
    {
        if (File.Exists(DBPath))
        {
            string json = File.ReadAllText(DBPath);
            return JsonSerializer.Deserialize<List<PlayerRecordDTO>>(json) ?? new List<PlayerRecordDTO>();
        }
        else
        {
            return new List<PlayerRecordDTO>();
        }
    }

    public static PlayerRecordDTO GetRecordByName(string name) => 
        GetRecords().Find(user => user.Name == name) ?? new PlayerRecordDTO(name);


    public static void CreateOrUpdateRecord(string name, int highScore)
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


    static void SaveRecords(List<PlayerRecordDTO> records)
    {
        string json = JsonSerializer.Serialize(records);
        File.WriteAllText(DBPath, json);
    }
}

// TODO: Грамотно обработать исключение System.IO.IOException которое вызывается при попытке записи/чтения в файл, который занят другим процессом.  