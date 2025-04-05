using Framework.Extensions;
using System;
using UnityEngine;

public static class Direction8Extensions
{
    private static readonly Direction8[] _direction8NeighborArray = new Direction8[]
    {
        Direction8.Right,
        Direction8.DownRight,
        Direction8.Down,
        Direction8.DownLeft,
        Direction8.Left,
        Direction8.UpLeft,
        Direction8.Up,
        Direction8.UpRight,
    };

    public static Direction8[] Direction8NeighborArray => _direction8NeighborArray;

    public static Vector2 ToVector2(this Direction8 dir8)
    {
        return ToVector2(dir8, 0);
    }

    public static Vector2 ToVector2(Direction8 dir8, int _ = 0)
    {
        return dir8 switch
        {
            Direction8.None => Vector2.zero,
            Direction8.Right => Vector2.right,
            Direction8.DownRight => new Vector2(1, -1).normalized,
            Direction8.Down => Vector2.down,
            Direction8.DownLeft => new Vector2(-1, -1).normalized,
            Direction8.Left => Vector2.left,
            Direction8.UpLeft => new Vector2(-1, 1).normalized,
            Direction8.Up => Vector2.up,
            Direction8.UpRight => new Vector2(1, 1).normalized,
            _ => throw new NotSupportedException(dir8 + " is not supported"),
        };
    }

    public static Direction8 FromVector2(Vector2 vector2)
    {
        int directionValue = 0;
        float delta = 0.2f;
        if (Math.Abs(vector2.y) > delta)
        {
            directionValue += (int)(vector2.y > 0 ? Direction8.Up : Direction8.Down);
        }

        if (Math.Abs(vector2.x) > delta)
        {
            directionValue += (int)(vector2.x > 0 ? Direction8.Right : Direction8.Left);
        }

        return (Direction8)directionValue;
    }

    public static Quaternion FromToRotation(Direction8 dir)
    {
        return FromToRotation(dir, Vector2.right);
    }

    public static Quaternion FromToRotation(Direction8 dir, Vector2 fromVect)
    {
        return dir.ToVector2().FromToRotation(fromVect);
    }

    public static Quaternion FromToRotation(Direction4 dir, Vector2 fromVect)
    {
        return FromToRotation((Direction8)dir, fromVect);
    }
}
