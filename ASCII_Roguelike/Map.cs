using GoRogue.FOV;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using SadRogue.Primitives.GridViews;
using System.Diagnostics.CodeAnalysis;
using static SadConsole.Readers.Playscii;

namespace SadConsoleGame;

internal class Map
{
    public IGridView<bool> wallFloorValues;

    public List<Entity> mapEntities= new List<Entity>(); //entities on the map

    public  ArrayView<Entity> arrayView;
    // Grid view we'll pass to FOV, which represents the transparency of terrain
    public  IGridView<bool> TransparencyView;

    // FOV instance we'll use
    public  IFOV FOV;

    public ScreenSurface mapSurface;
    public ScreenSurface SurfaceObject => mapSurface;
    public Entity player { get; set; }
    public Entity stair { get; set; }

    SadFont SquareFont = (SadFont) GameHost.Instance.LoadFont("./fonts/CheepicusExtended.font");
    public Map(int mapWidth, int mapHeight, int xPosition, int yPosition)
    {
        mapSurface = new ScreenSurface(mapWidth, mapHeight);
        mapSurface.Position= new Point(xPosition, yPosition);
        mapSurface.UseMouse = false;
        mapSurface.Font= SquareFont;

        

        

        NewMap(mapWidth, mapHeight);

        
    }

    //check if a position is occupied by a gameobject
    public bool IsPositionOccupied(Point position, [NotNullWhen(true)] out Entity? gameObject)
    {
        // Try to find a map object at that position
        foreach (Entity otherGameObject in mapEntities)
        {
            if (otherGameObject.position == position)
            {
                gameObject = otherGameObject;
                return true;
            }
            
        }

        gameObject = null;
        return false;
    }

    //return an empty random position
    private Point RandomEmptyPosition()
    {
        for (int i = 0; i < 1000; i++)
        {
            // Get a random position
            Point randomPosition = new Point(Game.Instance.Random.Next(0, mapSurface.Surface.Width), Game.Instance.Random.Next(0, mapSurface.Surface.Height));

            // Check if any object is already positioned there, repeat the loop if found
            bool foundObject = mapEntities.Any(obj => obj.position == randomPosition && obj.isWalkable==false);
            if (foundObject) continue;

            // If the code reaches here, we've got a good position, create the game object.
            return randomPosition;
                    }
        return Point.None;
    } 

    private void DungeonGen(int mapWidth,int mapHeight)
    {
        // The map will have a width of 60 and height of 40
        Generator generator = new Generator(mapWidth, mapHeight);

        // Add the steps to generate a map using the DungeonMazeMap built-in algorithm,
        // and generate the map.
        generator.ConfigAndGenerateSafe(gen =>
        {
            gen.AddSteps(DefaultAlgorithms.BasicRandomRoomsMapSteps());
        });
        


        wallFloorValues = generator.Context.GetFirst<ISettableGridView<bool>>("WallFloor");

        

        foreach (var pos in wallFloorValues.Positions())
        {
            if (wallFloorValues[pos])
                mapEntities.Add(new Entity(true, true, new ColoredGlyph(Color.Yellow, Color.Black, '.'), pos, mapSurface));

            else
                mapEntities.Add(new Entity(false, false, new ColoredGlyph(Color.White, Color.DarkGray, '#'), pos, mapSurface));
        }

        


        


    }

    public void NewMap(int mapWidth,int mapHeight)
    {
        mapEntities.Clear();
        DungeonGen(mapWidth, mapHeight);

        //place player
        player = new Entity(true, false, new ColoredGlyph(Color.Red, Color.Black, '@'), RandomEmptyPosition(), mapSurface);
        player.Fov(this);
   
    }
}
