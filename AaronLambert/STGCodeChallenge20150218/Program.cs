using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace STGCodeChallenge20150218
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = Properties.Settings.Default.InputFileName;

            if (args.Length > 0)
            {
                filename = args[0];
            }

            string[] filedata = System.IO.File.ReadAllLines(filename);

            // The first line of the file tells us how many test cases there will be
            int Count;
            if (!GetInt(filedata[0], out Count))
            {
                ExitApp();
            }

            for (int i = 0; i < Count; i++)
            {
                string line = filedata[i + 1];
                BigInteger r; // The Square of the radius of the city
                BigInteger k; // Maximum number of police stations

                string[] n = line.Split(' ');
                if (!GetBigInt(n[0], out r)) ExitApp();
                if (!GetBigInt(n[1], out k)) ExitApp();

                // Now we need to find how many points (suburbs) are on the edge of the circle
                // We can count up the number in one quadrant of the circle and multiply by 4
                BigInteger Max = FindSqRt(r);
                BigInteger suburbs = 0;
                for (BigInteger x = 0; x < Max; x++)
                {
                    // a^2 + b^2 = c^2
                    // b^2 = c^2 - a^2
                    BigInteger b2 = r - (x * x);
                    BigInteger y = FindSqRt(b2);
                    if (y * y == b2)
                    {
                        // The suburb lies right on the border
                        suburbs = suburbs + 4;
                    }
                }

                // Output the result
                Console.WriteLine(k >= suburbs ? "possible" : "impossible");
            }
            ExitApp();
        }

        static bool GetInt(string s, out int i)
        {
            bool success = int.TryParse(s, out i);
            if (!success)
            {
                Console.WriteLine("Invalid input");
            }
            return success;
        }
        static bool GetBigInt(string s, out BigInteger i)
        {
            bool success = BigInteger.TryParse(s, out i);
            if (!success)
            {
                Console.WriteLine("Invalid input");
            }
            return success;
        }

        static void ExitApp()
        {
            Console.ReadLine();
            Environment.Exit(0);
        }

        static BigInteger FindSqRt(BigInteger i)
        {
            BigInteger r = 1;
            while (r * r < i)
            {
                r++;
            }
            return r;
        }
    }
}
