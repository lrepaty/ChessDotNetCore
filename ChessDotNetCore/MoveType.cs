namespace ChessDotNetCore
{
    [Flags]
    /// <summary>Move type</summary>
    public enum MoveType: byte
    {
        /// <summary>Invalid</summary>
        Invalid = 1,
        /// <summary>Move</summary>
        Move = 2,
        /// <summary>Capture</summary>
        Capture = 4,
        /// <summary>Castling</summary>
        Castling = 8,
        /// <summary>Promotion</summary>
        Promotion = 16,
        /// <summary>EnPassant</summary>
        EnPassant = 32
    }
}
