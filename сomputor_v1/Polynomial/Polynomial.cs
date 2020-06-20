using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace сomputor_v1
{
    public class Polynomial
    {
        private string input;
        private PolynomialBlock[] subStrings;

        private static string patternBlock = "([-]?([0-9](,[0-9])*(\\.[0-9]+)?)[*]X[\\^][0-9])";

        public Polynomial(string input)
        {
            this.input = StringPreprocess(input);
            this.subStrings = GetSubStrings(this.input);
            foreach (var subString in this.subStrings)
            {
                Console.WriteLine(subString);
            }
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