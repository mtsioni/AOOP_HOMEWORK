using Avalonia;

namespace GridGame;

class Program
{
    [STAThread] //to run on a single thread
    public static void Main(string[] args) =>
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args); // opens the window and starts the evet loop aka application

    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder
            .Configure<App>() //tells Avalonia that "App" is the starting point
            .UsePlatformDetect() // detects the OS and uses the correct renderer
            .WithInterFont() // loads the default font
            .LogToTrace(); //sends interal Avalonia logs to debug output //for debugging
}