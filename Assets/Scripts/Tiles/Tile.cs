using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "HexTile")]
public class Tile : ScriptableObject
{
    public new string name;
    public Sprite sprite; //the sprite displayed by this object
    public Contents contents; //what this sprite contains
    
}



