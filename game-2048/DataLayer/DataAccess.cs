using System.Text.Json;

namespace game_2048.DataLayer;

public class DataAccess
{
    private static string DBPath => "records.json";

    public static List<PlayerRecordDTO> LoadRecords()
    {
        if (File.Exists(DBPath))
        {
            string json = File.ReadAllText(DBPath);
            return JsonSerializer.Deserialize<List<PlayerRecordDTO>>(json);
        }
        else
        {
            return new List<PlayerRecordDTO>();
        }
    }

    public static void SaveRecords(List<PlayerRecordDTO> records)
    {
        string json = JsonSerializer.Serialize(records);
        File.WriteAllText(DBPath, json);
    }
}