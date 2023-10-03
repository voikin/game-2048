namespace game_2048.DataLayer.dtos;

public class PlayerRecordDTO
{
    public string Name { get; }
    public int HighScore { get; set; }

    public PlayerRecordDTO(string name, int highScore = 0)
    {
        Name = name;
        HighScore = highScore;
    }
}