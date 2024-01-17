using System.Collections.ObjectModel;

namespace ChessDotNetCore
{
    /// <summary>
    /// Defines class Piece
    /// </summary>
    public abstract class Piece
    {
        #region Properties
        /// <summary>
        /// Owner
        /// </summary>
        public Player Owner { get; protected set; }

        /// <summary>
        /// Is promotion result
        /// </summary>
        public bool IsPromotionResult { get; protected set; }
        /// <summary>
        /// value
        /// </summary>
        protected virtual byte Value { get; }

        /// <summary>
        /// Piece value
        /// </summary>
        public virtual byte PieceValue
        {
            get
            {
                return (byte)(2 * Value + (Owner == Player.White ? 1 : 0));
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get Piece with inverted owner
        /// </summary>
        public abstract Piece GetWithInvertedOwner();

        /// <summary>
        /// Get Piece with IsPromotionResult
        /// </summary>
        public abstract Piece AsPromotion();

        /// <summary>
        ///  Determines whether the specified object is equal to the current Piece.
        /// </summary>
        /// <param name="obj">   The object to compare with the current Piece.</param>
        /// <returns>
        /// true if the specified object is equal to the current Piece; otherwise, false.
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null || GetType() != obj.GetType())
                return false;
            Piece piece1 = this;
            Piece piece2 = (Piece)obj;
            return piece1.Owner == piece2.Owner;
        }

        /// <summary>
        ///     Hash function.
        /// </summary>
        /// <returns>
        ///     A hash code for the current Piece.
        /// </returns>
        public override int GetHashCode()
        {
            return new { Piece = GetFenCharacter(), Owner }.GetHashCode();
        }

        /// <summary>
        ///  Returns a value that indicates whether two Piece values are equal.
        /// </summary>
        /// <param name="piece1">   The first Piece to compare.</param>
        /// <param name="piece2">   The second Piece to compare.</param>
        /// <returns>
        /// true if left and right are equal; otherwise, false.
        /// </returns>
        public static bool operator ==(Piece? piece1, Piece? piece2)
        {
            if (ReferenceEquals(piece1, piece2))
                return true;
            if ((object?)piece1 == null || (object?)piece2 == null)
                return false;
            return piece1.Equals(piece2);
        }

        /// <summary>
        ///  Returns a value that indicates whether two Piece values are not equal.
        /// </summary>
        /// <param name="piece1">   The first Piece to compare.</param>
        /// <param name="piece2">   The second Piece to compare.</param>
        /// <returns>
        /// true if left and right are not equal; otherwise, false.
        /// </returns>
        public static bool operator !=(Piece? piece1, Piece? piece2)
        {
            if (ReferenceEquals(piece1, piece2))
                return false;
            if ((object?)piece1 == null || (object?)piece2 == null)
                return true;
            return !piece1.Equals(piece2);
        }

        /// <summary>
        ///     Get Fen character.
        /// </summary>
        /// <returns>
        ///     Fen character for current Piece.
        /// </returns>
        public abstract char GetFenCharacter();

        /// <summary>
        /// Test if move is valid
        /// </summary>
        /// <param name="move">     Move to validate</param>
        /// <param name="game">     Chessgame</param>
        /// <returns>
        /// true if valid, false if not
        /// </returns>
        public abstract bool IsValidMove(Move move, ChessGame game);

        /// <summary>
        /// Get valid moves
        /// </summary>
        /// <param name="from">                 Position from</param>
        /// <param name="returnIfAny">          Return if any move is valid</param>
        /// <param name="game">                 Chessgame</param>
        /// <param name="gameMoveValidator">    Game move validater</param>
        /// <returns>
        /// Valid moves
        /// </returns>
        public abstract ReadOnlyCollection<Move> GetValidMoves(Position from, bool returnIfAny, ChessGame game, Func<Move, bool> gameMoveValidator);
        #endregion
    }
}
