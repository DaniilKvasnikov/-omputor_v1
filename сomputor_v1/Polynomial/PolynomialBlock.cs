﻿namespace сomputorV1.Polynomial
{
    public class PolynomialBlock
    {
        private readonly int exponent;
        private double constant;

        public PolynomialBlock(double constant, int exponent)
        {
            this.constant = constant;
            this.exponent = exponent;
        }

        public override string ToString()
        {
            var str = constant + "*X^" + exponent;
            return str;
        }

        public void ConstantRevert()
        {
            constant = -constant;
        }

        public int GetExponent()
        {
            return exponent;
        }

        public double GetConstant()
        {
            return constant;
        }

        public void AddConstant(double addConstant)
        {
            constant += addConstant;
        }
    }
}