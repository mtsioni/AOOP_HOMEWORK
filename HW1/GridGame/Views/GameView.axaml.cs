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
    private void OnCellClicked(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button) return;
        if (DataContext is not GameViewModel vm) return;

        if (button.DataContext is CellViewModel cell)
        {
            vm.SelectCell(cell);
        }
    }

    private void OnConfirmClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is GameViewModel vm)
        {
            vm.ConfirmMove();
        }
    }

    private void OnSaveClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is GameViewModel vm)
            vm.RequestSave();
    }

    private void OnResetClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is GameViewModel vm)
            vm.ResetGame();
    }

    private void OnPresetChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is GameViewModel vm)
        {
            vm.LoadPreset();
        }
    }
}