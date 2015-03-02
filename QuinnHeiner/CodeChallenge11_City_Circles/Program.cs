using System;
using System.Collections.Generic;
using System.Linq;

/*
Challenge #11

Problem Statement
 
Roy lives in a city that is circular in shape on a 2D plane. The city center is located at origin (0,0) and it has suburbs lying on the lattice 
 points (points with integer coordinates). The city Police Department Headquarters can only protect those suburbs which are located strictly 
 inside the city. The suburbs located on the border of the city are still unprotected. So the police department decides to build at most k 
 additional police stations at some suburbs. Each of these police stations can protect the suburb it is located in.
 
Given the radius of the city, Roy has to determine if it is possible to protect all the suburbs.
 
Input Format 
The first line of input contains integer t, t test cases follow. 
Each of next t lines contains two space separated integers: r, the square of the radius of the city and k, the maximum number of police stations 
 the headquarters is willing to build.
 
Constraints 
1≤t≤103 
1≤r≤2×109 
0≤k≤2×109
 
Output Format 
For each test case, print in a new line possible if it is possible to protect all the suburbs, otherwise print impossible.
 
Sample Input
 
5
1 3
1 4
4 4
25 11
25 12
 
Sample Output
 
impossible
possible
possible
impossible
possible
 */
namespace CodeChallenge11_City_Circles
{
	public class Program
	{
		static void Main(string[] args)
		{
			string input;
			const int maxTestCaseCount = 103;
			do
			{
				Console.WriteLine("\n\n\nEnter number of test cases (max {0:n0}) you wish to perform for the city radius check (enter q at any time to quit): ", maxTestCaseCount);
				input = Console.ReadLine();
				var cityCircles = BuildCityCirclesFromUserInput(input).ToList();

				if (cityCircles.Any())
				{
					Console.WriteLine("\n:::::::::::::::::::::::Results::::::::::::::::::::::::::");
					Console.WriteLine("Radius Squared \tMax Police Stations \tProtection Check");
					foreach (var city in cityCircles)
					{
						var cityProtectionCheck = CityProtectionCheck(city) ? "possible" : "impossible";
						Console.WriteLine("{0} \t\t\t{1}" + "\t\t\t {2}", Square(city.Radius), city.PoliceStationCount, cityProtectionCheck/*,GetLatticePointCount(city.Radius)*/);
					}
					Console.WriteLine("\n::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
				}
			} while (input != "q");
		}

		/// <summary>
		/// Returns boolean indicating if the number of given police stations >= number of suburbs on lattice points (based on city radius)
		/// </summary>
		/// <param name="city"></param>
		/// <returns></returns>
		private static bool CityProtectionCheck(CityCircle city)
		{
			var latticePointCount = GetLatticePointCount(city.Radius);
			return city.PoliceStationCount >= latticePointCount;
		}

		/// <summary>
		/// The number of lattice points on the circumference of a circle centered at (0, 0) with radius R is N(R)=rk(R^2), where rk(n) is the sum of squares function. 
		/// <para>The sum of squares function in this context is the number of representations of n by k perfect squares, allowing zeros and distinguishing signs and order.</para>
		/// <para>For example, if n = 5 and k = 2 (k is always 2 in this context), then the sum of squares would be the number of ways to represent 5 as the sum of two squares, or 8, i.e.</para>
		///
		///<para>(1)=	(-2)^2+(-1)^2</para>
		///<para>(2)	=	(-2)^2+1^2	</para>
		///<para>(3)	=	2^2+(-1)^2	</para>
		///<para>(4)	=	2^2+1^2	</para>
		///<para>(5)	=	(-1)^2+(-2)^2	</para>
		///<para>(6)	=	(-1)^2+2^2	</para>
		///<para>(7)	=	1^2+(-2)^2	</para>
		///<para>(8)	=	1^2+2^2</para>
		/// </summary>
		/// <param name="radius"></param>
		/// <returns></returns>
		private static int GetLatticePointCount(int radius)
		{
			var perfectSquares = BuildPerfectSquareArray(radius);
			var radiusSquared = Square(radius);
			var latticePointCount = 4; // there are always 4 lattice points at the quadrant boundaries
			foreach (var square in perfectSquares)
			{
				var currentNum = square;
				foreach (var currentCompareToNum in perfectSquares)
				{
					if (currentNum == currentCompareToNum && currentNum + currentCompareToNum == radiusSquared)
					{
						latticePointCount += 2;
					}
					else if (currentNum + currentCompareToNum == radiusSquared)
					{
						latticePointCount += 4;
					}
				}
			}
			return latticePointCount;
		}

		private static int Square(int num)
		{
			return num * num;
		}

		private static List<int> BuildPerfectSquareArray(int num)
		{
			var list = new List<int>();
			for (var i = 1; i <= num; i++)
			{
				list.Add(Square(i));
			}
			return list;
		}

		private static IEnumerable<CityCircle> BuildCityCirclesFromUserInput(string input)
		{
			int testCaseCount;
			const int maxTestCaseCount = 103;
			const int maxRadius = 1982119441; // must be a perfect square less than 2,000,000,000
			const int maxPoliceStationCount = 2000000000;
			var cityCircles = new List<CityCircle>();
			
			if (Int32.TryParse(input, out testCaseCount) && testCaseCount > 0 && testCaseCount <= maxTestCaseCount)
			{
				Console.WriteLine("\nFor each test case below, enter two space-separated integers for the square radius of the city and the number of police stations, respectively\n");
				for (var i = 0; i < testCaseCount; i++)
				{
					int radius;
					int policeStationCount;
					Console.WriteLine("Test Case {0}:", i + 1);
					input = Console.ReadLine();
					if (input == "q")
					{
						break;
					}
					var values = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					if (values.Length == 2
						&& Int32.TryParse(values[0], out radius)
						&& Int32.TryParse(values[1], out policeStationCount)
						&& radius >= 1 && radius <= maxRadius
						&& IsPerfectSquare(radius)
						&& policeStationCount >= 0 && policeStationCount <= maxPoliceStationCount)
					{
						cityCircles.Add(new CityCircle((int)Math.Sqrt(radius), policeStationCount));
					}
					else
					{
						Console.WriteLine("\nInvalid numbers detected. Square radius must be a perfect square between 0 and {0:n0}, and the police station count must be between 0 and {1:n0}", maxRadius, maxPoliceStationCount);
					}
				}
			}
			else
			{
				Console.WriteLine("\nInvalid number of test cases.  Enter a number between 1 and {0:n0}", maxTestCaseCount);
			}

			return cityCircles;
		}

		struct CityCircle
		{
			public readonly int Radius;
			public readonly int PoliceStationCount;

			public CityCircle(int radius, int policeStationCount)
			{
				Radius = radius;
				PoliceStationCount = policeStationCount;
			}
		}

		private static bool IsPerfectSquare(double num)
		{
			var isPerfectSquare = (Math.Sqrt(num)) % 1 == 0;
			return isPerfectSquare;
		}
	}
}
