using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOL_Controller : MonoBehaviour
{
    // Three-dimensional array representation of simulation space
    public GameObject[][][] entities;
    public GameObject holder;

    // Percent chance a given entity will start alive
    public static readonly int CHANCE_TO_LIVE = 20;


    // Start is called before the first frame update
    void Start()
    {
        // Because we can't assign a 3D array in the editor, we have
        // to create the entity array here. On the plus side, this means
        // the size and shape of the simulation space is flexible.
        GameObject holder = GameObject.Find("EntityHolder");

        // Holder has X number of Levels in it
        int numHolderChildren = holder.transform.childCount;
        entities = new GameObject[numHolderChildren][][];

        for ( int i = 0; i < numHolderChildren; i++ )
        {
            GameObject level = holder.transform.GetChild(i).gameObject;

            // Level has Y number of Rows in it
            int numLevelChildren = level.transform.childCount;
            entities[i] = new GameObject[numLevelChildren][];

            for ( int j = 0; j < numLevelChildren; j++ )
            {
                GameObject row = level.transform.GetChild(j).gameObject;

                // Row has Z number of objects in it
                int numRowChildren = row.transform.childCount;
                entities[i][j] = new GameObject[numRowChildren];

                for (int k = 0; k < numRowChildren; k++)
                {
                    entities[i][j][k] = row.transform.GetChild(k).gameObject;




                    // We (naively) give each entity a percent chance to start alive.
                    // This implementation will likely be improved in the future.

                    int lifeVal = Random.Range(1, 101);
                    if (lifeVal <= CHANCE_TO_LIVE)
                    {
                        GOL_Entity entity = entities[i][j][k].GetComponent<GOL_Entity>();
                        entity.isAlive = true;
                        entity.alpha = 1.0f;
                        entity.AssignOpacity();
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
