using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDisplay : MonoBehaviour
{
    public List<Tile> tiles; //this is a list of all the tiles in the game, changed between by calling their individual set functions
                            //0 = city, 1 = desert, 2 = soil, 3 = water. may add dedicated edge tiles later
    public Tile hexTile;
    private int height;
    private SpriteRenderer sr;
    private TileDisplay[] neighbours = new TileDisplay[6];//going hexagonal so 0 is north, 3 is south, 5 is immediately next to north
    private Vector2 position;
    public Vector2Int mapPosition { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        height = 0;
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = hexTile.sprite;
    }
    public void SetPosition( Vector2 newposition)
    {
        position = newposition;
        gameObject.transform.position = position;
    }
    public void SetHeight(int nheight)
    {
        height = nheight;
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
    
    public TileDisplay[] GetNeightbours()
    {
        return neighbours;
    }

    //called whenever the tile is changed to update the display
    private void changeTile()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = hexTile.sprite;
    }
    public void setRiver()
    {
        hexTile = tiles[3];
        changeTile();
    }
    public void setSoil()
    {
        hexTile = tiles[2];
        changeTile();
    }
    public void setDesert()
    {
        hexTile = tiles[1];
        changeTile();
    }
    public void setCity()
    {
        hexTile = tiles[0];
        changeTile();
    }
}
