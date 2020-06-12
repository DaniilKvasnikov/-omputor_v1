using System;
using System.Text.RegularExpressions;

namespace сomputor_v1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                try
                {
                    Polynomial polynomial = new Polynomial(arg);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            if (args.Length == 0)
                Console.WriteLine("No arg");
        }
    }
}