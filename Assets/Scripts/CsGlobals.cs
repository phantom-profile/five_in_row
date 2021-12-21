﻿//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

public static class CsGlobals
{
    public static byte gamerNumber = 1;
    
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

}
