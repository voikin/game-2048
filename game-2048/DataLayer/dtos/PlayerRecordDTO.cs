namespace game_2048;

public class PlayerRecordDTO
{
    public string Name { get; }
    private int HighScore { get; set; } = 0;

    public PlayerRecordDTO(string name)
    {
        Name = name;
    }
}