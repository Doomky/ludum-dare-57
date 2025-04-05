using System;
using System.Linq;
using static Framework.Extensions.StringExtensions;

namespace Framework.Extensions
{
    public static class EnumExtensions
    {
        public static string ToString(this Enum value, CaseType caseType)
        {
            return value
                .ToString()
                .ToString(caseType);
        }

        public static TEnum[] ToArray<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();
        }
    }
}