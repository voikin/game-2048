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

    public void GenerateNewDeck()
    {
        Deck = new int[4][];
        Deck[0] = new int[4];
        Deck[1] = new int[4];
        Deck[2] = new int[4];
        Deck[3] = new int[4];

        for (var i = 0; i < 2; i++)
        {
            var row = _random.Next(0, 4); 
            var col = _random.Next(0, 4); 
            while (Deck[row][col] != 0) 
            {
                row = _random.Next(0, 4); 
                col = _random.Next(0, 4); 
            }

            Deck[row][col] = _random.Next(0, 2) == 0 ? 2 : 4;
        }

    }

    private void InsertNewNumber()
    {
        var emptyPositions = new List<Tuple<int, int>>();
        for (var i = 0; i < 4; i++) {
            for (var j = 0; j < 4; j++) {
                if (Deck[i][j] == 0) {
                    emptyPositions.Add(new Tuple<int, int>(i, j));
                }
            }
        }

        if (emptyPositions.Count <= 0) return;
        var position = emptyPositions[_random.Next(emptyPositions.Count)];
        Deck[position.Item1][position.Item2] = _random.Next(1, 3) * 2;
    }

    private void ShiftLeft()
    {
        for (var i = 0; i < 4; i++)
        {
            var placePosition = 0;
            for (var j = 0; j < 4; j++)
            {
                if (Deck[i][j] == 0) continue;
                if (placePosition == j) continue;
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
                    if (placePosition == j) continue;
                    Deck[i][placePosition] = Deck[i][j];
                    Deck[i][j] = 0;
                }
            }
        }
    }

    private void ShiftRight()
    {
        for (var i = 0; i < 4; i++)
        {
            var placePosition = 3;
            for (var j = 3; j >= 0; j--)
            {
                if (Deck[i][j] == 0) continue;
                if (placePosition == j) continue;
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
                    if (placePosition == j) continue;
                    Deck[i][placePosition] = Deck[i][j];
                    Deck[i][j] = 0;
                }
            }
        }
    }

    private void ShiftUp()
    {
        for (var j = 0; j < 4; j++)
        {
            var placePosition = 0;
            for (var i = 0; i < 4; i++)
            {
                if (Deck[i][j] == 0) continue;
                if (placePosition == i) continue;
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
                    if (placePosition == i) continue;
                    Deck[placePosition][j] = Deck[i][j];
                    Deck[i][j] = 0;
                }
            }
        }
    }

    private void ShiftDown()
    {
        for (var j = 0; j < 4; ++j)
        {
            var placePosition = 3;
            for (var i = 3; i >= 0; --i)
            {
                if (Deck[i][j] == 0) continue;
                if (placePosition == i) continue;
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
                    if (placePosition == i) continue;
                    Deck[placePosition][j] = Deck[i][j];
                    Deck[i][j] = 0;
                }
            }
        }
    }

    private bool IsHaveSteps()
    {
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
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
        var score = 0;

        for (var i = 0; i < 4; i++) {
            for (var j = 0; j < 4; j++) {
                score += Deck[i][j];
            }
        }

        return score; 
    }
    
}