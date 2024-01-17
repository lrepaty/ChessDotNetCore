namespace ChessDotNetCore
{
    /// <summary>
    /// Defines class GameCreationData
    /// </summary>
    public class GameCreationData
    {
        #region Properties
        /// <summary>
        /// Chess board
        /// </summary>
        public Piece?[][]? Board { get; set; }

        /// <summary>
        /// All moves
        /// </summary>
        public DetailedMove[] AllMoves { get; set; } = new DetailedMove[]{};

        /// <summary>
        /// Current move position
        /// </summary>
        public int CurrentMovePosition { get; set; }

        /// <summary>
        /// Draw claimed
        /// </summary>
        public bool DrawClaimed { get; set; }

        /// <summary>
        /// Resigned player
        /// </summary>
        public Player Resigned { get; set; } = Player.None;

        /// <summary>
        /// Current player
        /// </summary>
        public Player CurrentPlayer { get; set; }

        /// <summary>
        /// Castling type
        /// </summary>
        public CastlingType CastlingType { get; set; }

        /// <summary>
        /// Position en passant
        /// </summary>
        public Position? EnPassant { get; set; }

        public int HalfMoveClock { get; set; }

        /// <summary>
        /// Half move clock
        /// </summary>
        public int FullMoveNumber { get; set; }
        #endregion
    }
}
