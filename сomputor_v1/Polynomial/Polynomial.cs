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
            for (var i = 0; i < subStrings.Length; i++)
            {
                var subString = subStrings[i];
                MatchCollection matches = Regex.Matches(subString, patternBlock);
                foreach (Match match in matches)
                {
                    string[] str = match.Value.Replace("*", "").Replace(".", ",").Replace("^", "").Split(new []{'X'});
                    double constant;
                    if (int.TryParse(str[0], out var intConstant))
                        constant = intConstant;
                    // else if (float.TryParse(str[0], out var floatConstant))
                    //     constant = floatConstant;
                    else if (Double.TryParse(str[0], out double number))
                        constant = number;
                    else
                        throw new Exception("Cannot convert " + str[0]);
                    if (!int.TryParse(str[1], out var exponent))
                        throw new Exception("Cannot convert " + str[1]);
                    PolynomialBlock block = new PolynomialBlock(constant, exponent);
                    result.Add(block);
                }
            }

            return result.ToArray();
        }

        private string StringPreprocess(string s)
        {
            return s.Replace(" ", "");
        }
    }
}