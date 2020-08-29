using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [SerializeField] int lineLength;
    [SerializeField] int numLines;
    [SerializeField] float offSet;

    [SerializeField] float xchange;
    [SerializeField] float ychange;
    [SerializeField] float linechange;
    [SerializeField] TileDisplay tileBase;

    //Variables for terrain generation
    [SerializeField] int prevailingWind; //same as neighbour numbering, 0 is north, 3 is south, north east is 1
    [SerializeField] int hillHeight; // the point at which a hill is high enough to generate a rainshadow, may also change shaders in future
    [SerializeField] int cityNum; //the number of concrete tiles to generate
    private List<TileDisplay> hillLocations;
    private int[] heighestPoint;


    private List<River> rivers;

    private TileDisplay[,] map;
    // Start is called before the first frame update
    void Start()
    {
        offSet = Random.Range(0, 100);
        map = new TileDisplay[lineLength, numLines];
        hillLocations = new List<TileDisplay>();
        TileDisplay.hillHeight = hillHeight;
        heighestPoint = new int[2];
        rivers = new List<River>();
        GenerateGrid(0, 0);
        setNeighbours();
        setHeights();
        generateTerrain();
    }
    public TileDisplay getTile(int x, int y)
    {
        return map[x, y];
    }
    //sets the height of each tile in the map, also saves the locations of the heighest point and all hills
    void setHeights()
    {
        float Perlin;
        int currentHeight;
        int heighest= 0;
        for (int y = 0; y < numLines; y++)
        {
            for (int x = 0; x<lineLength; x++)
            {
                //now adds a random offset
                Perlin = Mathf.PerlinNoise(map[x,y].transform.position.x*offSet, map[x, y].transform.position.y*offSet)*10;
                currentHeight = Mathf.RoundToInt(Perlin); //currently doesn't give a wide enough range of values
                //currentHeight = Mathf.RoundToInt(Random.Range(0, 10));
                Debug.Log("Pos: " + x+","+y+" Height:"+Perlin);
                map[x, y].setHeight(currentHeight);
                map[x, y].setSoil(); //updating the tiles material based on height
                if (currentHeight >= hillHeight)
                {
                    hillLocations.Add(map[x,y]);
                }
                if (currentHeight > heighest)
                {
                    heighest = currentHeight;
                    heighestPoint[0] = x;
                    heighestPoint[1] = y;
                }
            }
        }

    }


    /// <summary>
    /// Generates terrains based on geographical processes
    /// Currently generates:
    ///     Rivers: 
    ///     Deserts: form in the rain shadow of sufficiently high hills
    ///     
    ///     Cities: Generate a random number of cities
    /// </summary>
    void generateTerrain()
    {
        foreach (TileDisplay hill in hillLocations)
        {
            hillSmooth(hill);
            placeRiver(hill);
        }

    }
    //hills currently look too spikey, so moving their neighbours to be a similar height
    void hillSmooth(TileDisplay hill)
    {
        List<TileDisplay> neighbours = new List<TileDisplay>( hill.GetNeightbours());
        foreach(TileDisplay t in neighbours)
        {
            if (t != null)
            {
                if (t.GetHeight() < hill.GetHeight())
                {
                    t.setHeight(hill.GetHeight() - 1);
                }
            }
        }

    }
    //will currently stop if it meets another river
    void placeRiver(TileDisplay hill)
    {
        River newRiver;
        TileDisplay next = hill;
        TileDisplay currentLowest = null;
        List<TileDisplay> inRoute = new List<TileDisplay>();
        TileDisplay[] toCheck;
        int loops = 0;
        bool done = false;
        //this check is being done to reduce the number of rivers
        toCheck = next.GetNeightbours();
        for(int i = 0; i< 5; i++)
        {
            if(toCheck[i] != null)
            {
                if (toCheck[i].name == "Water")
                {
                    done = true;
                }
            }
        }

        while (!done)
        {
            loops += 1;
            if (loops > 999)
            {
                Debug.Log("Infinite looped");
                break;
            }
            if (next.getNextLowestNeighbour(inRoute) != null)
            {
                currentLowest = next.getNextLowestNeighbour(inRoute);
                if(currentLowest.name == "Water")
                {
                    done = true;
                }
                if (currentLowest.GetNumNeighbours() < 5)
                {
                    done = true;
                }
            }
            else //if stuck, backtrack
            {
                currentLowest = inRoute[inRoute.Count - 1];
            }
            if(currentLowest != null)
            {
                inRoute.Add(currentLowest);
                next = currentLowest;
            }
        }
        if (inRoute.Count != 0)
        {
            newRiver = new River(inRoute);
            //Debug.Log("River Finished");
            rivers.Add(newRiver);
        }
    }


    //initial grid creation
    void GenerateLine(float xStart, float yStart, int lineNum) //places a line of hexagons, line num tells it where in the map to place its line
    {
        TileDisplay currentTile;
        for(int i = 0; i <lineLength; i++)
        {
            if (i % 2 == 0)  //only moving down every second hexagon
            {
                currentTile = Instantiate(tileBase);
                currentTile.SetPosition(new Vector3(xStart + i * xchange, yStart));
                currentTile.mapPosition = new Vector2Int(i, lineNum);
                map[i, lineNum] = currentTile;
            }
            else
            {
                currentTile = Instantiate(tileBase);
                currentTile.SetPosition(new Vector3(xStart + i * xchange, yStart + ychange));
                currentTile.mapPosition = new Vector2Int(i, lineNum);
                map[i, lineNum] = currentTile;
            }
        }

    }
    void GenerateGrid (float xStart, float yStart)
    {
        for (int y = 0; y<numLines; y++)
        {
            GenerateLine(0, yStart + linechange * y, y);
        }

    }
    void setNeighbours() //for each tile in map, set its neighbours, as setting neighbours works in both directions, each tile only needs to set the 3 neighbours below it.
    {
        for (int y = 0; y< numLines; y++)
        {
            for (int x = 0; x < lineLength; x++)
            {
                Debug.Log("x " + x + " y" + y);
                if (x%2 == 0) //attaching x neighbours is different depending on if the tile is high or low
                {
                    if (x > 0)
                    {
                        map[x, y].AddNeighbour(map[x - 1, y], 4);
                    }
                    if (x < lineLength - 1)
                    { 
                        map[x, y].AddNeighbour(map[x + 1, y],2);
                    }
                }
                else
                {
                    if ((x > 0)&& (y < numLines - 1))
                    {
                        map[x, y].AddNeighbour(map[x - 1, y + 1], 4);
                    }
                    if((x < lineLength - 1)&& (y<numLines-1))
                    {
                        map[x, y].AddNeighbour(map[x + 1, y + 1], 2);
                    }
                }

                if (y < numLines-1)
                {
                    map[x, y].AddNeighbour(map[x, y + 1], 3);
                }
            }
        }
    }
}
