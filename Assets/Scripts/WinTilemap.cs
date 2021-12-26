using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WinTilemap : MonoBehaviour
{
    private Tilemap map;
    public TileBase TilesToSet1;
    public TileBase TilesToSet2;
    public TileBase TilesToSet3;
    private Vector3Int leftBottom = new Vector3Int(1, -6, 0);

    
    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<Tilemap>();
        for (int i = 0; i < 11; i++)
        for (int j = 0; j < 11; j++)
            switch (CsGlobals.winMap[i, j])
            {
                case 1:
                    map.SetTile(new Vector3Int(i + leftBottom.x, j + leftBottom.y, 0), TilesToSet1);
                    break;
                case 2:
                    map.SetTile(new Vector3Int(i + leftBottom.x, j + leftBottom.y, 0), TilesToSet2);
                    break;
                case 3:
                    map.SetTile(new Vector3Int(i + leftBottom.x, j + leftBottom.y, 0), TilesToSet3);
                    break;
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
