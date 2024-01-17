namespace ChessDotNetCore
{
    /// <summary>
    /// Defines class DetailedMove
    /// </summary>
    public class DetailedMove : Move
    {
        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="move">                 Move</param>
        /// <param name="piece">                Piece</param>
        /// <param name="castling">             Castling type</param>
        /// <param name="san">                  San</param>
        /// <param name="capturedPiece">        Captured piece</param>
        /// <param name="lastHalfMoveClock">    Last half move clock</param>
        /// <param name="enPassant">            En passant Position</param>
        /// <param name="lastCastlingType">     Last castling type</param>
        /// <param name="annotation">           Move suffix annotations</param>
        /// <param name="nag">                  NAG</param>
        /// <param name="comment">              Comment</param>
        public DetailedMove(Move move, Piece piece, Piece? capturedPiece, CastlingType castling, string san, int lastHalfMoveClock
            , Position? enPassant, CastlingType lastCastlingType, string annotation = "", int nag = 0, string comment = "")
            : base(move.OriginalPosition, move.NewPosition, move.Player, move.Promotion, move.MoveTime, move.MoveFromBook)
        {
            Piece = piece;
            CapturedPiece = capturedPiece;
            Castling = castling;
            SAN = san;
            LastHalfMoveClock = lastHalfMoveClock;
            LastEnPassant = enPassant;
            LastCastlingType = lastCastlingType;
            Annotation = annotation;
            NAG = nag;
            Comment = comment;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Moving piece
        /// </summary>
        public Piece Piece {get; private set; }

        /// <summary>
        /// Castling type
        /// </summary>
        public CastlingType Castling { get; private set; }

        /// <summary>
        /// SAN
        /// </summary>
        public string SAN { get; private set; }

        /// <summary>
        /// Captured piece
        /// </summary>
        public Piece? CapturedPiece { get; private set; }

        /// <summary>
        /// Last half move clock
        /// </summary>
        public int? LastHalfMoveClock { get; private set; }

        /// <summary>
        /// Last en passant Position
        /// </summary>
        public Position? LastEnPassant { get; private set; }

        /// <summary>
        /// Last CastlingType
        /// </summary>
        public CastlingType LastCastlingType { get; private set; }

        /// <summary>
        /// Move suffix annotations
        /// </summary>
        public string Annotation { get; set; }

        /// <summary>
        /// NAG
        /// </summary>
        public int NAG { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        public string Comment { get; set; }
        #endregion
    }
}
