using System;
using System.IO;
using NUnit.Framework;
using сomputorV1;
using сomputorV1.Exception;
using сomputorV1.Polynomial;

namespace computor_v1_unit_test
{
    [TestFixture]
    public class Tests
    {
        // [TestCase("5 * X^0 + 4 * X^1 - 9.3 * X^2 = 1 * X^0")] 
        // [TestCase("5 * x^0 + 4 * x^1 - 9.3 * x^2 = 1 * x^0")]
        // [TestCase("5 * X^0 - 9.3 * X^2 = 1 * X^0")]
        // [TestCase("5 * X^0 + 4 * X^1 = 4 * X^0")]
        // [TestCase("5 * X^0 = 4 * X^0 + 7 * X^1")]
        // [TestCase("2*X^2 = 4 * x^0")]
        // [TestCase("5 * X ^ 0 = 4 * X ^ 0 + 7 * X ^ 1")]
        // [TestCase("5 * X ^ 0 + 13 * X ^ 1 + 3 * X ^ 2 = 1 * X ^ 0 + 1 * X ^ 1")]
        // [TestCase("6 * X^0 + 11 * x^1 + 5 * x^2 = 1 * X^0 + 1 * X^1")]
        // [TestCase("6 * X ^ 0 + 11 * X ^ 1 + 5 * X ^ 2 = 1 * X ^ 0 + 1 * X ^ 1")]
        // [TestCase("2*X^2 = 4")]
        // [TestCase("2*X^2 = 4 * x")]
        // [TestCase("4 * X^0 = 8")]
        // public void Test1(string arg)
        // {
        //     Polynomial polynomial = new Polynomial(arg);
        //     var res = polynomial.CheckAnswer(polynomial.GetAnswers()).Where(e => Math.Abs(e) > 0.0001f).ToArray();
        //     Assert.IsEmpty(res);
        // }
        //
        // [TestCase("20*X^0 = 0")]
        // [TestCase(typeof(Exception), "8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0")]
        // [TestCase(typeof(Exception), "42*X^0 = 42*X^0")]
        // [TestCase(typeof(Exception), "2*X^2-3*X+4*X^0=0")]
        // [TestCase(typeof(Exception), "5 * X^0 = 5 * X^0")]
        // [TestCase(typeof(Exception), "5 * X^0 + 3 * x^1 + 3 * x^2 = 1 * X^0 + 0 * X^1")]
        // [TestCase(typeof(Exception), "5 * X ^ 0 + 3 * X ^ 1 + 3 * X ^ 2 = 1 * X ^ 0 + 0 *  X ^ 1")]
        // public void DegreeError(System.Type type, string arg)
        // {
        //     Assert.Throws( type, () => new Polynomial(arg));
        // }
        //
        // [TestCase(typeof(Exception), "5 * X^0 + 4 * X^1 - 9.3 * X^2 = = 1 * X^0")]
        // [TestCase(typeof(Exception), "5 * X ^ 0 = 5 * X ^ 0")]
        // [TestCase(typeof(Exception), "4 * X ^ 0 = 8 * X ^ 0 ")]
        // public void Error(System.Type type, string arg)
        // {
        //     Assert.Throws( type, () => new Polynomial(arg));
        // }
        //
        // [TestCase("8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0")]
        // [TestCase("42*X^0 = 42*X^0")]
        // [TestCase("2*X^2-3*X+4*X^0=0")]
        // [TestCase("5 * X^0 = 5 * X^0")]
        // [TestCase("4 * X^0 = 8")]
        // [TestCase("5 * X^0 + 3 * x^1 + 3 * x^2 = 1 * X^0 + 0 * X^1")]
        // [TestCase("20*X^0 = 0")]
        // [TestCase("2*X^2 = 4")]
        // [TestCase("2*X^2 = 4 * x")]
        // [TestCase("5 * X ^ 0 + 3 * X ^ 1 + 3 * X ^ 2 = 1 * X ^ 0 + 0 *  X ^ 1")]
        // [TestCase("0 = 0")]
        // [TestCase("1*x= 0")]
        // [TestCase("2x = 0")]
        // [TestCase("3x^0 = 0")]
        // [TestCase("4*x^2 = 0")]
        // [TestCase("-x = 0")]
        // [TestCase("", false)]
        // [TestCase("0^1 = 0", false)]
        // public void TestRegex(string str, bool res = true)
        // {
        //     string pattern = Polynomial.patternFull;
        //     string patternElements = Polynomial.patternBlock;
        //     str = str.Replace(" ", "").ToUpper();
        //     var currReg = Regex.IsMatch(str, pattern);
        //     var currFloat = Regex.IsMatch(str, patternElements);
        //     Console.WriteLine($"({pattern}) -> {currReg}");
        //     Console.WriteLine($"({patternElements}) -> {currFloat}");
        //     foreach (Match match in Regex.Matches(str, patternElements))
        //     {
        //         Console.Write($"({match.Value})");
        //     }
        //     Console.WriteLine($"");
        //     Assert.IsTrue(currReg == res);
        // }
        //
        // [TestCase("1 * X ^ 2 = 0")]
        // [TestCase("-1* X ^ -2= 0")]
        // [TestCase("-   X ^ 2 = 0")]
        // [TestCase("1   X ^ 2 = 0")]
        // [TestCase("1 * X     = 0")]
        // [TestCase("    X ^ 2 = 0")]
        // [TestCase("1         = 0")]
        // [TestCase("1.1         = 0")]
        // [TestCase(".1         = 0")]
        // [TestCase("1..2        = 0")]
        // public void TestRegexStandart(string str, bool res = true)
        // {
        //     string floatP = "-?\\d*(\\.?\\d+)?";
        //     PrintRegexResult(str.Replace(" ", "").ToUpper(), floatP, res);
        //     string xP = $"(({floatP}|-)?\\*?X(\\^{floatP})?)|({floatP})";
        //     PrintRegexResult(str.Replace(" ", "").ToUpper(), xP, res);
        //     string xF = $"^{xP}={xP}$";
        //     PrintRegexResult(str.Replace(" ", "").ToUpper(), xF, res);
        // }
        //
        // private void PrintRegexResult(string str, string patternElements, bool res)
        // {
        //     var currFloat = Regex.IsMatch(str, patternElements);
        //     Console.WriteLine($"({patternElements}) -> {currFloat}");
        //     foreach (Match match in Regex.Matches(str, patternElements))
        //     {
        //         if (match.Value.Length > 0)
        //             Console.Write($"({match.Value})");
        //     }
        //     Console.WriteLine("");
        //
        //     Assert.IsTrue(currFloat == res);
        // }

