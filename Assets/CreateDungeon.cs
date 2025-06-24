using System.Collections;
using System.Collections.Generic;
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
        BSP(root, 3);

    }

    void BSP(Leaf leaf, int splitDepth)
    {

        if (leaf == null) { return; }

        if (splitDepth <= 0)
        {
            leaf.Draw(0);
            return;
        }

        if (leaf.Split())
        {
            BSP(leaf.leftChild, splitDepth - 1);
            BSP(leaf.rightChild, splitDepth - 1);
        }
        else
        {
            leaf.Draw(0);
        }

    }

    void Update()
    {

    }
}