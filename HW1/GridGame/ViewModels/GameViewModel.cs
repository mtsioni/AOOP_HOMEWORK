// It will basically be the game board, current turn, player list, scores, etc.

using System.Collections.ObjectModel;
using GridGame.Models;
using GridGame.Services;

namespace GridGame.ViewModels;

public class GameViewModel : ViewModelBase
{
    private readonly GameCoordinator _coordinator; //game engine, (thanks for calling it an engine)
    public int Rows => _coordinator.Grid.Properties.Rows;
    public int Columns => _coordinator.Grid.Properties.Columns;
    public ObservableCollection<CellViewModel> Cells { get; } // is like a List but the UI automatically updates when items are added or removed
    public ObservableCollection<PlayerViewModel> Players { get; } // players
    
    // Dynamic presets list (PAVEL FILL THIS FROM JSON)
    public ObservableCollection<Map> Maps { get; } = new();
    public int MapIndex {get; set;} // selected map's index
    public MapHandler _mapHandler;

    public double GridDisplayWidth => (_coordinator.Grid.Properties.Columns + 1) * 54; // 50 for the cell size + 2 for the border; we add 1 to columns because we also show the right border of the last column

    // ─── Turn info ───
    private int _turnNumber;
    public int TurnNumber
    {
        get => _turnNumber;
        set
        {
            _turnNumber = value;
            OnPropertyChanged();
        }
    }

    // The name and color of the player whose turn it is
    private string _currentPlayerName = string.Empty;
    public string CurrentPlayerName
    {
        get => _currentPlayerName;
        set
        {
            _currentPlayerName = value;
            OnPropertyChanged();
        }
    }

    private string _currentPlayerColor = "#FFFFFF";
    public string CurrentPlayerColor
    {
        get => _currentPlayerColor;
        set
        {
            _currentPlayerColor = value;
            OnPropertyChanged();
        }
    }

    // ─── Winning display ─────
    private string _winMessage = string.Empty;
    public string WinMessage
    {
        get => _winMessage;
        set
        {
            _winMessage = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsGameOver));
        }
    }

    // true when game is over... UI uses this to show/hide the win banner
    public bool IsGameOver => !string.IsNullOrEmpty(_winMessage);

    // ─── Available moves ────
    private int _availableMoves;
    public int AvailableMoves
    {
        get => _availableMoves;
        set
        {
            _availableMoves = value;
            OnPropertyChanged();
        }
    }

    private CellViewModel? _selectedCell; // the cell the current player has clicked but not confirmed yet

    // ─── Constructor ──────
    public GameViewModel(List<(string Name, string Color)> playerSetup)
    {
        _mapHandler = new MapHandler();
        for (int i = 0; i < _mapHandler.MapData.Saves.Count; i++)
        {
            Maps.Add(_mapHandler.LoadMap(i));
        }
        var playerModels = playerSetup // player id's start at 2 (0=empty, 1=wall)
            .Select((p, i) => new Player(i + 2, p.Color, p.Name))
            .ToList();

        MapIndex = 0;
        var grid = new Grid(Maps[MapIndex]);

        _coordinator = new GameCoordinator(grid, playerModels);
        Players = new ObservableCollection<PlayerViewModel>(     // build PlayerViewModels
            playerModels.Select(p => new PlayerViewModel(p))
        );

        Cells = new ObservableCollection<CellViewModel>();  // build CellViewModels; one per cell
        for (int r = 0; r < Maps[MapIndex].Rows+1; r++)
        {
            for (int c = 0; c < Maps[MapIndex].Columns+1; c++)
            {
                Cells.Add(new CellViewModel(r, c, Maps[MapIndex].Cells[_coordinator.Grid.RowColumnToIndex(r, c)], _coordinator.Players));
            }
        }
        RefreshGrid();
        RefreshTurnInfo();
        RefreshPlayers();
    }

    // Called when user chooses a preset (PAVEL plug JSON here later)
    public void LoadPreset()
    {
        _coordinator.Grid.SetGrid(Maps[MapIndex]);
        Cells.Clear();
        for (int r = 0; r < Maps[MapIndex].Rows + 1; r++)
        {
            for (int c = 0; c < Maps[MapIndex].Columns + 1; c++)
            {
                Cells.Add(new CellViewModel(r, c, Maps[MapIndex].Cells[_coordinator.Grid.RowColumnToIndex(r, c)], _coordinator.Players));
            }
        }

        RefreshGrid();
        RefreshTurnInfo();
        RefreshPlayers();
    }

    // Placeholder: PAVEL implement save later
    public void RequestSave()
    {
        _mapHandler.Save();
    }

    public void ResetGame()
    {
        int mapIndexTemp = MapIndex;
        _mapHandler.ResetMap(MapIndex);
        Map map = _mapHandler.MapData.Saves[MapIndex];
        map.Name = Maps[MapIndex].Name;
        Maps[MapIndex] = map;
        MapIndex = mapIndexTemp;
        _coordinator.Grid.SetGrid(Maps[MapIndex]);
        _coordinator.TotalTurns = 0;
        _coordinator.GameStatus = 0;
        _coordinator.LastMoveHolder = null;
        _coordinator.Turn = 0;

        RefreshGrid();
        RefreshTurnInfo();
        RefreshPlayers();

        WinMessage = "";
        CheckWinCondition();
    }

    // ─── Cell selection ───
    public void SelectCell(CellViewModel cell)  // called when the user clicks a cell
    {
        if (_selectedCell != null)
            _selectedCell.IsSelected = false; // deselect the previous cell if any

        _selectedCell = cell; // select the new cell
        cell.IsSelected = true;
    }

    // ─── Confirm move ───
    public void ConfirmMove() // called when the user clicks the Confirm button
    {
        if (_selectedCell == null) return;

        // tell the coordinator which cell the player selected
        var currentPlayer = _coordinator.Players[_coordinator.Turn];
        currentPlayer.SelectedRow = _selectedCell.Row;
        currentPlayer.SelectedColumn = _selectedCell.Column;

        // ask the coordinator to process the move
        _coordinator.AdvanceGame();

        // deselect the cell visually
        _selectedCell.IsSelected = false;
        _selectedCell = null;

        // refresh everything the UI shows
        RefreshGrid();
        RefreshTurnInfo();
        RefreshPlayers();
        CheckWinCondition();
    }

    // ─── Refresh helpers ──────
    private void RefreshGrid() // reads the grid from the coordinator and updates all CellViewModels
    {
        for (int i = 0; i < Cells.Count; i++)
        {
            Cells[i].Value = _coordinator.Grid.Properties.Cells[i];
        }
    }

    private void RefreshTurnInfo()  // updates turn number, current player name and color
    {
        TurnNumber = _coordinator.TotalTurns;
        var current = Players[_coordinator.Turn];
        CurrentPlayerName = current.DisplayName;
        CurrentPlayerColor = current.Color;
        AvailableMoves = _coordinator.Players[_coordinator.Turn].MovesLeft ?? 0;
    }

    private void RefreshPlayers() // tells each PlayerViewModel whether it is their turn
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].IsActive = (i == _coordinator.Turn);
            Players[i].Refresh();
        }
    }

    // Checks if the game is over and sets the win message
    private void CheckWinCondition()
    {
        if (_coordinator.GameStatus == 1)
        {
            WinMessage = "It's a draw!";
        }
        else if (_coordinator.GameStatus > 1)
        {
            var winner = Players.First(p => p.PlayerID == _coordinator.GameStatus);
            WinMessage = $"{winner.DisplayName} wins!";
        }
    }
}