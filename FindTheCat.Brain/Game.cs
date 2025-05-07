using FindTheCat.Brain.Enums;
using FindTheCat.Brain.Models;
using FindTheCat.Brain.Models.Computer;
using System.Security.Cryptography;

namespace FindTheCat.Brain;

public sealed class Game {
    private readonly Settings _settings;
    private List<BoxStates> _boxes;
    private List<int> _catHistory;
    private List<int> _finderHistory;
    private readonly ComputerBrain? _computerBrain;
    private readonly Func<Task>? _playerCatTurn;
    private readonly Func<Task>? _playerFinderTurn;
    private GameStates _gameState;
    public Game(Settings settings, Func<Task>? playerCatTurn = null, Func<Task>? playerFinderTurn = null)
    {
        _settings = settings;
        if (_settings.Cat == PlayerTypes.Human && playerCatTurn == null)
        {
            throw new ArgumentException("When cat is a human you need to give action for it to be able to play", nameof(playerCatTurn));
        }

        if (_settings.Finder == PlayerTypes.Human && playerFinderTurn == null)
        {
            throw new ArgumentException("When finder is a human you need to give action for it to be able to play", nameof(playerFinderTurn));
        }

        _playerCatTurn = playerCatTurn;
        _playerFinderTurn = playerFinderTurn;
        _boxes = Enumerable.Repeat(BoxStates.Empty, _settings.NumOfBoxs).ToList();
        if (_settings.Cat == PlayerTypes.Computer || _settings.Finder == PlayerTypes.Computer)
            _computerBrain = new ComputerBrain(
                    new ComputerSettings() { 
                        Dificulty = _settings.Dificulty, 
                        NumOfBox = _settings.NumOfBoxs, 
                        Type = _settings.Cat == PlayerTypes.Computer ? ComputerType.Cat : ComputerType.Finder
                    });

        _catHistory = [];
        _gameState = GameStates.Starting;
        _finderHistory = [];
    }

    public async void StartGame()
    {
        await EvokeNextGameStateMove();
    }

    public List<BoxStates> BoxState => _boxes;
    public GameStates GameState => _gameState;

    public async void PlayerMove(int position)
    {
        if (position < 0 || position >= _settings.NumOfBoxs)
            throw new Exception("Invalid move");

        if (_gameState == GameStates.CatTurn)
        {
            if (_boxes[position] != BoxStates.Empty)
                throw new Exception("Invalid move");

            var lastPosition = _catHistory.Last();

            if (lastPosition - 1 != position && lastPosition + 1 != position)
                throw new Exception("Invalid move");

            SetCatPosition(position);

            await EvokeNextGameStateMove();
            return;
        }

        _finderHistory.Append(position);

        if (IsGameOver(position))
            return;

        await EvokeNextGameStateMove();
    }

    private void SetCatPosition(int position)
    {
        if(_catHistory.Count != 0)
            _boxes[_catHistory.Last()] = BoxStates.Empty;

        _boxes[position] = BoxStates.Cat;
        _catHistory.Add(position);
    }

    private async Task ComputerMove()
    {
        if (_gameState == GameStates.CatTurn && _settings.Cat == PlayerTypes.Computer)
        {
            var position = _computerBrain!.GetNextMove(_catHistory.Count == 0 ? null : _catHistory.Last());
            SetCatPosition(position);
            await EvokeNextGameStateMove();
        }

        if (_gameState == GameStates.FinderTurn && _settings.Finder == PlayerTypes.Computer)
        {
            var position = _computerBrain!.GetNextMove(_finderHistory.Last());
          
            _finderHistory.Add(position);

            if (IsGameOver(position))
                return;

            await EvokeNextGameStateMove();
        }
    }

    private async Task EvokeNextGameStateMove()
    {
        if (GameState == GameStates.CatTurn)
        {
            _gameState = GameStates.FinderTurn;
            if (_settings.Finder == PlayerTypes.Computer)
                await ComputerMove();
            else
                await _playerFinderTurn.Invoke();
            return;
        }

        _gameState = GameStates.CatTurn;
        if (_settings.Cat == PlayerTypes.Computer)
            await ComputerMove();
        else
            await _playerCatTurn.Invoke();
    }

    private bool IsGameOver(int finderGuess)
    {
        if (_boxes[finderGuess] == BoxStates.Cat)
        {
            _gameState = GameStates.Ended;
            return true;
        }

        return false;
    }
}
