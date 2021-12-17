using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static CsGlobals;

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

        leftBottomTilemapLimit = new Vector3Int(CsGlobals.leftLimit, CsGlobals.bottomLimit, 0);
        rigthUpperTilemapLimit = new Vector3Int(CsGlobals.rightLimit, CsGlobals.upperLimit, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 clickWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int clickCellPosition = map.WorldToCell(clickWorldPosition);

            //Debug.Log(clickCellPosition);
            
            Vector3 leftBottomCameraLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));   //Получаем вектор нижнего левого угла камеры
            Vector3 rigthUpperCameraLimit = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));   //Получаем верхний правый угол камеры

            
            if (((leftBottomTilemapLimit.x  <= clickCellPosition.x) & (clickCellPosition.x < rigthUpperTilemapLimit.x)) & 
                ((leftBottomTilemapLimit.y  <= clickCellPosition.y) & (clickCellPosition.y < rigthUpperTilemapLimit.y)) &
                ((leftBottomCameraLimit.x-1 <= clickCellPosition.x) & (clickCellPosition.x < rigthUpperCameraLimit.x))  & 
                ((leftBottomCameraLimit.y-1 <= clickCellPosition.y) & (clickCellPosition.y < rigthUpperCameraLimit.y)))
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
                    if (isWin(clickCellPosition.x - leftBottomTilemapLimit.x, 
                        clickCellPosition.y - leftBottomTilemapLimit.y, CsGlobals.gamerNumber))
                        Debug.Log(CsGlobals.gamerNumber);
                    
                    CsGlobals.gamerNumber++;
                    if (CsGlobals.gamerNumber > 3) CsGlobals.gamerNumber = 1;
                    //if (CsGlobals.gamerNumber < 1) CsGlobals.gamerNumber = 3;
                }
            }
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
                return true;
        }

        return false;
    }
    
}
