using System;
using System.Text.RegularExpressions;

namespace сomputor_v1
{
    public class Polynomial
    {
        private string input;

        private static string patternBlock = "([-+]?([0-9](,[0-9])*(\\.[0-9]+)?)[*]X[\\^][0-9])";

        public Polynomial(string input)
        {
            this.input = string_preprocess(input);

            var subStrings = this.input.Split(new[] {'='});
            if (subStrings.Length > 2)
                throw new Exception("Error input: " + input);

            foreach (var subString in subStrings)
            {
                MatchCollection matches = Regex.Matches(subString, patternBlock);
                foreach (Match match in matches)
                {
                    Console.WriteLine(match.Groups[1].Value);
                }
            }
        }

        private string string_preprocess(string s)
        {
            return s.Replace(" ", "");
        }
    }
}