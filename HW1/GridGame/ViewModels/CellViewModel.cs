using GridGame.Models;
namespace GridGame.ViewModels;

public class CellViewModel : ViewModelBase
{
    // Row & Column tells us where this cell lives on the grid
    public uint Row { get; }  // not set; because the cell's position never changes, 
                              // automatically a readonly because its setted in the constructor
    public uint Column { get; }  // -||-
    private uint _value; // 0 -> empty, 1 -> wall, 2...n -> players
    public uint Value
    {
        get => _value;
        set
        {
            _value = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(BackgroundColor));
            OnPropertyChanged(nameof(IsEmpty));
            OnPropertyChanged(nameof(IsWall));
        }
    }
    
    public bool IsEmpty => _value == 0; // true if cell is not taken by anyone and is not a wall
    public bool IsWall => _value == 1; // true if cell is a wall

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

    private readonly List<string> _playerColors; // list of player colors, so we know what color to paint the cell
    
    // color should this cell get painted
    public string BackgroundColor
    {
        get
        {
            if (_isSelected) return "#FFFFFF"; // white when selected
            if (IsWall) return "#333333";      // dark grey for walls
            if (IsEmpty) return "#1A1A1A";     // near black for empty

            // _value is a player ID starting at 2
            // so player index in the list is _value - 2
            int playerIndex = (int)_value - 2;
            if (playerIndex >= 0 && playerIndex < _playerColors.Count)
                return _playerColors[playerIndex];

            return "#1A1A1A"; // fallback
        }
    }


    public CellViewModel(uint row, uint column, uint value, List<string> playerColors) //constructor, needs to know its position, value and player colors to know how to paint itself
    {
        Row = row;
        Column = column;
        _value = value;
        _isSelected = false;
        _playerColors = playerColors;
    }
}