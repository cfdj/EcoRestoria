using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Sprite sr; //the sprite displayed by this object
    Tile[] neighbours; //going hexagonal so 0 is north, 3 is south, 5 is immediately next to north
    public Contents contents; //what this sprite contains
    public int height;

    public Tile()
    {
        neighbours = new Tile[6];
    }
    public void AddNeighbour(Tile neighbour,int pos) //called by another tile to connect these as neighbours.
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
}



