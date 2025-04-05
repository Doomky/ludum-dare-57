[System.Flags]
public enum Direction4
{
    None    = 0,

    Right   = 1 << 0,
    Down    = 1 << 1,
    Left    = 1 << 2,
    Up      = 1 << 3,

    All = Right | Down | Left | Up,
}
