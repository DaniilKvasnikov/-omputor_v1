using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using сomputor_v1;

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
        [TestCase("-5 * X^2 = 4")]
        public void Test1(string arg)
        {
            Polynomial polynomial = new Polynomial(arg);
            var res = polynomial.CheckAnswer(polynomial.GetAnswers()).Where(e => Math.Abs(e) > 0.001f).ToArray();
            Assert.IsEmpty(res);
        }
        
        [TestCase(typeof(Exception), "8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0")]
        [TestCase(typeof(Exception), "42*X^0 = 42*X^0")]
        [TestCase(typeof(Exception), "2*X^2-3*X+4*X^0=0")]
        [TestCase(typeof(Exception), "5 * X^0 = 5 * X^0")]
        [TestCase(typeof(Exception), "4 * X^0 = 8")]
        [TestCase(typeof(Exception), "5 * X^0 + 13 * X^1 + 3 * X^2 = 1 * X^0 + 1 * X^1")]
        [TestCase(typeof(Exception), "6 * X^0 + 11 * x^1 + 5 * x^2 = 1 * X^0 + 1 * X^1")]
        [TestCase(typeof(Exception), "5 * X^0 + 3 * x^1 + 3 * x^2 = 1 * X^0 + 0 * X^1")]
        [TestCase(typeof(Exception), "20*X^0 = 0")]
        [TestCase(typeof(Exception), "")]
        public void DegreeError(System.Type type, string arg)
        {
            Assert.Throws( type, () => new Polynomial(arg));
        }

        [TestCase(typeof(Exception), "5 * X^0 + 4 * X^1 - 9.3 * X^2 = = 1 * X^0")]
        [TestCase(typeof(Exception), "5 * X^0 + 4 * X^1 - 9.3 * X^2 + 1 * X^0")]
        public void Error(System.Type type, string arg)
        {
            Assert.Throws( type, () => new Polynomial(arg));
        }
    }
}