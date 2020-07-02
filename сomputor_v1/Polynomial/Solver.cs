using System;
using System.Linq;
using сomputor_v1.Exception;

namespace сomputor_v1.Polynomial
{
    public class Solver
    {
        private PolynomialBlock[] polynomialBlocks;

        public Solver(PolynomialBlock[] polynomialBlocks)
        {
            this.polynomialBlocks = polynomialBlocks;
        }

        public double[] CheckAnswer(double[] doubles)
        {
            var results = new double[doubles.Length];
            for (var i = 0; i < doubles.Length; i++)
            {
                var d = doubles[i];
                results[i] = 0f;
                foreach (var block in polynomialBlocks)
                    results[i] += block.GetConstant() * Math.Pow(d, block.GetExponent());
            }

            return results;
        }

        public double[] GetAnswer(int degree)
        {
            switch (degree)
            {
                case 2:
                    return SolveQuadratic();
                case 1:
                    return SolveLinear();
                case 0:
                    return ZerroExponent();
            }

            throw new ExceptionDegreeLimit("The polynomial degree is strictly greater than 2, I can't solve.");
        }

        private double[] SolveLinear()
        {
            var b = GetConstant(1);
            var c = GetConstant(0);
            return new[] {-c / b};
        }

        private double[] SolveQuadratic()
        {
            var a = GetConstant(2);
            var b = GetConstant(1);
            var c = GetConstant(0);
            var discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
                var exception = string.Format($"discriminant({discriminant})<0." +
                                              $"A negative discriminant indicates that neither of the solutions are real numbers.)\n" +
                                              $"And answer is (-{b} + √{discriminant}) / {2 * a} or (-{b} - √{discriminant}) / {2 * a}");
                throw new ExceptionNegativeDiscriminant(exception);
            }

            Console.WriteLine($"Discriminant {discriminant}");
            if (discriminant == 0)
                return new[] {-b / (2 * a)};
            return new[] {(-b + Math.Sqrt(discriminant)) / (2 * a), (-b - Math.Sqrt(discriminant)) / (2 * a)};
        }

        private double[] ZerroExponent()
        {
            var c = GetConstant(0);
            if (c == 0)
                throw new ExceptionEachRealNumber("Each real number is a solution.");
            throw new ExceptionNoSolutions("There are no solutions.");
        }

        private double GetConstant(int i)
        {
            return polynomialBlocks.FirstOrDefault(block => block.GetExponent() == i)?.GetConstant() ?? 0;
        }
    }
}