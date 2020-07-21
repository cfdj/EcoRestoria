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
    public List<Tile> GeneratableTiles;

    private Tile[,] map;
    // Start is called before the first frame update
    void Start()
    {
        map = new Tile[lineLength, numLines];
        GenerateGrid(0, 0);
        setNeighbours();
    }

    void GenerateLine(float xStart, float yStart, int lineNum) //places a line of hexagons, line num tells it where in the map to place its line
    {
        for(int i = 0; i <lineLength; i++)
        {
            if (i % 2 == 0)  //only moving down every second hexagon
            {
                map[i, lineNum] = Instantiate(GeneratableTiles[0], new Vector3(xStart + i * xchange, yStart), Quaternion.identity);
                
            }
            else
            {
                map[i, lineNum] = Instantiate(GeneratableTiles[0], new Vector3(xStart + i * xchange, yStart + ychange), Quaternion.identity);
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
