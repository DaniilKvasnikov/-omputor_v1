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
        private double[] answers;
        
        public static string floatP = "-?\\d*(\\.?\\d+)?";
        public static string patternBlock = $"(({floatP}|-)?\\*?X(\\^{floatP})?)|({floatP})";
        
        public static string patternFull = $"^{patternBlock}={patternBlock}$";

        public Polynomial(string input)
        {
            try
            {
                this.input = StringPreprocess(input);
                if (!CorrectInput(this.input))
                    throw new Exception(string.Format("Error format string: {0}\n", this.input));
                
                polynomialBlocks = InitPolynomialBlocks(this.input);
                
                Console.Write("Reduced form: {0}\n", GetReducedForm());
                
                var degree = GetPolynomialDegree();
                Console.Write("Polynomial degree: {0}\n", degree);
                
                answers = GetAnswer(degree);
                
                double[] results = CheckAnswer(answers);

                Console.WriteLine("Discriminant is strictly positive, the solutions are:");
                for (var i = 0; i < answers.Length; i++)
                {
                    var answer = answers[i];
                    var result = results[i];
                    Console.WriteLine("x={0}(f(x) = {1})", answer, result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private PolynomialBlock[] InitPolynomialBlocks(string input)
        {
            return CutExpression(GetSubStrings(input));
        }

        public static bool CorrectInput(string input)
        {
            input = StringPreprocess(input);
            var IsMatch = Regex.IsMatch(input, patternBlock);
            return IsMatch;
        }

        private int GetPolynomialDegree()
        {
            return polynomialBlocks.Max(e => e.GetExponent());
        }

        private string GetReducedForm()
        {
            string str = "";
            for (var i = 0; i < polynomialBlocks.Length; i++)
            {
                var subString = polynomialBlocks[i];
                if (polynomialBlocks[i].GetConstant() >= 0 && i > 0)
                    str += "+";
                str += subString;
            }

            str += "=0";

            return str;
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

        private double[] GetAnswer(int degree)
        {
            switch (degree)
            {
                case 2:
                    return SolveQuadratic();
                case 1:
                    return SolveLinear();
                case 0:
                    ZerroExponent();
                    break;
            }
            throw new Exception("The polynomial degree is strictly greater than 2, I can't solve.");
        }

        private void ZerroExponent()
        {
            double c = GetParam(0);
            if (c != 0)
                throw new Exception("Each real number is a solution.");
            else
                throw new Exception("There are no solutions.");
        }

        private double[] SolveLinear()
        {
            double b = GetParam(1);
            double c = GetParam(0);
            return new []{-c/b};
        }

        private double[] SolveQuadratic()
        {
            double a = GetParam(2);
            double b = GetParam(1);
            double c = GetParam(0);
            double discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
                var exception = string.Format("discriminant({2})<0. " +
                                              "A negative discriminant indicates that neither of the solutions are real numbers.)" +
                                              "\nAnd answer is (-{0} ± √{1}) / {2}", b, discriminant, 2 * a);
                throw new Exception(exception);
            }
            if (discriminant == 0)
                return new []{-b / (2 * a)};
            return new []{(-b + Math.Sqrt(discriminant)) / (2 * a), (-b - Math.Sqrt(discriminant)) / (2 * a)};
        }

        private double GetParam(int i)
        {
            return polynomialBlocks.FirstOrDefault(block => block.GetExponent() == i)?.GetConstant() ?? 0;
        }

        private PolynomialBlock[] CutExpression(PolynomialBlock[] polynomialBlocks1)
        {
            List<PolynomialBlock> newpolynomialBlocks = new List<PolynomialBlock>();
            foreach (var block in polynomialBlocks1)
            {
                var to = newpolynomialBlocks.FirstOrDefault(e =>
                    e.GetExponent() == block.GetExponent() && e != block);
                if (to == null)
                    newpolynomialBlocks.Add(block);
                else
                    to.AddConstant(block.GetConstant());
            }

            return newpolynomialBlocks
                .OrderBy(e => e.GetSortParam())
                // .Where(e => e.GetConstant() != 0)
                .ToArray();
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
                if (match.Value.Length == 0)
                    continue;
                string[] str = match.Value
                    .Replace(".", ",")
                    .Replace("+", "")
                    .Split(new []{'X', '*', '^'})
                    .Where(s => s.Length > 0)
                    .ToArray();
                //TODO: more variants
                Console.Write($"({str})");
                double constant = GetDouble(str[0]);
                int exponent = str.Length == 2 ? GetInt(str[1]) : 1;
                result.Add(new PolynomialBlock(constant, exponent));
            }
            Console.WriteLine($"");

            return result;
        }

        private double GetDouble(string str)
        {
            if (!double.TryParse(str, out var res))
                throw new Exception("Cannot convert " + str);
            return res;
        }
        
        private int GetInt(string str)
        {
            if (!int.TryParse(str, out var res))
                throw new Exception("Cannot convert " + str);
            return res;
        }

        public static string StringPreprocess(string s)
        {
            return s.Replace(" ", "")
                    .ToUpper();
        }
    }
}