namespace ChessDotNetCore
{
    public class GameResultArgs : EventArgs
    {
        /// <summary>Game Result</summary>
        public GameResult GameResult;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result">     GameResult</param>
        public GameResultArgs(GameResult result) => GameResult = result;
    }
}