        [TestCase("x = 1", 1)]
        [TestCase("-x = 2", -2)]
        public void LinearTest(string arg, double result)
        {
            var p = new Polynomial(arg);
            var answer = p.GetAnswers();
            Assert.AreEqual(answer.Length, 1);
            Assert.AreEqual(answer[0], result);
        }

        [TestCase("6x^2 + 11x - 35 = 0", new[] {1.6666, -3.5})]
        [TestCase("x² - 16 = 0", new double[] {4, -4})]
        [TestCase("x² - 7x = 0", new double[] {7, 0})]
        [TestCase("2x² + 8x = 0", new double[] {0, -4})]
        [TestCase("-1x^2 - 9x = 0", new double[] {-9, 0})]
        [TestCase("-4x^2 - 7x +12 = 0", new[] {-2.81552, 1.0655})]
        [TestCase("2x^2 - 4x - 2 = 0", new[] {2.414213, -0.414213})]
        [TestCase("x² - 64 = 0", new double[] {8, -8})]
        [TestCase("x^2+4x+4=0", new double[] {-2})]
        [TestCase("5 + X +4 *x = X ^ 2 + 1 *  X ^ 2", new[] {-0.7655, 3.2655})]
        public void QuadraticTest(string arg, double[] results)
        {
            var p = new Polynomial(arg);
            var answer = p.GetAnswers();
            Assert.AreEqual(answer.Length, results.Length);
            for (var i = 0; i < results.Length; i++) Assert.IsTrue(Math.Abs(answer[i] - results[i]) < TOLERANCE);
        }

        public double TOLERANCE { get; } = 0.0001;

        [TestCase(typeof(ExceptionStringFormat), "5 * X^0 + 4 * X^1 - 9.3 * X^2 = = 1 * X^0")]
        [TestCase(typeof(ExceptionStringFormat), "a = d")]
        [TestCase(typeof(ExceptionStringFormat), "6x^2 + 11x - 35")]
        [TestCase(typeof(ExceptionStringFormat), "x + a = 0 - d")]
        [TestCase(typeof(ExceptionEachRealNumber), "0 = 0")]
        [TestCase(typeof(ExceptionNoSolutions), "1 = 0")]
        [TestCase(typeof(ExceptionDegreeLimit), "1x^3 = 0")]
        [TestCase(typeof(ExceptionNegativeDiscriminant), "9.1x^2 − 6x + 2 = 0")]
        [TestCase(typeof(ExceptionNegativeDiscriminant), "9x² + 49 = 0")]
        [TestCase(typeof(ExceptionNegativeDiscriminant), "-2x² - 4 = 0")]
        [TestCase(typeof(ExceptionNegativeDiscriminant), "5 + 3 * X + X ^ 2 = 1 * X ^ 0 + 0 *  X ^ 1")]
        [TestCase(typeof(ExceptionStringFormat), "9.1x^.2 = 0")]
        [TestCase(typeof(ExceptionStringFormat), "9.1x.2 = 0")]
        [TestCase(typeof(ExceptionStringFormat), "9.1x2 = 0")]
        [TestCase(typeof(ExceptionStringFormat), "9.1x^2  ++ 1= 0")]
        [TestCase(typeof(ExceptionStringFormat), "9.1x2  + 1= 0")]
        [TestCase(typeof(ExceptionStringFormat),
            "123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789.123456789123456789x^2 = 0")]
        public void ExceptionTest(Type type, string arg)
        {
            Assert.Throws(type, () => new Polynomial(arg));
        }

        [TestCase("Tests\\Test.txt")]
        public void TestFromFile(string filePath)
        {
            Assert.IsTrue(File.Exists(filePath));
            var strs = File.ReadAllLines(filePath);
            foreach (var str in strs)
            {
                var newFile = filePath + ".log";
                //TODO:write to file
                Console.WriteLine($"{str}");
                Program.Main(new[] {str});
                Console.WriteLine("//------------------------------------");
            }
        }

        [TestCase("Tests")]
        public void TestFromDir(string dirPath)
        {
            var fileEntries = Directory.GetFiles(dirPath);
            foreach (var fileEntry in fileEntries) TestFromFile(fileEntry);
        }
    }
}