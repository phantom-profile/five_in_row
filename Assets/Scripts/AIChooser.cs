using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public enum Marker
{
    Empty = 0,
    First = 1,
    Second = 2,
    Third = 3
}

public class AIChooser
{
    private Dictionary<byte, Marker> _playersOrder;

	/* 			"xxxxx", 					"xxx0xx", 					"xxxx0x", 
				"0xxxx0", 					"0xxxx#", 					"xxx0x", 
				"xx0xx", 					"0x0xx0", 					"00xxx0", 
				"00xxx#", 					"00xx0", 					"0x0x0"*/

    /*private Dictionary<(Marker, int), int> _costs = new Dictionary<(Marker, int), int>()
    {
        {(Marker.First, 1), 99999},		{(Marker.First, 2), 3000}, 	{(Marker.First, 3), 3000},
        {(Marker.First, 4), 7000}, 		{(Marker.First, 5), 4000}, 	{(Marker.First, 6), 3000},
        {(Marker.First, 7), 3000}, 		{(Marker.First, 8), 800}, 	{(Marker.First, 9), 3000},
        {(Marker.First, 10), 1500}, 	{(Marker.First, 11), 200}, 	{(Marker.First, 12), 150},

        {(Marker.Second, 1), 79999}, 	{(Marker.Second, 2), 2400}, {(Marker.Second, 3), 2400},
        {(Marker.Second, 4), 5600}, 	{(Marker.Second, 5), 3200}, {(Marker.Second, 6), 2400},
        {(Marker.Second, 7), 2400}, 	{(Marker.Second, 8), 640}, 	{(Marker.Second, 9), 2400},
        {(Marker.Second, 10), 1200}, 	{(Marker.Second, 11), 160}, {(Marker.Second, 12), 120},

        {(Marker.Third, 1), 63999}, 	{(Marker.Third, 2), 1920}, 	{(Marker.Third, 3), 1920},
        {(Marker.Third, 4), 4480}, 		{(Marker.Third, 5), 2560}, 	{(Marker.Third, 6), 1920},
        {(Marker.Third, 7), 1920}, 		{(Marker.Third, 8), 512}, 	{(Marker.Third, 9), 1920},
        {(Marker.Third, 10), 960}, 		{(Marker.Third, 11), 128}, 	{(Marker.Third, 12), 96},
    };*/

	private Dictionary<(byte, int), int> _costs = new Dictionary<(byte, int), int>()
    {
        {(1, 1), 99999},	{(1, 2), 3000}, 	{(1, 3), 3000},
        {(1, 4), 7000}, 	{(1, 5), 4000}, 	{(1, 6), 3000},
        {(1, 7), 3000}, 	{(1, 8), 800}, 		{(1, 9), 3000},
        {(1, 10), 1500}, 	{(1, 11), 200}, 	{(1, 12), 150},

        {(2, 1), 79999}, 	{(2, 2), 2400}, 	{(2, 3), 2400},
        {(2, 4), 5600}, 	{(2, 5), 3200}, 	{(2, 6), 2400},
        {(2, 7), 2400}, 	{(2, 8), 640}, 		{(2, 9), 2400},
        {(2, 10), 1200}, 	{(2, 11), 160}, 	{(2, 12), 120},

        {(3, 1), 63999}, 	{(3, 2), 1920}, 	{(3, 3), 1920},
        {(3, 4), 4480}, 	{(3, 5), 2560}, 	{(3, 6), 1920},
        {(3, 7), 1920}, 	{(3, 8), 512}, 		{(3, 9), 1920},
        {(3, 10), 960}, 	{(3, 11), 128}, 	{(3, 12), 96},
    };


    // Legend
    // x – our moves, 0 – free space, # – other players
    private readonly string[] _patterns =
    {
        "xxxxx", "xxx0xx", "xxxx0x", "0xxxx0", "0xxxx#", "xxx0x", "xx0xx",
        "0x0xx0", "00xxx0", "00xxx#", "00xx0", "0x0x0"
    };

    public AIChooser()
    {
    }
    
		private bool CheckNeighbours(int x, int y)
		{
			//Debug.Log("CheckNeighbours 0");
			
			for (int deltaY = -2; deltaY <= 2; deltaY++)
			{
				int yDeltaY = y + deltaY;

				if (yDeltaY < 0 || yDeltaY >= CsGlobals.GetYSize())
				{
					continue;
				}

				for (int deltaX = -2; deltaX <= 2; deltaX++)
				{
					int xDeltaX = x + deltaX;

					if (xDeltaX < 0 || xDeltaX >= CsGlobals.GetXSize())
					{
						continue;
					}

					if (CsGlobals.map[xDeltaX, yDeltaY] == (byte) Marker.Empty)
					{
						continue;
					}

					return true;
				}
			}

			return false;
		}

