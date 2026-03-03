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
        if (DataContext is SetupViewModel vm) // WHAT IS THIS
        {
            // Tell ViewModel to re-check if we can start
            vm.OnPlayerNameChanged();
        }
    }

    private static (int[] Cells, int Rows, int Columns) LoadPreset1()
    {
        // Temporary: load fake hardcoded "Preset #1" size. Change here.
        int rows = 5;
        int cols = 5;

        int[] cells = new int[(rows + 1) * (cols + 1)];
        return (cells, rows, cols);
    }

    // Called when the Start Game button is clicked
    private void OnStartGameClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is not SetupViewModel setupVM) return;

        var players = setupVM.GetPlayers();

        var (cells, rows, cols) = LoadPreset1();

        var gameVM = new GameViewModel(cells, rows, cols, players);

        var mainWindow = TopLevel.GetTopLevel(this) as Window;
        if (mainWindow?.DataContext is MainWindowViewModel mainVM)
        {
            mainVM.CurrentView = gameVM;
        }
    }
}