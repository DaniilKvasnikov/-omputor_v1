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
            return constant + " : " + exponent;
        }

        public void ConstantRevert()
        {
            constant = -constant;
        }
    }
}