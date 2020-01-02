using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Console_MatrixPathFinder
{
	/*
      * https://www.coderbyte.com/editor/Correct%20Path:Csharp
      * 
         Have the function CorrectPath(str) read the str parameter being passed, 
             which will represent the movements made in a 5x5 grid of cells starting from the top left position. 
         The characters in the input string will be entirely composed of: r, l, u, d, ?. 
         Each of the characters stand for the direction to take within the grid, 
             for example: r = right, l = left, u = up, d = down. 
         Your goal is to determine what characters the question marks should be in order for a path to be created 
             to go from the top left of the grid all the way to the bottom right 
             without touching previously travelled on cells in the grid. 

         For example: if str is "r?d?drdd" then your program should output the final correct string that 
             will allow a path to be formed from the top left of a 5x5 grid to the bottom right. 
         For this input, your program should therefore return the string rrdrdrdd. 

         There will only ever be one correct path and there will always be at least one question mark within the input string.

         Input: "???rrurdr?" 
         Output: dddrrurdrd

         Input: "drdr??rrddd?" 
         Output: drdruurrdddd

        1. For input 'r?d?drdd' the output was incorrect. The correct output is rrdrdrdd

        2. For input 'rd?u??dld?ddrr' the output was incorrect. The correct output is rdrurrdldlddrr

        3. For input 'ddr?rdrrd?dr' the output was incorrect. The correct output is ddrurdrrdldr

        4. For input 'rdrdr??rddd?dr' the output was incorrect. The correct output is rdrdruurdddldr


  * 		 */

	class Program
	{
		static void Main(string[] args)
		{
			var sw = new Stopwatch();
			sw.Start();

			// System.Diagnostics.Debug.Assert(CorrectPath("???rrurdr?") == "dddrrurdrd", @"For input '???rrurdr?' the output was incorrect. The correct output is dddrrurdrd");
			// System.Diagnostics.Debug.Assert(CorrectPath("drdr??rrddd?") == "drdruurrdddd", @"For input 'drdr??rrddd?' the output was incorrect. The correct output is drdruurrdddd");
			// System.Diagnostics.Debug.Assert(CorrectPath("r?d?drdd") == "rrdrdrdd", @"For input 'r?d?drdd' the output was incorrect. The correct output is rrdrdrdd");
			// System.Diagnostics.Debug.Assert(CorrectPath("rd?u??dld?ddrr") == "rdrurrdldlddrr", @"For input 'rd?u??dld?ddrr' the output was incorrect. The correct output is rdrurrdldlddrr");
			// System.Diagnostics.Debug.Assert(CorrectPath("ddr?rdrrd?dr") == "ddrurdrrdldr", @"For input 'ddr?rdrrd?dr' the output was incorrect. The correct output is ddrurdrrdldr");
			// System.Diagnostics.Debug.Assert(CorrectPath("rdrdr??rddd?dr") == "rdrdruurdddldr", @"For input 'rdrdr??rddd?dr' the output was incorrect. The correct output is rdrdruurdddldr");
			// 
			// Console.WriteLine("All Passed...");

			//var retType = ValidatePath("drrrruuldlddrddr");

			//var retType = ValidatePath("rrrrddlulddldrrurd");

			// CorrectPath("???rrurdr?");
			//CorrectPath("r?d?drdd");
			//CorrectPath("drdr??rrddd?");

			Console.WriteLine(CorrectPath("r???dd??l??ld??u?d"));

			// keep this function call here
			//Console.WriteLine(CorrectPath(Console.ReadLine()));

			Console.WriteLine($"Elapsed time (ms): {sw.ElapsedMilliseconds}");

			Console.ReadLine();
		}

		/// <summary>
		/// Find the correct path
		/// </summary>
		public static string CorrectPath(string str)
		{
			List<string> allPossiblePaths = GetAllPossiblePaths(str).Distinct().ToList();

			Console.WriteLine("------All Possible Paths:------");
			Console.WriteLine("-------------------------------");
			allPossiblePaths.ToList().ForEach(x => Console.WriteLine($"{x}: {x.Length}"));
			Console.WriteLine("-------------------------------\n\n");

			var allValidPaths = new List<string>();
			foreach (var itmPath in allPossiblePaths)
			{
				if (ValidatePath(itmPath))
				{
					allValidPaths.Add(itmPath);
				}
			}

			Console.WriteLine("------All Correct Paths:------");
			Console.WriteLine("-------------------------------");
			allValidPaths.ToList().ForEach(x => Console.WriteLine($"{x}: {x.Length}"));
			Console.WriteLine("-------------------------------\n\n");

			// code goes here  
			if (allValidPaths.Count > 0) { return allValidPaths[0]; }
			else return str;

		}

		/// <summary>
		/// Gets all possible paths
		/// </summary>
		public static IEnumerable<string> GetAllPossiblePaths(string str)
		{
			// If first move is unknown then possiblities are right or down
			if (str[0] == '?')
			{
				var allPossibleMoves = GetVariationsWithReplace(str, new List<char>() { 'd', 'r' }, 0);

				// Loop through all possible moves and re-evaluate function
				foreach (var item in allPossibleMoves)
				{
					foreach (var itmRet in GetAllPossiblePaths(item))
					{
						yield return itmRet;
					}
				}
			}
			// If last move is unknown then possibilites are right or down
			else if (str[str.Length - 1] == '?')
			{
				var allPossibleMoves = GetVariationsWithReplace(str, new List<char>() { 'd', 'r' }, (str.Length - 1));

				// Loop through all possible moves and re-evaluate function
				foreach (var item in allPossibleMoves)
				{
					foreach (var itmRet in GetAllPossiblePaths(item))
					{
						yield return itmRet;
					}
				}
			}
			else
			{
				// Loop from second elem to the second last element
				for (int i = 1; i < str.Length - 1; i++)
				{
					// If current path is unknown then guess the combinations
					if (str[i] == '?')
					{
						var prevDir = str[i - 1];
						var nextDir = str[i + 1];
						var isNextDirKnown = nextDir != '?';

						List<char> possibleMoves = new List<char>();

						if (prevDir == 'd')
						{
							possibleMoves.AddRange(new char[] { 'd', 'l', 'r' });
						}
						else if (prevDir == 'u')
						{
							possibleMoves.AddRange(new char[] { 'u', 'l', 'r' });
						}
						else if (prevDir == 'r')
						{
							possibleMoves.AddRange(new char[] { 'u', 'd', 'r' });
						}
						else if (prevDir == 'l')
						{
							possibleMoves.AddRange(new char[] { 'u', 'd', 'l' });
						}

						// Next direction can not be opposite of current direction.
						// So if next direction is known, have this condition applied.
						if (isNextDirKnown)
						{
							if (nextDir == 'l')
							{
								possibleMoves.Remove('r');
							}
							else if (nextDir == 'r')
							{
								possibleMoves.Remove('l');
							}
							else if (nextDir == 'u')
							{
								possibleMoves.Remove('d');
							}
							else if (nextDir == 'd')
							{
								possibleMoves.Remove('u');
							}
						}


						var allPossibleMoves = GetVariationsWithReplace(str, possibleMoves, (i));

						// Loop through all possible moves and re-evaluate function
						foreach (var item in allPossibleMoves)
						{
							foreach (var itmRet in GetAllPossiblePaths(item))
							{
								yield return itmRet;
							}
						}
					}
				}
			}

			// if all caught-up return
			if (!str.ToList().Contains('?'))
			{
				yield return str;
			}
		}

		/// <summary>
		/// Get all variatins
		/// </summary>
		public static IEnumerable<string> GetVariationsWithReplace(string inpStr, List<char> replacements, int pos)
		{
			if (replacements != null && replacements.Count > 0)
			{
				foreach (var itmChar in replacements)
				{
					StringBuilder sb = new StringBuilder(inpStr);
					sb[pos] = itmChar;

					var subStringToPartiallyValidate = sb.ToString().Substring(0, pos + 1);
					//if (subStringToPartiallyValidate.Contains('?'))
					//{
					//	subStringToPartiallyValidate = subStringToPartiallyValidate.Substring(0, subStringToPartiallyValidate.IndexOf('?'));
					//}


					// Partially validate input, skipping for first and last position
					if (pos == 0 || pos == (inpStr.Length - 1))
					{
						yield return sb.ToString();
					}
					else if (ValidateBoundaries(subStringToPartiallyValidate) && ValidateSamePathNotTraversed(subStringToPartiallyValidate))
					{
						yield return sb.ToString();
					}
				}
			}
		}

		/// <summary>
		/// Validate given path
		/// </summary>
		public static bool ValidatePath(string str)
		{
			// Basic check no of rights and downs, rights and downs 
			var total_r = str.Count(x => x == 'r');
			var total_l = str.Count(x => x == 'l');
			var total_d = str.Count(x => x == 'd');
			var total_u = str.Count(x => x == 'u');

			if (total_r - total_l != 4) { return false; }
			if (total_d - total_u != 4) { return false; }

			// Check it doesn't goes out of matrix
			if (!ValidateBoundaries(str)) { return false; }

			// Check whether path doesn't gets visited multiple times
			if (!ValidateSamePathNotTraversed(str)) { return false; }

			return true;
		}

		/// <summary>
		/// Validate boundaries
		/// </summary>
		public static bool ValidateBoundaries(string str)
		{
			// Check it doesn't goes out of matrix
			int effective_downTraversed = 0, max_downTraversed = 0, min_downTraversed = 0, effective_rightTraversed = 0, max_rightTraversed = 0, min_rightTraversed = 0;
			foreach (var itmPath in str)
			{
				if (itmPath == 'd') { effective_downTraversed++; if (max_downTraversed < effective_downTraversed) { max_downTraversed = effective_downTraversed; } }
				if (itmPath == 'u') { effective_downTraversed--; if (min_downTraversed > effective_downTraversed) { min_downTraversed = effective_downTraversed; } }
				if (itmPath == 'r') { effective_rightTraversed++; if (max_rightTraversed < effective_rightTraversed) { max_rightTraversed = effective_rightTraversed; } }
				if (itmPath == 'l') { effective_rightTraversed--; if (min_rightTraversed < effective_rightTraversed) { min_rightTraversed = effective_rightTraversed; } }
			}

			if (max_downTraversed > 4 || max_rightTraversed > 4 || min_downTraversed < 0 || min_rightTraversed < 0) { return false; }

			return true;
		}

		/// <summary>
		/// Validate that same path not traversed
		/// </summary>
		public static bool ValidateSamePathNotTraversed(string str)
		{
			List<string> lstTraversed = new List<string>();
			int x1 = 0, y1 = 0, x2 = 0, y2 = 0;

			for (int i = 0; i < str.Length; i++)
			{
				// have current path as prev path
				x1 = x2;
				y1 = y2;

				switch (str[i])
				{
					case 'd':
						y2++;
						break;

					case 'u':
						y2--;
						break;

					case 'r':
						x2++;
						break;

					case 'l':
						x2--;
						break;

					default:
						break;
				}

				// Add path
				var pathTraversed = $"{x1}{y1}{x2}{y2}";
				var pathTraversed_2 = $"{x2}{y2}{x1}{y1}"; // alternative path

				if (lstTraversed.Contains(pathTraversed) || lstTraversed.Contains(pathTraversed_2))
				{
					return false;
				}

				lstTraversed.Add(pathTraversed);
			}

			return true;
		}

	}
}
