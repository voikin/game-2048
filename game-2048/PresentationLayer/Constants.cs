namespace game_2048.PresentationLayer;

public struct Constants
{
    public static readonly Dictionary<int, ConsoleColor> Color = new()
    {
        { 0, ConsoleColor.Black }, { 2, ConsoleColor.DarkRed }, { 4, ConsoleColor.Red }, 
        { 8, ConsoleColor.DarkYellow }, { 16, ConsoleColor.Yellow }, { 32, ConsoleColor.DarkGreen }, 
        { 64, ConsoleColor.Green }, { 128, ConsoleColor.DarkCyan }, { 256, ConsoleColor.Cyan }, 
        { 512, ConsoleColor.DarkBlue }, { 1024, ConsoleColor.Blue }, { 2048, ConsoleColor.Magenta }
    };
}