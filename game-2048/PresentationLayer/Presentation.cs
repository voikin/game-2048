using game_2048.DataLayer.dtos;
using game_2048.LogicLayer;
using game_2048.LogicLayer.Models;
using game_2048.PresentationLayer.helpers;

namespace game_2048.PresentationLayer;

public partial class Presentation
{
    private readonly Logic _logic;

    public Presentation(Logic logicLayer)
    {
        _logic = logicLayer;
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

            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
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
                case ConsoleKey.UpArrow:
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
                case ConsoleKey.Enter:
                    switch (cursorPosition)
                    {
                        case 1:
                            ShowHighScore();
                            break;
                        case 2:
                            StartGame(null);
                            break;
                        case 3:
                            LoadGame();
                            break;
                        case 4:
                            EndGame();
                            break;
                    }

                    break;
            }
        }
    }

    private void LoadGame()
    {
        var sessions = _logic.GetSessionNames();
        if (sessions.Count == 0)
        {
            Console.WriteLine("Не найдено ни одной записи сохранения! Для продолжения нажмите ENTER");
            Console.ReadLine();
            return;
        }

        int cursorPosition = 1;
        ConsoleKeyInfo key;

        while (true)
        {
            Console.Clear();
            for (int i = 0; i < sessions.Count; i++)
            {
                Console.WriteLine(cursorPosition == i + 1 ? $"> {sessions[i]}" : $"  {sessions[i]}");
            }

            key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                {
                    if (cursorPosition == sessions.Count)
                    {
                        cursorPosition = 1;
                    }
                    else
                    {
                        cursorPosition++;
                    }

                    continue;
                }
                case ConsoleKey.UpArrow:
                {
                    if (cursorPosition == 0)
                    {
                        cursorPosition = sessions.Count - 1;
                    }
                    else
                    {
                        cursorPosition--;
                    }

                    continue;
                }
                case ConsoleKey.Enter:
                {
                    var data = _logic.LoadSession(sessions[cursorPosition - 1]);
                    StartGame(data);
                    break;
                }
            }
        }
    }

    private void ShowHighScore()
    {
        var highScores = _logic.GetHighScores();
        if (highScores.Count == 0)
        {
            Console.WriteLine("Таблица рекордов пуста. Для продолжения нажмите ENTER");
            Console.ReadLine();
            return;
        }

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
        data ??= _logic.NewGame();
        ConsoleKeyInfo key;
        PrintDeck(data.Deck.Deck);
        while (true)
        {
            key = Console.ReadKey();
            var isArrowKey = new[]
                    { ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow }
                .Contains(key.Key);
            var isSaveKey = key is { Modifiers: ConsoleModifiers.Control, Key: ConsoleKey.S };
            
            if (isArrowKey)
            {
                data = _logic.Move(key.Key);
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

    private void PrintDeck(int[][] deck)
    {
        RGBToHSL converter = new RGBToHSL();

        for (int i = 0; i < 4; i++)
        {
            Console.Write("|");
            for (int j = 0; j < 4; j++)
            {
                if (j != 0)
                {
                    Console.Write("|");
                }
                int value = deck[i][j];
                int r = 255;
                int g = 255 - (value) * 10;
                int b = 0;

                HSLColor color = converter.Convert(r, g, b);

                Console.BackgroundColor = color.ToConsoleColor();
                Console.Write($"{value}\t");
                Console.ResetColor();
            }
            Console.Write("|");
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

            if (name != null && MyRegex().IsMatch(name))
            {
                break;
            }

            Console.WriteLine("Введено некорректное имя. Попробуйте еще раз.");
        }

        if (isSaving)
        {
            _logic.SaveSession(name);
            Console.WriteLine("Возвращайтесь как можно скорее!");
        }

        else
        {
            _logic.SaveHighScore(name, out var place, out var highScores);
            Console.WriteLine($"Вы заняли место №{place} в таблице лидеров! Ниже представлены лучшие результаты:");
            ShowHighScore(highScores);
            Console.WriteLine("Возвращайтесь как можно скорее!");
        }

        Environment.Exit(1);
    }

    [System.Text.RegularExpressions.GeneratedRegex("^[a-zA-Z0-9]+$")]
    private static partial System.Text.RegularExpressions.Regex MyRegex();
}