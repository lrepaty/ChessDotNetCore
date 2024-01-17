using ChessDotNetCore.Pieces;
using System.Diagnostics;
using System.Linq.Expressions;

namespace ChessDotNetCore.PgnParsing
{
    /// <summary>
    /// Class implementing the parsing of a PGN document. PGN is a standard way of recording chess games.
    /// </summary>
    public class PgnParser
    {
        #region Types
        /// <summary>
        /// Parsing Phase
        /// </summary>
        public enum ParsingPhase: byte
        {
            /// <summary>No phase set yet</summary>
            None         = 0,
            /// <summary>Openning a file</summary>
            OpeningFile  = 1,
            /// <summary>Reading the file content into memory</summary>
            ReadingFile  = 2,
            /// <summary>Raw parsing the PGN file</summary>
            RawParsing   = 3,
            /// <summary>Creating the book</summary>
            CreatingBook = 10,
            /// <summary>Processing is finished</summary>
            Finished     = 255
        }
        #endregion

        #region Members
        /// <summary>true to cancel the parsing job</summary>
        protected static bool               m_isJobCancelled;
        /// <summary>ChessGame use to play as we decode</summary>
        protected readonly ChessGame?       m_chessGame;
        /// <summary>true to diagnose the parser. This generate exception when a move cannot be resolved</summary>
        private readonly bool               m_isDiagnoseOn;
        /// <summary>PGN Lexical Analyser</summary>
        protected PgnLexical?               m_pgnLexical;
        /// <summary>Maximum depth of the moves</summary>
        protected static int                m_maxDepth = int.MaxValue / 2;
        public static event EventHandler?   OnError;
        #endregion

        #region Ctor
        /// <summary>
        /// Class Ctor
        /// </summary>
        /// <param name="isDiagnoseOn"> true to diagnose the parser</param>
        public PgnParser(bool isDiagnoseOn)
        {
            m_chessGame     = new();
            m_isDiagnoseOn  = isDiagnoseOn;
            m_pgnLexical    = null;
        }

        /// <summary>
        /// Class Ctor
        /// </summary>
        /// <param name="chessGame"> ChessGame to use</param>
        public PgnParser(ChessGame chessGame)
        {
            m_chessGame     = chessGame;
            m_isDiagnoseOn  = false;
            m_pgnLexical    = null;
        }
        #endregion

        #region Properties
        /// <summary>
        /// PGN Lexical Analyser
        /// </summary>
        public PgnLexical? PgnLexical => m_pgnLexical;

        /// <summary>
        /// true if job has been cancelled
        /// </summary>
        public static bool IsJobCancelled => m_isJobCancelled;

