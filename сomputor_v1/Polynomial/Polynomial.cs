using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using сomputor_v1.Exception;

namespace сomputor_v1.Polynomial
{
    public class Polynomial
    {
        private string input;
        private PolynomialBlock[] polynomialBlocks;
        private double[] answers;
        private Solver solver;
        
        public static string patternFloat = @"\d*(?:\.\d*)?";
        public static string patternBlock = $@"((^|\+)|-)(({patternFloat})?\*?X(\^{patternFloat})?)|((^|\+|-){patternFloat})";
        public static string patternFull = $@"({patternBlock})+";

        public static bool CorrectInput(string input)
        {
            string[] inputs = input.Split('=');
            if (inputs.Length != 2)
                throw new ExceptionStringFormat(input);
            foreach (string s in inputs)
            {
                var pattern = patternFull;
                var IsMatch = Regex.IsMatch(s, pattern);
                var match = Regex.Match(s, pattern);
                if (!IsMatch || match.Length == 0 || !match.Value.Equals(s))
                    return false;
            }
            return true;
        }

        public Polynomial(string input)
        {
            try
            {
                this.input = StringPreprocess(input);
                if (!CorrectInput(this.input))
                    throw new ExceptionStringFormat(this.input);
                
                polynomialBlocks = InitPolynomialBlocks(this.input);
                solver = new Solver(polynomialBlocks);
                
                Console.Write("Reduced form: {0}\n", GetReducedForm());
                
                var degree = GetPolynomialDegree();
                Console.Write("Polynomial degree: {0}\n", degree);
                
                answers = solver.GetAnswer(degree);
                
                double[] results = solver.CheckAnswer(answers);
                Console.WriteLine("The solutions are:");
                for (var i = 0; i < answers.Length; i++)
                {
                    var answer = answers[i];
                    var result = results[i];
                    Console.WriteLine("x={0}(f(x) = {1})", answer, result);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private PolynomialBlock[] InitPolynomialBlocks(string input)
        {
            PolynomialBlock[] polynomialBlocks = GetSubStrings(input);
            PrintPolynomialBlocks(polynomialBlocks, "Parsing result");
            polynomialBlocks = CutExpression(polynomialBlocks);
            PrintPolynomialBlocks(polynomialBlocks, "Expression and sort result");
            return polynomialBlocks;
        }

        private void PrintPolynomialBlocks(PolynomialBlock[] polynomialBlocks, string Name)
        {
            Console.Write($"{Name}: ");
            foreach (PolynomialBlock polynomialBlock in polynomialBlocks)
            {
                Console.Write($"({polynomialBlock})");
            }
            Console.WriteLine("");
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

            var size = newpolynomialBlocks.Count;
            return newpolynomialBlocks
                .OrderBy(e => e.GetExponent())
                .Where(e => e.GetConstant() != 0 || size == 1)
                .ToArray();
        }

        private PolynomialBlock[] GetSubStrings(string s)
        {
            var subStrings = s.Split(new[] {'='});
            if (subStrings.Length != 2)
                throw new ExceptionStringFormat(s);

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
                var matchBlock = Regex.Match(match.Value, $"((\\*?X\\^?))");
                string[] strs = matchBlock.Value.Length == 0 ? new []{match.Value, "", "0"} : match.Value.Replace(matchBlock.Value, $" {matchBlock.Value} ").Split(' ');
                double constant = strs[0].Equals("") ? 1 : GetDouble(strs[0].Replace(".", ","));
                int exponent = !strs[2].Equals("") ? GetInt(strs[2]) : strs[1].Equals(matchBlock.Value) ? 1 : 0;
                result.Add(new PolynomialBlock(constant, exponent));
            }

            return result;
        }

        private double GetDouble(string str)
        {
            if (str.Equals("-"))
                return -1.0;
            if (str.Equals("+"))
                return 1.0;
            if (!double.TryParse(str, out var res))
                throw new ExceptionStringFormat($"Double format error: {str}");
            return res;
        }
        
        private int GetInt(string str)
        {
            if (!int.TryParse(str, out var res))
                throw new ExceptionStringFormat($"Int format error: {str}");
            return res;
        }

        public static string StringPreprocess(string s)
        {
            return s.Replace(" ", "")
                .Replace("²", "^2")
                .Replace("−", "-")
                    .ToUpper();
        }
    }
}