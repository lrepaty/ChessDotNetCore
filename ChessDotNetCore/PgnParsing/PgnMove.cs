namespace ChessDotNetCore.PgnParsing
{
    public class PgnMove
    {
        #region Properties
        /// <summary>
        /// San move
        /// </summary>
        public string? SanMove { get; set; }

        /// <summary>
        /// Move suffix annotations
        /// </summary>
        public required string Annotation { get; set; }

        /// <summary>
        /// NAG
        /// </summary>
        public int NAG { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        public required string Comment { get; set; }
        #endregion
    }
}
