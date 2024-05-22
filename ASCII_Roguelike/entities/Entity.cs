using GoRogue.FOV;
using SadRogue.Primitives.GridViews;


namespace SadConsoleGame.entities;

internal class Entity
{
    public bool discovered;//if the entity was viewd by the player
    public bool isTransparent { get; protected set; }//if it can see through
    public bool isWalkable { get; protected set; } //if the surface can be walked across
    public Point position { get; protected set; }//entity position
    public ColoredGlyph appearance { get; set; }//entity appearance 
    protected int health;

    protected ColoredGlyph mapAppearance = new ColoredGlyph(); //store appearance of the map's cell below the entity 

    public Entity( bool isTransparent, bool walkable, ColoredGlyph appearance, Point position, IScreenSurface hostingSurface)
    {
        this.discovered = false;
        this.isTransparent = isTransparent;
        this.isWalkable = walkable;
        this.appearance = appearance;
        this.position = position;
        this.health = health;
        hostingSurface.Surface[position].CopyAppearanceTo(mapAppearance);
        DrawEntity(hostingSurface);
    }

    

    public virtual bool Touched(Entity source, Map map)
    {
        if (isWalkable) { return true; }
        return false;
    }
    public virtual void LoseHealth(int damage, Map map)
    {

    }
    protected void DrawEntity(IScreenSurface screenSurface)
    {
        appearance.CopyAppearanceTo(screenSurface.Surface[position]);
        screenSurface.IsDirty = true;
    }
}