		private void setNewCost(int x, int y)
		{
			//int newCost = 0;

			if (!CheckNeighbours(x, y))
			{
				return;
			}
			//Debug.Log("setNewCost 0");

			// Reset cost for passed x and y
			foreach (KeyValuePair<byte, int[,]> kvp in CsGlobals.Costs)
			{
				var costs = kvp.Value;
				costs[x, y] = 0;
			}

			(int x, int y)[] deltas = {(-1, 1), (0, 1), (1, 1), (1, 0)};
			//Debug.Log("setNewCost 1");

			foreach (var dlt in deltas)
			{
				var s1 = "";
				for (int i = -5; i <= 5; i++)
				{
					s1 += $"{CsGlobals.map[x + dlt.x * i, y + dlt.y * i]}";
				}
				//Debug.Log("setNewCost 2");

				for (int i = 1; i <= 3; i++)
				{
					var s = s1.Substring(0, 5) + i.ToString() + s1.Substring(6, 5);

					for (int j = 1; j <= 3; j++)
					{
						s = s.Replace(j.ToString(), i == j ? "x" : "#");
					}
					//Debug.Log("setNewCost 3");
				
					// FINISH IT!

					int[] maxCost = new int[] {0, 0, 0};
					//int maxCost2 = 0;
					//int maxCost3 = 0;
					var sReverse = s.Reverse().ToString();

					for (int k = 0; k < _patterns.Length; k++)
					{
						int found = s.IndexOf(_patterns[k]);
						if (found != -1)
						{
							maxCost[0] = (maxCost[0] > getMaxCost((byte) 1, (byte) i, k)) ? maxCost[0] : getMaxCost((byte) 1, (byte) i, k);
							maxCost[1] = (maxCost[1] > getMaxCost((byte) 2, (byte) i, k)) ? maxCost[1] : getMaxCost((byte) 2, (byte) i, k);
							maxCost[2] = (maxCost[2] > getMaxCost((byte) 3, (byte) i, k)) ? maxCost[2] : getMaxCost((byte) 3, (byte) i, k);
						}
						//Debug.Log("setNewCost 4");

						found = sReverse.IndexOf(_patterns[k]);
						if (found != -1)
						{
							maxCost[0] = (maxCost[0] > getMaxCost((byte) 1, (byte) i, k)) ? maxCost[0] : getMaxCost((byte) 1, (byte) i, k);
							maxCost[1] = (maxCost[1] > getMaxCost((byte) 2, (byte) i, k)) ? maxCost[1] : getMaxCost((byte) 2, (byte) i, k);
							maxCost[2] = (maxCost[2] > getMaxCost((byte) 3, (byte) i, k)) ? maxCost[2] : getMaxCost((byte) 3, (byte) i, k);
						}

						foreach (KeyValuePair<byte, int[,]> kvp in CsGlobals.Costs)
						{
							var costs = kvp.Value;
							var playerNum = kvp.Key;
							costs[x, y] += maxCost[playerNum - 1];// + (System.DateTime.Now.Millisecond) % 30;
						}
					}

					//CsGlobals.Costs[(byte) i][x, y] += maxCost;
				}
			}
		}

		private int getMaxCost(byte playerNumber, byte patternPlayerNumber, int pattern)
		{
			//Debug.Log("getMaxCost 0");
			byte deltaNumber = (byte) (patternPlayerNumber - playerNumber);
			while (deltaNumber < 0 || 2 < deltaNumber)
			{
				if (deltaNumber < 0)
					deltaNumber += 3;
				if (deltaNumber > 2)
					deltaNumber -= 3;
			}
			//Debug.Log("getMaxCost 1");
			
			switch (deltaNumber)
			{
				case 0:
					return _costs[(playerNumber, pattern + 1)];
					break;
				case 1:
					switch (playerNumber)
					{
						case 1:
							return (int) (_costs[(playerNumber, pattern + 1)] * 0.8);
							break;
						case 2:
							return (int) (_costs[(playerNumber, pattern + 1)] * 0.8);
							break;
						case 3:
							return (int) (_costs[(playerNumber, pattern + 1)] * 0.8);
							break;
					}
					break;
				case 2:
					switch (playerNumber)
					{
						case 1:
							return (int) (_costs[(playerNumber, pattern + 1)] * 0.5);
							break;
						case 2:
							return (int) (_costs[(playerNumber, pattern + 1)] * 0.5);
							break;
						case 3:
							return (int) (_costs[(playerNumber, pattern + 1)] * 0.5);
							break;
					}
					break;
			}

			return -1;
		}


		// Param: new move's x and y coords
		public void UpdateCosts(int x, int y)
		{
			foreach (KeyValuePair<byte, int[,]> kvp in CsGlobals.Costs)
			{
				//Debug.Log("UpdateCosts 3");

				var costs = kvp.Value;
				costs[x, y] = -1;
			}
			
			// foreach (var (playerNum, costs) in CsGlobals.Costs)
			// {
			//Debug.Log("UpdateCosts 0");
			for (int deltaX = -5; deltaX <= 5; deltaX++)
			{
				//Debug.Log("UpdateCosts 1");

				int xDeltaX = x + deltaX;
				for (int deltaY = -5; deltaY <= 5; deltaY++)
				{	

					int yDeltaY = y + deltaY;
					if (CsGlobals.map[xDeltaX, yDeltaY] != 0)
					{
						continue;
					}
					
					//Debug.Log("UpdateCosts 2");

					setNewCost(xDeltaX, yDeltaY);
				}
			}
			// }
		}

	public List<(int x, int y)> GetPossibleMoves()
    {
		List<(int x, int y)> winMoves = new List<(int x, int y)>();

		var cost = CsGlobals.Costs[CsGlobals.gamerNumber];
		int winCosts = 0;
		
		for (var i = 0; i < CsGlobals.GetXSize(); i++)
			for (var j = 0; j < CsGlobals.GetYSize(); j++)
			{
				if (cost[i, j] == winCosts)
				{
					winMoves.Add((i, j));
				}
				else if (cost[i, j] > winCosts)
				{
					winCosts = cost[i, j];
					winMoves.Clear();
					winMoves.Add((i, j));
				}
			}
		
        return winMoves;
    }
}
