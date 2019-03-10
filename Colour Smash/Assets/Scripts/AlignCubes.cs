using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignCubes : MonoBehaviour
{
    public static int gridSize;
    public static AlignCubes instance;
    public List<GameObject> prefabs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gridSize = 4;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (PlayZone.instance == null)
                {
                    new PlayZone();
                }
                PlayZone.instance.currentState[i, j] = Instantiate(prefabs[Random.Range(0, 3)], new Vector3(i - (gridSize - 1)/2f, j - (gridSize - 1) / 2f, 0), Quaternion.identity).transform.GetChild(0).GetComponent<TileProperty>(); 
                PlayZone.instance.currentState[i, j].name = i.ToString() + j.ToString();
                PlayZone.instance.currentState[i, j].iPos = i;
                PlayZone.instance.currentState[i, j].jPos = j;
            }
        }
    }
    
}
