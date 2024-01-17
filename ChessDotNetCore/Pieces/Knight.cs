using System.Collections.ObjectModel;

namespace ChessDotNetCore.Pieces
{
    /// <summary>
    /// Defines class Knight
    /// </summary>
    public class Knight : Piece
    {
        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public Knight() : this(Player.None)
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="owner">Owner</param>
        public Knight(Player owner)
        {
            Owner = owner;
            IsPromotionResult = false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// value
        /// </summary>
        protected override byte Value { get; } = 1;
        #endregion

        #region Methods
        /// <summary>
        /// value
        /// </summary>
        public override Piece AsPromotion()
        {
            var copy = new Knight(Owner);
            copy.IsPromotionResult = true;
            return copy;
        }

        public override Piece GetWithInvertedOwner()
        {
            return new Knight(ChessUtilities.GetOpponentOf(Owner));
        }

        /// <summary>
        /// Get Knight with inverted owner
        /// </summary>
        public override char GetFenCharacter()
        {
            return Owner == Player.White ? 'N' : 'n';
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
            if ((posDelta.DistanceX != 2 || posDelta.DistanceY != 1) && (posDelta.DistanceX != 1 || posDelta.DistanceY != 2))
                return false;
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
            var directions = new sbyte[][] { new sbyte[] { 2, 1 }, new sbyte[] { -2, -1 }, new sbyte[] { 1, 2 }, new sbyte[] { -1, -2 },
                        new sbyte[] { 1, -2 }, new sbyte[] { -1, 2 }, new sbyte[] { 2, -1 }, new sbyte[] { -2, 1 } };
            foreach (sbyte[] dir in directions)
            {
                if ((int)from.Line + dir[0] < 0 || (int)from.Line + dir[0] >= l1
                    || from.Rank + dir[1] < 1 || from.Rank + dir[1] > l0)
                    continue;
                var move = new Move(from, new Position(from.Line + dir[0], (byte)(from.Rank + dir[1])), piece.Owner);
                if (gameMoveValidator(move))
                {
                    validMoves.Add(move);
                    if (returnIfAny)
                        return new ReadOnlyCollection<Move>(validMoves);
                }
            }
            return new ReadOnlyCollection<Move>(validMoves);
        }
        #endregion
    }
}
