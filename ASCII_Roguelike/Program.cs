using SadConsole.Configuration;
using SadConsoleGame;


Settings.WindowTitle = "My SadConsole Game";

Builder configuration = new Builder()
    .SetScreenSize(GameSettings.GameWidth, GameSettings.GameHeight)
    //.SetStartingScreen<RootScreen>()
    .SetStartingScreen<CharCreationScreen>()
    .IsStartingScreenFocused(true)
    .ConfigureFonts((fConfig, game) =>
    {
        // Default font 
        fConfig.UseCustomFont("fonts/ThinExtended.font");
        // Map font 
        fConfig.AddExtraFonts("fonts/CheepicusExtended.font");
    });
;


Game.Create(configuration);
Game.Instance.Run();
Game.Instance.Dispose();

