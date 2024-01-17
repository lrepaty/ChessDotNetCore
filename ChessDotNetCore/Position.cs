using System.Globalization;

namespace ChessDotNetCore
{
    #region Types
    /// <summary>Line</summary>
    public enum Line: sbyte
    {
        a = 0,
        b = 1,
        c = 2,
        d = 3,
        e = 4,
        f = 5,
        g = 6,
        h = 7,
        None = -1
    }
    #endregion

    /// <summary>
    /// Defines class Position
    /// </summary>
    public class Position
    {
        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public Position()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="line">     Line</param>
        /// <param name="rank">     Rank</param>
        public Position(Line line, byte rank)
        {
            Line = line;
            Rank = rank;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="position">     Position</param>
        public Position(string position)
        {
            if (position == null)
            {
                throw new ArgumentNullException(nameof(position));
            }
            if (position.Length != 2)
            {
                throw new ArgumentException("Length of `pos` is not 2.");
            }

            position = position.ToLowerInvariant();
            char file = position[0];
            char rank = position[1];
            byte _rank;
            switch (file)
            {
                case 'a':
                    Line = Line.a;
                    break;
                case 'b':
                    Line = Line.b;
                    break;
                case 'c':
                    Line = Line.c;
                    break;
                case 'd':
                    Line = Line.d;
                    break;
                case 'e':
                    Line = Line.e;
                    break;
                case 'f':
                    Line = Line.f;
                    break;
                case 'g':
                    Line = Line.g;
                    break;
                case 'h':
                    Line = Line.h;
                    break;
                default:
                    throw new ArgumentException("First char of `pos` not in range A-F.");
            }

            if (byte.TryParse(rank.ToString(), out _rank))
            {
                if (_rank < 1 || _rank > 8)
                {
                    throw new ArgumentException("Second char of `pos` not in range 1-8.");
                }
                Rank = _rank;
            }
            else
            {
                throw new ArgumentException("Second char of `pos` not in range 1-8.");
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Line
        /// </summary>
        public Line Line { get; private set; }

        /// <summary>
        /// Rank
        /// </summary>
        public byte Rank { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        ///  Determines whether the specified object is equal to the current Position.
        /// </summary>
        /// <param name="obj">   The object to compare with the current Position.</param>
        /// <returns>
        /// true if the specified object is equal to the current Position; otherwise, false.
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null || GetType() != obj.GetType())
                return false;
            Position? pos2 = (Position?)obj;
            return Line == pos2!.Line && Rank == pos2.Rank;
        }

        /// <summary>
        ///     Hash function.
        /// </summary>
        /// <returns>
        ///     A hash code for the current Position.
        /// </returns>
        public override int GetHashCode()
        {
            return new { Line, Rank }.GetHashCode();
        }

        /// <summary>
        ///  Returns a value that indicates whether two Position values are equal.
        /// </summary>
        /// <param name="position1">    The first Position to compare.</param>
        /// <param name="position2">    The second Position to compare.</param>
        /// <returns>
        /// true if left and right are equal; otherwise, false.
        /// </returns>
        public static bool operator ==(Position? position1, Position? position2)
        {
            if (ReferenceEquals(position1, position2))
                return true;
            if ((object?)position1 == null || (object?)position2 == null)
                return false;
            return position1.Equals(position2);
        }

        /// <summary>
        ///  Returns a value that indicates whether two Position values are not equal.
        /// </summary>
        /// <param name="position1">    The first Position to compare.</param>
        /// <param name="position2">    The second Position to compare.</param>
        /// <returns>
        /// true if left and right are not equal; otherwise, false.
        /// </returns>
        public static bool operator !=(Position? position1, Position? position2)
        {
            if (ReferenceEquals(position1, position2))
                return false;
            if ((object?)position1 == null || (object?)position2 == null)
                return true;
            return !position1.Equals(position2);
        }

        /// <summary>
        ///     Returns a string that represents the current Position.
        /// </summary>
        /// <returns>
        ///     A string that represents the current Position.
        /// </returns>
        public override string ToString()
        {
            return Line.ToString() + Rank.ToString(CultureInfo.InvariantCulture);
        }
        #endregion
    }
}
