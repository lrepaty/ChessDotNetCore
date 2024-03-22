namespace ChessDotNetCore
{
    /// <summary>
    /// Defines class Move
    /// </summary>
    public class Move
    {
        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="originalPosition"> Original position</param>
        /// <param name="newPosition">      New position</param>
        /// <param name="player">           Player</param>
        /// <param name="promotion">        Promotion char</param>
        /// <param name="moveTime">         Move time</param>
        /// <param name="moveFromBook">     Move  from book</param>
        public Move(Position originalPosition, Position newPosition, Player player, char? promotion = null, TimeSpan moveTime = new(), bool moveFromBook = false)
        {
            OriginalPosition = originalPosition;
            NewPosition = newPosition;
            Player = player;
            if (promotion.HasValue)
            {
                Promotion = char.ToUpper(promotion.Value);
            }
            else
            {
                Promotion = null;
            }
            MoveTime = moveTime;
            MoveFromBook = moveFromBook;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="originalPosition"> Original position</param>
        /// <param name="newPosition">      New position</param>
        /// <param name="player">           Player</param>
        /// <param name="promotion">        Promotion char</param>
        /// <param name="moveTime">         Move time</param>
        /// <param name="moveFromBook">     Move  from book</param>
        public Move(byte originalPosition, byte newPosition, Player player, char? promotion = null, TimeSpan moveTime = new(), bool moveFromBook = false)
        {
            OriginalPosition = ChessUtilities.GetPositionAt(originalPosition);
            NewPosition = ChessUtilities.GetPositionAt(newPosition);
            Player = player;
            if (promotion.HasValue)
            {
                Promotion = char.ToUpper(promotion.Value);
            }
            else
            {
                Promotion = null;
            }
            MoveTime = moveTime;
            MoveFromBook = moveFromBook;
        }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="originalPosition"> Original position</param>
        /// <param name="newPosition">      New position</param>
        /// <param name="player">           Player</param>
        /// <param name="promotion">        Promotion char</param>
        public Move(string originalPosition, string newPosition, Player player, char? promotion = null)
        {
            OriginalPosition = new Position(originalPosition);
            NewPosition = new Position(newPosition);
            Player = player;
            if (promotion.HasValue)
            {
                Promotion = char.ToUpper(promotion.Value);
            }
            else
            {
                Promotion = null;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Original position
        /// </summary>
        public Position OriginalPosition { get; private set; }

        /// <summary>
        /// New position
        /// </summary>
        public Position NewPosition { get; private set; }


        /// <summary>
        /// Player
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// Promotion char
        /// </summary>
        public char? Promotion { get; private set; }

        /// <summary>
        /// Move tme
        /// </summary>
        public TimeSpan MoveTime { get; set; }

        /// <summary>
        /// Move from book
        /// </summary>
        public bool MoveFromBook { get; private set; }

        /// <summary>
        ///  Determines whether the specified object is equal to the current Move.
        /// </summary>
        /// <param name="obj">   The object to compare with the current Move.</param>
        /// <returns>
        /// true if the specified object is equal to the current Move; otherwise, false.
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            Move move1 = this;
            var move2 = (Move)obj;
            return move1.OriginalPosition.Equals(move2.OriginalPosition)
                && move1.NewPosition.Equals(move2.NewPosition)
                && move1.Player == move2.Player
                && move1.Promotion == move2.Promotion;
        }

        /// <summary>
        ///     Hash function.
        /// </summary>
        /// <returns>
        ///     A hash code for the current Move.
        /// </returns>
        public override int GetHashCode()
        {
            return new { OriginalPosition, NewPosition, Player, Promotion }.GetHashCode();
        }

        /// <summary>
        ///  Returns a value that indicates whether two Move values are equal.
        /// </summary>
        /// <param name="move1">   The first Move to compare.</param>
        /// <param name="move2">   The second Move to compare.</param>
        /// <returns>
        /// true if left and right are equal; otherwise, false.
        /// </returns>
        public static bool operator ==(Move? move1, Move? move2)
        {
            if (ReferenceEquals(move1, move2))
                return true;
            if ((object?)move1 == null || (object?)move2 == null)
                return false;
            return move1.Equals(move2);
        }

        /// <summary>
        ///  Returns a value that indicates whether two Move values are not equal.
        /// </summary>
        /// <param name="move1">   The first value to compare.</param>
        /// <param name="move2">   The second value to compare.</param>
        /// <returns>
        /// true if left and right are not equal; otherwise, false.
        /// </returns>
        public static bool operator !=(Move? move1, Move? move2)
        {
            if (ReferenceEquals(move1, move2))
                return false;
            if ((object?)move1 == null || (object?)move2 == null)
                return true;
            return !move1.Equals(move2);
        }

        /// <summary>
        ///     Returns a string that represents the current Move.
        /// </summary>
        /// <returns>
        ///     A string that represents the current Move.
        /// </returns>
        public override string ToString()
        {
            return OriginalPosition.ToString() + "-" + NewPosition.ToString() + Promotion?? "";
        }
        #endregion
    }
}
