// This file represents the set up screen. 

using System.Collections.ObjectModel;

namespace GridGame.ViewModels;

public class SetupPlayerViewModel : ViewModelBase // One player during setup
{
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private string _color = "#FF0000"; // Default to red
    public string Color
    {
        get => _color;
        set
        {
            _color = value;
            OnPropertyChanged();
        }
    }

    public SetupPlayerViewModel(string initialColor)
    {
        _color = initialColor;
    }
}

public class SetupViewModel : ViewModelBase // Main setup view model
{
    public ObservableCollection<string> AvailableColors { get; } = new()
    {
        "#FF0000", 
        "#00FF00", 
        "#0000FF", 
        "#FF00FF", 
        "#FFFF00", 
        "#00FFFF", 
        "#FFA500", 
        "#FFFFFF"  
    };

  
    public List<string> AvailableSizes { get; } = new()   // Available board sizes
    {
        "6x6", "8x8", "10x10"
    };

    // Number of players (2-5)
    private uint _playerCount = 2;
    public uint PlayerCount
    {
        get => _playerCount;
        set
        {
            if (value < 2 || value > 5) return;
            _playerCount = value;
            OnPropertyChanged();
            RebuildPlayerList(); // Rebuild the player list when count changes
        }
    }

    public ObservableCollection<SetupPlayerViewModel> Players { get; } = new(); // The list of players being set up

    private string _selectedSize = "6x6"; // Selected board size (e.g. "6x6")
    public string SelectedSize
    {
        get => _selectedSize;
        set
        {
            _selectedSize = value;
            OnPropertyChanged();
        }
    }

    // Whether we can proceed w/ the game (all players must have names)
    public bool CanStart => Players.All(p => !string.IsNullOrWhiteSpace(p.Name)); 

    public SetupViewModel()
    {
        // Start with 2 players
        RebuildPlayerList();
    }

    // Rebuilds the player list when player count changes. Assigns default colors to each
    private void RebuildPlayerList()
    {
        Players.Clear();

        // Cycle through available colors
        for (uint i = 0; i < _playerCount; i++)
        {
            string color = AvailableColors[(int)(i % AvailableColors.Count)];
            Players.Add(new SetupPlayerViewModel(color));
        }

        OnPropertyChanged(nameof(CanStart));
    }

    // Called whenever a player's name changes. Checks if all players have names so CanStart updates
    public void OnPlayerNameChanged()
    {
        OnPropertyChanged(nameof(CanStart));
    }

    // Called when user clicks Start Game. Returns the setup data to pass to GameViewModel
    public (uint Rows, uint Columns, List<(string Name, string Color)> Players) GetGameSetup()
    {
        // Parse "6x6/8x8/10x10" into rows and columns.. eg.  gives ["6", "6"] → size = 6
        var sizeParts = SelectedSize.Split('x');
        uint size = uint.Parse(sizeParts[0]);

        // Collect player names and colors
        var playerData = Players
            .Select(p => (p.Name, p.Color))
            .ToList();

        return (size, size, playerData);
    }
}