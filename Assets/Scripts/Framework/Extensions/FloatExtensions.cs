namespace Framework.Extensions
{
    public static class FloatExtensions
    {
        private const string SignedIntFormat = "+ #;- #;0";
        private const string UnsignedIntFormat = "#;- #;0";

        private const string SignedFloatFormat_SingleDecimal = "+ 0.0;- 0.0";
        private const string UnsignedFloatFormat_SingleDecimal = "0.0;- 0.0";

        private const string SignedFloatFormat_TwoDecimal = "+ 0.00;- 0.00";
        private const string UnsignedFloatFormat_TwoDecimal = "0.00;- 0.00";

        public static string ToFormattedString(this float value, bool signed)
        {
            if (signed)
            {
                if ((value > 0 && value < 1) || (value % 1) != 0)
                {
                    if (((value * 10) % 1) != 0)
                    {
                        return value.ToString(SignedFloatFormat_TwoDecimal);
                    }
                    else
                    {
                        return value.ToString(SignedFloatFormat_SingleDecimal);
                    }
                }

                return value.ToString(SignedIntFormat);
            }
            else
            {
                if ((value > 0 && value < 1) || (value % 1) != 0)
                {
                    if (((value * 10) % 1) != 0)
                    {
                        return value.ToString(UnsignedFloatFormat_TwoDecimal);
                    }
                    else
                    {
                        return value.ToString(UnsignedFloatFormat_SingleDecimal);
                    }
                }

                return value.ToString(UnsignedIntFormat);
            }
        }
    }
}