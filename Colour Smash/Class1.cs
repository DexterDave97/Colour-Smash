using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoardManager : MonoBehaviour
{


    public enum GState
    {
        Playing,
        WaitingForMoveToEnd,
        GameOver
    }

    public GState state;
    [Range(0, 2f)]
    public float delay;
    private bool MoveMade = false;
    private bool[] linecomplete = new bool[4] { true, true, true, true };
    private Tile[,] allTiles = new Tile[4, 4];
    private List<Tile> FreeTiles = new List<Tile>();
    private List<Tile[]> columns = new List<Tile[]>();
    private List<Tile[]> rows = new List<Tile[]>();


    public GameObject GameOverText;
    public Text newScoretoBeatText;
    public GameObject wonText;
    public Text Gscore;
    public GameObject gOverpanel;
    private bool hasWon = false;
    private int newHighTile;
    // Use this for initialization


    void Start()
    {
        if (PlayerPrefs.HasKey("newHighTile"))
        {
            newHighTile = PlayerPrefs.GetInt("newHighTile");
        }
        else
        {
            newHighTile = 2048;
        }

        //  PlayerPrefs.DeleteAll();







        newScoretoBeatText.text = "join the numbers and get to the \n" + "<b>" + newHighTile.ToString() + "</b> tile!";

        gOverpanel.SetActive(false);
        Tile[] inGameTiles = GameObject.FindObjectsOfType<Tile>();
        foreach (Tile t in inGameTiles)

        {

            allTiles[t.row, t.column] = t;
            t.Number = 0;
            FreeTiles.Add(t);
        }

        columns.Add(new Tile[] { allTiles[0, 0], allTiles[1, 0], allTiles[2, 0], allTiles[3, 0] });
        columns.Add(new Tile[] { allTiles[0, 1], allTiles[1, 1], allTiles[2, 1], allTiles[3, 1] });
        columns.Add(new Tile[] { allTiles[0, 2], allTiles[1, 2], allTiles[2, 2], allTiles[3, 2] });
        columns.Add(new Tile[] { allTiles[0, 3], allTiles[1, 3], allTiles[2, 3], allTiles[3, 3] });

        rows.Add(new Tile[] { allTiles[0, 0], allTiles[0, 1], allTiles[0, 2], allTiles[0, 3] });
        rows.Add(new Tile[] { allTiles[1, 0], allTiles[1, 1], allTiles[1, 2], allTiles[1, 3] });
        rows.Add(new Tile[] { allTiles[2, 0], allTiles[2, 1], allTiles[2, 2], allTiles[2, 3] });
        rows.Add(new Tile[] { allTiles[3, 0], allTiles[3, 1], allTiles[3, 2], allTiles[3, 3] });


        GenerateTiles();
        GenerateTiles();



    }

    // Update is called once per frame
    void Update()
    {


    }
    public void Move(InputManager.M_Direction direction)
    {
        emptyMerge();
        Debug.Log(direction.ToString() + "Move ");
        MoveMade = false;
        if (delay > 0)
        {
            StartCoroutine(MoveCoroutine(direction));
        }
        else
        {


            for (int i = 0; i < rows.Count; i++)

            {
                switch (direction)
                {
                    case InputManager.M_Direction.DOWN:
                        while (MakeOneMoveUp(columns[i]))
                        {
                            MoveMade = true;
                        };
                        break;
                    case InputManager.M_Direction.UP:
                        while (MakeOneMoveDown(columns[i]))
                        {
                            MoveMade = true;
                        };
                        break;
                    case InputManager.M_Direction.LEFT:
                        while (MakeOneMoveDown(rows[i]))
                        {
                            MoveMade = true;
                        };
                        break;
                    case InputManager.M_Direction.RIGHT:
                        while (MakeOneMoveUp(rows[i]))
                        {
                            MoveMade = true;
                        };
                        break;
                }
            }
            if (MoveMade)
            {
                UpdateFreeTiles();

                GenerateTiles();
                if (!canMove())
                {
                    GameOver();
                }

            }
        }
    }
    private void UpdateFreeTiles()
    {
        FreeTiles.Clear();
        foreach (Tile t in allTiles)
        {
            if (t.Number == 0)
                FreeTiles.Add(t);
        }
    }


    void GenerateTiles()
    {
        if (FreeTiles.Count > 0)
        {
            int randnum = Random.Range(0, FreeTiles.Count);
            int randomNum = Random.Range(0, 10);
            if (randomNum == 0)
            {
                randomNum = 4;
            }
            else
            {
                randomNum = 2;
            }
            FreeTiles[randnum].PlayAppear();
            FreeTiles[randnum].Number = randomNum;
            FreeTiles.RemoveAt(randnum);

        }
    }
    bool MakeOneMoveDown(Tile[] lineofTiles)
    {
        for (int i = 0; i < lineofTiles.Length - 1; i++)
        {
            //move block
            if (lineofTiles[i].Number == 0 && lineofTiles[i + 1].Number != 0)
            {
                lineofTiles[i].Number = lineofTiles[i + 1].Number;
                lineofTiles[i + 1].Number = 0;
                return true;
            }
            if (lineofTiles[i].Number != 0 && lineofTiles[i].Number == lineofTiles[i + 1].Number && lineofTiles[i].merged == false && lineofTiles[i + 1].merged == false)
            {
                lineofTiles[i].PlayMerged();
                lineofTiles[i].Number *= 2;
                lineofTiles[i + 1].Number = 0;
                lineofTiles[i].merged = true;
                ScoreTrack.instance.score += lineofTiles[i].Number;
                if (lineofTiles[i].Number == newHighTile)
                {
                    //  Won();
                    newHighTile *= 2;
                    newScoretoBeatText.text = "join the numbers and get to the \n" + "<b>" + newHighTile.ToString() + "</b> tile!";
                    PlayerPrefs.SetInt("newHighTile", newHighTile);
                }
                return true;
            }

        }
        return false;
    }

    bool MakeOneMoveUp(Tile[] lineofTiles)
    {
        for (int i = lineofTiles.Length - 1; i > 0; i--)
        {
            //move block
            if (lineofTiles[i].Number == 0 && lineofTiles[i - 1].Number != 0)
            {
                lineofTiles[i].Number = lineofTiles[i - 1].Number;
                lineofTiles[i - 1].Number = 0;
                return true;
            }
            if (lineofTiles[i].Number != 0 && lineofTiles[i].Number == lineofTiles[i - 1].Number && lineofTiles[i].merged == false && lineofTiles[i - 1].merged == false)
            {
                lineofTiles[i].PlayMerged();
                lineofTiles[i].Number *= 2;
                lineofTiles[i - 1].Number = 0;
                lineofTiles[i].merged = true;
                ScoreTrack.instance.score += lineofTiles[i].Number;
                if (lineofTiles[i].Number == newHighTile)
                {
                    //  Won();
                    newHighTile *= 2;
                    newScoretoBeatText.text = "join the numbers and get to the \n" + "<b>" + newHighTile.ToString() + "</b> tile!";
                    PlayerPrefs.SetInt("newHighTile", newHighTile);
                }
                return true;
            }
        }
        return false;
    }
    private void emptyMerge()
    {
        foreach (Tile t in allTiles)
        {
            t.merged = false;
        }
    }


    bool canMove()
    {
        if (FreeTiles.Count > 0)
        {
            return true;
        }
        else
        {
            for (int i = 0; i < columns.Count; i++)
            {
                for (int j = 0; j < rows.Count - 1; j++)
                {
                    if (allTiles[j, i].Number == allTiles[j + 1, i].Number)
                    {
                        return true;
                    }

                }
            }




            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < columns.Count - 1; j++)
                {
                    if (allTiles[i, j].Number == allTiles[i, j + 1].Number)
                    {
                        return true;
                    }

                }
            }
        }
        return false;

    }
    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
    private void GameOver()
    {
        state = GState.GameOver;
        Gscore.text = ScoreTrack.instance.score.ToString();
        gOverpanel.SetActive(true);
    }
    private void Won()
    {
        GameOverText.SetActive(false);
        wonText.SetActive(true);
        Gscore.text = ScoreTrack.instance.score.ToString();
        gOverpanel.SetActive(true);
    }


    IEnumerator MoveCoroutine(InputManager.M_Direction md)
    {
        state = GState.WaitingForMoveToEnd;
        switch (md)
        {
            case InputManager.M_Direction.DOWN:
                for (int i = 0; i < columns.Count; i++)
                    StartCoroutine(MoveOneLineUp(columns[i], i));
                break;
            case InputManager.M_Direction.LEFT:
                for (int i = 0; i < rows.Count; i++)
                    StartCoroutine(MoveOneLineDown(rows[i], i));
                break;
            case InputManager.M_Direction.RIGHT:
                for (int i = 0; i < rows.Count; i++)
                    StartCoroutine(MoveOneLineUp(rows[i], i));
                break;
            case InputManager.M_Direction.UP:
                for (int i = 0; i < columns.Count; i++)
                    StartCoroutine(MoveOneLineDown(columns[i], i));
                break;

        }
        while (!(linecomplete[0] && linecomplete[1] && linecomplete[2] && linecomplete[3]))
        {
            yield return null;
        }
        if (MoveMade)
        {
            UpdateFreeTiles();
            GenerateTiles();
            //  SaveAllTiles();
            if (!canMove())
            {
                GameOver();
            }


        }

        state = GState.Playing;
        StopAllCoroutines();
    }

    IEnumerator MoveOneLineUp(Tile[] t, int m)
    {

        linecomplete[m] = false;
        while (MakeOneMoveUp(t))
        {
            MoveMade = true;
            yield return new WaitForSeconds(delay);
        }
        linecomplete[m] = true;
    }
    IEnumerator MoveOneLineDown(Tile[] t, int m)
    {
        linecomplete[m] = false;
        while (MakeOneMoveDown(t))
        {
            MoveMade = true;
            yield return new WaitForSeconds(delay);
        }
        linecomplete[m] = true;
    }
    /*  private void SaveAllTiles()
      {
          for (int i = 0; i < 16;i++)
          {

              PlayerPrefs.SetInt("Tile" + i.ToString(),allTiles[i/4,i%4].Number);
              Debug.Log(PlayerPrefs.GetInt("Tile" + i.ToString()));

          }
      }
      */

}