using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDungeon: MonoBehaviour
{

    public int mapWidth = 50; //X direction
    public int mapDepth = 50; //Z direction
    public int scale = 2;
    Leaf root;

    // Start is called before the first frame update
    void Start()
    {

        root = new Leaf(0, 0, mapWidth, mapDepth, scale);
        //root.Draw();


        int leftWidth = Random.Range(1, 20);

        Leaf left = new Leaf(0, 0, leftWidth, mapDepth, scale);
        Leaf right = new Leaf(leftWidth, 0, mapWidth / 2 , mapDepth, scale);
        left.Draw();
        right.Draw();



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
