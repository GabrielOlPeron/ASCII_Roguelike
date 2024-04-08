namespace SadConsoleGame;

internal class RootScreen : ScreenObject
{
    private Console stats;
    private Console log;
    private Map map;
    public RootScreen()
    {
        //Game.Instance.StartingConsole.Font = Game.Instance.Fonts["fonts/ThinExtended.font"];
        // stats console
        CreateConsole(stats,true, 35, 50, 125, 0, Color.Black, Color.AliceBlue);

        // log console
        CreateConsole(log, true, 125, 10, 0, 40, Color.Black, Color.AliceBlue);

       
        //map
        map =new Map(71,38,1,1);
        Children.Add(map.SurfaceObject);

        
    }

    private void CreateConsole(Console console,bool hasBorder,int width,int height,int xPosition,int yPosition, Color backgroundColor, Color borderColor)
    {
        console = new(width, height);
        console.Position = (xPosition, yPosition);
        console.Surface.DefaultBackground = backgroundColor;
        console.Clear();
        console.Cursor.IsEnabled = false;
        console.Cursor.IsVisible = false;
        console.Cursor.MouseClickReposition = false;
        console.IsFocused = false;
        console.FocusOnMouseClick = true;

        


        if (hasBorder)
        {
            console.DrawBox(new Rectangle(0, 0, console.Width, console.Height), ShapeParameters.CreateStyledBoxThin(borderColor));

        }


        Children.Add(console);
    }
}
