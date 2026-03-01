// It will basically be the game board, current turn, player list, scores, etc.

using System.Collections.ObjectModel;
using GridGame.Models;
using GridGame.Services;

namespace GridGame.ViewModels;

public class GameViewModel : ViewModelBase
{
    private readonly GameCoordinator _coordinator; //game engine
    public uint Rows => _coordinator.Grid.Rows;
    public uint Columns => _coordinator.Grid.Columns;
    public ObservableCollection<CellViewModel> Cells { get; } // is like a List but the UI automatically updates when items are added or removed
    public ObservableCollection<PlayerViewModel> Players { get; } // players
    public double GridDisplayWidth => (_coordinator.Grid.Columns + 1) * 52.0; // 50 for the cell size + 2 for the border; we add 1 to columns because we also show the right border of the last column

    // ─── Turn info ───
    private uint _turnNumber;
    public uint TurnNumber
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
    private uint _availableMoves;
    public uint AvailableMoves
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
    public GameViewModel(uint rows, uint columns, List<(string Name, string Color)> playerSetup)
    {
        var playerModels = playerSetup // player id's start at 2 (0=empty, 1=wall)
            .Select((p, i) => new Player((uint)(i + 2), p.Color, p.Name))
            .ToList();

        var playerColors = playerSetup.Select(p => p.Color).ToList(); // extract just the color list to share with all cells
        var grid = new Grid(rows, columns); // create the coordinator with an empty grid

        _coordinator = new GameCoordinator(grid, playerModels);
        Players = new ObservableCollection<PlayerViewModel>(     // build PlayerViewModels
            playerModels.Select(p => new PlayerViewModel(p))
        );

        Cells = new ObservableCollection<CellViewModel>();  // build CellViewModels; one per cell
        for (uint r = 0; r < rows; r++)
        {
            for (uint c = 0; c < columns; c++)
            {
                Cells.Add(new CellViewModel(r, c, 0, playerColors));
            }
        }

        RefreshTurnInfo(); // set initial turn display
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
        var currentPlayer = _coordinator.Players[(int)_coordinator.Turn];
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
            Cells[i].Value = _coordinator.Grid.Cells[i];
        }
    }

    private void RefreshTurnInfo()  // updates turn number, current player name and color
    {
        TurnNumber = _coordinator.TotalTurns;
        var current = Players[(int)_coordinator.Turn];
        CurrentPlayerName = current.DisplayName;
        CurrentPlayerColor = current.Color;
        AvailableMoves = _coordinator.Players[(int)_coordinator.Turn].MovesLeft ?? 0;
    }

    private void RefreshPlayers() // tells each PlayerViewModel whether it is their turn
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].IsActive = (i == (int)_coordinator.Turn);
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