using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime;

public class GOL_Controller : MonoBehaviour
{
    // Three-dimensional array representation of simulation space
    public GameObject[][][] entities;
    public GameObject holder;
    public bool loopEdges = true;

    // Percent chance a given entity will start alive
    public static readonly int CHANCE_TO_LIVE = 5;


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
                    GOL_Entity entity = entities[i][j][k].GetComponent<GOL_Entity>();
                    

                    // We (naively) give each entity a percent chance to start alive.
                    // This implementation will likely be improved in the future.

                    int lifeVal = Random.Range(1, 101);
                    if (lifeVal <= CHANCE_TO_LIVE)
                    {
                        
                        entity.isAlive = true;
                        entity.alpha = 1.0f;
                        entity.AssignOpacity();
                    }
                }
            }
        }

        for ( int i = 0; i < entities.Length; i++ )
        {
            for ( int j = 0; j < entities[i].Length; j++ )
            {
                for ( int k = 0; k < entities[i][j].Length; k++ )
                {
                    try
                    {
                        GOL_Entity entity = entities[i][j][k].GetComponent<GOL_Entity>();
                        AddNeighborsToEntity(entity, i, j, k);
                    }
                    catch (System.IndexOutOfRangeException oore)
                    {
                        // If something is out of range, our shape
                        // isn't a cube. We just want to keep going.
                    }
                    
                }
            }
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Assigns the neighbors to an entity. A neighbor is a separate
    /// entity that is one 'space' away from the current entity. In
    /// three dimensions, this is up to 26 entities.
    /// 
    /// If looping edges are enabled, an entity whose neighbor would
    /// be out of range will instead refer to the other side of the
    /// plane, like Pac-Man.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="level">Level of provided entity</param>
    /// <param name="row">Row of provided entity</param>
    /// <param name="col">Column of provided entity</param>
    void AddNeighborsToEntity(GOL_Entity entity, int level, int row, int col)
    {
        // If any of the following is true, this is a bad request
        if ( level < 0 || row < 0 || col < 0 
            || level >= entities.Length 
            || row >= entities[level].Length 
            || col >= entities[level][row].Length)
        {
            return;
        }

        // Start with the 'top-top-left' of the box
        int nL = level - 1;
        int nR = row - 1;
        int nC = col - 1;

        for ( int i = 0; i < 26; i++ )
        {
            if (i > 0)
            {
                if (i % 9 == 0)
                {
                    nR -= 2;
                    nC -= 2;
                    nL++;
                }
                else if (i % 3 == 0)
                {
                    nR -= 2;
                    nC++;
                }
                else
                {
                    nR++;
                }
            }

            // Skip the center point (this entity)
            if ( i != 13 )
            {
                AddNeighbor(entity, nL, nR, nC);
            }
        }
    }


    void AddNeighbor(GOL_Entity entity, int nL, int nR, int nC)
    {
        GameObject n = null;
        n = FindNeighbor(nL, nR, nC);
        if (n != null)
        {
            entity.neighbors.Add(n);
        }
    }
    

    GameObject FindNeighbor(int level, int row, int col)
    {
        GameObject neighbor = null;
        bool wrapped = false;
        if ( level < 0 )
        {
            wrapped = true;
            level = entities.Length - 1;
        }
        if ( row < 0 )
        {
            wrapped = true;
            row = entities[level].Length - 1;
        }
        if ( col < 0 )
        {
            wrapped = true;
            col = entities[level][row].Length - 1;
        }

        if ( level >= entities.Length )
        {
            wrapped = true;
            level = 0;
        }
        if ( row >= entities[level].Length )
        {
            wrapped = true;
            row = 0;
        }
        if ( col >= entities[level][row].Length )
        {
            wrapped = true;
            col = 0;
        }

        if ( wrapped && !loopEdges )
        {
            return null;
        }

        // TODO: Make this function handle an irregular
        // (non-cuboid) environment

        neighbor = entities[level][row][col];
        return neighbor;
    }
}
