using System;
using UnityEngine;

public static partial class Direction4Extensions
{
    private static readonly Direction4[] _direction4NeighborArray  = new Direction4[]
    { 
        Direction4.Right, 
        Direction4.Down, 
        Direction4.Left, 
        Direction4.Up 
    };

    public static Direction4[] Direction4NeighborArray => _direction4NeighborArray;

    public static Vector2Int ToVector2Int(this Direction4 dir4)
    {
        return dir4 switch
        {
            Direction4.None => Vector2Int.zero,
            Direction4.Right => Vector2Int.right,
            Direction4.Down => Vector2Int.down,
            Direction4.Left => Vector2Int.left,
            Direction4.Up => Vector2Int.up,
            _ => throw new NotSupportedException(dir4 + " is not supported"),
        };
    }

    public static Vector2Int ToVector2Int(this Direction8 dir8)
    {
        return dir8 switch
        {
            Direction8.None => Vector2Int.zero,
            Direction8.Right => Vector2Int.right,
            Direction8.DownRight => new Vector2Int(1, -1),
            Direction8.Down => Vector2Int.down,
            Direction8.DownLeft => new Vector2Int(-1, -1),
            Direction8.Left => Vector2Int.left,
            Direction8.UpLeft => new Vector2Int(-1, 1),
            Direction8.Up => Vector2Int.up,
            Direction8.UpRight => new Vector2Int(1, 1),
            _ => throw new NotSupportedException(dir8 + " is not supported"),
        };
    }

    public static Vector2 ToVector2(this Direction4 dir4)
    {
        return ToVector2(dir4, 0);
    }

    public static Vector2 ToVector2(Direction4 dir4, int _ = 0)
    {
        return dir4 switch
        {
            Direction4.None => Vector2.zero,
            Direction4.Right => Vector2.right,
            Direction4.Down => Vector2.down,
            Direction4.Left => Vector2.left,
            Direction4.Up => Vector2.up,
            _ => throw new NotSupportedException(dir4 + " is not supported"),
        };
    }

    public static Quaternion FromToRotation(Direction4 dir)
    {
        return Quaternion.FromToRotation(dir.ToVector2(), Vector2.right);
    }
}
