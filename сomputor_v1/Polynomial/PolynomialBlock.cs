namespace сomputor_v1
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
            return constant + " : " + exponent;
        }
    }
}