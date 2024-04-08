using System.Diagnostics.CodeAnalysis;

namespace SadConsoleGame;

internal class Map
{
    private ScreenSurface mapSurface;
    public ScreenSurface SurfaceObject => mapSurface;
    public GameObject player { get; set; }

    SadFont SquareFont = (SadFont) GameHost.Instance.LoadFont("./fonts/CheepicusExtended.font");
    public Map(int mapWidth, int mapHeight, int xPosition, int yPosition)
    {
        mapSurface = new ScreenSurface(mapWidth, mapHeight);
        mapSurface.Position= new Point(xPosition, yPosition);
        mapSurface.UseMouse = false;
        mapSurface.Font= SquareFont;




        FillBackground();

        player = new GameObject(new ColoredGlyph(Color.White, Color.Black, '@'), mapSurface.Surface.Area.Center, mapSurface);
    }

    private void FillBackground()
    {
        Color[] colors = new[] { Color.LightGreen, Color.Coral, Color.CornflowerBlue, Color.DarkGreen };
        float[] colorStops = new[] { 0f, 0.35f, 0.75f, 1f };

        Algorithms.GradientFill(mapSurface.FontSize,
                                mapSurface.Surface.Area.Center,
                                mapSurface.Surface.Width / 3,
                                45,
                                mapSurface.Surface.Area,
                                new Gradient(colors, colorStops),
                                (x, y, color) => mapSurface.Surface[x, y].Background = color);
    }
}
