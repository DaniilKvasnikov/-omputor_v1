using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace сomputor_v1
{
    public class Polynomial
    {
        private string input;
        private PolynomialBlock[] polynomialBlocks;
        private double discriminant;
        private double[] answers;

        private const float TOLERANCE = 0.001f;

        private static string patternBlock = "([-]?([0-9](,[0-9])*(\\.[0-9]+)?)[*]X[\\^][0-9])";

        public Polynomial(string input)
        {
            this.input = StringPreprocess(input);
            polynomialBlocks = GetSubStrings(this.input);
            polynomialBlocks = CutExpression(polynomialBlocks);
            answers = GetAnswer(polynomialBlocks);
            foreach (var subString in polynomialBlocks)
            {
                Console.WriteLine(subString);
            }
            Console.WriteLine("discriminant = " + discriminant);
            foreach (var answer in answers)
            {
                Console.WriteLine("answer = " + answer);
            }

            double[] results = CheckAnswer(answers);
            foreach (var result in results)
            {
                Console.WriteLine("result = " + result);
            }
        }

        public double[] GetAnswers()
        {
            return answers;
        }

        public double[] CheckAnswer(double[] doubles)
        {
            double[] results = new double[doubles.Length];
            for (var i = 0; i < doubles.Length; i++)
            {
                var d = doubles[i];
                results[i] = 0f;
                foreach (var block in polynomialBlocks)
                {
                    results[i] += block.GetConstant() * Math.Pow(d, block.GetExponent());
                }
            }

            return results;
        }

        private double[] GetAnswer(PolynomialBlock[] polynomialBlocks1)
        {
            double a = polynomialBlocks1.FirstOrDefault(block => block.GetExponent() == 2f).GetConstant();
            double b = polynomialBlocks1.FirstOrDefault(block => block.GetExponent() == 1f).GetConstant();
            double c = polynomialBlocks1.FirstOrDefault(block => block.GetExponent() == 0f).GetConstant();
            double discriminant = b * b - 4 * a * c;
            return new []{(-b + Math.Sqrt(discriminant)) / (2 * a), (-b - Math.Sqrt(discriminant)) / (2 * a)};
        }

        private PolynomialBlock[] CutExpression(PolynomialBlock[] polynomialBlocks1)
        {
            List<PolynomialBlock> newpolynomialBlocks = new List<PolynomialBlock>();
            foreach (var block in polynomialBlocks1)
            {
                var to = newpolynomialBlocks.FirstOrDefault(e =>
                    Math.Abs(e.GetExponent() - block.GetExponent()) < TOLERANCE && e != block);
                if (to == null)
                    newpolynomialBlocks.Add(block);
                else
                    to.AddConstant(block.GetConstant());
            }

            return newpolynomialBlocks.OrderBy(e => e.GetSortParam()).ToArray();
        }

        private PolynomialBlock[] GetSubStrings(string s)
        {
            var subStrings = s.Split(new[] {'='});
            if (subStrings.Length != 2)
                throw new Exception("Error input: " + s);

            List<PolynomialBlock> result = new List<PolynomialBlock>();
            
            result.AddRange(GetPolynomialBlock(subStrings[0]));
            
            List<PolynomialBlock> right = GetPolynomialBlock(subStrings[1]);
            foreach (var block in right)
                block.ConstantRevert();
            result.AddRange(right);

            return result.ToArray();
        }

        private List<PolynomialBlock> GetPolynomialBlock(string subString)
        {
            List<PolynomialBlock> result = new List<PolynomialBlock>();
            MatchCollection matches = Regex.Matches(subString, patternBlock);
            foreach (Match match in matches)
            {
                string[] str = match.Value.Replace("*", "")
                    .Replace(".", ",")
                    .Replace("^", "")
                    .Split('X');
                result.Add(new PolynomialBlock(GetDouble(str[0]), GetDouble(str[1])));
            }

            return result;
        }

        private double GetDouble(string str)
        {
            if (!double.TryParse(str, out var res))
                throw new Exception("Cannot convert " + str);
            return res;
        }

        private string StringPreprocess(string s)
        {
            return s.Replace(" ", "");
        }
    }
}