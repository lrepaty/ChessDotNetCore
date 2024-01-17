using System.Collections.ObjectModel;

namespace ChessDotNetCore.Pieces
{
    /// <summary>
    /// Defines class Bishop
    /// </summary>
    public class Bishop : Piece
    {
        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public Bishop() : this(Player.None)
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="owner">Owner</param>
        public Bishop(Player owner)
        {
            Owner = owner;
            IsPromotionResult = false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// value
        /// </summary>
        protected override byte Value { get; } = 2;
        #endregion

        #region Methods
        /// <summary>
        /// Get Bishop with IsPromotionResult
        /// </summary>
        public override Piece AsPromotion()
        {
            var copy = new Bishop(Owner);
            copy.IsPromotionResult = true;
            return copy;
        }

        /// <summary>
        /// Get Bishop with inverted owner
        /// </summary>
        public override Piece GetWithInvertedOwner()
        {
            return new Bishop(ChessUtilities.GetOpponentOf(Owner));
        }

        /// <summary>
        ///     Get Fen character.
        /// </summary>
        /// <returns>
        ///     Fen character for current Bishop.
        /// </returns>
        public override char GetFenCharacter()
        {
            return Owner == Player.White ? 'B' : 'b';
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
            Position origin = move.OriginalPosition;
            Position destination = move.NewPosition;

            var posDelta = new PositionDistance(origin, destination);
            if (posDelta.DistanceX != posDelta.DistanceY)
                return false;
            bool increasingRank = destination.Rank > origin.Rank;
            bool increasingLine = (int)destination.Line > (int)origin.Line;
            for (int f = (int)origin.Line + (increasingLine ? 1 : -1), r = origin.Rank + (increasingRank ? 1 : -1);
                 increasingLine ? f < (int)destination.Line : f > (int)destination.Line;
                 f += increasingLine ? 1 : -1, r += increasingRank ? 1 : -1)
            {
                if (game.GetPieceAt((Line)f, r)! != null!)
                {
                    return false;
                }
            }
            return true;
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
            List<Move> validMoves = [];
            Piece piece = game.GetPieceAt(from)!;
            byte l0 = game.BoardHeight;
            byte l1 = game.BoardWidth;
            for (sbyte i = -7; i < 8; i++)
            {
                if (i == 0)
                    continue;
                if (from.Rank + i > 0 && from.Rank + i <= l0
                    && (int)from.Line + i > -1 && (int)from.Line + i < l1)
                {
                    var move = new Move(from, new Position(from.Line + i, (byte)(from.Rank + i)), piece.Owner);
                    if (gameMoveValidator(move))
                    {
                        validMoves.Add(move);
                        if (returnIfAny)
                            return new ReadOnlyCollection<Move>(validMoves);
                    }
                }
                if (from.Rank - i > 0 && from.Rank - i <= l0
                    && (int)from.Line + i > -1 && (int)from.Line + i < l1)
                {
                    var move = new Move(from, new Position(from.Line + i, (byte)(from.Rank - i)), piece.Owner);
                    if (gameMoveValidator(move))
                    {
                        validMoves.Add(move);
                        if (returnIfAny)
                            return new ReadOnlyCollection<Move>(validMoves);
                    }
                }
            }
            return new ReadOnlyCollection<Move>(validMoves);
        }
        #endregion
    }
}
