namespace game_2048.LogicLayer.Models;

public record GameData
{
    public bool IsGame { get; set; } = false;

    public GameDeck Deck = new();
}
