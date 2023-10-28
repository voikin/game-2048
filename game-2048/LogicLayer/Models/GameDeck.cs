namespace game_2048.LogicLayer.Models;

public class GameDeck
{
    public int[][] Deck { get; private set; } = new int[4][];
    private readonly Dictionary<ConsoleKey, Action> _direction = new();
    private readonly Random _random = new();
    

    public GameDeck()
    {
       SetUpDict(); 
    }

    public GameDeck(int[][] deck)
    {
        Deck = deck;
        SetUpDict();
    }

    void SetUpDict()
    {
        _direction.Add(ConsoleKey.RightArrow, ShiftRight);
        _direction.Add(ConsoleKey.LeftArrow, ShiftLeft);
        _direction.Add(ConsoleKey.UpArrow, ShiftUp);
        _direction.Add(ConsoleKey.DownArrow, ShiftDown); 
    }

    public int[][] GenerateNewDeck()
    {
        Deck = new int[4][];
        Deck[0] = new int[4];
        Deck[1] = new int[4];
        Deck[2] = new int[4];
        Deck[3] = new int[4];

        Random random = new Random(); 

        for (int i = 0; i < 2; i++)
        {
            var row = random.Next(0, 4); 
            var col = random.Next(0, 4); 
            while (Deck[row][col] != 0) 
            {
                row = random.Next(0, 4); 
                col = random.Next(0, 4); 
            }

            Deck[row][col] = random.Next(0, 2) == 0 ? 2 : 4;
        }

        return Deck;
    }

    void InsertNewNumber()
    {
        List<Tuple<int, int>> emptyPositions = new List<Tuple<int, int>>();
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                if (Deck[i][j] == 0) {
                    emptyPositions.Add(new Tuple<int, int>(i, j));
                }
            }
        }

        if (emptyPositions.Count > 0) {
            Tuple<int, int> position = emptyPositions[_random.Next(emptyPositions.Count)];
            Deck[position.Item1][position.Item2] = _random.Next(1, 3) * 2;
        }
    }

    void ShiftLeft()
    {
        for (int i = 0; i < 4; i++)
        {
            int placePosition = 0;
            for (int j = 0; j < 4; j++)
            {
                if (Deck[i][j] != 0)
                {
                    if (placePosition != j)
                    {
                        if (Deck[i][placePosition] != 0 && Deck[i][placePosition] == Deck[i][j])
                        {
                            Deck[i][placePosition] *= 2;
                            Deck[i][j] = 0;
                            placePosition++;
                        }
                        else if (Deck[i][placePosition] == 0)
                        {
                            Deck[i][placePosition] = Deck[i][j];
                            Deck[i][j] = 0;
                        }
                        else
                        {
                            placePosition++;
                            if (placePosition != j)
                            {
                                Deck[i][placePosition] = Deck[i][j];
                                Deck[i][j] = 0;
                            }
                        }
                    }
                }
            }
        }
    }

    void ShiftRight()
    {
        for (int i = 0; i < 4; i++)
        {
            int placePosition = 3;
            for (int j = 3; j >= 0; j--)
            {
                if (Deck[i][j] != 0)
                {
                    if (placePosition != j)
                    {
                        if (Deck[i][placePosition] != 0 && Deck[i][placePosition] == Deck[i][j])
                        {
                            Deck[i][placePosition] *= 2;
                            Deck[i][j] = 0;
                            placePosition--;
                        }
                        else if (Deck[i][placePosition] == 0)
                        {
                            Deck[i][placePosition] = Deck[i][j];
                            Deck[i][j] = 0;
                        }
                        else
                        {
                            placePosition--;
                            if (placePosition != j)
                            {
                                Deck[i][placePosition] = Deck[i][j];
                                Deck[i][j] = 0;
                            }
                        }
                    }
                }
            }
        }
    }

    void ShiftUp()
    {
        for (int j = 0; j < 4; j++)
        {
            int placePosition = 0;
            for (int i = 0; i < 4; i++)
            {
                if (Deck[i][j] != 0)
                {
                    if (placePosition != i)
                    {
                        if (Deck[placePosition][j] != 0 && Deck[placePosition][j] == Deck[i][j])
                        {
                            Deck[placePosition][j] *= 2;
                            Deck[i][j] = 0;
                            placePosition++;
                        }
                        else if (Deck[placePosition][j] == 0)
                        {
                            Deck[placePosition][j] = Deck[i][j];
                            Deck[i][j] = 0;
                        }
                        else
                        {
                            placePosition++;
                            if (placePosition != i)
                            {
                                Deck[placePosition][j] = Deck[i][j];
                                Deck[i][j] = 0;
                            }
                        }
                    }
                }
            }
        }
    }

    void ShiftDown()
    {
        for (int j = 0; j < 4; ++j)
        {
            int placePosition = 3;
            for (int i = 3; i >= 0; --i)
            {
                if (Deck[i][j] != 0)
                {
                    if (placePosition != i)
                    {
                        if(Deck[placePosition][j] != 0 && Deck[placePosition][j] == Deck[i][j])
                        {
                            Deck[placePosition][j] *= 2;
                            Deck[i][j] = 0;
                            placePosition--;
                        }
                        else if (Deck[placePosition][j] == 0)
                        {
                            Deck[placePosition][j] = Deck[i][j];
                            Deck[i][j] = 0;
                        }
                        else
                        {
                            placePosition--;
                            if (placePosition != i)
                            {
                                Deck[placePosition][j] = Deck[i][j];
                                Deck[i][j] = 0;
                            }
                        }
                    }
                }
            }
        }
    }
    
    public bool IsHaveSteps()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (Deck[i][j] == 0)
                {
                    return true; 
                }
                if (j < 3 && Deck[i][j] == Deck[i][j + 1]) 
                {
                    return true; 
                }
                if (i < 3 && Deck[i][j] == Deck[i + 1][j]) 
                {
                    return true; 
                }
            }
        }

        return false; 
    }
    
    public bool Move(ConsoleKey key)
    {
        var originalBoard = Deck.Select(a => a.ToArray()).ToArray();

        _direction[key]();

        if (!originalBoard.Select((arr, index) => arr.SequenceEqual(Deck[index])).All(b => b))
        {
            InsertNewNumber();
        }

        return IsHaveSteps();
    }

    public int CalculateScore()
    {
        int score = 0;

        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                score += Deck[i][j];
            }
        }

        return score; 
    }
    
}