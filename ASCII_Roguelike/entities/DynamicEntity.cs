namespace SadConsoleGame.entities
{
    internal class DynamicEntity : Entity
    {
        public int moveSpeed; //move speed in tiles/turn
        protected int damage;
        public DynamicEntity(int moveSpeed, bool isTransparent, bool walkable, ColoredGlyph appearance, Point position, IScreenSurface hostingSurface)
            : base(isTransparent, walkable, appearance, position, hostingSurface)
        {
            this.moveSpeed = moveSpeed;
            
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
        public virtual string Analyze()
        {
            return null;
        }
        public virtual bool Move(Direction dir, Map map)
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
