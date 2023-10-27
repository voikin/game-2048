using game_2048.DataLayer.dtos;
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
            Console.WriteLine($"Добро пожаловать в игру 2048! Выберите пункт меню:");

            Console.WriteLine(cursorPosition == 1 ? "> Показать лучшие результаты" : "  Показать лучшие результаты");
            Console.WriteLine(cursorPosition == 2 ? "> Начать новую игру" : "  Начать новую игру");
            Console.WriteLine(cursorPosition == 3 ? "> Загрузить игру" : "  Загрузить игру");
            Console.WriteLine(cursorPosition == 4 ? "> Выход" : "  Выход");

            key = Console.ReadKey();

            if (key.Key.ToString() == "DownArrow")
            {
                if (cursorPosition == 4)
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
                    cursorPosition = 4;
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
                        LoadGame();
                        break;
                    case 4:
                        EndGame();
                        break;
                }
            }
        }
    }

    private void LoadGame()
    {
        var sessions = Logic.GetSessionNames();
        if (sessions.Count == 0)
        {
            Console.WriteLine("Не найдено ни одной записи сохранения!");
            return;
        }

        int cursorPosition = 1;
        ConsoleKeyInfo key;

        while (true)
        {
            Console.Clear();
            for (int i = 0; i < sessions.Count; i++)
            {
                Console.WriteLine(cursorPosition == i-1 ? $"> {sessions[i]}" : $"  {sessions[i]}");
            }
            
            key = Console.ReadKey();

            if (key.Key.ToString() == "DownArrow")
            {
                if (cursorPosition == session.Count)
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
                    cursorPosition = session.Count;
                }
                else
                {
                    cursorPosition--;
                }

                continue;
            }

            if (key.Key.ToString() == "Enter")
            {
                var data = Logic.LoadSession(sessions[cursorPosition - 1]);
                StartGame(data);
            }
        }
    }

    private void ShowHighScore()
    {
        var highScores = Logic.GetHighScores();
        for (var i = 1; i <= highScores.Count && i <= 10; i++)
        {
            var currScore = highScores[i - 1];
            Console.WriteLine($"{i}) {currScore.Name} - {currScore.HighScore}");
        }
    }

    private void ShowHighScore(List<PlayerRecordDTO> highScores)
    {
        for (var i = 1; i <= highScores.Count && i <= 10; i++)
        {
            var currScore = highScores[i - 1];
            Console.WriteLine($"{i}) {currScore.Name} - {currScore.HighScore}");
        }
    }

    private void StartGame(GameData? data)
    {
        data ??= Logic.NewGame();
        ConsoleKeyInfo key;
        PrintDeck(data.Deck.Deck);

        while (true)
        {
            Console.Clear();
            key = Console.ReadKey();
            var isArrowKey = new[]
                    { ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow }
                .Contains(key.Key);
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
                        Console.WriteLine("|" + cell.PadLeft(halfPadSize + cell.Length / 2).PadRight(padSize) + "|");
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

    private void EndGame()
    {
        Console.Clear();
        Console.WriteLine("Ждем вас снова!");
        Environment.Exit(1);
    }

    private void EndGame(bool isSaving)
    {
        Console.Clear();
        Console.WriteLine(isSaving ? "Сохранение игры" : "Конец игры");
        string name;
        while (true)
        {
            Console.Write("Введите имя (допустимо только использование английских букв и цифр): ");
            name = Console.ReadLine();

            if (name != null && System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z0-9]+$"))
            {
                break;
            }

            Console.WriteLine("Введено некорректное имя. Попробуйте еще раз.");
        }

        if (isSaving)
        {
            Logic.SaveSession(name);
            Console.WriteLine("Возвращайтесь как можно скорее!");
        }

        else
        {
            int place;
            List<PlayerRecordDTO> highScores;
            Logic.SaveHighScore(name, out place, out highScores);
            Console.WriteLine($"Вы заняли место №{place} в таблице лидеров! Ниже представлены лучшие результаты:");
            ShowHighScore(highScores);
            Console.WriteLine("Возвращайтесь как можно скорее!");
        }
        
        Environment.Exit(1);
    }
}