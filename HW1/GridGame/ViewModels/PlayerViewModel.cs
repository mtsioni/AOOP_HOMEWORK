using System.ComponentModel;
using GridGame.Models;

namespace GridGame.ViewModels;

public class PlayerViewModel : ViewModelBase
{
    private readonly Player _player;
    public uint PlayerID => _player.PlayerID;
    public string DisplayName => _player.Name; //name showed in the player's list
    public string Color => _player.PlayerColor;
    public string DisplayColor => IsActive ? Color : "#555555";
    public uint? MovesLeft => _player.MovesLeft; // ? = can be null (before the game calculates it)
    private bool _isActive; //is true when it's this player's turn
    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(DisplayColor));
        }
    }
    public PlayerViewModel(Player player) //constructor
    {
        _player = player;
        _isActive = false;
    }

    public void Refresh() // method for when the player's data change and it needs update
    {
        OnPropertyChanged(nameof(MovesLeft));
        OnPropertyChanged(nameof(DisplayColor));
    }
}