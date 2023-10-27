using game_2048.DataLayer;
using game_2048.DataLayer.dtos;
using game_2048.LogicLayer.Models;

namespace game_2048.LogicLayer;

public class Logic
{
    private GameData _data = new();
    private DataAccess ScoresDB;
    private SessionDataAccess SessionsDB;


    public Logic(DataAccess scoresDb, SessionDataAccess sessionsDB)
    {
        ScoresDB = scoresDb;
        SessionsDB = sessionsDB;
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

        _data.IsGame = _data.Deck.Move(key);;

        return _data;

    }

    public List<string> GetSessionNames() => SessionsDB.GetSessionNames();

    public GameData LoadSession(string name)
    {
        int[,] loadedDeck = SessionsDB.GetSession(name);
        _data.Deck.Deck = loadedDeck;
        _data.IsGame = true;
        return _data;
    }

    public void SaveSession(string name)
    {
        SessionsDB.SaveSession(_data.Deck.Deck, name);
    }

    public List<PlayerRecordDTO> GetHighScores() => ScoresDB.GetRecords();

    public void SaveHighScore(string name, out int place, out List<PlayerRecordDTO> highScores)
    {
        var oldUser = ScoresDB.GetRecordByName(name);
        var highScore = _data.Deck.CalculateScore();
        if (oldUser.HighScore < highScore)
        {
            ScoresDB.CreateOrUpdateRecord(name, highScore); 
        }
        highScores = ScoresDB.GetRecords();
        place = ScoresDB.GetRecords().FindIndex(rec => rec.Name == name);
    }
}