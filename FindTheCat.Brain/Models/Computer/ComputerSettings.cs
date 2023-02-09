using FindTheCat.Brain.Enums;

namespace FindTheCat.Brain.Models.Computer;

public sealed class ComputerSettings
{
    public int NumOfBox { get; init; }
    public ComputerType Type { get; init; }
    public Dificulty Dificulty { get; init; }
}
