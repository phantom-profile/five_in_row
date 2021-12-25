using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

public static class CsGlobals
{
    public static byte gamerNumber = 1;

	public static bool FirstTile = true;
    
    public static int leftLimit   = -100;
    public static int rightLimit  =  100;
    public static int bottomLimit = -100;
    public static int upperLimit  =  100;

	public static int GetXSize()
	{
		return rightLimit - leftLimit;
	}

	public static int GetYSize()
	{
		return upperLimit - bottomLimit;
	}

	public static byte[,] map = new byte[GetXSize(), GetYSize()];
	
	public static Dictionary<byte, int[,]> Costs = new Dictionary<byte, int[,]>
    {
    	{1, new int[GetXSize(), GetYSize()]},
    	{2, new int[GetXSize(), GetYSize()]},
    	{3, new int[GetXSize(), GetYSize()]},
    };

	public static bool[] RealPlayers = new bool[] {true, true, true};

}
