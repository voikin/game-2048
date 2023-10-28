using game_2048.DataLayer;
using game_2048.DataLayer.dtos;
using game_2048.LogicLayer.Models;

namespace game_2048.LogicLayer;

public class Logic
{
    private readonly GameData _data = new();
    private readonly DataAccess _scoresDb;
    private readonly SessionDataAccess _sessionsDb;


    public Logic(DataAccess scoresDb, SessionDataAccess sessionsDb)
    {
        _scoresDb = scoresDb;
        _sessionsDb = sessionsDb;
    }    

    public GameData NewGame()
    {
        _data.Deck.GenerateNewDeck();
        _data.IsGame = true;
        return _data;
    }

    public GameData Move(ConsoleKey key)
    {
        if (!_data.IsGame) return _data;

        _data.IsGame = _data.Deck.Move(key);

        return _data;

    }

    public List<string> GetSessionNames() => _sessionsDb.GetSessionNames();

    public GameData LoadSession(string name)
    {
        int[][] loadedDeck = _sessionsDb.GetSession(name);
        _data.Deck= new(loadedDeck);
        _data.IsGame = true;
        return _data;
    }

    public void SaveSession(string name)
    {
        _sessionsDb.SaveSession(_data.Deck.Deck, name);
    }
    
    public List<PlayerRecordDTO> GetHighScores() => _scoresDb.GetRecords();
    
    public void SaveHighScore(string name, out int place, out List<PlayerRecordDTO> highScores)
    {
        var oldUser = _scoresDb.GetRecordByName(name);
        var highScore = _data.Deck.CalculateScore();
        if (oldUser.HighScore < highScore)
        {
            _scoresDb.CreateOrUpdateRecord(name, highScore); 
        }
        highScores = _scoresDb.GetRecords();
        place = _scoresDb.GetRecords().FindIndex(rec => rec.Name == name) + 1;
    }
}