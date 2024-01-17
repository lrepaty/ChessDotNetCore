namespace ChessDotNetCore
{
    /// <summary>Result of the current board. Game is finished unless OnGoing or Check</summary>
    public enum GameResult: byte
    {
        /// <summary>Game is going on</summary>
        OnGoing,
        /// <summary>3 times the same board</summary>
        ThreeFoldRepeat,
        /// <summary>50 times without moving a pawn or eating a piece</summary>
        FiftyRuleRepeat,
        /// <summary>No more move for the next player</summary>
        Stalemated,
        /// <summary>Not enough pieces to do a check mate</summary>
        InsufficientMaterial,
        /// <summary>Check</summary>
        Check,
        /// <summary>Checkmate</summary>
        Mate,
        /// <summary>Wite resign</summary>
        WhiteResign,
        /// <summary>Black resign</summary>
        BlackResign,
        /// <summary>Wite in timeout, black won</summary>
        WhiteTimeout,
        /// <summary>Black in timeout, white won</summary>
        BlackTimeout,
        /// <summary>Unknown</summary>
        Unknown
    }
}
