using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [SerializeField] int lineLength;
    [SerializeField] int numLines;

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
        map = new TileDisplay[lineLength, numLines];
        hillLocations = new List<TileDisplay>();
        heighestPoint = new int[2];
        rivers = new List<River>();
        GenerateGrid(0, 0);
        setNeighbours();
        setHeights();
        generateTerrain();
    }

    //sets the height of each tile in the map, also saves the locations of the heighest point and all hills
    void setHeights()
    {
        int currentHeight;
        int heighest= 0;
        for (int y = 0; y < numLines; y++)
        {
            for (int x = 0; x<lineLength; x++)
            {
                //currentHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x/100, y/100)*10); //currently doesn't give a wide enough range of values
                currentHeight = Mathf.RoundToInt(Random.Range(0, 10));
                Debug.Log("Pos: " + x+","+y+" Height:"+currentHeight);
                map[x, y].SetHeight(currentHeight);
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
    ///     Rivers: From the hills via the a greedy shortest path
    ///     Deserts: form in the rain shadow of sufficiently high hills
    ///     
    ///     Cities: Generate a random number of cities
    /// </summary>
    void generateTerrain()
    {
        foreach (TileDisplay hill in hillLocations) {
            if (hill.hexTile.name != "Water") //checking its not already a river
            {
                placeRiver(hill);
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
        int lowest = 999; //initialised far larger than any real height
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
            toCheck = next.GetNeightbours();
            for (int i = 0; ((i <5) &&(!done)); i++)
            {
                if (toCheck[i]!= null ) 
                {
                    if(toCheck[i].name != "Water")
                    {
                        if (toCheck[i].GetHeight() < lowest &&!inRoute.Contains(toCheck[i]))
                        {
                            currentLowest = toCheck[i];
                            lowest = currentLowest.GetHeight();
                        }
                    }
                    else {
                        done = true;
                    }
                }
                else //if the river has hit an edge or another river, stop
                {
                    done = true;
                }

            }
            if(currentLowest != null)
            {
                inRoute.Add(currentLowest);
                lowest = 999;
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
