namespace ChessDotNetCore.PgnParsing
{
    /// <summary>
    /// Parser exception
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="errTxt">      Error Message</param>
    /// <param name="codeInError"> Code in error</param>
    /// <param name="ex">          Inner exception</param>
    [Serializable]
    public class PgnParserException(string? errTxt, string? codeInError, Exception? ex) : Exception(errTxt, ex)
    {
        #region Ctor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errTxt">       Error Message</param>
        /// <param name="codeInError">  Code in error</param>
        public PgnParserException(string? errTxt, string? codeInError) : this(errTxt, codeInError, ex: null) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errTxt">       Error Message</param>
        public PgnParserException(string? errTxt) : this(errTxt, codeInError: "", ex: null) { }

        /// <summary>
        /// Constructor
        /// </summary>
        public PgnParserException() : this(errTxt: "", codeInError: "", ex: null) { }
        #endregion

        #region Properties
        /// <summary>Code which is in error</summary>
        public string? CodeInError { get; } = codeInError;         /// <summary>Array of move position</summary>
        public PgnMove[]? PgnMoveList { get; set; } = null;
        #endregion
    } // Class PgnParserException}
}
