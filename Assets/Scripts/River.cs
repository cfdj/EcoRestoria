using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River : Object
{
    [SerializeField] Tile river;
    List<TileDisplay> Segments; //the tiles present in a rive

    //creates an empty river
    public River()
    {
        Segments = new List<TileDisplay>();
    }

    public River(List<TileDisplay> inRiver)
    {
        Segments = inRiver;
        foreach(TileDisplay t in Segments)
        {
            t.setRiver();
        }
    }

    //Should this change the tile being displayed to a river?
    //Yes for clarity in other places
    public void AddSegment(TileDisplay newSeg)
    {
        Segments.Add(newSeg);
        newSeg.setRiver();
    }
    public void AddSegments(List<TileDisplay> newSegs)
    {
        Segments.AddRange(newSegs);
        foreach(TileDisplay t in newSegs)
        {
            t.setRiver();
        }
    }

    public List<TileDisplay> GetSegments()
    {
        return Segments;
    }
    public int Getlength()
    {
        return Segments.Count;
    }
}
