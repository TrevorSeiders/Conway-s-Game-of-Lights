using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOL_Entity : MonoBehaviour
{
    bool isAlive = false;
    GameObject[] neighbors;

    float alpha = 0.0f;
    Color color = Color.black;
    Material mat;


    // Start is called before the first frame update
    void Start()
    {
        mat = gameObject.GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void UpdateStatus()
    {
        int numLivingNeighbors = 0;
        foreach ( GameObject n in neighbors )
        {
            if ( n.GetComponent<GOL_Entity>().isAlive )
            {
                numLivingNeighbors++;
            }
        }

        if ( isAlive )
        {
            if ( numLivingNeighbors < 2 || numLivingNeighbors > 3 )
            {
                isAlive = false;
            }
        }
        else
        {
            if ( numLivingNeighbors == 3 )
            {
                isAlive = true;
                color = new Color(Random.value, Random.value, Random.value);
                mat.color = color;
            }
        }

        if ( isAlive )
        {
            alpha += 0.1f;
        }
        else
        {
            alpha = 0.0f;
        }
    }
}
