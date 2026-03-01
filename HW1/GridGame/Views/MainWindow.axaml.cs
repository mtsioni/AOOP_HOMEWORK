using Avalonia.Controls;

namespace GridGame.Views;

// "partial" links this file to MainWindow.axaml
// together they form one complete class
public partial class MainWindow : Window
{
    public MainWindow()
    {
        // Reads the MainWindow.axaml file and builds
        // all the UI elements defined in it
        InitializeComponent();
    }
}