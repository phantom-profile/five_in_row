using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    
	public Sprite PlayerSprite;
	public Sprite AISprite;
	public Button Button1;
	public Button Button2;
	public Button Button3;

	// Start is called before the first frame update
    public void Play(int index)
    {
        SceneManager.LoadScene(index);
        for (int y = 0; y < CsGlobals.GetYSize(); y++)
            for (int x = 0; x < CsGlobals.GetXSize(); x++)
				CsGlobals.map[x, y] = 0;      
		
		CsGlobals.gamerNumber = 1;
		//CsGlobals.RealPlayers = new bool[] {true, true, true};         
		CsGlobals.FirstTile = true;
		
		
    }
    
    public void Update()
	{
		if (CsGlobals.RealPlayers[0])
        			Button1.GetComponent<Image>().sprite = PlayerSprite;
        		else
        			Button1.GetComponent<Image>().sprite = AISprite;
        
        		if (CsGlobals.RealPlayers[1])
        			Button2.GetComponent<Image>().sprite = PlayerSprite;
        		else
        			Button2.GetComponent<Image>().sprite = AISprite;
        
        		if (CsGlobals.RealPlayers[2])
        			Button3.GetComponent<Image>().sprite = PlayerSprite;
        		else
        			Button3.GetComponent<Image>().sprite = AISprite;
	}

    // Update is called once per frame
    public void Exit()
    {
        Application.Quit();
    }

	public void ChangePlayer1()
    {
        CsGlobals.RealPlayers[0] = !CsGlobals.RealPlayers[0];

        if (CsGlobals.RealPlayers[0])
			Button1.GetComponent<Image>().sprite = PlayerSprite;
		else
			Button1.GetComponent<Image>().sprite = AISprite;
        //Debug.Log(CsGlobals.RealPlayers[0].ToString());

    }

	public void ChangePlayer2()
    {
        CsGlobals.RealPlayers[1] = !CsGlobals.RealPlayers[1];

        if (CsGlobals.RealPlayers[1])
			Button2.GetComponent<Image>().sprite = PlayerSprite;
		else
			Button2.GetComponent<Image>().sprite = AISprite;   
        //Debug.Log(CsGlobals.RealPlayers[1].ToString());
          
    }

	public void ChangePlayer3()
    {
    	CsGlobals.RealPlayers[2] = !CsGlobals.RealPlayers[2];

        if (CsGlobals.RealPlayers[2])
			Button3.GetComponent<Image>().sprite = PlayerSprite;
		else
			Button3.GetComponent<Image>().sprite = AISprite;      
        //Debug.Log(CsGlobals.RealPlayers[2].ToString());
        
    }
}
