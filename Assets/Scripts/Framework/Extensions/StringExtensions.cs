using Framework.Helpers;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using System.Linq;

namespace Framework.Extensions
{
    public static class StringExtensions
    {
        private const string EscapedCurlyBracketPattern = @"(\{{2}|\}{2})";
        private const string FormatParameterPattern = @"\{(\d+)(?:\:?[^}]*)\}";

        public enum CaseType
        {
            CamelCase,
            SnakeCase
        }

        public static string ToString(this string str, CaseType inputFormatType = CaseType.CamelCase)
        {
            switch (inputFormatType)
            {
                case CaseType.CamelCase:
                    {
                        return Regex.Replace(str, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                    }

                case CaseType.SnakeCase:
                    {
                        return str.Replace('_', ' ');
                    }

                default:
                    {
                        throw new NotSupportedException($"Could not support input format type: {inputFormatType}");
                    }
            }
        }

        public static int GetFormatArgumentsCount(this string str)
        {
            // removes escaped curly brackets
            string strWithoutEscapedCurlyBracket = Regex.Replace(str, EscapedCurlyBracketPattern, string.Empty);

            System.Collections.Generic.IEnumerable<int> a = Regex
                .Matches(strWithoutEscapedCurlyBracket, FormatParameterPattern)
                .OfType<Match>()
                .SelectMany(match => match.Groups.OfType<Group>().Skip(1))
                .Select(index => Int32.Parse(index.Value));
            
            if (!a.Any())
            {
                return 0;
            }

            return a.Max() + 1;
        }

        public static string Colorize(this string str, Color color, bool includeAlpha = true)
        {
            return Colorize(str, color.ToHtmlString(includeAlpha, false));
        }

        public static string Colorize(this string str, string hexacode)
        {
            return $"<color=#{hexacode}>{str}</color>";
        }
        
        public static string AsSpriteTag(this string value)
        {
            return $"<sprite name=\"{value}\">";
        }

        private static string WrapWithTag(this string str, string tag, string attribute, string attributeValue)
        {
            return $"<{tag} {attribute}=\"{attributeValue}\">{str}</{tag}>";
        }

        private static string WrapWithTag(this string str, string tag, string tagValue = null)
        {
            return string.IsNullOrEmpty(tagValue) ? $"<{tag}>{str}</{tag}>" : $"<{tag}={tagValue}>{str}</{tag}>";
        }
    }
}