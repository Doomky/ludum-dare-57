using System;
using UnityEngine;

namespace Framework.Extensions
{
    public static class Vector2IntExtensions 
    {
        public enum Direction
        {
            Unknown,
            Horizontal,
            Vertical
        }

        public static int ManhattanDistance(this Vector2Int v, Vector2Int w)
        {
            return Math.Abs(v.x - w.x) + Math.Abs(v.y - w.y);
        }

        public static int ManhattanMagnitude(this Vector2Int v)
        {
            return Math.Abs(v.x) + Math.Abs(v.y);
        }

        public static bool IsAlignedOnAxis(this Vector2Int v, Vector2Int w)
        {
            return v.x == w.x || v.y == w.y;
        }

        public static Direction GetDirection(this Vector2Int v)
        {
            if (v.x * v.y != 0)
            {
                return Direction.Unknown;
            }

            return v.x == 0 ? Direction.Horizontal : Direction.Vertical;
        }

        public static Direction4 ToDirection4(this Vector2Int v)
        {
            if (v.x * v.y != 0 || (v.x == 0 && v.y == 0))
            {
                return Direction4.None;
            }

            if (v.x == 0)
            {
                return v.y > 0 ? Direction4.Up : Direction4.Down;
            }
            else
            {
                return v.x > 0 ? Direction4.Right : Direction4.Left;
            }
        }

        public static Vector2 ToVector2(this Vector2Int v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector3Int ToVector3Int(this Vector2Int v)
        {
            return new Vector3Int(v.x, v.y, 0);
        }

        public static void ForEach4DirNeighbors(this Vector2Int position, Action<Vector2Int> action)
        {
            int count = Direction4Extensions.Direction4NeighborArray.Length;
            for (int i = 0; i < count; i++)
            {
                action.Invoke(position + Direction4Extensions.Direction4NeighborArray[i].ToVector2Int());
            }
        }

        public static void ForEach8DirNeighbors(this Vector2Int position, Action<Vector2Int> action)
        {
            int count = Direction8Extensions.Direction8NeighborArray.Length;
            for (int i = 0; i < count; i++)
            {
                action.Invoke(position + Direction8Extensions.Direction8NeighborArray[i].ToVector2Int());
            }
        }

        public static Vector2Int Normalized(this Vector2Int position)
        {
            position.x = position.x < 0 ? -1 : position.x > 0 ? 1 : 0;
            position.y = position.y < 0 ? -1 : position.y > 0 ? 1 : 0;

            return position;
        }
    }
}
