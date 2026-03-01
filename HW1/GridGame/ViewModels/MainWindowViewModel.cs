// It will be the top-level ViewModel for the MainWindow, managing the overall game state and interactions

namespace GridGame.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
     private object _currentView  = null!;
    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public MainWindowViewModel()
    {
        // Start the app on the setup screen
        CurrentView = new SetupViewModel();
    }
}