        /// <summary>
        /// Sorted List of Book.BookKey and Book.BookValue
        /// </summary>
        private static SortedList<Book.BookKey, Book.BookValue>? SoredListBookKeyList { get; set; }
        public DateTime MaxUpdateDatum { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize the parser using the content of a PGN file
        /// </summary>
        /// <param name="fileName"> File name</param>
        /// <param name="startpos"> Starting position</param>
        /// <returns>true if succeed, false if failed</returns>
        public bool InitFromFile(string fileName, long startpos = 0)
        {
            bool retVal;

            m_pgnLexical ??= new();
            retVal         = m_pgnLexical.InitFromFile(fileName, startpos);
            return retVal;
        }

        /// <summary>
        /// Initialize the parser using a PGN text
        /// </summary>
        /// <param name="text">  PGN Text</param>
        public void InitFromString(string text)
        {
            m_pgnLexical ??= new();
            m_pgnLexical.InitFromString(text);
        }

        /// <summary>
        /// Initialize from a PGN buffer object
        /// </summary>
        /// <param name="pgnLexical">    PGN Lexical Analyser</param>
        public void InitFromPgnBuffer(PgnLexical pgnLexical) => m_pgnLexical = pgnLexical;

        /// <summary>
        /// Return the code of the current game
        /// </summary>
        /// <returns>
        /// Current game
        /// </returns>
        protected string GetCodeInError(long startPos, int length) => m_pgnLexical!.GetStringAtPos(startPos, length)!;

        /// <summary>
        /// Return the code of the current game
        /// </summary>
        /// <param name="tok">  Token</param>
        /// <returns>
        /// Current game
        /// </returns>
        protected string GetCodeInError(PgnLexical.Token tok) => m_pgnLexical!.GetStringAtPos(tok.StartPos, tok.Size)!;

        /// <summary>
        /// Return the code of the current game
        /// </summary>
        /// <param name="pgnGame">    PGN game</param>
        /// <returns>
        /// Current game
        /// </returns>
        protected string GetCodeInError(PgnGame pgnGame) => GetCodeInError(pgnGame.StartingPos, pgnGame.Length);

        /// <summary>
        /// Callback for
        /// </summary>
        /// <param name="cookie">           Callback cookie</param>
        /// <param name="phase">            Parsing phase OpeningFile,ReadingFile,RawParsing,AnalysingMoves</param>
        /// <param name="fileIndex">        File index</param>
        /// <param name="fileCount">        Number of files to parse</param>
        /// <param name="fileName">         File name</param>
        /// <param name="currentBufferPos"> Current bufferPosition in MB</param>
        /// <param name="textSizeInMb">     Text Size in MB</param>
        public delegate void DelProgressCallBack(object? cookie, ParsingPhase phase, int fileIndex, int fileCount, string? fileName, int currentBufferPos, int textSizeInMb);


        /// <summary>
        /// Add bookkey in sorted list
        /// </summary>
        /// <param name="move">             Move position</param>
        /// <param name="gameResult">       Game result</param>
        private void AddBookKey(Move move, string gameResult)
        {
            Book.BookKey bookKey;
            Book.BookValue bookValue;

            bookKey = new();
            bookKey.Key = m_chessGame!.CurrentZobristKey;
            bookKey.Move = ChessUtilities.MoveToBookMove(move);
            lock (SoredListBookKeyList!)
            {
                if (SoredListBookKeyList.IndexOfKey(bookKey) == -1)
                    bookValue = new Book.BookValue();
                else
                    bookValue = SoredListBookKeyList[bookKey];
                if ((m_chessGame.CurrentPlayer == Player.White && gameResult == "1-0") ||
                    (m_chessGame.CurrentPlayer == Player.Black && gameResult == "0-1"))
                    bookValue.Wins++;
                else if (gameResult == "1/2-1/2")
                    bookValue.Draws++;
                else
                    bookValue.Lost++;
                if (SoredListBookKeyList.IndexOfKey(bookKey) > -1)
                    SoredListBookKeyList[bookKey] = bookValue;
                else
                    SoredListBookKeyList.Add(bookKey, bookValue);
            }
        }

        /// <summary>
        /// Apply a PgnMove to the ChessGame
        /// </summary>
        /// <param name="pgnGame">          PgnGame</param>
        /// <param name="pgnMove">          PgnMove</param>
        /// <param name="prevSanMove">      Previous San from PgnMove</param>
        public void ApplyPgnMoveToChessGame(PgnGame pgnGame, PgnMove pgnMove, string? prevSanMove, out Move move, bool makeMove = true)
        {
            Player player = m_chessGame!.CurrentPlayer;
            Position? origin = null;
            Position? destination = null;
            Piece? piece = null;
            char? promotion = null;
            bool continueNormalExecution = true;
            string pureMove, errorStr;
            int pos;
            try
            {
                pureMove = pgnMove.SanMove!.Replace("+", "").Replace("#", "").Replace("x", "");
                pos = pureMove.Length - 1;
                if (m_chessGame.NeedsPgnMoveSpecialTreatment(pureMove!, player))
                {
                    continueNormalExecution = m_chessGame.HandleSpecialPgnMove(pureMove!, player);
                }

                if (!continueNormalExecution) throw new PgnParserException();

                if (pureMove!.Length > 2)
                {
                    string possiblePromotionPiece = pureMove.Substring(pureMove.Length - 2).ToUpperInvariant();
                    if (possiblePromotionPiece[0] == '=')
                    {
                        promotion = possiblePromotionPiece[1];
                        pureMove = pureMove.Remove(pureMove.Length - 2, 2);
                    }
                }

                if (pureMove.ToUpperInvariant() == "O-O" || pureMove.ToUpperInvariant() == "0-0")
                {
                    byte r = (byte)(player == Player.White ? 1 : 8);
                    origin = new Position(Line.e, r);
                    destination = new Position(Line.g, r);
                    piece = new King(player);
                }
                else if (pureMove.ToUpperInvariant() == "O-O-O" || pureMove.ToUpperInvariant() == "0-0-0")
                {
                    byte r = (byte)(player == Player.White ? 1 : 8);
                    origin = new Position(Line.e, r);
                    destination = new Position(Line.c, r);
                    piece = new King(player);
                }
                else
                {
                    if (piece == null)
                    {
                        piece = m_chessGame.MapPgnCharToPiece(pureMove[0], player);
                    }
                    if (!(piece is Pawn))
                    {
                        pureMove = pureMove.Remove(0, 1);
                    }
                }
                int rankRestriction = -1;
                Line fileRestriction = Line.None;
                if (destination == null)
                {
                    if (pureMove.Length == 2)
                    {
                        destination = new Position(pureMove);
                    }
                    else if (pureMove.Length == 3)
                    {
                        if (char.IsDigit(pureMove[0]))
                        {
                            rankRestriction = int.Parse(pureMove[0].ToString());
                        }
                        else
                        {
                            bool recognized = Enum.TryParse<Line>(pureMove[0].ToString(), true, out fileRestriction);
                            if (!recognized)
                            {
                                throw new PgnParserException();
                            }
                        }
                        destination = new Position(pureMove.Remove(0, 1));
                    }
                    else if (pureMove.Length == 4)
                    {
                        origin = new Position(pureMove.Substring(0, 2));
                        destination = new Position(pureMove.Substring(2, 2));
                    }
                    else
                    {
                        throw new PgnParserException();
                    }
                }

                if (origin != null)
                {
                    move = new Move(origin, destination, player, promotion);
                    if (m_chessGame.IsValidMove(move))
                    {
                        if (SoredListBookKeyList != null)
                            AddBookKey(move, pgnGame.GameResult!);
                        if (makeMove)
                            m_chessGame.MakeMove(move, true);
                    }
                    else
                    {
                        throw new PgnParserException();
                    }
                }
                else
                {
                    Piece?[][]? board = m_chessGame.GetBoard();
                    List<Move> validMoves = [];
                    for (byte r = 0; r < m_chessGame.BoardHeight; r++)
                    {
                        if (rankRestriction != -1 && r != 8 - rankRestriction) continue;
                        for (byte f = 0; f < m_chessGame.BoardWidth; f++)
                        {
                            if (fileRestriction != Line.None && f != (int)fileRestriction) continue;
                            if (board![r][f] != piece) continue;
                            var m = new Move(new Position((Line)f, (byte)(8 - r)), destination, player, promotion);
                            if (m_chessGame.IsValidMove(m))
                            {
                                validMoves.Add(m);
                            }
                        }
                    }
                    if (validMoves.Count == 0) throw new PgnParserException();
                    if (validMoves.Count > 1) throw new PgnParserException();
                    if (SoredListBookKeyList != null)
                        AddBookKey(validMoves[0], pgnGame.GameResult!);
                    move = validMoves[0];
                    if (makeMove)
                        m_chessGame.MakeMove(move, true, out Piece? captured, pgnMove);
                }
            }
            catch
            {
                errorStr = $" - {pgnMove.SanMove}" + (pgnMove.Comment == "" ? (prevSanMove == null ? "" : $" before Move {prevSanMove}") : $" Comment {pgnMove.Comment}");
                if (m_isDiagnoseOn)
                {
                    Trace.WriteLine(errorStr, GetCodeInError(pgnGame));
                }
                throw new PgnParserException(errorStr, GetCodeInError(pgnGame));
            }
        }

        /// <summary>
        /// Apply a PgnMoves to the ChessGame
        /// </summary>
        /// <param name="pgnGame">        PGN game</param>
        public void ApplyPgnMovesToChessGame(PgnGame pgnGame)
        {
            try
            {
                int i;
                string? prevSanMove = null;
                i = 0;
                foreach (PgnMove pgnMove in pgnGame.PgnMoves!)
                {
                    if (i > 0)
                    {
                        prevSanMove = pgnGame.PgnMoves[i - 1].SanMove;
                    }
                    ApplyPgnMoveToChessGame(pgnGame, pgnMove, prevSanMove, out Move move);
                    if (pgnGame.ChessMoves != null)
                    {
                        pgnGame.ChessMoves!.Add(move);
                    }
                    pgnGame.PgnMoves[i].SanMove = m_chessGame!.LastMove!.SAN;
                    i++;
                }
            }
            catch(PgnParserException ex)
            {
                ex.PgnMoveList = [..pgnGame.PgnMoves!];
                throw;
            }
        }

        /// <summary>
        /// Parse PGN moves
        /// </summary>
        /// <param name="pgnGame">          Pgn Game</param>
        /// <param name="isBadMoveFound">   true if a bad move has been found</param>
        /// <param name="errTxt">           Error if any</param>
        /// <remarks>
        ///     movetext-section        ::= element-sequence game-termination
        ///     element-sequence        ::= {element}
        ///     element                 ::= move-number-indication | SAN-move | numeric-annotation-glyph
        ///     move-number-indication  ::= Integer {'.'}
        ///     recursive-variation     ::= '(' element-sequence ')'
        ///     game-termination        ::= '1-0' | '0-1' | '1/2-1/2' | '*'
        ///  </remarks>
        protected void ParseMoves(PgnGame pgnGame, out bool isBadMoveFound, out string? errTxt)
        {
            int plyIndex;
            PgnLexical.Token tok;
            PgnMove? pgnMove;
            string sanMove;

            errTxt = null;
            plyIndex = 2;
            tok = m_pgnLexical!.GetNextToken();
            isBadMoveFound = false;
            pgnMove = new()
            {
                SanMove = null,
                Annotation = "",
                NAG = 0,
                Comment = "",
            };
            sanMove = "";
            switch (tok.Type)
            {
                case PgnLexical.TokenType.Integer:
                case PgnLexical.TokenType.Symbol:
                case PgnLexical.TokenType.Nag:
                case PgnLexical.TokenType.Comment:
                    while (tok.Type != PgnLexical.TokenType.Eof && tok.Type != PgnLexical.TokenType.Termination)
                    {
                        switch (tok.Type)
                        {
                            case PgnLexical.TokenType.Integer:
                                break;
                            case PgnLexical.TokenType.Dot:
                                break;
                            case PgnLexical.TokenType.Symbol:
                                if (pgnGame.PgnMoves != null)
                                {
                                    if (pgnMove.SanMove != null && (pgnGame.PgnMoves.Count < m_maxDepth * 2))
                                    {
                                        pgnGame.PgnMoves.Add(pgnMove);
                                    }
                                    pgnMove = new()
                                    {
                                        SanMove = tok.StrValue,
                                        Annotation = "",
                                        NAG = 0,
                                        Comment = "",
                                    };
                                }
                                sanMove = tok.StrValue;
                                plyIndex++;
                                break;
                            case PgnLexical.TokenType.Dash:
                                errTxt = tok.StrValue;
                                break;
                            case PgnLexical.TokenType.Annotation:
                                pgnMove.Annotation = tok.StrValue;
                                break;
                            case PgnLexical.TokenType.Nag:
                                pgnMove.NAG = tok.IntValue;
                                break;
                            case PgnLexical.TokenType.Comment:
                                pgnMove.Comment = tok.StrValue;
                                break;
                            default:
                                if (tok.Type == PgnLexical.TokenType.OpenSBracket)
                                {
                                    m_pgnLexical.SelectPrevChr();
                                }
                                throw new PgnParserException($"Invalid Move {tok.StrValue}" + (pgnMove.Comment == "" ? (sanMove == null ? "" : $" before Move {sanMove}") : $" Comment {pgnMove.Comment}"));
                        }
                        tok = m_pgnLexical.GetNextToken();
                    }
                    if (tok.Type != PgnLexical.TokenType.Eof)
                    {
                        m_pgnLexical.AssumeToken(PgnLexical.TokenType.Termination, tok);
                    }
                    break;
                case PgnLexical.TokenType.Termination:
                    break;
                default:
                    m_pgnLexical.PushToken(tok);
                    break;
            }
            if (pgnGame.PgnMoves != null && pgnMove.SanMove != null && (pgnGame.PgnMoves.Count < m_maxDepth * 2))
            {
                pgnGame.PgnMoves.Add(pgnMove);
            }
            if (tok.Type != PgnLexical.TokenType.Eof)
            {
                m_pgnLexical.AssumeToken(PgnLexical.TokenType.Termination, tok);
            }
        }

        /// <summary>
        /// Parse PGN attributes
        /// </summary>
        /// <param name="attrs">    Returned list of attributes for this game</param>
        /// <returns>
        /// Attribute dictionary
        /// </returns>
        /// <remarks>
        ///     tag-section     ::= {tag-pair}
        ///     tag-pair        ::= '[' tag-name tag-value ']'
        ///     tag-name        ::= identifier
        ///     tag-value       ::= string
        /// </remarks>
        protected Dictionary<string, string>? ParseAttrs(Dictionary<string, string>? attrs)
        {
            Dictionary<string, string>? retVal;
            PgnLexical.Token            tok;
            PgnLexical.Token            tokName;
            PgnLexical.Token            tokValue;

            retVal  = attrs;
            tok     = m_pgnLexical!.GetNextToken();
            while (tok.Type == PgnLexical.TokenType.OpenSBracket)
            {
                tokName  = m_pgnLexical.AssumeToken(PgnLexical.TokenType.Symbol);
                tokValue = m_pgnLexical.AssumeToken(PgnLexical.TokenType.String);
                m_pgnLexical.AssumeToken(PgnLexical.TokenType.CloseSBracket);
                retVal?.Add(tokName.StrValue, tokValue.StrValue);
                tok = m_pgnLexical.GetNextToken();
            }
            m_pgnLexical.PushToken(tok);
            return retVal;
        }

        /// <summary>
        /// Parse a PGN text
        /// </summary>
        /// <param name="isAttrList">     Game to be filled with attributes</param>
        /// <param name="isMoveList">     Game to be filled with moves</param>
        /// <param name="isBadMoveFound"> true if a bad move has been found</param>
        /// <param name="errTxt">         Error if any</param>
        /// <returns>
        /// PGN game or null if none found
        /// </returns>
        /// <remarks>
        ///     PGN-game        ::= tag-section movetext-section
        ///     tag-section     ::= tag-pair
        /// </remarks>
        protected virtual PgnGame? ParsePgn(bool isAttrList, bool isMoveList, out bool isBadMoveFound, out string? errTxt)
        {
            PgnGame? pgnGame;
            PgnLexical.Token tok;
            string game;

            errTxt = null;
            isBadMoveFound = false;
            tok = m_pgnLexical!.PeekToken();
            if (tok.Type == PgnLexical.TokenType.Eof)
            {
                pgnGame = null;
            }
            else
            {
                pgnGame = new PgnGame(isAttrList, isMoveList)
                {
                    StartingPos = tok.StartPos
                };
                try
                {
                    game = GetCodeInError(pgnGame.StartingPos, 1);
                    if (game[0] != '[')
                    {
                        tok = m_pgnLexical.GetNextToken();
                        throw new Exception($"Oops! Game doesn't begin with '[' Pos={pgnGame.StartingPos}");
                    }
                    if (pgnGame.Attrs != null)
                    {
                        pgnGame.Attrs.Clear();
                    }
                    if (pgnGame.PgnMoves != null)
                    {
                        pgnGame.PgnMoves.Clear();
                    }
                    pgnGame.Attrs = ParseAttrs(pgnGame.Attrs);
                }
                catch (Exception ex)
                {
                    isBadMoveFound = true;
                    m_pgnLexical.SelectNextEvent();
                    tok = m_pgnLexical.PeekToken();
                    pgnGame.Length = (int)(tok.StartPos - pgnGame.StartingPos);
                    game = GetCodeInError(pgnGame.StartingPos, pgnGame.Length);
                    errTxt = $"PGN invalid - {ex.Message}\r\n{game}";
                    OnError?.Invoke(errTxt, new());
                    return pgnGame;
                }
                try
                {
                    m_pgnLexical.GameResult = pgnGame.GameResult;
                    ParseMoves(pgnGame, out isBadMoveFound, out errTxt);
                    tok = m_pgnLexical.PeekToken();
                    pgnGame.Length = (int)(tok.StartPos - pgnGame.StartingPos);
                }
                catch (Exception ex)
                {
                    isBadMoveFound = true;
                    m_pgnLexical.SelectNextEvent();
                    tok = m_pgnLexical.PeekToken();
                    pgnGame.Length = (int)(tok.StartPos - pgnGame.StartingPos);
                    game = GetCodeInError(pgnGame.StartingPos, pgnGame.Length);
                    errTxt = $"PGN contains a bad move - {ex.Message}\r\n{game}";
                    OnError?.Invoke(errTxt, new());
                }
            }
            return pgnGame;
        }

        /// <summary>
        /// Analyze the PGN games to find the non-ambiguous move list
        /// </summary>
        /// <param name="noMoveList">          true to ignore the move list</param>
        /// <param name="pgnGame">             Game being analyzed</param>
        /// <param name="skippedCount">        Number of games skipped</param>
        /// <param name="truncatedCount">      Number of games truncated</param>
        /// <param name="errTxt">              Error if any</param>
        /// <returns>
        /// false if invalid board
        /// </returns>
        public virtual bool AnalyzePgn(bool             noMoveList,
                                       PgnGame          pgnGame,
                                       ref int          skippedCount,
                                       ref int          truncatedCount,
                                       out string?      errTxt)
        {
            bool                    retVal = true;
            string?                 fen;
            List<PgnMove>?          pgnMoveList;
            errTxt = null;
            if (m_pgnLexical == null)
            {
                throw new MethodAccessException("Must initialize the parser first");
            }
            fen = pgnGame.Fen;
            if (!noMoveList)
            {
                pgnGame.ChessMoves = new List<Move>(256);
            }
            pgnMoveList = pgnGame.PgnMoves;
            try
            {
                try
                {
                    if (fen == null)
                        m_chessGame!.Initialize();
                    else
                        m_chessGame!.Initialize(fen);
                }
                catch (Exception ex)
                {
                    throw new PgnParserException(ex.Message, GetCodeInError(pgnGame));
                }
                m_chessGame.Attrs = pgnGame.Attrs;
                if (pgnMoveList! == null && pgnMoveList!.Count == 0)
                {
                    skippedCount++;
                }
                ApplyPgnMovesToChessGame(pgnGame);
            }
            catch (PgnParserException ex)
            {
                errTxt = $"PGN contains a bad move - {ex.Message}\r\n{ex.CodeInError}";
                OnError?.Invoke(errTxt, new());
                truncatedCount++;
                retVal = false;
            }
            return retVal;
        }


        /// <summary>
        /// Parse a single PGN/FEN game
        /// </summary>
        /// <param name="noMoveList">          true to ignore the move list</param>
        /// <param name="skipCount">           Number of games skiped</param>
        /// <param name="truncatedCount">      Number of games truncated</param>
        /// <param name="pgnGame">             Returned PGN game</param>
        /// <param name="errTxt">              Error if any</param>
        /// <returns>
        /// false if the board specified by FEN is invalid.
        /// </returns>
        public bool ParseSingle(bool noMoveList,
                                out int skipCount,
                                out int truncatedCount,
                                out PgnGame? pgnGame,
                                out string? errTxt)
        {
            bool retVal;
            if (m_pgnLexical == null)
            {
                throw new MethodAccessException("Must initialize the parser first");
            }
            skipCount = 0;
            truncatedCount = 0;
            pgnGame = ParsePgn(isAttrList: true, isMoveList: true, out bool isBadMoveFound, out errTxt);
            if (isBadMoveFound)
            {
                skipCount++;
                retVal = false;
            }
            else
            {
                if (pgnGame != null)
                {

                    retVal = AnalyzePgn(noMoveList,
                                        pgnGame,
                                        ref skipCount,
                                        ref truncatedCount,
                                        out errTxt);
                }
                else
                {
                    pgnGame = new PgnGame(createAttrList: true, createMoveList: true);
                    retVal = true;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Gets the list of all raw PGN in the specified text
        /// </summary>
        /// <param name="getAttrList">  true to create attributes list</param>
        /// <param name="getMoveList">  true to create move list</param>
        /// <param name="skippedCount"> Number of game which has been skipped because of bad move</param>
        /// <param name="callback">     Callback</param>
        /// <param name="cookie">       Cookie for callback</param>
        public List<PgnGame> GetAllRawPgn(bool getAttrList, bool getMoveList, out int skippedCount, DelProgressCallBack? callback, object? cookie)
        {
            List<PgnGame> retVal;
            PgnGame?      pgnGame;
            int           bufferCount;
            int           bufferPos;
            int           oldBufferPos;

            skippedCount = 0;
            if (m_pgnLexical == null)
            {
                throw new MethodAccessException("Must initialize the parser first");
            }
            m_isJobCancelled = false;
            retVal           = new List<PgnGame>(1000000);
            bufferCount      = m_pgnLexical.BufferCount;
            bufferPos        = 0;
            oldBufferPos     = 0;
            callback?.Invoke(cookie, ParsingPhase.RawParsing, fileIndex: 0, fileCount: 0, fileName: null, currentBufferPos: 0, bufferCount);
            while (!m_isJobCancelled && (pgnGame = ParsePgn(getAttrList, getMoveList, out bool isBadMoveFound, out string? errTxt)) != null && !m_isJobCancelled)
            {
                if (isBadMoveFound)
                {
                    skippedCount++;
                }
                else
                {
                    if (pgnGame.PgnMoves == null || pgnGame.PgnMoves.Count != 0)
                    {
                        retVal.Add(pgnGame);
                    }
                    if (callback != null)
                    {
                        bufferPos  = m_pgnLexical.CurrentBufferPos;
                        if (bufferPos != oldBufferPos)
                        {
                            oldBufferPos = bufferPos;
                            if ((bufferPos % 100) == 0)
                            {
                                callback(cookie, ParsingPhase.RawParsing, 0, 0, null, bufferPos, bufferCount);
                            }
                        }
                    }
                }
            }
            callback?.Invoke(cookie, ParsingPhase.RawParsing, fileIndex: 0, fileCount: 0, fileName: null, m_pgnLexical.CurrentBufferPos, bufferCount);
            return retVal;
        }

        /// <summary>
        /// Gets the list of all raw PGN in the specified text
        /// </summary>
        /// <param name="getMoveList">  true to create move list</param>
        /// <param name="getAttrList">  true to create attributes list</param>
        /// <param name="skippedCount"> Number of games skipped because of bad moves</param>
        public List<PgnGame> GetAllRawPgn(bool getAttrList, bool getMoveList, out int skippedCount) => GetAllRawPgn(getAttrList, getMoveList, out skippedCount, callback: null, cookie: null);

        /// <summary>
        /// Analyze the games in the list in multiple threads
        /// </summary>
        /// <param name="pgnGames">       List of games</param>
        /// <param name="skippedCount">   Number of games skipped</param>
        /// <param name="truncatedCount"> Number of games truncated count</param>
        /// <param name="threadCount">    Thread count</param>
        /// <returns>
        /// List of analyzed games
        /// </returns>
        private List<PgnGame> AnalyzeInParallel(List<PgnGame> pgnGames, ref int skippedCount, ref int truncatedCount, int threadCount)
        {
            int     localSkippedCount       = 0;
            int     localTruncatedCount     = 0;
            List<PgnGame> analzedpgnGames   = [];

            Parallel.For(0, threadCount, (threadIndex) =>
            {
                PgnGame   pgnGame;
                int       index;
                int       start;
                int       gamePerThread;
                int       skippedCount = 0;
                int       truncatedCount = 0;
                PgnParser parser;

                parser        = new PgnParser(false);
                parser.InitFromPgnBuffer(m_pgnLexical!);
                gamePerThread = (pgnGames.Count + threadCount - 1) / threadCount;
                start         = threadIndex * gamePerThread;
                index         = start;
                while (index < start + gamePerThread && index < pgnGames.Count && !m_isJobCancelled)
                {
                    pgnGame = pgnGames[index];
                    if (parser.AnalyzePgn(false,
                                          pgnGame,
                                          ref skippedCount,
                                          ref truncatedCount,
                                          out string? tmpErrTxt))
                    {
                        lock (analzedpgnGames)
                        {
                            analzedpgnGames.Add(pgnGame);
                        }
                    }
                    lock (analzedpgnGames)
                    {
                        localSkippedCount += skippedCount;
                        localTruncatedCount += truncatedCount;
                    }
                    skippedCount = 0;
                    truncatedCount = 0;
                    index++;
                }
            });
            skippedCount += localSkippedCount;
            truncatedCount += localTruncatedCount;
            return analzedpgnGames;
        }

        /// <summary>
        /// Analyze the games in the list in multiple threads
        /// </summary>
        /// <param name="pgnGameList">      List of Pgn moves</param>
        /// <param name="callback">         Delegate callback (can be null)</param>
        /// <param name="cookie">           Cookie for the callback</param>
        /// <param name="threadCount">      Number of threads</param>
        /// <param name="oldBufferPos">     Old Buffer position</param>
        /// <param name="textSizeInMb">     Text Siye in Mb</param>
        /// <param name="skippedCount">     Number of games skipped</param>
        /// <param name="truncatedCount">   Number of games truncated</param>
        /// <returns>
        /// List of analyzed games
        /// </returns>
        private List<PgnGame> AnalyzeParallel(List<PgnGame> pgnGameList, DelProgressCallBack? callback, object? cookie,
                                    int threadCount, int oldBufferPos, int textSizeInMb, ref int skippedCount, ref int truncatedCount)
        {
            int bufferPos;
            List<PgnGame> analzedpgnGames = AnalyzeInParallel(pgnGameList, ref skippedCount, ref truncatedCount, threadCount);
            m_pgnLexical!.FlushOldBuffer();
            if (callback != null)
            {
                bufferPos = m_pgnLexical.CurrentBufferPos;
                if (bufferPos != oldBufferPos)
                {
                    callback(cookie, ParsingPhase.RawParsing, fileIndex: 0, fileCount: 0, fileName: null, bufferPos, textSizeInMb);
                }
            }
            return analzedpgnGames;
        }

        /// <summary>
        /// Parse a PGN text file. The move list are returned as a list of array of int. Each int encoding the starting position in the first 8 bits and the ending position in the second 8 bits
        /// </summary>
        /// <param name="pgnGameList">    List of Pgn moves</param>
        /// <param name="callback">       Delegate callback (can be null)</param>
        /// <param name="cookie">         Cookie for the callback</param>
        /// <param name="procesCount">    Number of games processed</param>
        /// <param name="skippedCount">   Number of games skipped</param>
        /// <param name="truncatedCount"> Number of games truncated</param>
        /// <param name="maxDepth">       Maximum depth of the moves</param>
        public bool ParseMultiple(out List<PgnGame> pgnGameList,
                                  DelProgressCallBack? callback,
                                  object? cookie,
                                  out int procesCount,
                                  out int skippedCount,
                                  out int truncatedCount,
                                  int maxDepth = 0)

        {
            bool retVal = true;
            PgnGame? pgnGame;
            int threadCount;
            int bufferCount;
            int bufferPos;
            int oldBufferPos;
            bool isBadMoveFound;
            const int gamePerThread = 16;
            int batchSize;
            int textSizeInMb;
            List<PgnGame> readPgnGameList;

            if (m_pgnLexical == null)
            {
                throw new MethodAccessException("Must initialize the parser first");
            }
            procesCount = 0;
            skippedCount = 0;
            truncatedCount = 0;
            if (maxDepth != 0)
            {
                m_maxDepth = maxDepth;
            }
            pgnGameList = new List<PgnGame>();
            if (m_isJobCancelled)
            {
                retVal = false;
            }
            else
            {
                threadCount = Environment.ProcessorCount;
                bufferCount = m_pgnLexical.BufferCount;
                oldBufferPos = 0;
                textSizeInMb = (int)(m_pgnLexical.TextSize / 1048576);
                callback?.Invoke(cookie, ParsingPhase.RawParsing, fileIndex: 0, fileCount: 0, fileName: null, currentBufferPos: 0, textSizeInMb);
                if (threadCount == 1)
                {
                    while (!m_isJobCancelled && (pgnGame = ParsePgn(isAttrList: true, isMoveList: true, out isBadMoveFound, out string? errTxt)) != null)
                    {
                        procesCount++;
                        if (isBadMoveFound)
                        {
                            skippedCount++;
                        }
                        else if (pgnGame.PgnMoves != null && pgnGame.PgnMoves.Count != 0)
                        {
                            retVal = AnalyzePgn(false,
                                                pgnGame,
                                                ref skippedCount,
                                                ref truncatedCount,
                                                out errTxt);
                            if (retVal)
                            {
                                if (pgnGame.PgnMoves != null)
                                {
                                    pgnGameList.Add(pgnGame);
                                }
                                if (callback != null)
                                {
                                    bufferPos = m_pgnLexical.CurrentBufferPos;
                                    if (bufferPos != oldBufferPos)
                                    {
                                        callback(cookie, ParsingPhase.RawParsing, fileIndex: 0, fileCount: 0, fileName: null, bufferPos, textSizeInMb);
                                        oldBufferPos = bufferPos;
                                    }
                                }
                                m_pgnLexical.FlushOldBuffer();
                            }
                        }
                    }
                }
                else
                {
                    readPgnGameList = [];
                    batchSize = gamePerThread * threadCount;
                    while (!m_isJobCancelled && (pgnGame = ParsePgn(isAttrList: true, isMoveList: true, out isBadMoveFound, out string? errTxt)) != null)
                    {
                        procesCount++;
                        if (isBadMoveFound)
                        {
                            skippedCount++;
                        }
                        else if (pgnGame.PgnMoves != null && pgnGame.PgnMoves.Count != 0)
                        {
                            readPgnGameList.Add(pgnGame);
                            if (readPgnGameList.Count == batchSize)
                            {
                                pgnGameList.AddRange(AnalyzeParallel(readPgnGameList, callback, cookie, threadCount, oldBufferPos, textSizeInMb, ref skippedCount, ref truncatedCount));
                                readPgnGameList.Clear();
                            }
                        }
                    }
                    if (readPgnGameList.Count >= threadCount)
                    {
                        pgnGameList.AddRange(AnalyzeParallel(readPgnGameList, callback, cookie, threadCount, oldBufferPos, textSizeInMb, ref skippedCount, ref truncatedCount));
                        readPgnGameList.Clear();
                    }
                    else
                    {
                        foreach (PgnGame pgnGameTmp in readPgnGameList)
                        {
                            retVal = AnalyzePgn(false,
                                                pgnGameTmp,
                                                ref skippedCount,
                                                ref truncatedCount,
                                                out string? errTxt);
                            if (retVal)
                            {
                                if (pgnGameTmp.PgnMoves != null)
                                {
                                    pgnGameList.Add(pgnGameTmp);
                                }
                            }
                        }
                        readPgnGameList.Clear();
                    }
                }
                callback?.Invoke(cookie, ParsingPhase.RawParsing, fileIndex: 0, fileCount: 0, fileName: null, bufferCount, textSizeInMb);
            }
            m_maxDepth = int.MaxValue / 2;
            return retVal;
        }

        /// <summary>
        /// Parse a series of PGN games
        /// </summary>
        /// <param name="fileNames">                Array of file name</param>
        /// <param name="maxDepth">                 Maximum depth of the moves</param>
        /// <param name="callback">                 Delegate callback (can be null)</param>
        /// <param name="cookie">                   Cookie for the callback</param>
        /// <param name="soredListBookKeyList">     Sorted List of Book.BookKey and Book.BookValue</param>
        /// <param name="totalProcessed">           Number of games processed</param>
        /// <param name="totalSkipped">             Number of games skipped because of error</param>
        /// <param name="totalTruncated">           Number of games truncated</param>
        /// <param name="errTxt">                   Returned error if return value is false</param>
        /// <returns>true if succeed, false if error</returns>
        public static bool ExtractBookKeyListFromMultipleFiles(string[] fileNames, int maxDepth, DelProgressCallBack? callback, object? cookie, out SortedList<Book.BookKey, Book.BookValue> soredListBookKeyList,
            out int totalProcessed, out int totalSkipped, out int totalTruncated, out string? errTxt)
        {
            bool retVal = true;
            int fileIndex;
            string fileName;
            PgnParser parser;

            m_isJobCancelled = false;
            totalProcessed = 0;
            totalSkipped = 0;
            totalTruncated = 0;
            errTxt = null;
            fileIndex = 0;
            SoredListBookKeyList = new SortedList<Book.BookKey, Book.BookValue>(1000000);
            while (fileIndex < fileNames.Length && errTxt == null && !m_isJobCancelled)
            {
                fileName = fileNames[fileIndex++];
                callback?.Invoke(cookie, ParsingPhase.OpeningFile, fileIndex, fileNames.Length, fileName, currentBufferPos: 0, textSizeInMb: 0);
                parser = new PgnParser(false);
                callback?.Invoke(cookie, ParsingPhase.ReadingFile, fileIndex, fileNames.Length, fileName, currentBufferPos: 0, textSizeInMb: 0);
                try
                {
                    if (parser.InitFromFile(fileName))
                    {
                        retVal = parser.ParseMultiple(out List<PgnGame> pgnGameList, callback, cookie, out int processedCount, out int skipCount, out int truncatedCount, maxDepth);
                        if (retVal)
                        {
                            totalProcessed += processedCount;
                            totalSkipped += skipCount;
                            totalTruncated += truncatedCount;
                        }
                    }
                }
                catch
                {
                }
            }
            soredListBookKeyList = SoredListBookKeyList;
            if (errTxt == null && m_isJobCancelled)
            {
                errTxt = "Cancelled by the user";
            }
            retVal = (errTxt == null);
            return retVal;
        }

        /// <summary>
        /// Call to cancel the parsing job
        /// </summary>
        public static void CancelParsingJob() => m_isJobCancelled = true;
        #endregion

    } // Class PgnParser
} // Namespace
