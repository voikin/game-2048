namespace game_2048.LogicLayer.Models;

public class GameData
{
    private int _score = 0;
    public bool IsGame { get; set; } = false;

    public int Score
    {
        set
        {
            if (value <= 0) throw new ArgumentException(); // TODO поменять на нормальное исключение.
            _score = value;
        }
        get => _score;
    }
    public GameDeck Deck = new();
}