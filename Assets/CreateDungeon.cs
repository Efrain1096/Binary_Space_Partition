using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDungeon : MonoBehaviour
{

    public int mapWidth = 50; //X direction
    public int mapDepth = 50; //Z direction
    public int scale = 2;
    Leaf root;

    // Start is called before the first frame update
    void Start()
    {

        root = new Leaf(0, 0, mapWidth, mapDepth, scale);
        BSP(root, 20);

    }

    void BSP(Leaf l, int splitDepth)
    {

        if (l == null) { return; }

        if (splitDepth <= 0)
        {
            l.Draw(0);
            return;
        }

        if (l.Split())
        {
            BSP(l.leftChild, splitDepth - 1);
            BSP(l.rightChild, splitDepth - 1);
        }
        else
        {
            l.Draw(0);
        }

    }

    void Update()
    {

    }
}