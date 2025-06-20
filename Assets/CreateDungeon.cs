using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDungeon : MonoBehaviour
{
    public int mapWidth = 50;
    public int mapDepth = 50;
    public int scale = 2;
    Leaf root;

    byte[,] map;
    List<Vector2> corridors = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        map = new byte[mapWidth, mapDepth];
        for (int z = 0; z < mapDepth; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                map[x, z] = 1;
            }
        }

        root = new Leaf(0, 0, mapWidth, mapDepth, scale);
        BSP(root, 6); // The second argument is how many "levels" or how deep the tree will be.

        AddCorridors();
        AddRandomCorridors(10);

        DrawMap();
    }

    void AddRandomCorridors(int numHalls)
    {
        for (int i = 0; i < numHalls; i++)
        {
            int startX = Random.Range(5, mapWidth - 5);
            int startZ = Random.Range(5, mapDepth - 5);
            int length = Random.Range(5, mapWidth - 5);

            if (Random.Range(0, 100) < 50)
            { line(startX, startZ, length, startZ); }
            else
            { line(startX, startZ, startX, length); }
        }
    }

    void BSP(Leaf leaf, int sDepth)
    {
        if (leaf == null) { return; }

        if (sDepth <= 0)
        {
            leaf.Draw(map);
            corridors.Add(new Vector2(leaf.xPos + leaf.width / 2, leaf.zPos + leaf.depth / 2)); //This will get the center of the leaf, or room.
            return;
        }

        if (leaf.Split())
        {
            BSP(leaf.leftChild, sDepth - 1); //Recursion at its finest!
            BSP(leaf.rightChild, sDepth - 1);
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
            if ((int)corridors[i].x == (int)corridors[i - 1].x || (int)corridors[i].y == (int)corridors[i - 1].y)
            {
                line((int)corridors[i].x, (int)corridors[i].y, (int)corridors[i - 1].x, (int)corridors[i - 1].y);
            }
            else
            {
                line((int)corridors[i].x, (int)corridors[i].y, (int)corridors[i].x, (int)corridors[i - 1].y);
                line((int)corridors[i].x, (int)corridors[i].y, (int)corridors[i - 1].x, (int)corridors[i].y);
            }
        }
    }

    void DrawMap()
    {
        for (int z = 0; z < mapDepth; z++)
            for (int x = 0; x < mapWidth; x++)
            {
                if (map[x, z] == 1) // Create the blocks that are used for the walls.
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(x * scale, 10, z * scale);
                    cube.transform.localScale = new Vector3(scale, scale, scale);
                }
                else if (map[x, z] == 2) // Create and color the blocks that represent the corridors.
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(x * scale, 10, z * scale);
                    cube.transform.localScale = new Vector3(scale, scale, scale);
                    cube.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0, 0));
                }
            }
    }


    public void line(int x1, int y1, int x2, int y2) //An adapted version of Bresenham's line algorithm for blocks instead of pixels.
    {
        int w = x2 - x1;
        int h = y2 - y1;
        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;

        if (w < 0) { dx1 = -1; }
        else if (w > 0) { dx1 = 1; }

        if (h < 0) { dy1 = -1; }
        else if (h > 0) { dy1 = 1; }

        if (w < 0) { dx2 = -1; }
        else if (w > 0) { dx2 = 1; }

        int longest = Mathf.Abs(w);
        int shortest = Mathf.Abs(h);
        if (!(longest > shortest))
        {
            longest = Mathf.Abs(h);
            shortest = Mathf.Abs(w);
            if (h < 0) { dy2 = -1; }
            else if (h > 0) { dy2 = 1; }

            dx2 = 0;
        }
        int numerator = longest >> 1;
        for (int i = 0; i <= longest; i++)
        {
            map[x1, y1] = 0;
            numerator += shortest;
            if (!(numerator < longest))
            {
                numerator -= longest;
                x1 += dx1;
                y1 += dy1;
            }
            else
            {
                x1 += dx2;
                y1 += dy2;
            }
        }
    }
}
