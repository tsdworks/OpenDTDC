﻿using OpenBveApi.Interface;

namespace CsvRwRouteParser
{
	internal enum Direction
	{
		Left = -1,
		Both = 0,
		Right = 1,
		None = 2,
		Invalid = int.MaxValue
	}

	internal partial class Parser
	{
		internal static Direction FindDirection(string Direction, string Command, bool IsWallDike, int Line, string File)
		{
			Direction = Direction.Trim();
			switch (Direction.ToUpperInvariant())
			{
				case "-1":
				case "L":
				case "LEFT":
					return CsvRwRouteParser.Direction.Left;
				case "B":
				case "BOTH":
					return CsvRwRouteParser.Direction.Both;
				case "+1":
				case "1":
				case "R":
				case "RIGHT":
					return CsvRwRouteParser.Direction.Right;
				case "0":
					// BVE is inconsistant: Walls / Dikes use 0 for *both* sides, stations use 0 for none....
					return IsWallDike ? CsvRwRouteParser.Direction.Both : CsvRwRouteParser.Direction.None;
				case "N":
				case "NONE":
				case "NEITHER":
					return CsvRwRouteParser.Direction.None;
				default:
					Plugin.CurrentHost.AddMessage(MessageType.Error, false, "Direction is invalid in " + Command + " at line " + Line + " in file " + File);
					return CsvRwRouteParser.Direction.Invalid;

			}
		}
	}
}
