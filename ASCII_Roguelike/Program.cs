using SadConsole.Configuration;

Settings.WindowTitle = "My SadConsole Game";


Builder configuration = new Builder().SetScreenSize(90, 30).UseDefaultConsole().OnStart(Startup);

Game.Create(configuration);
Game.Instance.Run();
Game.Instance.Dispose();

static void Startup(object? sender, GameHost host)
{
    Console startingConsole = Game.Instance.StartingConsole;
}