using System.Collections.ObjectModel;

namespace ChessDotNetCore.Pieces
{
    /// <summary>
    /// Defines class Rook
    /// </summary>
    public class Rook : Piece
    {
        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public Rook() : this(Player.None)
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="owner">Owner</param>
        public Rook(Player owner)
        {
            Owner = owner;
            IsPromotionResult = false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// value
        /// </summary>
        protected override byte Value { get; } = 3;
        #endregion

        #region Methods
        /// <summary>
        /// Get Rook with IsPromotionResult
        /// </summary>
        public override Piece AsPromotion()
        {
            var copy = new Rook(Owner);
            copy.IsPromotionResult = true;
            return copy;
        }

        /// <summary>
        /// Get Rook with inverted owner
        /// </summary>
        public override Piece GetWithInvertedOwner()
        {
            return new Rook(ChessUtilities.GetOpponentOf(Owner));
        }

        /// <summary>
        ///     Get Fen character.
        /// </summary>
        /// <returns>
        ///     Fen character for current Rook.
        /// </returns>
        public override char GetFenCharacter()
        {
            return Owner == Player.White ? 'R' : 'r';
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
            if (posDelta.DistanceX != 0 && posDelta.DistanceY != 0)
                return false;
            bool increasingRank = destination.Rank > origin.Rank;
            bool increasingLine = (int)destination.Line > (int)origin.Line;
            if (posDelta.DistanceX == 0)
            {
                int f = (int)origin.Line;
                for (int r = origin.Rank + (increasingRank ? 1 : -1);
                    increasingRank ? r < destination.Rank : r > destination.Rank;
                    r += increasingRank ? 1 : -1)
                {
                    if (game.GetPieceAt((Line)f, r)! != null)
                    {
                        return false;
                    }
                }
            }
            else // (posDelta.DeltaY == 0)
            {
                int r = origin.Rank;
                for (int f = (int)origin.Line + (increasingLine ? 1 : -1);
                    increasingLine ? f < (int)destination.Line : f > (int)destination.Line;
                    f += increasingLine ? 1 : -1)
                {
                    if (game.GetPieceAt((Line)f, r)! != null)
                    {
                        return false;
                    }
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
            ChessUtilities.ThrowIfNull(from, nameof(from));
            List<Move> validMoves = [];
            Piece piece = game.GetPieceAt(from)!;
            byte l0 = game.BoardHeight;
            byte l1 = game.BoardWidth;
            for (sbyte i = -7; i < 8; i++)
            {
                if (i == 0)
                    continue;
                if (from.Rank + i > 0 && from.Rank + i <= l0)
                {
                    var move = new Move(from, new Position(from.Line, (byte)(from.Rank + i)), piece.Owner);
                    if (gameMoveValidator(move))
                    {
                        validMoves.Add(move);
                        if (returnIfAny)
                            return new ReadOnlyCollection<Move>(validMoves);
                    }
                }
                if ((int)from.Line + i > -1 && (int)from.Line + i < l1)
                {
                    var move = new Move(from, new Position(from.Line + i, from.Rank), piece.Owner);
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
