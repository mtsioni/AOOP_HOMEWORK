using GridGame.Models;
using GridGame.Services;
namespace GridGame.ViewModels;

public class CellViewModel : ViewModelBase
{
    // Row & Column tells us where this cell lives on the grid
    public int Row { get; }  // not set; because the cell's position never changes, 
                              // automatically a readonly because it's set in the constructor
    public int Column { get; }  // -||- <- is this like a srawing that it's vertical?
    private int _value; // 0 -> empty, 1 -> wall, 2...n -> players
    public List<Player> Players {get; set;}
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(BackgroundColor));
        }
    }

    // whether this cell is currently selected by the active player
    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(BackgroundColor));
        }
    }
    
    // color should this cell get painted
    public string BackgroundColor
    {
        get
        {
            if (IsSelected) return "#FFFFFF"; // white when selected
            if (Value == 1) return "#333333";      // dark grey for walls
            if (Value == 0) return "#1A1A1A";     // near black for empty

            // _value is a player ID starting at 2
            // so player index in the list is _value - 2
            if ((Value-2) >= 0 && (Value-2) < Players.Count)
                return Players[Value-2].PlayerColor;

            return "#333333"; // if there is less players than were in the last save, turn their corpses into walls
        }
    }


    public CellViewModel(int row, int column, int value, List<Player> players) //constructor, needs to know its position, value and player colors to know how to paint itself
    {
        Row = row;
        Column = column;
        Value = value;
        IsSelected = false;
        Players = players;
    }
}