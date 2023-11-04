namespace game_2048.DataLayer.dtos;

public record PlayerRecordDto(string Name, int HighScore = 0)
{
    public int HighScore { get; set; } = HighScore;
}