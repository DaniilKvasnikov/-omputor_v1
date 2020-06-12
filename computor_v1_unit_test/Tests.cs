using System;
using NUnit.Framework;
using сomputor_v1;

namespace computor_v1_unit_test
{
    [TestFixture]
    public class Tests
    {
        [TestCase("5 * X^0 + 4 * X^1 - 9.3 * X^2 = 1 * X^0")]
        public void Test1(string arg)
        {
            Polynomial polynomial = new Polynomial("5 * X^0 + 4 * X^1 - 9.3 * X^2 = 1 * X^0");
        }

        [TestCase(typeof(Exception), "5 * X^0 + 4 * X^1 - 9.3 * X^2 = = 1 * X^0")]
        public void Error(System.Type type, string arg)
        {
            
            Assert.Throws( type, () =>
            {
                Polynomial polynomial = new Polynomial(arg);});
        }
    }
}