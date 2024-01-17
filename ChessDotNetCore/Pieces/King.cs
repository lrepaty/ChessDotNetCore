using System.Collections.ObjectModel;

namespace ChessDotNetCore.Pieces
{
    /// <summary>
    /// Defines class King
    /// </summary>
    public class King : Piece
    {
        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public King() : this(Player.None)
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="owner">Owner</param>
        public King(Player owner) : this(owner, true)
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="owner">Owner</param>
        public King(Player owner, bool hasCastlingAbility)
        {
            Owner = owner;
            HasCastlingAbility = hasCastlingAbility;
            IsPromotionResult = false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// King Has castling ability
        /// </summary>
        public bool HasCastlingAbility
        {
            get;
            set;
        }

        /// <summary>
        /// value
        /// </summary>
        protected override byte Value { get; } = 5;
        #endregion

        #region Methods
        /// <summary>
        /// Get King with IsPromotionResult
        /// </summary>
        public override Piece AsPromotion()
        {
            var copy = new King(Owner, HasCastlingAbility);
            copy.IsPromotionResult = true;
            return copy;
        }

        /// <summary>
        /// Get King with inverted owner
        /// </summary>
        public override Piece GetWithInvertedOwner()
        {
            return new King(ChessUtilities.GetOpponentOf(Owner));
        }

        /// <summary>
        ///     Get Fen character.
        /// </summary>
        /// <returns>
        ///     Fen character for current King.
        /// </returns>
        public override char GetFenCharacter()
        {
            return Owner == Player.White ? 'K' : 'k';
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
            Position origin = move.OriginalPosition;
            Position destination = move.NewPosition;
            var distance = new PositionDistance(origin, destination);
            if (((distance.DistanceX == 1 && distance.DistanceY == 1)
                || (distance.DistanceX == 0 && distance.DistanceY == 1)
                || (distance.DistanceX == 1 && distance.DistanceY == 0)) &&
                (game.GetPieceAt(destination)! == null! || game.GetPieceAt(destination)!.Owner == ChessUtilities.GetOpponentOf(move.Player)))
            {
                return true;
            }

            if (distance.DistanceX == 2)
            {
                if (move.Player == Player.White)
                {
                    if (origin.Rank != 1 || destination.Rank != 1) return false;
                    if (game.InitialWhiteKingLine == Line.e && game.InitialWhiteRookLineKingsideCastling == Line.h && destination.Line == Line.g)
                    {
                        return CanCastle(origin, new Position(Line.h, 1), game);
                    }
                    if (game.InitialWhiteKingLine == Line.e && game.InitialWhiteRookLineQueensideCastling == Line.a && destination.Line == Line.c)
                    {
                        return CanCastle(origin, new Position(Line.a, 1), game);
                    }
                }
                else
                {
                    if (origin.Rank != 8 || destination.Rank != 8) return false;
                    if (game.InitialBlackKingLine == Line.e && game.InitialBlackRookLineKingsideCastling == Line.h && destination.Line == Line.g)
                    {
                        return CanCastle(origin, new Position(Line.h, 8), game);
                    }
                    if (game.InitialBlackKingLine == Line.e && game.InitialBlackRookLineQueensideCastling == Line.a && destination.Line == Line.c)
                    {
                        return CanCastle(origin, new Position(Line.a, 8), game);
                    }
                }
            }

            if (game.GetPieceAt(destination) is Rook)
            {
                return CanCastle(origin, destination, game);
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Can castle
        /// </summary>
        /// <param name="origin">       Origin position</param>
        /// <param name="destination">  Destination</param>
        /// <param name="game">         Chessgame</param>
        /// <returns>
        /// true if can castle, false if not
        /// </returns>
        protected virtual bool CanCastle(Position origin, Position destination, ChessGame game)
        {
            if (!HasCastlingAbility) return false;
            if (Owner == Player.White)
            {
                if (origin.Rank != 1 || destination.Rank != 1) return false;

                if (destination.Line == game.InitialWhiteRookLineKingsideCastling)
                {
                    if (!game.CanCastle(game.CastlingType, CastlingType.WhiteCastleKingSide))
                    {
                        return false;
                    }
                }
                else if (destination.Line == game.InitialWhiteRookLineQueensideCastling)
                {
                    if (!game.CanCastle(game.CastlingType, CastlingType.WhiteCastleQueenSide))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                if (game.IsInCheck(Player.White))
                {
                    return false;
                }

                Line[] betweenKingAndFinal = ChessUtilities.LinesBetween(origin.Line, destination.Line == game.InitialWhiteRookLineKingsideCastling ? Line.g : Line.c, false, true);
                foreach (Line f in betweenKingAndFinal)
                {
                    if (f != destination.Line && game.GetPieceAt(f, 1)! != null)
                    {
                        return false;
                    }
                    if (game.WouldBeInCheckAfter(new Move(origin, new Position(f, 1), Player.White), Player.White))
                    {
                        return false;
                    }
                }

                Line[] betweenRookAndFinal = ChessUtilities.LinesBetween(destination.Line, destination.Line == game.InitialWhiteRookLineKingsideCastling ? Line.f : Line.d, false, true);
                foreach (Line f in betweenRookAndFinal)
                {
                    Piece? p = game.GetPieceAt(f, 1);
                    if (f != destination.Line && p! != null! && !(p is King))
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (origin.Rank != 8 || destination.Rank != 8) return false;

                if (destination.Line == game.InitialBlackRookLineKingsideCastling)
                {
                    if (!game.CanCastle(game.CastlingType, CastlingType.BlackCastleKingSide))
                    {
                        return false;
                    }
                }
                else if (destination.Line == game.InitialBlackRookLineQueensideCastling)
                {
                    if (!game.CanCastle(game.CastlingType, CastlingType.BlackCastleQueenSide))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                if (game.IsInCheck(Player.Black))
                {
                    return false;
                }

                Line[] betweenKingAndFinal = ChessUtilities.LinesBetween(origin.Line, destination.Line == game.InitialBlackRookLineKingsideCastling ? Line.g : Line.c, false, true);
                foreach (Line f in betweenKingAndFinal)
                {
                    if (f != destination.Line && game.GetPieceAt(f, 8)! != null)
                    {
                        return false;
                    }
                    if (game.WouldBeInCheckAfter(new Move(origin, new Position(f, 8), Player.Black), Player.Black))
                    {
                        return false;
                    }
                }

                Line[] betweenRookAndFinal = ChessUtilities.LinesBetween(destination.Line, destination.Line == game.InitialBlackRookLineKingsideCastling ? Line.f : Line.d, false, true);
                foreach (Line f in betweenRookAndFinal)
                {
                    Piece? p = game.GetPieceAt(f, 8);
                    if (f != destination.Line && p! != null! && !(p is King))
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
            var directions = new List<sbyte[]>() { new sbyte[] { 0, 1 }, new sbyte[] { 1, 0 }, new sbyte[] { 0, -1 }, new sbyte[] { -1, 0 },
                        new sbyte[] { 1, 1 }, new sbyte[] { 1, -1 }, new sbyte[] { -1, 1 }, new sbyte[] { -1, -1 } };
            if (piece.Owner == Player.White && game.InitialWhiteKingLine == Line.e && from.Line == game.InitialWhiteKingLine && from.Rank == 1)
            {
                if (game.InitialWhiteRookLineKingsideCastling == Line.h) directions.Add(new sbyte[] { 2, 0 });
                if (game.InitialWhiteRookLineQueensideCastling == Line.a) directions.Add(new sbyte[] { -2, 0 });
            }
            if (piece.Owner == Player.Black && game.InitialBlackKingLine == Line.e && from.Line == game.InitialBlackKingLine && from.Rank == 8)
            {
                if (game.InitialBlackRookLineKingsideCastling == Line.h) directions.Add(new sbyte[] { 2, 0 });
                if (game.InitialBlackRookLineQueensideCastling == Line.a) directions.Add(new sbyte[] { -2, 0 });
            }
            if ((piece.Owner == Player.White ? game.InitialWhiteKingLine : game.InitialBlackKingLine) == from.Line && from.Rank == (piece.Owner == Player.White ? 1 : 8))
            {
                if (piece.Owner == Player.White)
                {
                    sbyte d1 = game.InitialWhiteRookLineKingsideCastling - from.Line;
                    sbyte d2 = game.InitialWhiteRookLineQueensideCastling - from.Line;
                    if (Math.Abs(d1) != 1)
                    {
                        directions.Add(new sbyte[] { d1, 0 });
                    }
                    if (Math.Abs(d2) != 1)
                    {
                        directions.Add(new sbyte[] { d2, 0 });
                    }
                }
                else
                {
                    sbyte d1 = game.InitialBlackRookLineKingsideCastling - from.Line;
                    sbyte d2 = game.InitialBlackRookLineQueensideCastling - from.Line;
                    if (Math.Abs(d1) != 1)
                    {
                        directions.Add(new sbyte[] { d1, 0 });
                    }
                    if (Math.Abs(d2) != 1)
                    {
                        directions.Add(new sbyte[] { d2, 0 });
                    }
                }
            }
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
