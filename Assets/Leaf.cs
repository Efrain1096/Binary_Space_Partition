using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Leaf
{

    int xPos;
    int zPos;
    int width;
    int depth;
    int scale;
    int roomMin = 5;

    public Leaf leftChild;
    public Leaf rightChild;
    public Leaf(int x, int z, int w, int d, int s) // The constructor for the leaf nodes.
    {
        xPos = x;
        zPos = z;
        width = w;
        depth = d;
        scale = s;
    }

    public bool Split()
    {

        if (width <= roomMin || depth <= roomMin) { return false; } // This will restrict our sections or "rooms" not be tiny and useless.

        bool splitHorizontal = Random.Range(0, 100) > 50;

        if (width > depth && width / depth >= 1.2) { splitHorizontal = false; }
        else if (depth > width && depth / width >= 1.2) { splitHorizontal = true; }


        int max = (splitHorizontal ? depth : width) - roomMin;

        if (max <= roomMin) { return false; }

        if (splitHorizontal)
        {
            int leftDepth = Random.Range(roomMin, max);
            leftChild = new Leaf(xPos, zPos, width, leftDepth, scale);
            rightChild = new Leaf(xPos, zPos + leftDepth, width, depth - leftDepth, scale);
        }
        else
        {
            int leftWidth = Random.Range(roomMin, max);
            leftChild = new Leaf(xPos, zPos, leftWidth, depth, scale);
            rightChild = new Leaf(xPos + leftWidth, zPos, width - leftWidth, depth, scale);


        }
        return true;


        /*  int leftWidth = Random.Range( (int)(mapDepth * 0.1f), (int)(mapDepth * 0.7f));
            Leaf left = new Leaf(0, 0, leftWidth, mapDepth, scale);
            Leaf right = new Leaf(leftWidth, 0, mapWidth - leftWidth , mapDepth, scale);
            left.Draw();
            right.Draw();
            */
    }

    public void Draw(int level)
    {
        Color c = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        for (int x = xPos; x < width + xPos; x++)
        {
            for (int z = zPos; z < depth + zPos; z++) // Create 3D cubes of 1 unit. 
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(x * scale, level * 20, z * scale); // Muliply the scale by the cube's position to separate them apart.
                cube.transform.localScale = new Vector3(scale, scale, scale);
                cube.GetComponent<Renderer>().material.SetColor("_Color", c); // Add some color to distiguish the different partitions.
            }
        }
    }
}