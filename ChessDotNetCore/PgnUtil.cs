using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using ChessDotNetCore.PgnParsing;

namespace ChessDotNetCore
{
    /// <summary>
    /// Utility class to help handling PGN files. Help filtering PGN files or creating one from an existing board
    /// </summary>
    public static class PgnUtil
    {
        #region Inner Class
        /// <summary>Information use to filter a PGN file</summary>
        public class FilterClause
        {
            /// <summary>All ELO rating included if true</summary>
            public bool IsAllRanges { get; set; }
            /// <summary>Includes unrated games if true</summary>
            public bool IncludesUnrated { get; set; }
            /// <summary>If not all ELO rating included, hash of all ELO which must be included. Each value represent a range (value, value+99)</summary>
            public Dictionary<int, int>? HashRanges { get; set; }
            /// <summary>All players included if true</summary>
            public bool IncludeAllPlayers { get; set; }
            /// <summary>Hash of all players to include if not all included</summary>
            public Dictionary<string, string?>? HashPlayerList { get; set; }
            /// <summary>Includes all ending if true</summary>
            public bool IncludeAllEnding { get; set; }
            /// <summary>true to include game winned by white player</summary>
            public bool IncludeWhiteWinningEnding { get; set; }
            /// <summary>true to include game winned by black player</summary>
            public bool IncludeBlackWinningEnding { get; set; }
            /// <summary>true to include draws game </summary>
            public bool IncludeDrawEnding { get; set; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Open an file for reading
        /// </summary>
        /// <param name="inpFileName"> File name to open</param>
        /// <returns>
        /// Stream or null if unable to open the file.
        /// </returns>
        public static Stream? OpenInpFile(string inpFileName, out string? errTxt)
        {
            Stream? retVal;
            errTxt = null;
            try
            {
                retVal = File.OpenRead(inpFileName);
            }
            catch (Exception)
            {
                errTxt = $"Unable to open the file - {inpFileName}";
                retVal = null;
            }
            return retVal;
        }

        /// <summary>
        /// Creates a new file
        /// </summary>
        /// <param name="outFileName"> Name of the file to create</param>
        /// <returns>
        /// Stream or null if unable to create the file.
        /// </returns>
        public static StreamWriter? CreateOutFile(string outFileName)
        {
            StreamWriter? retVal;
            Stream streamOut;
            try
            {
                streamOut = File.Create(outFileName);
                retVal = new StreamWriter(streamOut, Encoding.UTF8);
            }
            catch (Exception)
            {
                throw new Exception($"Unable to create the file - {outFileName}");
            }
            return retVal;
        }

        /// <summary>
        /// Write a PGN game in the specified output stream
        /// </summary>
        /// <param name="pgnBuffer"> PGN buffer</param>
        /// <param name="writer">    Text writer</param>
        /// <param name="pgnGame">   PGN game</param>
        private static void WritePgn(PgnLexical pgnBuffer, TextWriter writer, PgnGame pgnGame) => writer.Write(pgnBuffer.GetStringAtPos(pgnGame.StartingPos, pgnGame.Length));

        /// <summary>
        /// Gets the information about a PGN game
        /// </summary>
        /// <param name="rawGame">    Raw PGN game</param>
        /// <param name="gameResult"> Result of the game</param>
        /// <param name="gameDate">   Date of the game</param>
        private static void GetPgnGameInfo(PgnGame rawGame,
                                           out string? gameResult,
                                           out string? gameDate)
        {
            if (rawGame.Attrs == null || !rawGame.Attrs.TryGetValue("Result", out gameResult))
            {
                gameResult = null;
            }
            if (rawGame.Attrs == null || !rawGame.Attrs.TryGetValue("Date", out gameDate))
            {
                gameDate = null;
            }
        }

        /// <summary>
        /// Scan the PGN stream to retrieve some informations
        /// </summary>
        /// <param name="pgnGames">      PGN games</param>
        /// <param name="setPlayerList"> Set to be filled with the players list</param>
        /// <param name="minElo">        Minimum ELO found in the games</param>
        /// <param name="maxElo">        Maximum ELO found in the games</param>
        /// <returns>
        /// List of raw games without the move list
        /// </returns>
        public static void FillFilterList(List<PgnGame> pgnGames, HashSet<string> setPlayerList, ref int minElo, ref int maxElo)
        {
            int avgElo;
            string? player;

            foreach (PgnGame pgnGame in pgnGames)
            {
                if (setPlayerList != null)
                {
                    player = pgnGame.WhitePlayerName;
                    if (player != null && !setPlayerList.Contains(player))
                    {
                        setPlayerList.Add(player);
                    }
                    player = pgnGame.BlackPlayerName;
                    if (player != null && !setPlayerList.Contains(player))
                    {
                        setPlayerList.Add(player);
                    }
                }
                if (pgnGame.WhiteElo != -1 && pgnGame.BlackElo != -1)
                {
                    avgElo = (pgnGame.WhiteElo + pgnGame.BlackElo) / 2;
                    if (avgElo > maxElo)
                    {
                        maxElo = avgElo;
                    }
                    if (avgElo < minElo)
                    {
                        minElo = avgElo;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the specified game must be retained accordingly to the specified filter
        /// </summary>
        /// <param name="rawGame">      PGN Raw game</param>
        /// <param name="avgElo">       Game average ELO</param>
        /// <param name="filterClause"> Filter clause</param>
        /// <returns>
        /// true if must be retained
        /// </returns>
        private static bool IsRetained(PgnGame rawGame, int avgElo, FilterClause filterClause)
        {
            bool retVal;

            if (avgElo == -1)
            {
                retVal = filterClause.IncludesUnrated;
            }
            else if (filterClause.IsAllRanges)
            {
                retVal = true;
            }
            else
            {
                avgElo = avgElo / 100 * 100;
                retVal = filterClause.HashRanges!.ContainsKey(avgElo);
            }
            if (retVal)
            {
                if (!filterClause.IncludeAllPlayers || !filterClause.IncludeAllEnding)
                {
                    GetPgnGameInfo(rawGame, out string? gameResult, out _);
                    if (!filterClause.IncludeAllPlayers)
                    {
                        if (!filterClause.HashPlayerList!.ContainsKey(rawGame.BlackPlayerName ?? "") &&
                            !filterClause.HashPlayerList!.ContainsKey(rawGame.WhitePlayerName ?? ""))
                        {
                            retVal = false;
                        }
                    }
                    if (retVal && !filterClause.IncludeAllEnding)
                    {
                        if (gameResult == "1-0")
                        {
                            retVal = filterClause.IncludeWhiteWinningEnding;
                        }
                        else if (gameResult == "0-1")
                        {
                            retVal = filterClause.IncludeBlackWinningEnding;
                        }
                        else if (gameResult == "1/2-1/2")
                        {
                            retVal = filterClause.IncludeDrawEnding;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// Filter the content of the PGN file in the input stream to fill the output stream
        /// </summary>
        /// <param name="pgnParser">    PGN parser</param>
        /// <param name="rawGames">     List of PGN raw games without move list</param>
        /// <param name="textWriter">   Output stream. If null, just run to determine the result count.</param>
        /// <param name="filterClause"> Filter clause</param>
        /// <returns>
        /// Number of resulting games.
        /// </returns>
        public static int FilterPgn(PgnParser pgnParser, List<PgnGame> rawGames, TextWriter? textWriter, FilterClause filterClause)
        {
            int retVal;
            int whiteElo;
            int blackElo;
            int avgElo;

            retVal = 0;
            try
            {
                foreach (PgnGame rawGame in rawGames)
                {
                    whiteElo = rawGame.WhiteElo;
                    blackElo = rawGame.BlackElo;
                    avgElo = (whiteElo != -1 && blackElo != -1) ? (whiteElo + blackElo) / 2 : -1;
                    if (IsRetained(rawGame, avgElo, filterClause))
                    {
                        if (textWriter != null)
                        {
                            WritePgn(pgnParser.PgnLexical!, textWriter, rawGame);
                        }
                        retVal++;
                    }
                }
                textWriter?.Flush();
            }
            catch (Exception exc)
            {
                throw new Exception($"Error writing in destination file.\r\n{exc.Message}");
            }
            return retVal;
        }


        /// <summary>
        /// Generates the Pgn Header from Attrs
        /// </summary>
        /// <param name="attrs">       Attrs
        /// string representation the Header from Attrs
        /// </returns>
        public static string GetPgnHeader(Dictionary<string, string>? attrs)
        {
            StringBuilder strBuilder;
            strBuilder = new StringBuilder(256);
            KeyValuePair<string, string>[] array = [..attrs!];
            for (int i = 0; i < array.Length; i++)
            {
                strBuilder.Append($"[{array[i].Key} \"{array[i].Value}\"]");
                if (i < array.Length - 1)
                {
                    strBuilder.Append("\r\n");
                }
            }
            return strBuilder.ToString();
        }

        /// <summary>
        /// Generates the Pgn Header from ChessGame
        /// </summary>
        /// <param name="chessGame">       Actual chess game (after the move)</param>
        /// <returns>
        /// string representation the Header from ChessGame
        /// </returns>
        public static string GetPgnHeaderFromBoard(ChessGame chessGame, out string result)
        {
            if (chessGame.IsWinner(Player.White))
            {
                result = "1-0";
            }
            else if (chessGame.IsWinner(Player.Black))
            {
                result = "0-1";
            }
            else if (chessGame.IsDraw())
            {
                result = "1/2-1/2";
            }
            else
            {
                result = "*";
            }
            if (!chessGame.Attrs!.ContainsKey("Result"))
            {
                chessGame.Attrs!["Result"] = result;
            }
            if (chessGame.GameResult == GameResult.WhiteTimeout || chessGame.GameResult == GameResult.BlackTimeout)
            {
                chessGame.Attrs!["Termination"] = "time forfeit";
            }
            else
            {
                chessGame.Attrs!["Termination"] = "normal";
            }
            return GetPgnHeader(chessGame.Attrs);
        }

        /// <summary>
        /// Generates the Pgn moves into string
        /// </summary>
        /// <param name="pgnMaveList">     List of PGN moves</param>
        /// <param name="startPlayer">     Start Player</param>
        /// <param name="incudeMoveAttrs"> true to include move attributes</param>
        /// <returns>
        /// string representation the Pgn moves
        /// </returns>
        public static string GetPgnMoves(List<PgnMove> pgnMaveList, Player startPlayer, bool incudeMoveAttrs)
        {
            int moveIndex;
            int increment;
            StringBuilder strBuilder;
            StringBuilder lineStrBuilder;
            int moveCount;

            moveCount = pgnMaveList.Count;
            strBuilder = new StringBuilder(10 * moveCount + 256);
            lineStrBuilder = new StringBuilder(256);
            lineStrBuilder.Length = 0;
            increment = startPlayer == Player.White ? 0 : 1;
            for (moveIndex = 0; moveIndex < moveCount; moveIndex++)
            {
                if (moveIndex > 0)
                {
                    lineStrBuilder.Append(" ");
                }
                if (incudeMoveAttrs && lineStrBuilder.Length > 60)
                {
                    strBuilder.Append(lineStrBuilder);
                    strBuilder.Append("\r\n");
                    lineStrBuilder.Length = 0;
                }
                var move = pgnMaveList[moveIndex];
                if (((moveIndex + increment) & 1) == 0)
                {
                    lineStrBuilder.Append(((moveIndex + 1 + increment) / 2 + 1).ToString(CultureInfo.InvariantCulture));
                    lineStrBuilder.Append(". ");
                }
                lineStrBuilder.Append(move.SanMove);
                if (incudeMoveAttrs)
                {
                    if (move.Annotation != "")
                    {
                        lineStrBuilder.Append($" {move.Annotation}");
                    }
                    if (move.NAG != 0)
                    {
                        lineStrBuilder.Append($" {move.NAG}");
                    }
                    if (move.Comment != "")
                    {
                        lineStrBuilder.Append($" {move.Comment}");
                    }
                }

            }
            strBuilder.Append(lineStrBuilder);
            return strBuilder.ToString();
        }

        /// <summary>
        /// Generates the PGN representation of the chess game
        /// </summary>
        /// <param name="chessGame">       Actual chess game (after the move)</param>
        /// <param name="includeRedoMove"> true to include redo move</param>
        /// <returns>
        /// string representation the Pgn moves
        /// </returns>
        public static string GetPgnMovesFromBoard(ChessGame chessGame, bool includeRedoMove, bool incudeMoveAttrs)
        {
            ReadOnlyCollection<DetailedMove> moves;
            List<PgnMove> pgnMoveList;

            moves = (includeRedoMove) ? chessGame.AllMoves : chessGame.Moves;
            pgnMoveList = moves.Select(m => new PgnMove
            {
                SanMove = m.SAN,
                Annotation = m.Annotation,
                NAG = m.NAG,
                Comment = m.Comment,
            }).ToList();
            return GetPgnMoves(pgnMoveList, chessGame.StartPlayer, incudeMoveAttrs);
        }

        /// <summary>
        /// Generates the PGN representation of the chess game
        /// </summary>
        /// <param name="chessGame">       Actual chess game (after the move)</param>
        /// <param name="includeRedoMove"> true to include redo move</param>
        /// <returns>
        /// PGN representation of the game
        /// </returns>
        public static string GetPgnFromBoard(ChessGame chessGame, bool includeRedoMove, bool incudeMoveAttrs)
        {
            return GetPgnHeaderFromBoard(chessGame, out string result) + "\r\n" + GetPgnMovesFromBoard(chessGame, includeRedoMove, incudeMoveAttrs) + " " + result + "\r\n";
        }
        #endregion
    } // Class PgnUtil
} // Namespace
