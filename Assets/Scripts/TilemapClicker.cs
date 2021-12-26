using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static CsGlobals;
//using AIChoser;

public class TilemapClicker : MonoBehaviour
{
    public TileBase TilesToSet1;
    public TileBase TilesToSet2;
    public TileBase TilesToSet3;
    
    private Tilemap map;
    private Camera mainCamera;

    private Vector3Int leftBottomTilemapLimit;
    private Vector3Int rigthUpperTilemapLimit;
   
    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<Tilemap>();
        mainCamera = Camera.main;

        Debug.Log(CsGlobals.RealPlayers[0].ToString());
        Debug.Log(CsGlobals.RealPlayers[1].ToString());
        Debug.Log(CsGlobals.RealPlayers[2].ToString());
        
        leftBottomTilemapLimit = new Vector3Int(CsGlobals.leftLimit, CsGlobals.bottomLimit, 0);
        rigthUpperTilemapLimit = new Vector3Int(CsGlobals.rightLimit, CsGlobals.upperLimit, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
            return;
        }

        if (CsGlobals.RealPlayers[CsGlobals.gamerNumber - 1])
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 clickWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

                Vector3Int clickCellPosition = map.WorldToCell(clickWorldPosition);

                //Debug.Log(clickCellPosition);

                Vector3 leftBottomCameraLimit =
                    Camera.main.ViewportToWorldPoint(new Vector3(0, 0,
                        Camera.main.nearClipPlane)); //Получаем вектор нижнего левого угла камеры
                Vector3 rigthUpperCameraLimit =
                    Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f,
                        Camera.main.nearClipPlane)); //Получаем верхний правый угол камеры


                if (((leftBottomTilemapLimit.x <= clickCellPosition.x) &
                     (clickCellPosition.x < rigthUpperTilemapLimit.x)) &
                    ((leftBottomTilemapLimit.y <= clickCellPosition.y) &
                     (clickCellPosition.y < rigthUpperTilemapLimit.y)) &
                    ((leftBottomCameraLimit.x - 1 <= clickCellPosition.x) &
                     (clickCellPosition.x < rigthUpperCameraLimit.x)) &
                    ((leftBottomCameraLimit.y - 1 <= clickCellPosition.y) &
                     (clickCellPosition.y < rigthUpperCameraLimit.y)))
                {
                    if (CsGlobals.map[clickCellPosition.x - leftBottomTilemapLimit.x,
                        clickCellPosition.y - leftBottomTilemapLimit.y] == 0)
                    {
                        switch (CsGlobals.gamerNumber)
                        {
                            case 1:
                                map.SetTile(clickCellPosition, TilesToSet1);
                                break;
                            case 2:
                                map.SetTile(clickCellPosition, TilesToSet2);
                                break;
                            case 3:
                                map.SetTile(clickCellPosition, TilesToSet3);
                                break;
                            default:
                                return;
                        }

                        CsGlobals.map[clickCellPosition.x - leftBottomTilemapLimit.x,
                            clickCellPosition.y - leftBottomTilemapLimit.y] = CsGlobals.gamerNumber;

                        Debug.Log(CsGlobals.gamerNumber);
                        Debug.Log(clickCellPosition);
                        Debug.Log(new Vector3Int(clickCellPosition.x - leftBottomTilemapLimit.x,
                            clickCellPosition.y - leftBottomTilemapLimit.y, 0));

                        if (isWin(clickCellPosition.x - leftBottomTilemapLimit.x,
                            clickCellPosition.y - leftBottomTilemapLimit.y, CsGlobals.gamerNumber))
                        {
                            SceneManager.LoadScene(2);
                            return;
                        }

                        AIChooser AI = new AIChooser();
                        AI.UpdateCosts(clickCellPosition.x - leftBottomTilemapLimit.x,
                            clickCellPosition.y - leftBottomTilemapLimit.y);
                        FirstTile = false;

                        CsGlobals.gamerNumber++;
                        if (CsGlobals.gamerNumber > 3) CsGlobals.gamerNumber = 1;
                        //if (CsGlobals.gamerNumber < 1) CsGlobals.gamerNumber = 3;
                    }
                }
            }
        }
        else if (FirstTile)
        {
            CsGlobals.map[0 - leftBottomTilemapLimit.x,
                0 - leftBottomTilemapLimit.y] = CsGlobals.gamerNumber;

            AIChooser AI = new AIChooser();
            AI.UpdateCosts(0 - leftBottomTilemapLimit.x,
                0 - leftBottomTilemapLimit.y);
            
            CsGlobals.gamerNumber++;
            if (CsGlobals.gamerNumber > 3) CsGlobals.gamerNumber = 1;
            FirstTile = false;

        }
        else
        {
            //StartCoroutine(waiter());

            AIChooser AI = new AIChooser();
            List<(int x, int y)> winMoves = AI.GetPossibleMoves();
        
            System.Random rnd = new System.Random();
            (int x, int y) point;
                if (winMoves.Count <= 0)
                    point = (100, 100);
                else
                point = winMoves[rnd.Next(winMoves.Count)];
            Vector3Int clickCellPosition = new Vector3Int(point.x + leftBottomTilemapLimit.x, 
                point.y + leftBottomTilemapLimit.y, 0);
            
            switch (CsGlobals.gamerNumber)
            {
                case 1:
                    map.SetTile(clickCellPosition, TilesToSet1);
                    break;
                case 2:
                    map.SetTile(clickCellPosition, TilesToSet2);
                    break;
                case 3:
                    map.SetTile(clickCellPosition, TilesToSet3);
                    break;
                default:
                    return;
            }

            CsGlobals.map[clickCellPosition.x - leftBottomTilemapLimit.x,
                clickCellPosition.y - leftBottomTilemapLimit.y] = CsGlobals.gamerNumber;
                
            Debug.Log(CsGlobals.gamerNumber);
            Debug.Log(clickCellPosition);
            Debug.Log(new Vector3Int(point.x, point.y, 0));
            if (isWin(clickCellPosition.x - leftBottomTilemapLimit.x,
                clickCellPosition.y - leftBottomTilemapLimit.y, CsGlobals.gamerNumber))
            {
                SceneManager.LoadScene(2);
                return;
            }

            AI.UpdateCosts(clickCellPosition.x - leftBottomTilemapLimit.x,
                clickCellPosition.y - leftBottomTilemapLimit.y);
            
            CsGlobals.gamerNumber++;
            if (CsGlobals.gamerNumber > 3) CsGlobals.gamerNumber = 1;
            FirstTile = false;

        }


    }

    private static readonly int[,] deltaArray = new int[,] {{-1, 1}, {0, 1}, {1, 1}, {1, 0}};

    bool isWin(int x, int y, byte n)
    {
        for (var j = 0; j < 4; j++)
        {
            var chipsNumberInRow = 1;
            for (var k = 1; k >= -1; k = k - 2)
            {
                var dx = k * deltaArray[j, 0];
                var dy = k * deltaArray[j, 1];
                for (var i = 1; i < 5; i++)
                {
                    if (CsGlobals.map[x + i*dx, y + i*dy] == n)
                        chipsNumberInRow++;
                    else
                        break;
                }
            }

            if (chipsNumberInRow >= 5)
            {
                for (int i = -5; i <= 5; i++)
                    for (int k = -5; k <= 5; k++)
                        CsGlobals.winMap[i + 5, k + 5] = CsGlobals.map[x + i, y + k];
                return true;
            }
        }

        return false;
    }

}
