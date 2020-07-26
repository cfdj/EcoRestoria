using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River : Object
{
    [SerializeField] Tile river;
    List<TileDisplay> Segments; //the tiles present in a rive
    int lowestHeight =99;
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
        updateLowestHeight();
    }

    //Should this change the tile being displayed to a river?
    //Yes for clarity in other places
    public void AddSegment(TileDisplay newSeg)
    {
        Segments.Add(newSeg);
        newSeg.setRiver();
        if(newSeg.GetHeight() < lowestHeight)
        {
            lowestHeight = newSeg.GetHeight();
            foreach(TileDisplay t in Segments)
            {
                t.setHeight(lowestHeight);
            }
        }

    }
    public void AddSegments(List<TileDisplay> newSegs)
    {
        Segments.AddRange(newSegs);
        foreach(TileDisplay t in newSegs)
        {
            t.setRiver();
        }
        updateLowestHeight();
    }
    public int getLowestHeight()
    {
        return lowestHeight;
    }

    public List<TileDisplay> GetSegments()
    {
        return Segments;
    }
    public int Getlength()
    {
        return Segments.Count;
    }
    void updateLowestHeight()
    {
        foreach(TileDisplay t in Segments)
        {
            if(t.GetHeight() < lowestHeight)
            {
                lowestHeight = t.GetHeight();
            }
        }
        foreach(TileDisplay t in Segments)
        {
            t.setHeight(lowestHeight);
        }
    }
}
