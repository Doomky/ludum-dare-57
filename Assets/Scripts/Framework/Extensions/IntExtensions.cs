namespace Framework.Extensions
{
    public static class IntExtensions
    {
        private const string SignedFormat = "+ #;- #;0";

        public static string ToSignedString(this int value)
        {
            return value.ToString(SignedFormat);
        }
    }
}