using GoRogue.DiceNotation;
using GoRogue.FOV;
using GoRogue.GameFramework;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives.GridViews;
using System.Reflection.Metadata;

namespace SadConsoleGame;


internal class RootScreen : ScreenObject
{
    private Console stats;
    private Console log;
    private Map map;

    int n = 99;
    public RootScreen()
    {
        int r = Dice.Roll("1d13+2");
        // stats console
        stats= CreateConsole(stats,true, 36, 50, 124, 0, Color.Black, Color.AliceBlue);

        // log console
        log= CreateConsole(log, true, 124, 11, 0, 39, Color.Black, Color.AliceBlue);

        //map
        map =new Map(71,38,1,1);
        Children.Add(map.SurfaceObject);

        
    }

    private Console CreateConsole(Console console,bool hasBorder,int width,int height,int xPosition,int yPosition, Color backgroundColor, Color borderColor)
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
        return console;
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        bool handled = false;

        if (keyboard.IsKeyPressed(Keys.Up))
        {
            MovePlayer(Direction.Up, handled);
            
        }
        else if (keyboard.IsKeyPressed(Keys.Down))
        {
            MovePlayer(Direction.Down, handled);
        }

        if (keyboard.IsKeyPressed(Keys.Left))
        {
            MovePlayer(Direction.Left, handled);
        }
        else if (keyboard.IsKeyPressed(Keys.Right))
        {
            MovePlayer(Direction.Right, handled);
        }
        return handled;
    }

    public void MovePlayer(Direction dir,bool handled)
    {
        map.player.Move(map.player.position + dir, map);

        handled = true;
        
    }

}
