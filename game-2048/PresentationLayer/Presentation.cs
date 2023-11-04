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
        var cursorPosition = 1;
        var isEnd = false;
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Добро пожаловать в игру 2048! Выберите пункт меню:");

            Console.WriteLine(cursorPosition == 1 ? "> Показать лучшие результаты" : "  Показать лучшие результаты");
            Console.WriteLine(cursorPosition == 2 ? "> Начать новую игру" : "  Начать новую игру");
            Console.WriteLine(cursorPosition == 3 ? "> Загрузить игру" : "  Загрузить игру");
            Console.WriteLine(cursorPosition == 4 ? "> Выход" : "  Выход");

            var key = Console.ReadKey();

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
                            isEnd = true;
                            break;
                    }

                    break;
            }

            if (isEnd) return;
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

        var cursorPosition = 1;

        while (true)
        {
            Console.Clear();
            for (var i = 0; i < sessions.Count; i++)
            {
                Console.WriteLine(cursorPosition == i + 1 ? $"> {sessions[i]}" : $"  {sessions[i]}");
            }

            var key = Console.ReadKey();

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
                    if (cursorPosition == 1)
                    {
                        cursorPosition = sessions.Count;
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
                    return;
                    break;
                }
                case ConsoleKey.Backspace:
                {
                    return;
                }
            }
        }
    }

    private void ShowHighScore()
    {
        Console.Clear();
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

        Console.Read();
    }

    private void ShowHighScore(List<PlayerRecordDto> highScores)
    {
        for (var i = 1; i <= highScores.Count && i <= 10; i++)
        {
            var currScore = highScores[i - 1];
            Console.WriteLine($"{i}) {currScore.Name} - {currScore.HighScore}");
        }
    }

    private void StartGame(GameData? data)
    {
        Console.Clear();
        data ??= _logic.NewGame();
        PrintDeck(data.Deck.Deck);
        while (true)
        {
            var key = Console.ReadKey();
            var isArrowKey = new[]
                    { ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow }
                .Contains(key.Key);
            var isSaveKey = key is { Modifiers: ConsoleModifiers.Control, Key: ConsoleKey.S };

            if (isArrowKey)
            {
                data = _logic.Move(key.Key);
                if (!data.IsGame)
                {
                    EndGame(false);
                    return;
                }

                Console.Clear();
                PrintDeck(data.Deck.Deck);
            }

            if (!isSaveKey) continue;
            EndGame(true);
            return;
        }
    }

    private void PrintDeck(int[][] deck)
    {
        for (var i = 0; i < 4; i++)
        {
            // Console.Write("|");
            for (var j = 0; j < 4; j++)
            {
                // if (j != 0)
                // {
                //     Console.Write("|");
                // }

                var value = deck[i][j];
                var spaces = " ".Repeat(4 - value.ToString().Length);

                var color = Constants.Color[value];

                Console.BackgroundColor = color;
                Console.ForegroundColor = (int)color == 0 ? ConsoleColor.White : ConsoleColor.Black;
                Console.Write($"{value}{spaces}");
                Console.ResetColor();
            }

            // Console.Write("|");
            Console.WriteLine();
        }
    }

    private void EndGame()
    {
        Console.Clear();
        Console.WriteLine("Ждем вас снова!");
    }

    private void EndGame(bool isSaving)
    {
        Console.Clear();
        Console.WriteLine(isSaving ? "Сохранение игры" : "Конец игры");
        string? name;
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
            _logic.SaveHighScore(name, out var place, out var highScores, out var score);
            Console.WriteLine($"Ваш счёт: {score}. Ваше место №{place} в таблице лидеров! Ниже представлены лучшие результаты:");
            ShowHighScore(highScores);
            Console.WriteLine("Возвращайтесь как можно скорее!");
        }
    }

    [System.Text.RegularExpressions.GeneratedRegex("^[a-zA-Z0-9]+$")]
    private static partial System.Text.RegularExpressions.Regex MyRegex();
}