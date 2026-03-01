using Avalonia;
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

     // Called whenever a player name TextBox changes
    private void OnPlayerNameChanged(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (DataContext is SetupViewModel vm)
        {
            // Tell ViewModel to re-check if we can start
            vm.OnPlayerNameChanged();
        }
    }

    // Called when the Start Game button is clicked
    private void OnStartGameClicked(object sender, RoutedEventArgs e)
    {
       if (DataContext is not SetupViewModel setupVM) return;

        // Get the setup data (player names, colors, board size)
        var (rows, columns, players) = setupVM.GetGameSetup();

        // Create the GameViewModel with this data
        var gameVM = new GameViewModel(rows, columns, players);

        // Find the root window and get its ViewModel
        var mainWindow = TopLevel.GetTopLevel(this) as Window;
        if (mainWindow?.DataContext is MainWindowViewModel mainVM)
        {
            // Switch the view to GameView by setting the GameViewModel
            mainVM.CurrentView = gameVM;
        }
    }
}