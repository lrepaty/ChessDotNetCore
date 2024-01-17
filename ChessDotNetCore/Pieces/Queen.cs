using System.Collections.ObjectModel;

namespace ChessDotNetCore.Pieces
{
    /// <summary>
    /// Defines class Queen
    /// </summary>
    public class Queen : Piece
    {
        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public Queen() : this(Player.None)
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="owner">Owner</param>
        public Queen(Player owner)
        {
            Owner = owner;
            IsPromotionResult = false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// value
        /// </summary>
        protected override byte Value { get; } = 4;
        #endregion

        #region Methods
        /// <summary>
        /// Get Queen with IsPromotionResult
        /// </summary>
        public override Piece AsPromotion()
        {
            var copy = new Queen(Owner);
            copy.IsPromotionResult = true;
            return copy;
        }

        /// <summary>
        /// Get Queen with inverted owner
        /// </summary>
        public override Piece GetWithInvertedOwner()
        {
            return new Queen(ChessUtilities.GetOpponentOf(Owner));
        }

        /// <summary>
        ///     Get Fen character.
        /// </summary>
        /// <returns>
        ///     Fen character for current Queen.
        /// </returns>
        public override char GetFenCharacter()
        {
            return Owner == Player.White ? 'Q' : 'q';
        }


        /// <summary>
        /// Test if move is valid
        /// </summary>
        /// <param name="move">     Move to validate</param>
        /// <param name="game">     Chessgame</param>
        /// <returns>
        /// true if valid, false if not
        /// </returns>
        public override bool IsValidMove(Move move, ChessGame game)
        {
            ChessUtilities.ThrowIfNull(move, nameof(move));
            ChessUtilities.ThrowIfNull(game, nameof(game));
            return new Bishop(Owner).IsValidMove(move, game) || new Rook(Owner).IsValidMove(move, game);
        }

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
        public override ReadOnlyCollection<Move> GetValidMoves(Position from, bool returnIfAny, ChessGame game, Func<Move, bool> gameMoveValidator)
        {
            ChessUtilities.ThrowIfNull(from, nameof(from));
            ReadOnlyCollection<Move> horizontalVerticalMoves = new Rook(Owner).GetValidMoves(from, returnIfAny, game, gameMoveValidator);
            if (returnIfAny && horizontalVerticalMoves.Count > 0)
                return horizontalVerticalMoves;
            ReadOnlyCollection<Move> diagonalMoves = new Bishop(Owner).GetValidMoves(from, returnIfAny, game, gameMoveValidator);
            return new ReadOnlyCollection<Move>(horizontalVerticalMoves.Concat(diagonalMoves).ToList());
        }
        #endregion
    }
}
