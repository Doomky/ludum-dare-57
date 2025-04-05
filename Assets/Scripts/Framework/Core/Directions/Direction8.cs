
[System.Flags]
public enum Direction8
{
    None        = 0,

    Right       = 1 << 0,
    DownRight   = 1 << 1,
    Down        = 1 << 2,
    DownLeft    = 1 << 3,
    Left        = 1 << 4,
    UpLeft      = 1 << 5,
    Up          = 1 << 6,
    UpRight     = 1 << 7,

    All         =  Right | DownRight | Down | DownLeft | Left | UpLeft | Up | UpRight,
}
