using System;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using сomputor_v1;
using сomputor_v1.Exception;

namespace computor_v1_unit_test
{
    [TestFixture]
    public class Tests
    {
        [TestCase("5 * X^0 + 4 * X^1 - 9.3 * X^2 = 1 * X^0")] 
        [TestCase("5 * x^0 + 4 * x^1 - 9.3 * x^2 = 1 * x^0")]
        [TestCase("5 * X^0 - 9.3 * X^2 = 1 * X^0")]
        [TestCase("5 * X^0 + 4 * X^1 = 4 * X^0")]
        [TestCase("5 * X^0 = 4 * X^0 + 7 * X^1")]
        [TestCase("2*X^2 = 4 * x^0")]
        [TestCase("5 * X ^ 0 = 4 * X ^ 0 + 7 * X ^ 1")]
        [TestCase("5 * X ^ 0 + 13 * X ^ 1 + 3 * X ^ 2 = 1 * X ^ 0 + 1 * X ^ 1")]
        [TestCase("6 * X^0 + 11 * x^1 + 5 * x^2 = 1 * X^0 + 1 * X^1")]
        [TestCase("6 * X ^ 0 + 11 * X ^ 1 + 5 * X ^ 2 = 1 * X ^ 0 + 1 * X ^ 1")]
        [TestCase("2*X^2 = 4")]
        [TestCase("2*X^2 = 4 * x")]
        [TestCase("4 * X^0 = 8")]
        public void Test1(string arg)
        {
            Polynomial polynomial = new Polynomial(arg);
            var res = polynomial.CheckAnswer(polynomial.GetAnswers()).Where(e => Math.Abs(e) > 0.0001f).ToArray();
            Assert.IsEmpty(res);
        }
        
        [TestCase("20*X^0 = 0")]
        [TestCase(typeof(Exception), "8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0")]
        [TestCase(typeof(Exception), "42*X^0 = 42*X^0")]
        [TestCase(typeof(Exception), "2*X^2-3*X+4*X^0=0")]
        [TestCase(typeof(Exception), "5 * X^0 = 5 * X^0")]
        [TestCase(typeof(Exception), "5 * X^0 + 3 * x^1 + 3 * x^2 = 1 * X^0 + 0 * X^1")]
        [TestCase(typeof(Exception), "5 * X ^ 0 + 3 * X ^ 1 + 3 * X ^ 2 = 1 * X ^ 0 + 0 *  X ^ 1")]
        public void DegreeError(System.Type type, string arg)
        {
            Assert.Throws( type, () => new Polynomial(arg));
        }

        [TestCase(typeof(Exception), "5 * X^0 + 4 * X^1 - 9.3 * X^2 = = 1 * X^0")]
        [TestCase(typeof(Exception), "5 * X ^ 0 = 5 * X ^ 0")]
        [TestCase(typeof(Exception), "4 * X ^ 0 = 8 * X ^ 0 ")]
        public void Error(System.Type type, string arg)
        {
            Assert.Throws( type, () => new Polynomial(arg));
        }

        [TestCase("8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0")]
        [TestCase("42*X^0 = 42*X^0")]
        [TestCase("2*X^2-3*X+4*X^0=0")]
        [TestCase("5 * X^0 = 5 * X^0")]
        [TestCase("4 * X^0 = 8")]
        [TestCase("5 * X^0 + 3 * x^1 + 3 * x^2 = 1 * X^0 + 0 * X^1")]
        [TestCase("20*X^0 = 0")]
        [TestCase("2*X^2 = 4")]
        [TestCase("2*X^2 = 4 * x")]
        [TestCase("5 * X ^ 0 + 3 * X ^ 1 + 3 * X ^ 2 = 1 * X ^ 0 + 0 *  X ^ 1")]
        [TestCase("0 = 0")]
        [TestCase("1*x= 0")]
        [TestCase("2x = 0")]
        [TestCase("3x^0 = 0")]
        [TestCase("4*x^2 = 0")]
        [TestCase("-x = 0")]
        [TestCase("", false)]
        [TestCase("0^1 = 0", false)]
        public void TestRegex(string str, bool res = true)
        {
            string pattern = Polynomial.patternFull;
            string patternElements = Polynomial.patternBlock;
            str = str.Replace(" ", "").ToUpper();
            var currReg = Regex.IsMatch(str, pattern);
            var currFloat = Regex.IsMatch(str, patternElements);
            Console.WriteLine($"({pattern}) -> {currReg}");
            Console.WriteLine($"({patternElements}) -> {currFloat}");
            foreach (Match match in Regex.Matches(str, patternElements))
            {
                Console.Write($"({match.Value})");
            }
            Console.WriteLine($"");
            Assert.IsTrue(currReg == res);
        }
        
        [TestCase("1 * X ^ 2 = 0")]
        [TestCase("-1* X ^ -2= 0")]
        [TestCase("-   X ^ 2 = 0")]
        [TestCase("1   X ^ 2 = 0")]
        [TestCase("1 * X     = 0")]
        [TestCase("    X ^ 2 = 0")]
        [TestCase("1         = 0")]
        [TestCase("1.1         = 0")]
        [TestCase(".1         = 0")]
        [TestCase("1..2        = 0")]
        public void TestRegexStandart(string str, bool res = true)
        {
            string floatP = "-?\\d*(\\.?\\d+)?";
            PrintRegexResult(str.Replace(" ", "").ToUpper(), floatP, res);
            string xP = $"(({floatP}|-)?\\*?X(\\^{floatP})?)|({floatP})";
            PrintRegexResult(str.Replace(" ", "").ToUpper(), xP, res);
            string xF = $"^{xP}={xP}$";
            PrintRegexResult(str.Replace(" ", "").ToUpper(), xF, res);
        }

        private void PrintRegexResult(string str, string patternElements, bool res)
        {
            var currFloat = Regex.IsMatch(str, patternElements);
            Console.WriteLine($"({patternElements}) -> {currFloat}");
            foreach (Match match in Regex.Matches(str, patternElements))
            {
                if (match.Value.Length > 0)
                    Console.Write($"({match.Value})");
            }
            Console.WriteLine("");

            Assert.IsTrue(currFloat == res);
        }
        
        [TestCase(typeof(ExceptionStringFormat), "5 * X^0 + 4 * X^1 - 9.3 * X^2 = = 1 * X^0")]
        [TestCase(typeof(ExceptionEachRealNumber), "0 = 0")]
        [TestCase(typeof(ExceptionNoSolutions), "1 = 0")]
        [TestCase(typeof(ExceptionDegreeLimit), "1x^3 = 0")]
        [TestCase(typeof(ExceptionNegativeDiscriminant), "9x2 − 6x + 2 = 0")]
        [TestCase(typeof(ExceptionNegativeDiscriminant), "9x.2 = 0")]
        public void ExceptionTest(System.Type type, string arg)
        {
            Assert.Throws( type, () => new Polynomial(arg));
        }

    }
}