using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProperty : MonoBehaviour
{
    public int iPos, jPos;
    public Color color;
    public string colorType = "Primary";

    void OnEnable()
    {
        colorType = "Primary";
        transform.parent.GetComponent<Animator>().Play("Spawn", 0, 0);
    }
    void OnDisable()
    {
        PlayZone.instance.currentState[iPos, jPos] = null;
    }
}
