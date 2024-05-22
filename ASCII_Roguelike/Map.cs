using GoRogue.FOV;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using SadConsoleGame.entities;
using SadConsoleGame.entities.creatures;
using SadRogue.Primitives.GridViews;
using System.Diagnostics.CodeAnalysis;
using static SadConsole.Readers.Playscii;

namespace SadConsoleGame;

internal class Map
{
    //map gridview
    public IGridView<bool> wallFloorValues;
    //discovered tiles
    public IGridView<bool> discoveredTiles;
    // Array of terrain at each location used in FOV
    public ArrayView<Entity> arrayView;
    // Grid view passed to FOV, which represents the transparency of terrain
    public  IGridView<bool> transparencyView;
    // FOV instance 
    public  IFOV fov; 
    //surface where the map is drawn
    public ScreenSurface mapSurface;
    //player entity
    public Player player; 
    //all entities on the map
    public List<Entity> mapEntities = new List<Entity>();
    //ScreenSurface font 
    private SadFont squareFont = (SadFont) GameHost.Instance.LoadFont("./fonts/CheepicusExtended.font");

    //player info
    public string race;
    public string charBackground ;
    public int strength;
    public int dexterity;
    public int constitution;
    public int intuition;
    public int charisma;

    //constructor
    public Map(int mapWidth, int mapHeight, int xPosition, int yPosition, string race, string bg, int str, int dex, int cons, int intu, int chari)
    {
        mapSurface = new ScreenSurface(mapWidth, mapHeight);
        mapSurface.Position= new Point(xPosition, yPosition);
        mapSurface.UseMouse = true;
        mapSurface.Font= squareFont;
        mapSurface.FocusOnMouseClick = true;
        mapSurface.MoveToFrontOnMouseClick = true;


        this.race = race;
        this.charBackground = bg;
        this.strength = str;
        this.dexterity = dex;
        this.constitution = cons;
        this.intuition = intu;
        this.charisma = chari;

        NewMap(mapWidth, mapHeight);

        
    }

    //check if a position is occupied by an entity
    public bool IsPositionOccupied(Point position, [NotNullWhen(true)] out Entity? entity)
    {
        // Try to find a map object at that position
        foreach (Entity otherGameObject in mapEntities)
        {
            if (otherGameObject.position == position)
            {
                entity = otherGameObject;
                return true;
            }
            
        }

        entity = null;
        return false;
    }

    //draw a new map
    public void NewMap(int mapWidth, int mapHeight)
    {
        mapEntities.Clear();
        DungeonGen(mapWidth, mapHeight);

        //place player
        //player = new DynamicEntity(1,true, false, new ColoredGlyph(Color.Red, Color.Black, '@'), RandomEmptyPosition(), mapSurface);
        player = new Player(charBackground,race,1,strength,dexterity,constitution,intuition,charisma,5,RandomEmptyPosition(),mapSurface);
        player.Fov(this);

        

    }

    //remove uma entidade do mapa
    public void RemoveEntity(Entity entity)
    {
        mapEntities.Remove(entity);
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

    //generate the dungeon structure
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

    
}
