namespace game_2048.LogicLayer.Models;

public class GameDeck
{
    public int[,] Deck { get; private set; } = new int[4,4];

    public int[,] GenerateNewDeck()
    {
        Deck = new int[4, 4]; 

        Random random = new Random(); 

        for (int i = 0; i < 2; i++)
        {
            var row = random.Next(0, 4); 
            var col = random.Next(0, 4); 
            while (Deck[row, col] != 0) 
            {
                row = random.Next(0, 4); 
                col = random.Next(0, 4); 
            }

            Deck[row, col] = random.Next(0, 2) == 0 ? 2 : 4;
        }

        return Deck;
    }

    public int MoveUp()
    {
        bool success = true;
        int upScore = 0;
        
        // move

        return success ? upScore : -1;
    }
    
    public int MoveDown()
    {
        bool success = true;
        int upScore = 0;
        
        // move

        return success ? upScore : -1;
    }
    
    public int MoveLeft()
    {
        bool success = true;
        int upScore = 0;
        
        // move

        return success ? upScore : -1;
    }
    
    public int MoveRight()
    {
        bool success = true;
        int upScore = 0;
        
        // move

        return success ? upScore : -1;
    }
}