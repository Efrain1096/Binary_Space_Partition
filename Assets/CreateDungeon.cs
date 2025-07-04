using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateDungeon : MonoBehaviour
{

    public int mapWidth = 50; //X direction
    public int mapDepth = 50; //Z direction
    public int scale = 2;

    public int depth = 6;
    Leaf root;

    byte[,] map; // This will be the map that tells us where there are 0s and 1s representing the state of the "dungeon".
    List<Vector2> corridors = new List<Vector2>();


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

        BSP(root, depth); // This creates a color-coded "map" of the sections of the dungeon. In order for it to work properly, both BSP and DrawMap have to be called.
        AddCorridors();
        DrawMap(); // This creates the actual map with the empty sections (rooms).

    }

    void BSP(Leaf leaf, int splitDepth)
    {

        if (leaf == null) { return; }

        if (splitDepth <= 0)
        {
            leaf.Draw(map);
            corridors.Add(new Vector2(leaf.xPos + leaf.width / 2, leaf.zPos + leaf.depth / 2));
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
            corridors.Add(new Vector2(leaf.xPos + leaf.width / 2, leaf.zPos + leaf.depth / 2));

        }

    }

    void AddCorridors()
    {
        for (int i = 1; i < corridors.Count; i++)
        {

            //We only want the corridors to be vertical or horizontal.
            if ((int)corridors[i].x == (int)corridors[i - 1].x || (int)corridors[i].y == (int)corridors[i - 1].y) // As-is, not all the rooms are joined by the corridors.
            {
                BresenhamLine((int)corridors[i].x, (int)corridors[i].y, (int)corridors[i - 1].x, (int)corridors[i - 1].y);
            }

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
                else if (map[x, z] == 2)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(x * scale, 10, z * scale);
                    cube.transform.localScale = new Vector3(scale, scale, scale);
                    cube.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0, 0)); // Add some color to distiguish the different partitions.
                }
            }
        }
    }

    //An adapted version of Bresenham's line algorithm. I will look into understanding it more.
    //https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
    public void BresenhamLine(int x0, int y0, int x1, int y1)
    {
        int w = x1 - x0;
        int h = y1 - y0;
        int dx0 = 0, dy0 = 0, dx1 = 0, dy1 = 0;
        if (w < 0) dx0 = -1; else if (w > 0) dx0 = 1;
        if (h < 0) dy0 = -1; else if (h > 0) dy0 = 1;
        if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
        int longest = Mathf.Abs(w);
        int shortest = Mathf.Abs(h);
        if (!(longest > shortest))
        {
            longest = Mathf.Abs(h);
            shortest = Mathf.Abs(w);
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
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
                x0 += dx1;
                y0 += dy1;
            }
        }
    }
}