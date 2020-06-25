namespace сomputor_v1.Polynomial
{
    public class PolynomialBlock
    {
        private double constant;
        private int exponent;

        public PolynomialBlock(double constant, int exponent)
        {
            this.constant = constant;
            this.exponent = exponent;
        }

        public override string ToString()
        {
            string str = constant + "*X^" + exponent;
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