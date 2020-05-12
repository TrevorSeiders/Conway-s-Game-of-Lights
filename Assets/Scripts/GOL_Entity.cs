using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOL_Entity : MonoBehaviour
{
    public bool isAlive = false;
    GameObject[] neighbors = new GameObject[0];

    public float alpha = 0.0f;
    Color color = Color.black;
    Material mat;

    // Delay between calls to update the game
    public float delayTime = 1.0f;
    public float timer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        mat = gameObject.GetComponent<Renderer>().material;
        AssignRandomColor();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= delayTime)
        {
            timer = 0f;
            UpdateStatus();
        }
    }


    /// <summary>
    /// Performs the necessary GoL checks and updates
    /// the status of the entity accordingly.
    /// </summary>
    public void UpdateStatus()
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
                AssignRandomColor();
            }
        }

        if ( isAlive )
        {
            alpha += 0.1f;
        }
        else
        {
            alpha -= 0.05f;
        }
    }


    /// <summary>
    /// Assigns a random color to the entity's material.
    /// </summary>
    public void AssignRandomColor()
    {
        color = new Color(Random.value, Random.value, Random.value, alpha);
        Material newMat = mat;
        newMat.color = color;
        mat = newMat;
    }


    public void AssignOpacity()
    {
        Material newMat = mat;
        color.a = alpha;
        mat.color = color;
        mat = newMat;
    }
}
