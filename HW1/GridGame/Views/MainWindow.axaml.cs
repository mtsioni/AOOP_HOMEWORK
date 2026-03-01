using Avalonia.Controls;
using GridGame.ViewModels;
namespace GridGame.Views;

// "partial" links this file to MainWindow.axaml; together they form one complete class
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent(); // reads the MainWindow.axaml file and builds all the UI elements defined in it
        DataContext = new MainWindowViewModel();  // attach the MainWindowViewModel
    }
}