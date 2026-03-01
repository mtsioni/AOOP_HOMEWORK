using Avalonia.Controls;
using Avalonia.Interactivity;
using GridGame.ViewModels;

namespace GridGame.Views;

public partial class GameView : UserControl
{
    public GameView()
    {
        InitializeComponent();
    }

    // Called when a grid cell is clicked
    private void OnCellClicked(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button) return;
        if (DataContext is not GameViewModel vm) return;

        // Find which cell was clicked by finding its index
        // The button's DataContext is the CellViewModel
        if (button.DataContext is CellViewModel cell)
        {
            vm.SelectCell(cell);
        }
    }

    // Called when Confirm Move button is clicked
    private void OnConfirmClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is GameViewModel vm)
        {
            vm.ConfirmMove();
        }
    }
}