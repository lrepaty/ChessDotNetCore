namespace ChessDotNetCore
{
    /// <summary>Castling type</summary>
    [Flags]
    public enum CastlingType: byte
    {
        /// <summary>Any castling is not possible</summary>
        None = 0,
        /// <summary>White castling king side is possible</summary>
        WhiteCastleKingSide = 1,
        /// <summary>Black castling king side is possible</summary>
        BlackCastleKingSide = 2,
        /// <summary>White castling queen side is possible</summary>
        WhiteCastleQueenSide = 4,
        /// <summary>Black castling queen side is possible</summary>
        BlackCastleQueenSide = 8,
    }
}
