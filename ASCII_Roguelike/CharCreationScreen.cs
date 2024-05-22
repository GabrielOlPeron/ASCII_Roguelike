using GoRogue.DiceNotation;
using SadConsole.UI.Controls;
using SadConsole.UI;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole.Configuration;
using System.Reflection.Metadata;
using GoRogue.GameFramework;

namespace SadConsoleGame
{


    internal class CharCreationScreen : ScreenObject
    {
        

        private Console charCreation;

        public string race;
        public string charBackground = "";
        public int strength;
        public int dexterity;
        public int constitution;
        public int intuition;
        public int charisma;

        private int str;
        private int dex;
        private int con;
        private int intu;
        private int chari;

        
        public CharCreationScreen()
        {
            

            
            CharCreation();

            
        }

        private Console DrawConsole(Console console, bool hasBorder, int width, int height, int xPosition, int yPosition, Color backgroundColor, Color borderColor)
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


            Children.Add(console);
            return console;
        }

        private void CharCreation()
        {
            ControlHost controls = new();

            //character Creation base screen
            charCreation =DrawConsole(charCreation, false, GameSettings.GameWidth, GameSettings.GameHeight, 0, 0, Color.Black, Color.White);
            charCreation.Cursor.IsEnabled = false;
            charCreation.Cursor.IsVisible = false;

            //flavor text surface
            ScreenSurface flavorText = new(55, 4);
            flavorText.Position = new Point(charCreation.Surface.Area.Center.X - (flavorText.Width / 2), 40);
            flavorText.UseMouse = false;

            //stats surface
            ScreenSurface statsText = new(52, 10);
            statsText.Position = new Point(charCreation.Surface.Area.Center.X + 40, 20);
            statsText.UseMouse = false;



            charCreation.Children.Add(flavorText);

            charCreation.SadComponents.Add(controls);

            //buttons
            //races
            var human = new Button3d(7, 3);
            human.Position = new Point((charCreation.Surface.Area.Center.X - (human.Width / 2)) - 15, 10);
            human.Text = "Human";
            controls.Add(human);

            var elf = new Button3d(7, 3);
            elf.Position = new Point(charCreation.Surface.Area.Center.X - (elf.Width / 2), 10);
            elf.Text = "Elf";
            controls.Add(elf);

            var dwarf = new Button3d(7, 3);
            dwarf.Position = new Point((charCreation.Surface.Area.Center.X - (dwarf.Width / 2)) + 15, 10);
            dwarf.Text = "Dwarf";
            controls.Add(dwarf);
            //backgrounds
            var peasant = new Button(27, 3);
            peasant.Position = new Point(charCreation.Surface.Area.Center.X - (42 + 26), 19);
            peasant.Text = "PEASANT: +2 str, +3 cons";

            var noble = new Button(27, 3);
            noble.Position = new Point(charCreation.Surface.Area.Center.X - (42 + 26), 21);
            noble.Text = "NOBLE:   +3 dex, +3 char";


            var soldier = new Button(27, 3);
            soldier.Position = new Point(charCreation.Surface.Area.Center.X - (42 + 26), 23);
            soldier.Text = "SOLDIER: +4 str, +2 cons";


            //misc
            var next = new Button(10, 3);
            next.Position = new Point(135, 42);
            next.Text = "NEXT";

            var reRoll = new Button(10, 3);
            reRoll.Position = new Point(charCreation.Surface.Area.Center.X + 42, 17);
            reRoll.Text = "Reroll";


            charCreation.Print(charCreation.Surface.Area.Center.X - 24 / 2, 5, "--:|| WHAT YOU ARE ||:--");

            //buttons click
            human.Click += (sender, args) => {
                controls.Add(reRoll);
                controls.Add(peasant);
                controls.Add(noble);
                controls.Add(soldier);

                race = "human";
                Roll(race, statsText);
                charCreation.Children.Add(statsText);

                charCreation.Print(charCreation.Surface.Area.Center.X - (42 + 27), 17, "-- WHAT IS YOUR BACKGROUND --");

                flavorText.Clear();
                flavorText.Print(1, 1, "Humans may not possess the raw strength of dwarves or");
                flavorText.Print(1, 2, "the agility of elves, but their versatile nature");
                flavorText.Print(1, 3, "enable them to thrive in any situation.");
            };

            elf.Click += (sender, args) => {
                controls.Add(reRoll);
                controls.Add(peasant);
                controls.Add(noble);
                controls.Add(soldier);

                race = "elf";
                Roll(race, statsText);
                charCreation.Children.Add(statsText);

                flavorText.Clear();
                flavorText.Print(1, 1, "Although they lack brute strength, with excellent ");
                flavorText.Print(1, 2, "intuition and great agility, elves dance effortlessly");
                flavorText.Print(1, 3, "across the battlefield.");
            };

            dwarf.Click += (sender, args) => {
                controls.Add(reRoll);
                controls.Add(peasant);
                controls.Add(noble);
                controls.Add(soldier);

                race = "dwarf";
                Roll(race, statsText);
                charCreation.Children.Add(statsText);

                flavorText.Clear();
                flavorText.Print(1, 1, "Dwarves have brute strength and unmatched resilience, ");
                flavorText.Print(1, 2, "though their steps may lack the grace of the more ");
                flavorText.Print(1, 3, "agile races.");
            };



            peasant.Click += (sender, args) => {
                controls.Add(next);

                strength = str + 2;
                dexterity = dex;
                constitution = con + 3;
                intuition = intu;
                charisma = chari;


                charBackground = "peasant";

                PrintStats(statsText);
            };

            noble.Click += (sender, args) => {
                controls.Add(next);

                strength = str;
                dexterity = dex + 3;
                constitution = con;
                intuition = intu;
                charisma = chari + 3;

                charBackground = "noble";

                PrintStats(statsText);
            };

            soldier.Click += (sender, args) => {
                controls.Add(next);

                strength = str + 4;
                dexterity = dex;
                constitution = con + 2;
                intuition = intu;
                charisma = chari;

                charBackground = "soldier";

                PrintStats(statsText);
            };

            reRoll.Click += (sender, args) => {
                Roll(race, statsText);
            };

            next.Click += (sender, args) => {
                charCreation.Clear();
                statsText.Clear();
                flavorText.Clear();
                charCreation.SadComponents.Remove(controls);
                Children.Remove(charCreation);


                

                Game.Instance.Screen = new RootScreen(race,charBackground,strength,dexterity,constitution,intuition,charisma);
                Game.Instance.Screen.IsFocused = true;
                Game.Instance.DestroyDefaultStartingConsole();
                
                
            };
        }

        private void Roll(string race, ScreenSurface statsText)
        {
            switch (race)
            {
                case "human":
                    strength = Dice.Roll("1d3+1");
                    dexterity = Dice.Roll("1d4+2");
                    constitution = Dice.Roll("1d4+3");
                    intuition = Dice.Roll("1d3+2");
                    charisma = Dice.Roll("1d5+3");
                    break;

                case "elf":
                    break;

                case "dwarf":
                    break;
            }

            str = strength;
            dex = dexterity;
            con = constitution;
            intu = intuition;
            chari = charisma;

            PrintStats(statsText);
        }

        private void PrintStats(ScreenSurface statsText)
        {
            statsText.Clear();
            statsText.Print(1, 1, "Strength - " + strength);
            statsText.Print(1, 3, "Dexterity - " + dexterity);
            statsText.Print(1, 5, "Constitution - " + constitution);
            statsText.Print(1, 7, "Intuition - " + intuition);
            statsText.Print(1, 9, "Charisma - " + charisma);
        }


    }


}
