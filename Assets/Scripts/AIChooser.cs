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

    private Dictionary<(Marker, int), int> _costs = new Dictionary<(Marker, int), int>()
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

    private List<(int x, int y)> GetFutureMoves()
    {
        var fMoves = new List<(int x, int y)>();

        for (int y = 0; y < CsGlobals.GetYSize(); y++)
        {
            for (int x = 0; x < CsGlobals.GetXSize(); x++)
            {
                // If (x,y) already taken then just continue
                if (CsGlobals.map[x, y] != (byte) Marker.Empty)
                {
                    continue;
                }

                bool ok = false;
                for (int deltaY = -1; deltaY < 2; deltaY++)
                {
                    int yDeltaY = y + deltaY;

                    if (yDeltaY < 0 || yDeltaY >= CsGlobals.GetYSize())
                    {
                        continue;
                    }

                    for (int deltaX = -1; deltaX < 2; deltaX++)
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

                        fMoves.Add((x, y));
                        ok = true;

                        break;
                    }

                    if (ok)
                    {
                        break;
                    }
                }
            }
        }

        return fMoves;
    }

    private void SetPlayers(byte playerNum)
    {
        switch (playerNum)
        {
            case 1:
                _playersOrder = new Dictionary<byte, Marker>
                {
                    {1, Marker.First},
                    {2, Marker.Second},
                    {3, Marker.Third},
                };
                break;
            case 2:
                _playersOrder = new Dictionary<byte, Marker>
                {
                    {2, Marker.First},
                    {3, Marker.Second},
                    {1, Marker.Third},
                };
                break;
            case 3:
                _playersOrder = new Dictionary<byte, Marker>
                {
                    {3, Marker.First},
                    {1, Marker.Second},
                    {2, Marker.Third},
                };
                break;
            default:
                throw new Exception("Impossible player num (only 3 supported).");
        }
    }

    private List<(int x, int y)> CalcRating(List<(int x, int y)> moves)
    {
        int winCosts = 0;
        List<(int x, int y)> winMoves = new List<(int x, int y)>();

        foreach (var point in moves)
        {
            (int x, int y)[] deltas = {(-1, 1), (0, 1), (1, 1), (1, 0)};
            int sumCost = 0;

            // First diag /
            foreach (var dlt in deltas)
            {
                var s1 = "";
                for (int i = -5; i <= 5; i++)
                {
                    s1 += $"{CsGlobals.map[point.x + dlt.x * i, point.y + dlt.y * i]}";
                }

				//Debug.Log(s1);

                for (int i = 1; i <= 3; i++)
                {
                    var s = s1.Substring(0, 5) + i.ToString() + s1.Substring(6, 5);

                    for (int j = 1; j <= 3; j++)
                    {
                        s = s.Replace(j.ToString(), i == j ? "x" : "#");
                    }

                    int maxCost = 0;
                    var sReverse = s.Reverse().ToString();

                    for (int k = 0; k < _patterns.Length; k++)
                    {
                        int found = s.IndexOf(_patterns[k]);
                        if (found != -1 &&
                            maxCost < _costs[(_playersOrder[(byte) i], k+1)])
                        {
                            maxCost = _costs[(_playersOrder[(byte) i], k+1)];
                        }

                        found = sReverse.IndexOf(_patterns[k]);
                        if (found != -1 &&
                            maxCost < _costs[(_playersOrder[(byte) i], k+1)])
                        {
                            maxCost = _costs[(_playersOrder[(byte) i], k+1)];
                        }
                    }
					
					//Debug.Log(s);
					//Debug.Log(sReverse);

                    sumCost += maxCost;
                }
            }

            if (sumCost == winCosts)
            {
                winMoves.Add(point);
            }
            else if (sumCost > winCosts)
            {
                winCosts = sumCost;
				winMoves.Clear();
                winMoves.Add(point);
            }

			//Debug.Log(sumCost);

            // s1-4 lines like '01130201122'
            // var s1 = ""; // first diag /
            // var s2 = ""; // second diag \
            // var s3 = ""; // horizontal line ––
            // var s4 = ""; // vertical line |

            // analyse string and set rating

            // First diag /
            // s1 += $"{CsGlobals.map[point.x - 5, point.y - 5]}";
            // s1 += $"{CsGlobals.map[point.x - 4, point.y - 4]}";
            // s1 += $"{CsGlobals.map[point.x - 3, point.y - 3]}";
            // s1 += $"{CsGlobals.map[point.x - 2, point.y - 2]}";
            // s1 += $"{CsGlobals.map[point.x - 1, point.y - 1]}";
            // s1 += $"{CsGlobals.map[point.x, point.y]}";
            // s1 += $"{CsGlobals.map[point.x + 1, point.y + 1]}";
            // s1 += $"{CsGlobals.map[point.x + 2, point.y + 2]}";
            // s1 += $"{CsGlobals.map[point.x + 3, point.y + 3]}";
            // s1 += $"{CsGlobals.map[point.x + 4, point.y + 4]}";
            // s1 += $"{CsGlobals.map[point.x + 5, point.y + 5]}";

            // s1[5] = 1 | 2 | 3

            // // Second diag \
            // s2 += $"{CsGlobals.map[point.x - 5, point.y + 5]}";
            // s2 += $"{CsGlobals.map[point.x - 4, point.y + 4]}";
            // s2 += $"{CsGlobals.map[point.x - 3, point.y + 3]}";
            // s2 += $"{CsGlobals.map[point.x - 2, point.y + 2]}";
            // s2 += $"{CsGlobals.map[point.x - 1, point.y + 1]}";
            // s2 += $"{CsGlobals.map[point.x, point.y]}";
            // s2 += $"{CsGlobals.map[point.x + 1, point.y - 1]}";
            // s2 += $"{CsGlobals.map[point.x + 2, point.y - 2]}";
            // s2 += $"{CsGlobals.map[point.x + 3, point.y - 3]}";
            // s2 += $"{CsGlobals.map[point.x + 4, point.y - 4]}";
            // s2 += $"{CsGlobals.map[point.x + 5, point.y - 5]}";
            //
            // // Horizontal line ––
            // s3 += $"{CsGlobals.map[point.x - 5, point.y]}";
            // s3 += $"{CsGlobals.map[point.x - 4, point.y]}";
            // s3 += $"{CsGlobals.map[point.x - 3, point.y]}";
            // s3 += $"{CsGlobals.map[point.x - 2, point.y]}";
            // s3 += $"{CsGlobals.map[point.x - 1, point.y]}";
            // s3 += $"{CsGlobals.map[point.x, point.y]}";
            // s3 += $"{CsGlobals.map[point.x + 1, point.y]}";
            // s3 += $"{CsGlobals.map[point.x + 2, point.y]}";
            // s3 += $"{CsGlobals.map[point.x + 3, point.y]}";
            // s3 += $"{CsGlobals.map[point.x + 4, point.y]}";
            // s3 += $"{CsGlobals.map[point.x + 5, point.y]}";
            //
            // // Vertical line |
            // s4 += $"{CsGlobals.map[point.x, point.y + 5]}";
            // s4 += $"{CsGlobals.map[point.x, point.y + 4]}";
            // s4 += $"{CsGlobals.map[point.x, point.y + 3]}";
            // s4 += $"{CsGlobals.map[point.x, point.y + 2]}";
            // s4 += $"{CsGlobals.map[point.x, point.y + 1]}";
            // s4 += $"{CsGlobals.map[point.x, point.y]}";
            // s4 += $"{CsGlobals.map[point.x, point.y - 1]}";
            // s4 += $"{CsGlobals.map[point.x, point.y - 2]}";
            // s4 += $"{CsGlobals.map[point.x, point.y - 3]}";
            // s4 += $"{CsGlobals.map[point.x, point.y - 4]}";
            // s4 += $"{CsGlobals.map[point.x, point.y - 5]}";
        }
		Debug.Log(winCosts);		

        return winMoves;
    }

    public List<(int x, int y)> GetPossibleMoves()
    {
        // Set players moving order
        SetPlayers(CsGlobals.gamerNumber);

        // Dict with empty position with at least 1 neighbor
        var fMoves = GetFutureMoves();

        // Calc and set maximum rating for each selected move
        // and return best of them
        var winMoves = CalcRating(fMoves);

        return winMoves;
    }
}
