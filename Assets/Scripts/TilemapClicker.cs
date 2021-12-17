using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static CsGlobals;

public class TilemapClicker : MonoBehaviour
{
    public int GameNumber = 0;
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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int clickCellPosition = map.WorldToCell(clickWorldPosition);

            Debug.Log(clickCellPosition);
            
            map.SetTile(clickCellPosition, TilesToSet1);
        }
    }
}
