using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDisplay : MonoBehaviour
{
    public List<Tile> tiles; //this is a list of all the tiles in the game, changed between by calling their individual set functions
                             //0 = city, 1 = desert, 2 = soil, 3 = water. may add dedicated edge tiles later
    public List<Material> materials;
    private MeshRenderer hexagon;
    public Tile hexTile;
    private int height;
    public static int hillHeight;
    private SpriteRenderer sr;
    private TileDisplay[] neighbours = new TileDisplay[6];//going hexagonal so 0 is north, 3 is south, 5 is immediately next to north
    private Vector2 position;
    private float heightchange = 0.1f;
    public Vector2Int mapPosition { get; set; }
    // Start is called before the first frame update
    void Start()
    { 
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = hexTile.sprite;
        sr.material = hexTile.material;
    }
    public void SetPosition( Vector2 newposition)
    {
        position = newposition;
        transform.position=new Vector3(newposition.x,newposition.y,transform.position.z);
    }
    public void setHeight(int nheight)
    {
        height = nheight;
        //transform.localScale = new Vector3(1, 1, 1-nheight * heightchange);
        transform.position = new Vector3(transform.position.x, transform.position.y, -nheight * heightchange);
        //call sprite update
    }
    public int GetHeight()
    {
        return height;
    }
    public void AddNeighbour(TileDisplay neighbour, int pos) //called by another tile to connect these as neighbours.
    {
        int npos = 0;
        neighbours[pos] = neighbour;
        switch (pos)
        {
            case 0:
                npos = 3;
                break;
            case 1:
                npos = 4;
                break;
            case 2:
                npos = 5;
                break;
            case 3:
                npos = 0;
                break;
            case 4:
                npos = 1;
                break;
            case 5:
                npos = 2;
                break;
        }
        neighbour.neighbours[npos] = this;


    }
    
    public int GetNumNeighbours()
    {
        int num = 0;
        for (int i = 0; i <5; i++)
        {
            if(neighbours[i] != null)
            {
                num++;
            }
        }
        return num;
    }
    public TileDisplay[] GetNeightbours()
    {
        return neighbours;
    }

    //called whenever the tile is changed to update the display
    private void changeTile(int tileNum)
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = hexTile.sprite;
        sr.material = hexTile.material;
        hexagon = gameObject.GetComponentInChildren<MeshRenderer>();
        hexagon.material = materials[tileNum];
    }
    public void setRiver()
    {
        if (hexTile != tiles[3])
        {
            setHeight(height - 1);
            hexTile = tiles[3];
            changeTile(3);
        }
    }
    public void setSoil()
    {
        if (height < hillHeight)
        {
            hexTile = tiles[4];
            changeTile(4);
        }
        else
        {
            hexTile = tiles[2];
            changeTile(2);
        }
    }
    public void setDesert()
    {
        hexTile = tiles[1];
        changeTile(1);
    }
    public void setCity()
    {
        hexTile = tiles[0];
        changeTile(0);
    }

    public TileDisplay getNextLowestNeighbour(List<TileDisplay> previousLowest)
    {
        TileDisplay lowestNeighbour = null;
        int lowest = 999;
        for (int i = 0; i<5; i++)
        {
            if (neighbours[i] != null)
            {
                if ((neighbours[i].height < lowest) && !previousLowest.Contains(neighbours[i]))
                {
                    lowestNeighbour = neighbours[i];
                    lowest = lowestNeighbour.height;
                }
            }
        }
        return lowestNeighbour;
    }
}
