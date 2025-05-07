namespace FindTheCat.Brain.Models.Computer;

public sealed class ComputerBrain
{
    private readonly ComputerSettings _settings;
    private readonly Random _random;
    public ComputerBrain(ComputerSettings settings)
    {
        _settings = settings;
        _random = new Random();
    }

    public int GetNextMove(int? place)
    {
        switch (_settings.Dificulty)
        {
            case Enums.Dificulty.Random:
                return _settings.Type == Enums.ComputerType.Cat ? RandomCatMove(place) : RandomFinderMove();
            case Enums.Dificulty.Algorithm:
                break;
            case Enums.Dificulty.Cheating:
                break;
            default:
                break;
        }
        return 0;
    }

    private int RandomCatMove(int? place)
    {
        if (place == null)
            return _random.Next(0, _settings.NumOfBox);

        if (place == 0)
            return 1;

        if (place == _settings.NumOfBox - 1)
            return _settings.NumOfBox - 2;

        if (_random.Next(0, 2) == 0)
            return (int)--place;

        return (int)++place;
    }

    private int RandomFinderMove() => _random.Next(0, _settings.NumOfBox);
}
