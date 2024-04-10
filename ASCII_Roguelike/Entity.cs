using GoRogue.FOV;
using SadRogue.Primitives.GridViews;


namespace SadConsoleGame;

internal class Entity
{
    public bool isTransparent { get; private set; }
    public bool isWalkable { get; private set; } //if the surface can be walked across
    public Point position { get; private set; }//gameobject position
    public ColoredGlyph appearance { get; set; }//gameobject appearance 

    private ColoredGlyph mapAppearance = new ColoredGlyph(); //store the map's cell below the gameobject appearance

    public Entity(bool isTransparent, bool walkable,ColoredGlyph appearance, Point position, IScreenSurface hostingSurface)
    {
        this.isTransparent = isTransparent;
        this.isWalkable = walkable;
        this.appearance = appearance;
        this.position = position;

        hostingSurface.Surface[position].CopyAppearanceTo(mapAppearance);
        DrawGameObject(hostingSurface);
    }

    private void DrawGameObject(IScreenSurface screenSurface)
    {
        appearance.CopyAppearanceTo(screenSurface.Surface[position]);
        screenSurface.IsDirty = true;
    }

    public bool Move(Point newPosition, Map map)
    {
        // Check new position is valid
        if (!map.SurfaceObject.IsValidCell(newPosition.X, newPosition.Y)) return false;

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
        mapAppearance.CopyAppearanceTo(map.SurfaceObject.Surface[position]);

        // Store the map cell of the new position
        map.SurfaceObject.Surface[newPosition].CopyAppearanceTo(mapAppearance);

        position = newPosition;
        DrawGameObject(map.SurfaceObject);
        Fov(map);

        return true;
    }

    public void Fov(Map map)
    {

        map.arrayView = new ArrayView<Entity>(map.mapSurface.Width, map.mapSurface.Height);
        map.TransparencyView = new LambdaTranslationGridView<Entity, bool>(map.arrayView, t => t.isTransparent);
        map.FOV = new RecursiveShadowcastingFOV(map.TransparencyView);

        map.arrayView.ApplyOverlay(
            pos => map.wallFloorValues[pos]
                ? new Entity(true, true, new ColoredGlyph(Color.White, Color.Black, '.'), pos, map.mapSurface)
                : new Entity(false, false, new ColoredGlyph(Color.White, Color.DarkGray, '#'), pos, map.mapSurface));

        map.FOV.Calculate(map.player.position, 50, Radius.Square);

        map.arrayView.ApplyOverlay(
            pos => map.FOV.BooleanResultView[pos]
                ? new Entity(true, true, new ColoredGlyph(Color.Black, Color.Black, '.'), Point.None, map.mapSurface)
                : new Entity(false, false, new ColoredGlyph(Color.Black, Color.Black, '#'), pos, map.mapSurface));

        map.mapSurface.IsDirty = true;
        DrawGameObject(map.mapSurface);
        
    }

    public virtual bool Touched(Entity source, Map map)
    {
        if (isWalkable) { return true; }
        return false;
    }
}