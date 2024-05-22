using GoRogue.DiceNotation;
using SadConsoleGame.entities;

namespace SadConsoleGame.entities.creatures
{
    internal class Human : Creature
    {
        //strength 2-5
        //dexterity 3-6
        //constitution 4-7
        //intuition 3-5
        //charisma 4-8

        public string name;
        public Human(string name,int level, Point position, IScreenSurface hostingSurface) : base(level, Dice.Roll("1d3+1"), Dice.Roll("1d4+2"), Dice.Roll("1d4+3"), Dice.Roll("1d3+2"), Dice.Roll("1d5+3"), true, false, new ColoredGlyph(Color.Red, Color.Black, 'H'), position, hostingSurface)
        {
            this.name = name;
        }
    }
}
