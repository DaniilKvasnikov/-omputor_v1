namespace сomputor_v1
{
    public class PolynomialBlock
    {
        private double constant;
        private double exponent;

        public PolynomialBlock(double constant, double exponent)
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

        public double GetSortParam()
        {
            return exponent;
        }

        public double GetExponent()
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