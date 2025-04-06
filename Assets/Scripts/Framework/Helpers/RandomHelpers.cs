using UnityEngine;

namespace Framework.Helpers
{
    public static class RandomHelpers
    {
        public static Vector2 GetRandomRectEdgePoint()
        {
            float floatingCoord = Random.Range(0f, 1f);
            float EdgeCoord = RandomHelpers.TestProbability(0.5f) ? 1 : 0;

            return RandomHelpers.TestProbability(0.5f) ? new Vector2(EdgeCoord, floatingCoord) : new Vector2(floatingCoord, EdgeCoord);
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
    }
}