using game_2048.LogicLayer;
using game_2048.LogicLayer.Models;
using game_2048.PresentationLayer.helpers;

namespace game_2048.PresentationLayer;

public class Presentation
{
    private readonly Logic Logic;

    public Presentation(Logic logicLayer)
    {
        Logic = logicLayer;
    }

    public void Start()
    {
        Menu();
    }

    private void Menu()
    {
        int cursorPosition = 1;  
        ConsoleKeyInfo key; 

        while (true) 
        {
            Console.Clear();
            Console.WriteLine($"Welcome to the Game Menu. Your current selected option is {cursorPosition}");

            Console.WriteLine(cursorPosition == 1 ? "> Показать лучшие результаты" : "  Показать лучшие результаты");
            Console.WriteLine(cursorPosition == 2 ? "> Начать игру" : "  Начать игру");
            Console.WriteLine(cursorPosition == 3 ? "> Выход" : "  Выход");

            key = Console.ReadKey();

            if (key.Key.ToString() == "DownArrow") 
            {
                if (cursorPosition == 3)
                {
                    cursorPosition = 1;
                }
                else 
                {
                    cursorPosition++;
                }
                continue; 
            }

            if (key.Key.ToString() == "UpArrow") 
            {
                if (cursorPosition == 1)
                {
                    cursorPosition = 3;
                }
                else
                {
                    cursorPosition--;
                }
                continue; 
            }

            if (key.Key.ToString() == "Enter") 
            {
                switch (cursorPosition)
                {
                    case 1:
                        ShowHighScore();
                        break;
                    case 2:
                        StartGame();
                        break;
                    case 3:
                        EndGame();
                        break;
                }
            }
        }
    }

    private void ShowHighScore()
    {
        // var highScores = Logic.GetHighScores();
        
    }

    private void StartGame()
    {
        var data = Logic.NewGame();
        ConsoleKeyInfo key; 
        PrintDeck(data.Deck.Deck);
        
        while (true)
        {
            Console.Clear();
            key = Console.ReadKey();
            var isArrowKey = new [] { ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow }.Contains(key.Key);
            var isSaveKey = key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.S;
            if (isArrowKey)
            {
                data = Logic.Move(key.Key);
                if (!data.IsGame) EndGame(false); 
                Console.Clear();
                PrintDeck(data.Deck.Deck);
            }

            if (isSaveKey)
            {
                EndGame(true);
            }
            
        }
    }

    private void PrintDeck(int[,] deck)
    {
        RGBToHSL converter = new RGBToHSL();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                var value = deck[i, j];
                const int r = 255;
                var g = 255 - (value - 2) * 10;
                const int b = 0;

                HSLColor color = converter.Convert(r, g, b);

                Console.BackgroundColor = color.ToConsoleColor();

                string cell = value.ToString();
                int padSize = 12;
                int halfPadSize = padSize / 2;
                
                Console.WriteLine("+------------+");
                for (int k = 0; k < 4; k++)
                {
                    if (k == 2)
                    {
                        Console.WriteLine("|" + cell.PadLeft(halfPadSize + cell.Length/2).PadRight(padSize) + "|");
                    }
                    else
                    {
                        Console.WriteLine("|" + "".PadRight(padSize) + "|");
                    }
                }
                Console.WriteLine("+------------+");
                Console.ResetColor();
            }
            Console.WriteLine();
        }
    }

    private void EndGame(bool isSaving)
    {
    }
}