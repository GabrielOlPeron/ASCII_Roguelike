using GoRogue.FOV;
using SadRogue.Primitives.GridViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadConsoleGame.entities.creatures
{
    internal class Player:Creature
    {
        Color customColor = new Color(25,25,25);
        int viewDistance;
        string race;
        string charBackground;
        public Player(string charBackground,string race,int level, int strength, int dexterity, int constitution, int intuition, int charisma,int viewDistance, Point position, IScreenSurface hostingSurface) : base (level, strength, dexterity, constitution, intuition, charisma, true, false, new ColoredGlyph(Color.Red, Color.Black, '@'), position, hostingSurface) 
        {
            this.viewDistance = viewDistance;
            this.race = race;   
            this.charBackground = charBackground;
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
                Fov(map);


            }
            return true;
        }
        public void Fov(Map map)
        {

            map.arrayView = new ArrayView<Entity>(map.mapSurface.Width, map.mapSurface.Height);
            map.transparencyView = new LambdaTranslationGridView<Entity, bool>(map.arrayView, t => t.isTransparent);
            map.fov = new RecursiveShadowcastingFOV(map.transparencyView);

            map.arrayView.ApplyOverlay(
                pos => map.wallFloorValues[pos]
                    ? new Entity(true, true, new ColoredGlyph(Color.White, Color.Black, '.'), pos, map.mapSurface)
                    : new Entity(false, false, new ColoredGlyph(Color.White, Color.DarkGray, '#'), pos, map.mapSurface));

            map.fov.Calculate(map.player.position, viewDistance, Radius.Circle);

            map.arrayView.ApplyOverlay(
                pos => map.fov.BooleanResultView[pos]
                    ? new Entity(true, true, new ColoredGlyph(Color.Black, Color.Black, '.'), Point.None, map.mapSurface)
                    : new Entity(false, false, new ColoredGlyph(Color.Black, Color.Black, '#'), pos, map.mapSurface));


            foreach (Entity entity in map.mapEntities)
            {
                Point pos=entity.position;
                if (entity.discovered == true&& map.fov.BooleanResultView[pos] == false)
                {
                    ColoredGlyph g = entity.appearance;
                    g.Foreground = customColor;
                    g.Background = Color.Black;
                    new Entity(true, true, g, pos, map.mapSurface);
                }
                else { }

                if (map.fov.BooleanResultView[pos] == true)
                {
                    entity.discovered=true;
                }  
            }

            
            
            map.mapSurface.IsDirty = true;
           DrawEntity(map.mapSurface);//draw player

        }


    }
}
