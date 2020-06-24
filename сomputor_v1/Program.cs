using System;
using System.Text.RegularExpressions;

namespace сomputor_v1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Example: сomputor_v1.exe \"5 * X^0 + 4 * X^1 - 9.3 * X^2 = 1 * X^0\"");
                return;
            }

            foreach (var arg in args)
            {
                try
                {
                    new Polynomial(arg);
                }
                catch (System.Exception)
                {
                }
            }

        }
    }
}