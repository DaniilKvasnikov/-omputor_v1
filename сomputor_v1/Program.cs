using System;

namespace сomputorV1
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

            try
            {
                new Polynomial.Polynomial(args[0]);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}