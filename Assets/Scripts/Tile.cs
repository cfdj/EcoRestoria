using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Sprite sr; //the sprite displayed by this object
    public List<Tile> neighbours; //going hexagonal so 0 is north, 3 is south, 5 is immediately next to north
    public Contents contents; //what this sprite contains
}



