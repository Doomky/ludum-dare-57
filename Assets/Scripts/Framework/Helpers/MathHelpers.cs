using UnityEngine;
using Framework;
using Framework.Managers;

namespace Framework.Helpers
{

    public static class MathHelpers
    {
        public static float OffsetFraction(float a, float b, float offset)
        {
            return (a + offset) / (b + offset);
        }

        public static bool TestProbability(float probability)
        {
            if (probability >= 1)
            {
                return true;
            }

            if (probability <= 0)
            {
                return false;
            }

            return Random.Range(0f, 1f) <= probability;
        }
        
        /// <summary>
        /// Allow to clamp a value from [0, infinity] to [0,1] while reducing the value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float ValueReduction(float x, float a, float b, float c)
        {
            float coeffA = x / (x + a/c);
            float coeffB = (b/c) * x;

            return 1 - Mathf.Exp(- coeffA * coeffB);
        }
    }
}