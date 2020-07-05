using System;
using System.IO;
using NUnit.Framework;
using сomputorV1.Exception;
using сomputorV1.Polynomial;

namespace computor_v1_unit_test
{
    [TestFixture]
    public class Tests
    {
        [TestCase(".1x = .1", 1)]
        [TestCase("-.1x = -1", 10)]
        [TestCase("-.1*x = -.1", 1)]
        [TestCase("-.1+x = .9", 1)]
        [TestCase("x = 1", 1)]
        [TestCase("x - 1.0 = 0", 1)]
        [TestCase("-x = 2", -2)]
        public void LinearTest(string arg, double result)
        {
            var p = new Polynomial(arg);
            var answer = p.GetAnswers();
            Assert.AreEqual(answer.Length, 1);
            Assert.IsTrue(Math.Abs(answer[0] - result) < TOLERANCE);
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
        [TestCase(typeof(ExceptionEachRealNumber), "0x = 0")]
        [TestCase(typeof(ExceptionNoSolutions), "1 = 0")]
        [TestCase(typeof(ExceptionDegreeLimit), "1x^3 = 0")]
        [TestCase(typeof(ExceptionNegativeDiscriminant), "9.1x^2 − 6x + 2 = 0")]
        [TestCase(typeof(ExceptionNegativeDiscriminant), "9x² + 49 = 0")]
        [TestCase(typeof(ExceptionNegativeDiscriminant), "-2x² - 4 = 0")]
        [TestCase(typeof(ExceptionNegativeDiscriminant), "5 + 3 * X + X ^ 2 = 1 * X ^ 0 + 0 *  X ^ 1")]
        [TestCase(typeof(ExceptionStringFormat), "9.1x^.2 = 0")]
        [TestCase(typeof(ExceptionStringFormat), "9.1x.2 = 0")]
        [TestCase(typeof(ExceptionStringFormat), "9.1x2 = 0")]
        [TestCase(typeof(ExceptionStringFormat), "9.1*2 = 0")]
        [TestCase(typeof(ExceptionStringFormat), "9.1x^2  ++ 1= 0")]
        [TestCase(typeof(ExceptionStringFormat), "9.1x2  + 1= 0")]
        [TestCase(typeof(ExceptionStringFormat), "9.x^2  + 1= 0")]
        [TestCase(typeof(ExceptionStringFormat), "9x^1.2  + 1= 0")]
        [TestCase(typeof(ExceptionStringFormat), "9x^2  + 1= 0 = 1")]
        [TestCase(typeof(ExceptionStringFormat), "a = 1")]
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
            Console.WriteLine($"{filePath}");
            var strs = File.ReadAllLines(filePath);
            foreach (var str in strs)
            {
                Console.WriteLine($"{str}");
                try
                {
                    new Polynomial(str);
                }
                catch (Exception)
                {
                    // ignored
                }

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