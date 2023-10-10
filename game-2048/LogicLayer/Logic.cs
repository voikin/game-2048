using game_2048.DataLayer;
using game_2048.LogicLayer.Models;

namespace game_2048.LogicLayer;

public class Logic
{
    private GameData _data = new();
    private DataAccess DB = new();
    
    public Logic(){}

    public GameData NewGame()
    {
        _data.Deck.GenerateNewDeck();
        _data.IsGame = true;
        return _data;
    }

    public GameData Move(ConsoleKey key)
    {
        if (!_data.IsGame) return _data;

        int deltaScore = 0;
        
        switch (key)
        {
            case ConsoleKey.UpArrow:
                deltaScore = _data.Deck.MoveUp();
                break;
            case ConsoleKey.DownArrow:
                deltaScore = _data.Deck.MoveDown();
                break;
            case ConsoleKey.LeftArrow:
                deltaScore = _data.Deck.MoveLeft();
                break;
            case ConsoleKey.RightArrow:
                deltaScore = _data.Deck.MoveRight();
                break;
        }

        if (deltaScore == -1)
        {
            _data.IsGame = false;
            return _data;
        }

        _data.Score += deltaScore;
        return _data;

    }
    
}