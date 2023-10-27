namespace game_2048.LogicLayer.Models;

public class GameDeck
{
    public int[,] Deck { get; private set; } = new int[4,4];
    private readonly Dictionary<ConsoleKey, Action> _direction = new();
    private readonly Random _random = new();
    

    public GameDeck()
    {
        _direction.Add(ConsoleKey.RightArrow, ShiftRight);
        _direction.Add(ConsoleKey.LeftArrow, ShiftLeft);
        _direction.Add(ConsoleKey.UpArrow, ShiftUp);
        _direction.Add(ConsoleKey.DownArrow, ShiftDown);
    }

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

    bool InsertNewNumber()
    {
        List<Tuple<int, int>> emptyPositions = new List<Tuple<int, int>>();
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                if (Deck[i, j] == 0) {
                    emptyPositions.Add(new Tuple<int, int>(i, j));
                }
            }
        }

        // Если есть свободные позиции, выберем случайную из них и установим там либо 2, либо 4
        if (emptyPositions.Count > 0) {
            Tuple<int, int> position = emptyPositions[_random.Next(emptyPositions.Count)];
            Deck[position.Item1, position.Item2] = _random.Next(1, 3) * 2;
            return true;
        }

        return false;
    }

    void ShiftLeft()
    {
        for (int i = 0; i < 4; i++)
        {
            int lastNonZeroIndex = -1;
            for (int j = 0; j < 4; j++)
            {
                if (Deck[i, j] != 0)
                {
                    if (lastNonZeroIndex != -1 && Deck[i, lastNonZeroIndex] == Deck[i, j])
                    {
                        Deck[i, lastNonZeroIndex] *= 2;
                        Deck[i, j] = 0;
                        lastNonZeroIndex = -1;
                    }
                    else
                    {
                        lastNonZeroIndex = j;
                    }
                }
            }
        }
    }
    
    void ShiftRight()
    {
        for (int i = 0; i < 4; ++i)
        {
            int lastNonZeroIndex = -1;
            for (int j = 3; j >= 0; --j)
            {
                if (Deck[i, j] != 0)
                {
                    if (lastNonZeroIndex != -1 && Deck[i, lastNonZeroIndex] == Deck[i, j])
                    {
                        Deck[i, lastNonZeroIndex] *= 2;
                        Deck[i, j] = 0;
                        lastNonZeroIndex = -1;
                    }
                    else
                    {
                        lastNonZeroIndex = j;
                    }
                }
            }
        }
    }
    
    void ShiftUp()
    {
        for (int j = 0; j < 4; ++j)
        {
            int lastNonZeroIndex = -1;
            for (int i = 0; i < 4; ++i)
            {
                if (Deck[i, j] != 0)
                {
                    if (lastNonZeroIndex != -1 && Deck[lastNonZeroIndex, j] == Deck[i, j])
                    {
                        Deck[lastNonZeroIndex, j] *= 2;
                        Deck[i, j] = 0;
                        lastNonZeroIndex = -1;
                    }
                    else
                    {
                        lastNonZeroIndex = i;
                    }
                }
            }
        }
    }

    void ShiftDown()
    {
        for (int j = 0; j < 4; ++j)
        {
            int lastNonZeroIndex = -1;
            for (int i = 3; i >= 0; --i)
            {
                if (Deck[i, j] != 0)
                {
                    if (lastNonZeroIndex != -1 && Deck[lastNonZeroIndex, j] == Deck[i, j])
                    {
                        Deck[lastNonZeroIndex, j] *= 2;
                        Deck[i, j] = 0;
                        lastNonZeroIndex = -1;
                    }
                    else
                    {
                        lastNonZeroIndex = i;
                    }
                }
            }
        }
    }
    
    public bool Move(ConsoleKey key)
    {
        bool success = true;
        
        var originalBoard = (int[,])Deck.Clone();

        _direction[key]();
        if (!Enumerable.SequenceEqual(originalBoard.Cast<int>(), Deck.Cast<int>()))
        {
            success = InsertNewNumber();
        }


        return success;

    }

    public int CalculateScore()
    {
        int score = 0;

        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                score += Deck[i, j];
            }
        }

        return score; 
    }
    
}