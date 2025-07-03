using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateDungeon : MonoBehaviour
{

    public int mapWidth = 50; //X direction
    public int mapDepth = 50; //Z direction
    public int scale = 2;
    Leaf root;

    byte[,] map; // This will be the map that tells us where there are 0s and 1s representing the state of the "dungeon".



    // Start is called before the first frame update
    void Start()
    {
        map = new byte[mapWidth, mapDepth];
        root = new Leaf(0, 0, mapWidth, mapDepth, scale);

        for (int z = 0; z < mapDepth; z++)// For the z-dimension.
        {
            for (int x = 0; x < mapWidth; x++)// For the x-dimension.
            {
                map[x, z] = 1; // Writing a 1 means a cube is meant to be created at the coordinate (x,z).
            }
        }

        BSP(root, 3); // This creates a color-coded "map" of the sections of the dungeon. In order for it to work properly, both BSP and DrawMap have to be called.
        DrawMap(); // This creates the actual map with the empty sections (rooms).

    }

    void BSP(Leaf leaf, int splitDepth)
    {

        if (leaf == null) { return; }

        if (splitDepth <= 0)
        {
            leaf.Draw(map);
            return;
        }

        if (leaf.Split())
        {
            BSP(leaf.leftChild, splitDepth - 1);
            BSP(leaf.rightChild, splitDepth - 1);
        }
        else
        {
            leaf.Draw(map);
        }

    }

    void DrawMap()
    {


        for (int z = 0; z < mapDepth; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (map[x, z] == 1) //If it's set to a value of 1, create a cube there.
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(x * scale, 10, z * scale);
                    cube.transform.localScale = new Vector3(scale, scale, scale);
                }
            }
        }
    }

    public void BresenhamLine(int x0, int y0, int x1, int y1) // This was provided by the intructor. Eventually I'll write my own implementation to better understand it.
    {
        int w = x1 - x0;
        int h = y1 - y0;
        int dx0 = 0, dy0 = 0, dx1 = 0, dy1 = 0;

        if (w < 0) { dx0 = -1; }
        else if (w > 0) { dx0 = -1; }

        if (h < 0) { dy0 = -1; }
        else if (h > 0) { dy0 = 1; }

        if (w < 0) { dx1 = -1; }
        else if (w > 0) { dx1 = -1; }

        int longest = Mathf.Abs(w);
        int shortest = Mathf.Abs(h);

        if (!(longest > shortest))
        {
            longest = Mathf.Abs(h);
            shortest = Mathf.Abs(w);

            if (h < 0) { dy1 = -1; }
            else if (h > 0) { dy1 = 1; }
            dx1 = 0;
        }

        int numerator = longest >> 1;
        for (int i = 0; i <= longest; i++)
        {
            map[x0, y0] = 2;
            numerator += shortest;

            if (!(numerator < longest))
            {
                numerator -= longest;
                x0 += dx0;
                y0 += dy0;
            }
            else
            {
                x0 = dx1;
                x1 = dy1;
            }
        }
    }
}