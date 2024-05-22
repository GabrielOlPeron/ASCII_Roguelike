using GoRogue.DiceNotation;
using SadConsole.UI;
using static System.Net.Mime.MediaTypeNames;

namespace SadConsoleGame.entities
{
    internal class Creature : DynamicEntity
    {

        int level;
        int strength; //define damage 
        int dexterity;//define dodge/hit chance 
        int constitution;//define health 
        int intuition;//define observation skill
        int charisma;// define social skill
        

        public Creature(int level,int strength, int dexterity, int constitution, int intuition, int charisma, bool isTransparent, bool walkable, ColoredGlyph appearance, Point position, IScreenSurface hostingSurface)
            : base(1, isTransparent, walkable, appearance, position, hostingSurface)
        {
            this.level = level;
            this.strength = strength+level*5;
            this.dexterity = dexterity + level * 5;
            this.constitution = constitution + level * 5;
            this.intuition = intuition + level * 5;
            this.charisma = charisma + level * 5;

            this.health = health + Dice.Roll($"1d3+1*{constitution}");
            this.damage = damage + Dice.Roll($"1d3+1*{strength}");
        }

        public override bool Touched(Entity source, Map map)
        {
            if (isWalkable)
            {
                InflictDamage(source, map);
                return true;
            }
            else
            {
                InflictDamage(source, map);
                return false;
            }

        }
        public override void LoseHealth(int damage, Map map)
        {
            health -= damage;

            if (health <= 0)
            {
                //remove entity from list
                map.RemoveEntity(this);
                //restore map tile
                mapAppearance.CopyAppearanceTo(map.mapSurface.Surface[position]);
            }
        }
        public override string Analyze()
        {
            return null;
        }
        public override bool Move(Direction dir, Map map)
        {

            for (int i = 0; i < moveSpeed; i++)
            {
                Point newPosition = map.player.position + dir;
                // Check new position is valid
                if (!map.mapSurface.IsValidCell(newPosition.X, newPosition.Y)) return false;

                // Check if other object is there
                if (map.IsPositionOccupied(newPosition, out Entity? foundObject))
                {
                    // We touched the other object, but they won't allow us to move into the space
                    if (!foundObject.Touched(this, map))
                    {
                        return false;
                    }

                }

                // Restore the old cell
                mapAppearance.CopyAppearanceTo(map.mapSurface.Surface[position]);

                // Store the map cell of the new position
                map.mapSurface.Surface[newPosition].CopyAppearanceTo(mapAppearance);

                position = newPosition;
                DrawEntity(map.mapSurface);
                


            }
            return true;
        }
        protected virtual void InflictDamage(Entity entity, Map map)
        {
            entity.LoseHealth(damage, map);
        }
        
    }
}
