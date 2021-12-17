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
    
    private int left_limit;
    private int right_limit;
    private int bottom_limit;
    private int upper_limit;
    
    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<Tilemap>();
        mainCamera = Camera.main;

        left_limit   = CsGlobals.left_limit;
        right_limit  = CsGlobals.right_limit;
        bottom_limit = CsGlobals.bottom_limit;
        upper_limit  = CsGlobals.upper_limit;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 clickWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int clickCellPosition = map.WorldToCell(clickWorldPosition);

            Debug.Log(clickCellPosition);
            
            Vector3 leftBottomCameraLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));   //Получаем вектор нижнего левого угла камеры
            Vector3 rigthUpperCameraLimit = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));   //Получаем верхний правый угол камеры

            
            if ((((float) left_limit <= clickCellPosition.x) & (clickCellPosition.x < (float) right_limit)) & 
                (((float) bottom_limit <= clickCellPosition.y) & (clickCellPosition.y < (float) upper_limit)) &
                ((leftBottomCameraLimit.x-1 <= clickCellPosition.x) & (clickCellPosition.x < rigthUpperCameraLimit.x)) & 
                ((leftBottomCameraLimit.y-1 <= clickCellPosition.y) & (clickCellPosition.y < rigthUpperCameraLimit.y)))
            {
                switch (CsGlobals.gamerNumber)
                {
                    case 0:
                        map.SetTile(clickCellPosition, TilesToSet1);
                        CsGlobals.gamerNumber++;
                        break;
                    case 1:
                        map.SetTile(clickCellPosition, TilesToSet2);
                        CsGlobals.gamerNumber++;
                        break;
                    case 2:
                        map.SetTile(clickCellPosition, TilesToSet3);
                        CsGlobals.gamerNumber++;
                        break;
                }
                if (CsGlobals.gamerNumber > 2) CsGlobals.gamerNumber = 0;
                if (CsGlobals.gamerNumber < 0) CsGlobals.gamerNumber = 2;
            }
        }
    }
}
