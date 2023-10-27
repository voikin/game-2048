using game_2048.DataLayer;
using game_2048.DataLayer.dtos;
using game_2048.LogicLayer.Models;

namespace game_2048.LogicLayer;

public class Logic
{
    private GameData _data = new();
    private DataAccess ScoresDB = new();
    
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

        _data.IsGame = _data.Deck.Move(key);;

        return _data;

    }

    void EndGame()
    {
        _data.IsGame = false;
        DB.CreateOrUpdateRecord("Nikita", 2048);
    }
    
    public List<PlayerRecordDTO> GetHighScores() => ScoresDB.GetRecords();

    public void SaveHighScore(string name, out int place, out List<PlayerRecordDTO> highScores)
    {
        ScoresDB.CreateOrUpdateRecord(name, _data.Deck.CalculateScore());
        highScores = ScoresDB.GetRecords();
        place = ScoresDB.GetRecords().FindIndex(rec => rec.Name == name);
    }
}