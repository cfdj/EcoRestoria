using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMove : MonoBehaviour
{
    public HexGrid map;
    Rigidbody rb;
    TileDisplay currentLocation;
    TileDisplay targetLocation;
    bool moving = false;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        map = FindObjectOfType<HexGrid>();
        currentLocation = map.getTile(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetLocation != null)
        {
            if (currentLocation != targetLocation)
            {
                transform.position = Vector3.MoveTowards(rb.position, targetLocation.gameObject.transform.position, speed);
                if (Vector3.Distance(transform.position, targetLocation.gameObject.transform.position) < 0.001f)
                {
                    currentLocation = targetLocation;
                    moving = false;
                }
            }
        }
    }
    public void Move(TileDisplay target)
    {
        targetLocation = target;
        moving = true;
    }
}
