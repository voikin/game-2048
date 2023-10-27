namespace game_2048.DataLayer.dtos;

public record PlayerRecordDTO
{
    public string Name { get; init; }
    public int HighScore { get; set; }
    
    public PlayerRecordDTO(string name, int highScore = 0)
    {
        Name = name;
        HighScore = highScore;
    }

}