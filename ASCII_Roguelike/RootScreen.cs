
using GoRogue.DiceNotation;
using SadConsole;
using SadConsole.Entities;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadConsoleGame.entities.creatures;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace SadConsoleGame;


internal class RootScreen : ScreenObject
{
    private Console stats;
    private Console log;
    private Map map;


    public string race;
    public string charBackground;
    public int strength;
    public int dexterity;
    public int constitution;
    public int intuition;
    public int charisma;

    public RootScreen(string race,string bg,int str,int dex,int cons,int intu,int chari)
    {
        this.race = race;
        this.charBackground = bg;
        this.strength = str;
        this.dexterity = dex;
        this.constitution = cons;
        this.intuition = intu;
        this.charisma = chari;  


        
        StartGame();  
    }

    private Console DrawConsole(Console console,bool hasBorder,int width,int height,int xPosition,int yPosition, Color backgroundColor, Color borderColor)
    {
        console = new(width, height);
        console.Position = (xPosition, yPosition);
        console.Surface.DefaultBackground = backgroundColor;
        console.Clear();
        console.Cursor.IsEnabled = false;
        console.Cursor.IsVisible = false;
        console.Cursor.MouseClickReposition = false;
        console.IsFocused = false;
        console.FocusOnMouseClick = false;
        
        


        if (hasBorder)
        {
            console.DrawBox(new Rectangle(0, 0, console.Width, console.Height), ShapeParameters.CreateStyledBoxThin(borderColor));

        }


        this.Children.Add(console);
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
        map.player.Move(dir, map);
        handled = true;
        
    }

    private void StartGame()
    {

        // stats console
        stats = DrawConsole(stats, true, 36, 50, 124, 0, Color.Black, Color.AliceBlue);

        // log console
        log = DrawConsole(log, true, 124, 11, 0, 39, Color.Black, Color.AliceBlue);

        //map
        map = new Map(71, 38, 1, 1, race, charBackground, strength, dexterity, constitution, intuition, charisma);
        this.Children.Add(map.mapSurface);

        




    }
}
