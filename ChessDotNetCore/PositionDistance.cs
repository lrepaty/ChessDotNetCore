namespace ChessDotNetCore
{
    /// <summary>
    /// Defines class PositionDistance
    /// </summary>
    public struct PositionDistance
    {
        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="position1">    Position 1</param>
        /// <param name="position2">    Position 2</param>
        public PositionDistance(Position? position1, Position? position2)
        {
            if (position1 == null)
                throw new ArgumentNullException(nameof(position1));
            if (position2 == null)
                throw new ArgumentNullException(nameof(position2));
            DistanceX = Math.Abs((int)position1.Line - (int)position2.Line);
            DistanceY = Math.Abs((int)position1.Rank - (int)position2.Rank);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Distance X
        /// </summary>
        public int DistanceX { get; private set; }

        /// <summary>
        /// Distance Y
        /// </summary>
        public int DistanceY { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        ///  Determines whether the specified object is equal to the current PositionDistance.
        /// </summary>
        /// <param name="obj">   The object to compare with the current PositionDistance.</param>
        /// <returns>
        /// true if the specified object is equal to the current PositionDistance; otherwise, false.
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var distance2 = (PositionDistance)obj;
            return DistanceX == distance2.DistanceX && DistanceY == distance2.DistanceY;
        }

        /// <summary>
        ///     Hash function.
        /// </summary>
        /// <returns>
        ///     A hash code for the current PositionDistance.
        /// </returns>
        public override int GetHashCode()
        {
            return new { DistanceX, DistanceY }.GetHashCode();
        }

        /// <summary>
        ///  Returns a value that indicates whether two PositionDistance values are equal.
        /// </summary>
        /// <param name="distance1">    The first PositionDistance to compare.</param>
        /// <param name="distance2">    The second PositionDistance to compare.</param>
        /// <returns>
        /// true if left and right are equal; otherwise, false.
        /// </returns>
        public static bool operator ==(PositionDistance distance1, PositionDistance distance2)
        {
            return distance1.Equals(distance2);
        }

        /// <summary>
        ///  Returns a value that indicates whether two PositionDistance values are not equal.
        /// </summary>
        /// <param name="distance1">    The first PositionDistance to compare.</param>
        /// <param name="distance2">    The second PositionDistance to compare.</param>
        /// <returns>
        /// true if left and right are not equal; otherwise, false.
        /// </returns>
        public static bool operator !=(PositionDistance distance1, PositionDistance distance2)
        {
            return !distance1.Equals(distance2);
        }
        #endregion
    }
}
