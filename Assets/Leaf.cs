using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{

    int xPos;
    int zPos;
    int width;
    int depth;
    int scale;

    public Leaf(int x, int z, int w, int d, int s) // The constructor for the leaf nodes.
    {
        xPos = x;
        zPos = z;
        width = w;
        depth = d;
        scale = s;

    }


    public void Draw()
    {

        Color c = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

        for (int x = xPos; x < width + xPos; x++)
        {
            for (int z = zPos; z < depth + zPos; z++) // Create 3D cubes of 1 unit. 
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(x * scale, 0, z * scale); // Muliply the scale by the cube's position to separate them apart.
                cube.transform.localScale = new Vector3(scale, scale, scale);
                cube.GetComponent<Renderer>().material.SetColor("_Color", c); // Add some color to distiguish the different partitions.

            }
        }



    }




}
