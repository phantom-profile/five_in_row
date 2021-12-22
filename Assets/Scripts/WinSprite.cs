using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSprite : MonoBehaviour
{
    public Sprite SpriteToSet1;
    public Sprite SpriteToSet2;
    public Sprite SpriteToSet3;
    
    // Start is called before the first frame update
    void Start()
    {
        Sprite SpriteToSet;
        
        switch (CsGlobals.gamerNumber)
        {
            case 1:
                SpriteToSet = SpriteToSet1;
                break;
            case 2:
                SpriteToSet = SpriteToSet2;
                break;
            case 3:
                SpriteToSet = SpriteToSet3;
                break;
            default:
                return;
        }

        this.gameObject.GetComponent<SpriteRenderer>().sprite = SpriteToSet;
    }
    
}
