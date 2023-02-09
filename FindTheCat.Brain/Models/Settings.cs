using FindTheCat.Brain.Enums;

namespace FindTheCat.Brain.Models;

public sealed class Settings
{
    public PlayerTypes Cat { get; init; }
    public PlayerTypes Finder { get; init; }
    public int NumOfBoxs { get; init; }
    public Dificulty Dificulty { get; init; }
}
