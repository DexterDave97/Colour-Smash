using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameOver;
    bool pauseState = false;
    public float delay;
    [Space]
    [SerializeField] private Color[] colors = new Color[12];
    [SerializeField] private Sprite[] sprites = new Sprite[13];
    void Start()
    {
        gameOver = false;
    }
    void Update()
    {
        //Debug.Log(pauseState);
        if(!pauseState)
            MoveTiles();
    }

    void MoveTiles()
    {
        if (Input.GetKeyDown(KeyCode.A) || TouchInputManager.instance.SwipeLeft)
        {
            StartCoroutine(SlideCoroutine(KeyCode.A));
        }

        else if (Input.GetKeyDown(KeyCode.D) || TouchInputManager.instance.SwipeRight)
        {
            StartCoroutine(SlideCoroutine(KeyCode.D));
        }

        else if (Input.GetKeyDown(KeyCode.W) || TouchInputManager.instance.SwipeUp)
        {
            StartCoroutine(SlideCoroutine(KeyCode.W));
        }

        else if (Input.GetKeyDown(KeyCode.S) || TouchInputManager.instance.SwipeDown)
        {
            StartCoroutine(SlideCoroutine(KeyCode.S));
        }
    }    

    void CheckToMerge(KeyCode key)
    {
        switch(key)
        {
            case KeyCode.A :
                for (int i = 1; i < AlignCubes.gridSize; i++)
                {
                    for (int j = 0; j < AlignCubes.gridSize; j++)
                    {
                        TileProperty thisCube = PlayZone.instance.currentState[i, j];
                        TileProperty otherCube = PlayZone.instance.currentState[i - 1, j];
                        if (otherCube && thisCube)
                        {
                            if ((otherCube.colorType == thisCube.colorType && otherCube.colorType == "Primary" && otherCube.color != thisCube.color) ||
                                (otherCube.colorType == thisCube.colorType && otherCube.colorType == "Secondary"))
                            {
                                #region Black Check
                                if (otherCube.iPos >0 && PlayZone.instance.currentState[otherCube.iPos - 1, otherCube.jPos] && PlayZone.instance.currentState[otherCube.iPos-1, otherCube.jPos].colorType=="Black")
                                {
                                    Destroy(PlayZone.instance.currentState[otherCube.iPos - 1, otherCube.jPos].transform.parent.gameObject);
                                    PlayZone.instance.currentState[otherCube.iPos - 1, otherCube.jPos] = null;
                                }
                                if (otherCube.jPos < AlignCubes.gridSize - 1 && PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos + 1] && PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos +1].colorType == "Black")
                                {
                                    Destroy(PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos + 1].transform.parent.gameObject);
                                    PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos + 1] = null;
                                }
                                if (otherCube.jPos > 0 && PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos - 1] && PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos -1].colorType == "Black")
                                {
                                    Destroy(PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos - 1].transform.parent.gameObject);
                                    PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos - 1] = null;
                                }
                                #endregion
                                PlayZone.instance.currentState[i - 1, j].gameObject.GetComponent<SpriteRenderer>().sprite = sprites[Mix(thisCube, otherCube, ref PlayZone.instance.currentState[i - 1, j])];
                                PlayZone.instance.currentState[i, j] = null;
                                PlayZone.instance.currentState[i - 1, j].transform.parent.GetComponent<Animator>().Play("Pop", 0, 0);
                                Destroy(thisCube.transform.parent.gameObject);
                                if (!PlayZone.instance.currentState[i - 1, j])
                                {
                                    Debug.Log(i + " " + j);
                                }
                                if (PlayZone.instance.currentState[i - 1, j].colorType == "Ultimate")
                                {
                                    Destroy(otherCube.transform.parent.gameObject);
                                    PlayZone.instance.currentState[i - 1, j] = null;
                                }
                            }
                        }
                    }
                }
                break;

            case KeyCode.D:
                for (int i = AlignCubes.gridSize - 2; i >= 0; i--)
                {
                    for (int j = 0; j < AlignCubes.gridSize; j++)
                    {
                        TileProperty thisCube = PlayZone.instance.currentState[i, j];
                        TileProperty otherCube = PlayZone.instance.currentState[i + 1, j];
                        if (otherCube && thisCube)
                        {
                            if ((otherCube.colorType == thisCube.colorType && otherCube.colorType == "Primary" && otherCube.color != thisCube.color) ||
                                (otherCube.colorType == thisCube.colorType && otherCube.colorType == "Secondary"))
                            {

                                #region Black Check
                                if (otherCube.iPos < AlignCubes.gridSize - 1 && PlayZone.instance.currentState[otherCube.iPos + 1, otherCube.jPos] && PlayZone.instance.currentState[otherCube.iPos + 1, otherCube.jPos].colorType == "Black")
                                {
                                    Destroy(PlayZone.instance.currentState[otherCube.iPos + 1, otherCube.jPos].transform.parent.gameObject);
                                    PlayZone.instance.currentState[otherCube.iPos + 1, otherCube.jPos] = null;
                                }
                                if (otherCube.jPos < AlignCubes.gridSize - 1 && PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos + 1] && PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos + 1].colorType == "Black")
                                {
                                    Destroy(PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos + 1].transform.parent.gameObject);
                                    PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos + 1] = null;
                                }
                                if (otherCube.jPos > 0 && PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos - 1] && PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos - 1].colorType == "Black")
                                {
                                    Destroy(PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos - 1].transform.parent.gameObject);
                                    PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos - 1] = null;
                                }
                                #endregion
                                PlayZone.instance.currentState[i + 1, j].gameObject.GetComponent<SpriteRenderer>().sprite = sprites[Mix(thisCube, otherCube, ref PlayZone.instance.currentState[i + 1, j])];
                                PlayZone.instance.currentState[i, j] = null;
                                PlayZone.instance.currentState[i + 1, j].transform.parent.GetComponent<Animator>().Play("Pop", 0, 0);
                                Destroy(thisCube.transform.parent.gameObject);
                                if (!PlayZone.instance.currentState[i + 1, j])
                                {
                                    Debug.Log(i + " " + j);
                                }
                                if (PlayZone.instance.currentState[i + 1, j].colorType == "Ultimate")
                                {
                                    Destroy(otherCube.transform.parent.gameObject);
                                    PlayZone.instance.currentState[i + 1, j] = null;
                                }
                            }
                        }
                    }
                }
                break;

            case KeyCode.W:
                for (int j = AlignCubes.gridSize-2; j >= 0; j--)
                {
                    for (int i = 0; i < AlignCubes.gridSize; i++)
                    {
                        TileProperty thisCube = PlayZone.instance.currentState[i, j];
                        TileProperty otherCube = PlayZone.instance.currentState[i, j + 1];
                        if (otherCube && thisCube)
                        {
                            if ((otherCube.colorType == thisCube.colorType && otherCube.colorType == "Primary" && otherCube.color != thisCube.color) ||
                                (otherCube.colorType == thisCube.colorType && otherCube.colorType == "Secondary"))
                            {
                                #region Black Check
                                if (otherCube.iPos > 0 && PlayZone.instance.currentState[otherCube.iPos - 1, otherCube.jPos] && PlayZone.instance.currentState[otherCube.iPos - 1, otherCube.jPos].colorType == "Black")
                                {
                                    Destroy(PlayZone.instance.currentState[otherCube.iPos - 1, otherCube.jPos].transform.parent.gameObject);
                                    PlayZone.instance.currentState[otherCube.iPos - 1, otherCube.jPos] = null;
                                }
                                if (otherCube.iPos < AlignCubes.gridSize -1 && PlayZone.instance.currentState[otherCube.iPos + 1, otherCube.jPos] && PlayZone.instance.currentState[otherCube.iPos + 1, otherCube.jPos].colorType == "Black")
                                {
                                    Destroy(PlayZone.instance.currentState[otherCube.iPos +1, otherCube.jPos].transform.parent.gameObject);
                                    PlayZone.instance.currentState[otherCube.iPos + 1, otherCube.jPos] = null;
                                }
                                if (otherCube.jPos >0 && PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos - 1] && PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos - 1].colorType == "Black")
                                {
                                    Destroy(PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos - 1].transform.parent.gameObject);
                                    PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos - 1] = null;
                                }
                                #endregion
                                PlayZone.instance.currentState[i, j + 1].gameObject.GetComponent<SpriteRenderer>().sprite = sprites[Mix(thisCube, otherCube, ref PlayZone.instance.currentState[i, j + 1])];
                                PlayZone.instance.currentState[i, j] = null;
                                PlayZone.instance.currentState[i, j + 1].transform.parent.GetComponent<Animator>().Play("Pop", 0, 0);
                                Destroy(thisCube.transform.parent.gameObject);
                                if (!PlayZone.instance.currentState[i, j + 1])
                                {
                                    Debug.Log(i + " " + j);
                                }
                                if (PlayZone.instance.currentState[i, j + 1].colorType == "Ultimate")
                                {
                                    Destroy(otherCube.transform.parent.gameObject);
                                    PlayZone.instance.currentState[i, j + 1] = null;
                                }
                            }
                        }
                    }
                }
                break;

            case KeyCode.S:
                for (int j = 1; j < AlignCubes.gridSize; j++)
                {
                    for (int i = 0; i < AlignCubes.gridSize; i++)
                    {
                        TileProperty thisCube = PlayZone.instance.currentState[i, j];
                        TileProperty otherCube = PlayZone.instance.currentState[i, j - 1];
                        if (otherCube && thisCube)
                        {
                            if ((otherCube.colorType == thisCube.colorType && otherCube.colorType == "Primary" && otherCube.color != thisCube.color) ||
                                (otherCube.colorType == thisCube.colorType && otherCube.colorType == "Secondary"))
                            {
                                #region Black Check
                                if (otherCube.iPos > 0 && PlayZone.instance.currentState[otherCube.iPos - 1, otherCube.jPos] && PlayZone.instance.currentState[otherCube.iPos - 1, otherCube.jPos].colorType == "Black")
                                {
                                    Destroy(PlayZone.instance.currentState[otherCube.iPos - 1, otherCube.jPos].transform.parent.gameObject);
                                    PlayZone.instance.currentState[otherCube.iPos - 1, otherCube.jPos] = null;
                                }
                                if (otherCube.iPos <AlignCubes.gridSize -1 && PlayZone.instance.currentState[otherCube.iPos + 1, otherCube.jPos] && PlayZone.instance.currentState[otherCube.iPos + 1, otherCube.jPos].colorType == "Black")
                                {
                                    Destroy(PlayZone.instance.currentState[otherCube.iPos + 1, otherCube.jPos].transform.parent.gameObject);
                                    PlayZone.instance.currentState[otherCube.iPos + 1, otherCube.jPos] = null;
                                }
                                if (otherCube.jPos>0 && PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos - 1] && PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos - 1].colorType == "Black")
                                {
                                    Destroy(PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos + 1].transform.parent.gameObject);
                                    PlayZone.instance.currentState[otherCube.iPos, otherCube.jPos + 1] = null;
                                }
                                #endregion
                                PlayZone.instance.currentState[i, j - 1].gameObject.GetComponent<SpriteRenderer>().sprite = sprites[Mix(thisCube, otherCube, ref PlayZone.instance.currentState[i, j - 1])];
                                PlayZone.instance.currentState[i, j] = null;
                                PlayZone.instance.currentState[i, j - 1].transform.parent.GetComponent<Animator>().Play("Pop", 0, 0);
                                Destroy(thisCube.transform.parent.gameObject);
                                if (!PlayZone.instance.currentState[i, j - 1])
                                {
                                    Debug.Log(i + " " + j);
                                }
                                if (PlayZone.instance.currentState[i, j - 1].colorType == "Ultimate")
                                {
                                    Destroy(otherCube.transform.parent.gameObject);
                                    PlayZone.instance.currentState[i, j - 1] = null;
                                }
                            }                            
                        }
                    }
                }
                break;
        }
    }

    void CreateNewTiles(KeyCode key)
    {
        int location = Random.Range(0, AlignCubes.gridSize);
        bool canCreate = false;
        switch(key)
        {
            case KeyCode.A:
                for(int i=0; i< AlignCubes.gridSize;i++)
                {
                    if(!PlayZone.instance.currentState[AlignCubes.gridSize - 1, i])
                    {
                        canCreate = true;
                        break;
                    }                    
                }
                if (canCreate)
                {
                    if (PlayZone.instance.currentState[AlignCubes.gridSize - 1, location] != null)
                    {
                        do
                        {
                            location = Random.Range(0, AlignCubes.gridSize);
                        } while (PlayZone.instance.currentState[AlignCubes.gridSize - 1, location] != null);
                    }
                    PlayZone.instance.currentState[AlignCubes.gridSize - 1, location] = Instantiate(AlignCubes.instance.prefabs[Random.Range(0, 3)], new Vector3((AlignCubes.gridSize - 1) / 2f, location - (AlignCubes.gridSize - 1) / 2f, 0), Quaternion.identity).transform.GetChild(0).GetComponent<TileProperty>();
                    PlayZone.instance.currentState[AlignCubes.gridSize - 1, location].iPos = AlignCubes.gridSize - 1;
                    PlayZone.instance.currentState[AlignCubes.gridSize - 1, location].jPos = location;
                }
                break;

            case KeyCode.D:
                for (int i = 0; i < AlignCubes.gridSize; i++)
                {
                    if (!PlayZone.instance.currentState[0, i])
                    {
                        canCreate = true;
                        break;
                    }
                }
                if (canCreate)
                {
                    if (PlayZone.instance.currentState[0, location] != null)
                    {
                        do
                        {
                            location = Random.Range(0, AlignCubes.gridSize);
                        } while (PlayZone.instance.currentState[0, location] != null);
                    }
                    PlayZone.instance.currentState[0, location] = Instantiate(AlignCubes.instance.prefabs[Random.Range(0, 3)], new Vector3(-(AlignCubes.gridSize - 1) / 2f, location - (AlignCubes.gridSize - 1) / 2f, 0), Quaternion.identity).transform.GetChild(0).GetComponent<TileProperty>();
                    PlayZone.instance.currentState[0, location].iPos = 0;
                    PlayZone.instance.currentState[0, location].jPos = location;
                }
                break;

            case KeyCode.W:
                for (int i = 0; i < AlignCubes.gridSize; i++)
                {
                    if (!PlayZone.instance.currentState[i, 0])
                    {
                        canCreate = true;
                        break;
                    }
                }
                if (canCreate)
                {
                    if (PlayZone.instance.currentState[location, 0] != null)
                    {
                        do
                        {
                            location = Random.Range(0, AlignCubes.gridSize);
                        } while (PlayZone.instance.currentState[location, 0] != null);
                    }
                    PlayZone.instance.currentState[location, 0] = Instantiate(AlignCubes.instance.prefabs[Random.Range(0, 3)], new Vector3(location - (AlignCubes.gridSize - 1) / 2f, -(AlignCubes.gridSize - 1) / 2f, 0), Quaternion.identity).transform.GetChild(0).GetComponent<TileProperty>();
                    PlayZone.instance.currentState[location, 0].iPos = location;
                    PlayZone.instance.currentState[location, 0].jPos = 0;
                }
                break;

            case KeyCode.S:
                for (int i = 0; i < AlignCubes.gridSize; i++)
                {
                    if (!PlayZone.instance.currentState[i, AlignCubes.gridSize - 1])
                    {
                        canCreate = true;
                        break;
                    }
                }
                if (canCreate)
                {
                    if (PlayZone.instance.currentState[location, AlignCubes.gridSize - 1] != null)
                    {
                        do
                        {
                            location = Random.Range(0, AlignCubes.gridSize);
                        } while (PlayZone.instance.currentState[location, AlignCubes.gridSize - 1] != null);
                    }
                    PlayZone.instance.currentState[location, AlignCubes.gridSize - 1] = Instantiate(AlignCubes.instance.prefabs[Random.Range(0, 3)], new Vector3(location - (AlignCubes.gridSize - 1) / 2f, (AlignCubes.gridSize - 1) / 2f, 0), Quaternion.identity).transform.GetChild(0).GetComponent<TileProperty>();
                    PlayZone.instance.currentState[location, AlignCubes.gridSize - 1].iPos = location;
                    PlayZone.instance.currentState[location, AlignCubes.gridSize - 1].jPos = AlignCubes.gridSize - 1;
                }
                break;
        }
    }
    
    int Mix(TileProperty t1, TileProperty t2, ref TileProperty newTile)
    {
        if ((t1.color == colors[0] && t2.color == colors[1]) || (t2.color == colors[0] && t1.color == colors[1]))
        {
            newTile.color = colors[5];
            newTile.colorType = "Secondary";
            return 5;
        }
        else if ((t1.color == colors[0] && t2.color == colors[2]) || (t2.color == colors[0] && t1.color == colors[2]))
        {
            newTile.color = colors[3];
            newTile.colorType = "Secondary";
            return 3;
        }
        else if ((t1.color == colors[1] && t2.color == colors[2]) || (t2.color == colors[1] && t1.color == colors[2]))
        {
            newTile.color = colors[4];
            newTile.colorType = "Secondary";
            return 4;
        }
        else if (t1.colorType == "Secondary" && t2.colorType == "Secondary" && t1.color != t2.color)
        {
            newTile.colorType = "Black";
            return 6;
        }
        #region scrap
        /*else if ((t1.color == colors[0] && t2.color == colors[3]) || (t2.color == colors[0] && t1.color == colors[3]))
        {
            newTile.color = colors[8];
            newTile.colorType = "Tertiary";
            return 8;
        }
        else if ((t1.color == colors[0] && t2.color == colors[5]) || (t2.color == colors[0] && t1.color == colors[5]))
        {
            newTile.color = colors[6];
            newTile.colorType = "Tertiary";
            return 6;
        }
        else if ((t1.color == colors[1] && t2.color == colors[5]) || (t2.color == colors[1] && t1.color == colors[5]))
        {
            newTile.color = colors[7];
            newTile.colorType = "Tertiary";
            return 7;
        }
        else if ((t1.color == colors[1] && t2.color == colors[4]) || (t2.color == colors[1] && t1.color == colors[4]))
        {
            newTile.color = colors[10];
            newTile.colorType = "Tertiary";
            return 10;
        }
        else if ((t1.color == colors[2] && t2.color == colors[3]) || (t2.color == colors[2] && t1.color == colors[3]))
        {
            newTile.color = colors[9];
            newTile.colorType = "Tertiary";
            return 9;
        }
        else if ((t1.color == colors[2] && t2.color == colors[4]) || (t2.color == colors[2] && t1.color == colors[4]))
        {
            newTile.color = colors[11];
            newTile.colorType = "Tertiary";
            return 11;
        }*/
        #endregion
        else if (t1.colorType == "Secondary" && t2.colorType == "Secondary" && t1.color == t2.color)
        {
            newTile.colorType = "Ultimate";
            return 12;
        }
        else
            return 12;
    }

    bool SlideToSide(KeyCode key)
    {
        int i, j;
        switch (key)
        {
            case KeyCode.A:
                for (i = 1; i < AlignCubes.gridSize; i++)
                {
                    for (j = 0; j < AlignCubes.gridSize; j++)
                    {
                        if (PlayZone.instance.currentState[i, j] && PlayZone.instance.currentState[i, j].colorType != "Black")
                        {
                            TileProperty otherCube = PlayZone.instance.currentState[i - 1, j];
                            if (otherCube == null)
                            {
                                PlayZone.instance.currentState[i, j].transform.parent.position += Vector3.left;
                                PlayZone.instance.currentState[i - 1, j] = PlayZone.instance.currentState[i, j];
                                PlayZone.instance.currentState[i, j] = null;
                                return true;
                            }
                        }
                    }
                }
                break;
            case KeyCode.D:
                for (i = AlignCubes.gridSize - 2; i >= 0; i--)
                {
                    for (j = 0; j < AlignCubes.gridSize; j++)
                    {
                        if (PlayZone.instance.currentState[i, j] && PlayZone.instance.currentState[i, j].colorType != "Black")
                        {
                            TileProperty thisCube = PlayZone.instance.currentState[i, j];
                            TileProperty otherCube = PlayZone.instance.currentState[i + 1, j];
                            if (otherCube == null)
                            {
                                PlayZone.instance.currentState[i, j].transform.parent.position += Vector3.right;
                                PlayZone.instance.currentState[i + 1, j] = PlayZone.instance.currentState[i, j];
                                PlayZone.instance.currentState[i, j] = null;
                                return true;
                            }
                        }
                    }
                }
                break;
            case KeyCode.W:
                for (j = AlignCubes.gridSize - 2; j >= 0; j--)
                {
                    for (i = 0; i < AlignCubes.gridSize; i++)
                    {
                        if (PlayZone.instance.currentState[i, j] && PlayZone.instance.currentState[i, j].colorType != "Black")
                        {
                            TileProperty otherCube = PlayZone.instance.currentState[i, j + 1];
                            if (otherCube == null)
                            {
                                PlayZone.instance.currentState[i, j].transform.parent.position += Vector3.up;
                                PlayZone.instance.currentState[i, j + 1] = PlayZone.instance.currentState[i, j];
                                PlayZone.instance.currentState[i, j] = null;
                                return true;
                            }
                        }
                    }
                }
                break;
            case KeyCode.S:
                for (j = 1; j < AlignCubes.gridSize; j++)
                {
                    for (i = 0; i < AlignCubes.gridSize; i++)
                    {
                        if (PlayZone.instance.currentState[i, j] && PlayZone.instance.currentState[i, j].colorType != "Black")
                        {
                            TileProperty otherCube = PlayZone.instance.currentState[i, j - 1];
                            if (otherCube == null)
                            {
                                PlayZone.instance.currentState[i, j].transform.parent.position += Vector3.down;
                                PlayZone.instance.currentState[i, j - 1] = PlayZone.instance.currentState[i, j];
                                PlayZone.instance.currentState[i, j] = null;
                                return true;
                            }
                        }
                    }
                }
                break;
        }

        return false;
    }

    /*void SlideToSide(KeyCode key)
   {
       int i, j, k;
       Vector2[,] PosVectors = new Vector2[AlignCubes.gridSize, AlignCubes.gridSize];
       for (i = 1; i < AlignCubes.gridSize; i++)
       {
           for (j = 0; j < AlignCubes.gridSize; j++)
           {
               PosVectors[i, j] = new Vector2(i, j);
           }
       }

       switch (key)
       {
           case KeyCode.A:
               for (i = 1; i < AlignCubes.gridSize; i++)
               {
                   for (j = 0; j < AlignCubes.gridSize; j++)
                   {
                       if (PlayZone.instance.currentState[i, j] && PlayZone.instance.currentState[i, j].colorType != "Black")
                       {
                           TileProperty thisCube = PlayZone.instance.currentState[i, j];
                           TileProperty otherCube = PlayZone.instance.currentState[i - 1, j];
                           k = i;
                           while (k > 0 && !PlayZone.instance.currentState[k - 1, j])
                           {
                               thisCube.iPos = k - 1;
                               PlayZone.instance.currentState[k - 1, j] = thisCube;
                               PlayZone.instance.currentState[k, j] = null;
                               PosVectors[i, j] = new Vector2(PosVectors[i, j].x-1, PosVectors[i, j].y);
                               k--;
                           }
                       }
                   }
               }
               break;
           case KeyCode.D:
               for (i = AlignCubes.gridSize - 2; i >= 0; i--)
               {
                   for (j = 0; j < AlignCubes.gridSize; j++)
                   {
                       if (PlayZone.instance.currentState[i, j] && PlayZone.instance.currentState[i, j].colorType != "Black")
                       {
                           TileProperty thisCube = PlayZone.instance.currentState[i, j];
                           TileProperty otherCube = PlayZone.instance.currentState[i + 1, j];
                           k = i;
                           while (k < AlignCubes.gridSize - 1 && !PlayZone.instance.currentState[k + 1, j])
                           {
                               thisCube.transform.parent.position += Vector3.right;
                               thisCube.iPos = k + 1;
                               PlayZone.instance.currentState[k + 1, j] = thisCube;
                               PlayZone.instance.currentState[k, j] = null;
                               k++;
                           }
                       }
                   }
               }
               break;
           case KeyCode.W:
               for (j = AlignCubes.gridSize - 2; j >= 0; j--)
               {
                   for (i = 0; i < AlignCubes.gridSize; i++)
                   {
                       if (PlayZone.instance.currentState[i, j] && PlayZone.instance.currentState[i, j].colorType != "Black")
                       {
                           TileProperty thisCube = PlayZone.instance.currentState[i, j];
                           TileProperty otherCube = PlayZone.instance.currentState[i, j + 1];
                           k = j;
                           while (k < AlignCubes.gridSize - 1 && !PlayZone.instance.currentState[i, k + 1])
                           {
                               thisCube.transform.parent.position += Vector3.up;
                               thisCube.jPos = k + 1;
                               PlayZone.instance.currentState[i, k + 1] = thisCube;
                               PlayZone.instance.currentState[i, k] = null;
                               k++;
                           }
                       }
                   }
               }
               break;
           case KeyCode.S:
               for (j = 1; j < AlignCubes.gridSize; j++)
               {
                   for (i = 0; i < AlignCubes.gridSize; i++)
                   {
                       if (PlayZone.instance.currentState[i, j] && PlayZone.instance.currentState[i, j].colorType != "Black")
                       {
                           TileProperty thisCube = PlayZone.instance.currentState[i, j];
                           TileProperty otherCube = PlayZone.instance.currentState[i, j - 1];
                           k = j;
                           while (k > 0 && !PlayZone.instance.currentState[i, k - 1])
                           {
                               thisCube.jPos = k - 1;
                               PlayZone.instance.currentState[i, k - 1] = thisCube;
                               PlayZone.instance.currentState[i, k] = null;
                               k--;
                           }
                       }
                   }
               }
               break;
       }
       SetTilePos(PlayZone.instance.currentState, PosVectors);
   }*/
    /*void SetTilePos(TileProperty[,] newMap, Vector2[,] movePositions)
    {
        for (int i = 0; i < AlignCubes.gridSize; i++)
            for (int j = 0; j < AlignCubes.gridSize; j++)
            {
                if(newMap[(int)movePositions[i, j].x,j])
                {
                    while(newMap[(int)movePositions[i, j].x, j].transform.parent.position!= new Vector3(movePositions[i,j].x - ((AlignCubes.gridSize - 1) / 2f), movePositions[i, j].y - (AlignCubes.gridSize - 1) / 2f))
                        newMap[(int)movePositions[i, j].x, j].transform.parent.position = Vector3.Lerp(newMap[(int)movePositions[i, j].x, j].transform.parent.position, new Vector3(movePositions[i, j].x - ((AlignCubes.gridSize - 1) / 2f), movePositions[i, j].y - (AlignCubes.gridSize - 1) / 2f),0.1f);
                }
            }
    }*/


    IEnumerator SlideCoroutine(KeyCode key)
    {
        pauseState = true;
        while (SlideToSide(key))
        {
            yield return new WaitForSeconds(delay);
        }
        CheckToMerge(key);
        while (SlideToSide(key))
        {
            yield return new WaitForSeconds(delay);
        }
        CreateNewTiles(key);
        pauseState = false;
        StopAllCoroutines();
    }
}