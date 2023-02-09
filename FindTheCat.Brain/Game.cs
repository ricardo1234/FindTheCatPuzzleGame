using FindTheCat.Brain.Enums;
using FindTheCat.Brain.Models;
using FindTheCat.Brain.Models.Computer;
using System.Runtime;

namespace FindTheCat.Brain;

public sealed class Game {
    private readonly Settings _settings;
    private BoxStates[] _boxes;
    private int _history;
    private readonly ComputerBrain _computerBrain;
    private readonly Action _playerTurn;
    public Game(Settings settings, Action PlayerTurn)
    {
        _settings = settings;
        _playerTurn = PlayerTurn;
        _boxes = Enumerable.Repeat(BoxStates.Empty, _settings.NumOfBoxs).ToArray();
        if (_settings.Cat == PlayerTypes.Computer || _settings.Finder == PlayerTypes.Computer)
            _computerBrain = new ComputerBrain(
                    new ComputerSettings() { 
                        Dificulty = _settings.Dificulty, 
                        NumOfBox = _settings.NumOfBoxs, 
                        Type = _settings.Cat == PlayerTypes.Computer ? ComputerType.Cat : ComputerType.Finder
                    });
    }

    public void StartGame()
    {
        if(_settings.Cat == PlayerTypes.Computer)
        {
            var position = _computerBrain.GetNextMove(null);
            _boxes[position] = BoxStates.Cat;
        }

        _playerTurn.Invoke();
    }

    //receber player move
    //validar se acabou
    //computador jogar
    //chamar player outra vez


    public void ComputerMove()
    {

    }

}
