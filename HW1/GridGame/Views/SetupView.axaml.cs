using Avalonia.Controls;
using Avalonia.Interactivity;
using GridGame.ViewModels;

namespace GridGame.Views;

public partial class SetupView : UserControl // "code-behind" for SetupView.axaml
{
    public SetupView()
    {
        InitializeComponent(); // Initialize the AXAML UI
        
        DataContext = new SetupViewModel(); // Create and attach the ViewModel
    }

    // Called when the Start Game button is clicked
    private void OnStartGameClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not SetupViewModel vm) return;

        var (rows, columns, players) = vm.GetGameSetup(); // Get the setup data

        var gameVM = new GameViewModel(rows, columns, players); // Create the GameViewModel with this setup data


        // Switch the main window to show GameView instead of SetupView
        // to be implemeted....
        
        // For now, I will just show  log success
        System.Diagnostics.Debug.WriteLine($"Starting game with {players.Count} players on {rows}x{columns}");
    }
}