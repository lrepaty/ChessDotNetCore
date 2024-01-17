using System.Collections.ObjectModel;
using System.Text;
using ChessDotNetCore.Pieces;
using ChessDotNetCore.PgnParsing;

namespace ChessDotNetCore
{
    /// <summary>
    /// Defines class ChessGame
    /// </summary>
    public class ChessGame
    {
        #region Members
        /// <summary>Draw claimed</summary>
        private bool m_drawn;
        /// <summary>Plyer resigned</summary>
        private Player m_resigned;
        /// <summary>Half move clock</summary>
        private int m_halfMoveClock;
        /// <summary>Full move number</summary>
        private int m_fullMoveNumber;
        /// <summary>Current move position</summary>
        private int m_currentMovePosition;
        /// <summary>50 times without moving a pawn or eating a piece</summary>
        private bool m_fiftyMoves;
        /// <summary>50 times without moving a pawn or eating a piece</summary>
        private bool m_unknown;

        /// <summary>Fen mappings</summary>
        private static Dictionary<char, Piece> m_fenMappings = new()
        {
            { 'K', new King(Player.White) },
            { 'k', new King(Player.Black) },
            { 'Q', new Queen(Player.White) },
            { 'q', new Queen(Player.Black) },
            { 'R', new Rook(Player.White) },
            { 'r', new Rook(Player.Black) },
            { 'B', new Bishop(Player.White) },
            { 'b', new Bishop(Player.Black) },
            { 'N', new Knight(Player.White) },
            { 'n', new Knight(Player.Black) },
            { 'P', new Pawn(Player.White) },
            { 'p', new Pawn(Player.Black) },
        };
        /// <summary>All moves</summary>
        private List<DetailedMove> m_allmoves = [];
        /// <summary>ZobristKeys List</summary>
        private List<ulong> m_zobristKeys = [];
        /// <summary>Chess board</summary>
        protected Piece?[][]? Board;
        /// <summary>White timeout</summary>
        private bool m_whiteTimeout;
        /// <summary>Black timeout</summary>
        private bool m_blackTimeout;
        #endregion

        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public ChessGame()
        {
            Initialize();
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="moves">                Moves</param>
        /// <param name="movesAreValidated">    Moves are already validated</param>
        /// <param name="initializeGameTimer">  Initialize GameTimer</param>
        public ChessGame(IEnumerable<Move> moves, bool movesAreValidated, bool initializeGameTimer = false) : this()
        {
            if (moves == null)
                throw new ArgumentNullException(nameof(moves));
            foreach (Move m in moves)
            {
                if (ApplyMove(m, movesAreValidated) == MoveType.Invalid)
                {
                    throw new ArgumentException("Invalid move passed to ChessGame constructor.");
                }
            }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="fen">                  Fen</param>
        public ChessGame(string fen)
        {
            Initialize(fen);
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="data">                 Game creation data</param>
        public ChessGame(GameCreationData data)
        {
            UseGameCreationData(data);
            SetNewGame();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Draw claimed
        /// </summary>
        public bool DrawClaimed
        {
            get
            {
                return m_drawn;
            }
        }

        /// <summary>
        /// Plyer resigned
        /// </summary>
        public Player Resigned
        {
            get
            {
                return m_resigned;
            }
        }

        /// <summary>
        /// Allowed fen parts length
        /// </summary>
        protected virtual byte AllowedFenPartsLength
        {
            get
            {
                return 6;
            }
        }

        /// <summary>
        /// Use tildes in fen generation
        /// </summary>
        protected virtual bool UseTildesInFenGeneration
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Initial white rook file kingside castling
        /// </summary>
        public Line InitialWhiteRookLineKingsideCastling { get; protected set; }

        /// <summary>
        /// Initial white rook file queensideside castling
        /// </summary>
        public Line InitialWhiteRookLineQueensideCastling { get; protected set; }

        /// <summary>
        /// Initial black rook file kingside castling
        /// </summary>
        public Line InitialBlackRookLineKingsideCastling { get; protected set; }


        /// <summary>
        /// Initial black rook file queensideside castling
        /// </summary>
        public Line InitialBlackRookLineQueensideCastling { get; protected set; }

        /// <summary>
        /// Initial white king file
        /// </summary>
        public Line InitialWhiteKingLine { get; protected set; }

        /// <summary>
        /// Initial black king file
        /// </summary>
        public Line InitialBlackKingLine { get; protected set; }

        /// <summary>
        /// Fen mappings
        /// </summary>
        public static Dictionary<char, Piece> FenMappings
        {
            get
            {
                return m_fenMappings;
            }
        }

        /// <summary>
        /// Start Fen
        /// </summary>
        public string? StartFen { get; private set; }

        /// <summary>
        /// Start player
        /// </summary>
        public Player StartPlayer { get; private set; }

        /// <summary>
        /// Current player
        /// </summary>
        public Player CurrentPlayer { get; protected set; }

        /// <summary>
        /// Half move clock
        /// </summary>
        public int HalfMoveClock
        {
            get
            {
                return m_halfMoveClock;
            }
        }

        /// <summary>
        /// Full move number
        /// </summary>
        public int FullMoveNumber
        {
            get
            {
                return m_fullMoveNumber;
            }
        }

        /// <summary>
        /// Pieces on board
        /// </summary>
        public ReadOnlyCollection<Piece?> PiecesOnBoard
        {
            get
            {
                return new ReadOnlyCollection<Piece?>(Board!.SelectMany(x => x).Where(x => x != null).ToList());
            }
        }

        /// <summary>
        /// Board width
        /// </summary>
        public virtual byte BoardWidth
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// Board height
        /// </summary>
        public virtual byte BoardHeight
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// All moves
        /// </summary>
        public ReadOnlyCollection<DetailedMove> AllMoves
        {
            get
            {
                return new ReadOnlyCollection<DetailedMove>(m_allmoves);
            }
            protected set
            {
                m_allmoves = value.ToList();
            }
        }

        /// <summary>
        /// Aktual moves
        /// </summary>
        public ReadOnlyCollection<DetailedMove> Moves
        {
            get
            {
                List<DetailedMove> _moves = new List<DetailedMove>(m_allmoves);
                if (m_currentMovePosition < _moves.Count - 1)
                    _moves.RemoveRange(m_currentMovePosition + 1, _moves.Count - m_currentMovePosition - 1);
                return new ReadOnlyCollection<DetailedMove>(_moves);
            }
        }

        /// <summary>
        /// Last aktual move
        /// </summary>
        public DetailedMove? LastMove
        {
            get
            {
                return m_currentMovePosition > -1 ? m_allmoves[m_currentMovePosition] : null;
            }
        }

        /// <summary>
        /// Next aktual move
        /// </summary>
        public DetailedMove? NextMove
        {
            get
            {
                return m_allmoves.Count > m_currentMovePosition + 1 ? m_allmoves[m_currentMovePosition + 1] : null;
            }
        }

        /// <summary>
        /// Previous move
        /// </summary>
        public DetailedMove? PreviousMove
        {
            get
            {
                return m_currentMovePosition > 0 ? m_allmoves[m_currentMovePosition - 1] : null;
            }
        }

        /// <summary>
        /// Castling type
        /// </summary>
        public CastlingType CastlingType { get; protected set; }

        /// <summary>
        /// En passant
        /// </summary>
        public Position? EnPassant { get; protected set; }

        /// <summary>
        /// Number of fen board rows
        /// </summary>
        protected virtual byte ValidFenBoardRows
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// return true when 3 times the same board
        /// </summary>
        public virtual bool ThreeFoldRepeatAndThisCanResultInDraw
        {
            get
            {
                int count;
                count = m_zobristKeys
                    .GroupBy(x => x)
                    .Max(g => g.Count());
                return count >= 3;
            }
        }

        /// <summary>
        /// return true when 50 times without moving a pawn or eating a piece
        /// </summary>
        public virtual bool FiftyMovesAndThisCanResultInDraw
        {
            get
            {
                return m_fiftyMoves;
            }
        }

        /// <summary>
        /// Start Fen
        /// </summary>
        public string? InitFen { get; } = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        /// <summary>
        /// Current Zobrist key value
        /// </summary>
        public ulong CurrentZobristKey
        {
            get
            {
                return ZobristKey.ComputeBoardZobristKey(this);
            }
        }
        /// <summary>
        /// Game result
        /// </summary>
        public virtual GameResult GameResult
        {
            get
            {
                if (m_resigned == Player.White)
                {
                    return GameResult.WhiteResign;
                }
                if (m_resigned == Player.Black)
                {
                    return GameResult.BlackResign;
                }
                if (IsStalemated(CurrentPlayer))
                {
                    return GameResult.Stalemated;
                }
                else if (IsInsufficientMaterial())
                {
                    return GameResult.InsufficientMaterial;
                }
                else if (ThreeFoldRepeatAndThisCanResultInDraw)
                {
                    return GameResult.ThreeFoldRepeat;
                }
                else if (FiftyMovesAndThisCanResultInDraw)
                {
                    return GameResult.FiftyRuleRepeat;
                }
                else if (IsCheckmated(CurrentPlayer))
                {
                    return GameResult.Mate;
                }
                else if (IsInCheck(CurrentPlayer))
                {
                    return GameResult.Check;
                }
                else if (m_whiteTimeout)
                {
                    return GameResult.WhiteTimeout;
                }
                else if (m_blackTimeout)
                {
                    return GameResult.BlackTimeout;
                }
                else if (m_unknown)
                {
                    return GameResult.Unknown;
                }
                else
                {
                    return GameResult.OnGoing;
                }
            }
        }

        /// <summary>Attributes</summary>
        public Dictionary<string, string>? Attrs { get; set; } = new();
        #endregion

        #region Methods
        /// <summary>
        /// Initialize chess game
        /// </summary>
        public void Initialize()
        {
            CurrentPlayer = Player.White;
            m_currentMovePosition = -1;
            Board = new Piece?[8][];
            Piece kw = FenMappings['K'];
            Piece kb = FenMappings['k'];
            Piece qw = FenMappings['Q'];
            Piece qb = FenMappings['q'];
            Piece rw = FenMappings['R'];
            Piece rb = FenMappings['r'];
            Piece nw = FenMappings['N'];
            Piece nb = FenMappings['n'];
            Piece bw = FenMappings['B'];
            Piece bb = FenMappings['b'];
            Piece pw = FenMappings['P'];
            Piece pb = FenMappings['p'];
            Piece? o = null;
            Board = new Piece?[8][]
            {
                new[] { rb, nb, bb, qb, kb, bb, nb, rb },
                new[] { pb, pb, pb, pb, pb, pb, pb, pb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { pw, pw, pw, pw, pw, pw, pw, pw },
                new[] { rw, nw, bw, qw, kw, bw, nw, rw }
            };
            CastlingType = CastlingType.WhiteCastleKingSide | CastlingType.WhiteCastleQueenSide |
                            CastlingType.BlackCastleKingSide | CastlingType.BlackCastleQueenSide;
            InitialWhiteRookLineKingsideCastling = InitialBlackRookLineKingsideCastling = Line.h;
            InitialWhiteRookLineQueensideCastling = InitialBlackRookLineQueensideCastling = Line.a;
            InitialWhiteKingLine = InitialBlackKingLine = Line.e;
            m_drawn = false;
            m_resigned = Player.None;
            m_halfMoveClock = 0;
            m_fullMoveNumber = 1;
            m_unknown = false;
            m_allmoves.Clear();
            m_zobristKeys.Clear();
            EnPassant = null;
            SetNewGame();
        }

        /// <summary>
        /// Initialize chess game
        /// </summary>
        /// <param name="fen">  Fen</param>
        public void Initialize(string fen)
        {
            GameCreationData data = FenStringToGameCreationData(fen);
            UseGameCreationData(data);
            SetNewGame();
        }

        public void ResetTimeout()
        {
            m_whiteTimeout = false;
            m_blackTimeout = false;
        }

        /// <summary>
        /// Set new game
        /// </summary>
        private void SetNewGame()
        {
            if (m_zobristKeys.Count == 0)
                m_zobristKeys.Add(CurrentZobristKey);
            m_currentMovePosition = m_allmoves.Count - 1;
            m_fiftyMoves = false;
            StartFen = GetFen();
            StartPlayer = CurrentPlayer;
            ResetTimeout();
        }

        /// <summary>
        /// Map pgn char to piece
        /// </summary>
        /// <param name="c">        Char</param>
        /// <param name="owner">    Owner</param>
        /// <returns>
        /// Piece
        /// </returns>
        public virtual Piece MapPgnCharToPiece(char c, Player owner)
        {
            switch (c)
            {
                case 'K':
                    return owner == Player.White ? FenMappings['K'] : FenMappings['k'];
                case 'Q':
                case 'D':
                    return owner == Player.White ? FenMappings['Q'] : FenMappings['q'];
                case 'R':
                case 'T':
                    return owner == Player.White ? FenMappings['R'] : FenMappings['r'];
                case 'B':
                case 'F':
                    return owner == Player.White ? FenMappings['B'] : FenMappings['b'];
                case 'N':
                case 'C':
                    return owner == Player.White ? FenMappings['N'] : FenMappings['n'];
                case 'P':
                    return owner == Player.White ? FenMappings['P'] : FenMappings['p'];
                default:
                    if (!char.IsLower(c))
                    {
                        throw new PgnParserException("Unrecognized piece type: " + c.ToString());
                    }
                    return owner == Player.White ? FenMappings['P'] : FenMappings['p'];
            }
        }

        /// <summary>
        /// Needs special treatment for PgnMove
        /// </summary>
        /// <param name="move">     Move</param>
        /// <param name="player">   Player</param>
        /// <returns>
        /// false
        /// </returns>
        public virtual bool NeedsPgnMoveSpecialTreatment(string move, Player player)
        {
            return false;
        }

        /// <summary>
        /// Handle special PgnMove
        /// </summary>
        /// <param name="move">     Move</param>
        /// <param name="player">   Player</param>
        /// <returns>
        /// false
        /// </returns>
        public virtual bool HandleSpecialPgnMove(string move, Player player)
        {
            return false;
        }

        /// <summary>
        /// Get chess board
        /// </summary>
        /// <returns>
        /// chess board
        /// </returns>
        public Piece?[][]? GetBoard()
        {
            return CloneBoard(Board);
        }

        /// <summary>
        /// Clone chess board
        /// </summary>
        /// <returns>
        /// chess board
        /// </returns>
        protected static Piece?[][]? CloneBoard(Piece?[][]? originalBoard)
        {
            ChessUtilities.ThrowIfNull(originalBoard, nameof(originalBoard));
            var newBoard = new Piece[originalBoard!.Length][];
            for (int i = 0; i < originalBoard.Length; i++)
            {
                newBoard[i] = new Piece[originalBoard[i].Length];
                Array.Copy(originalBoard[i], newBoard[i], originalBoard[i].Length);
            }
            return newBoard;
        }

        /// <summary>
        /// Set Initial Castling
        /// </summary>
        /// <param name="setAllCastling">   set all castling</param>
        public virtual void SetInitialCastling(bool setAllCastling)
        {
            if (setAllCastling)
                CastlingType = CastlingType.WhiteCastleKingSide | CastlingType.WhiteCastleQueenSide | CastlingType.BlackCastleKingSide | CastlingType.BlackCastleQueenSide;
            Piece?[] eighthRank = Board![0];
            Piece?[] firstRank = Board[7];

            InitialWhiteKingLine = (Line)Array.IndexOf(firstRank, new King(Player.White));
            InitialBlackKingLine = (Line)Array.IndexOf(eighthRank, new King(Player.Black));

            InitialWhiteRookLineKingsideCastling = (CastlingType & CastlingType.WhiteCastleKingSide) == CastlingType.WhiteCastleKingSide && InitialWhiteKingLine != Line.None
                ? (Line)Array.LastIndexOf(firstRank, new Rook(Player.White)) : Line.None;
            if (InitialWhiteRookLineKingsideCastling != Line.None && InitialWhiteRookLineKingsideCastling < InitialWhiteKingLine)
                InitialWhiteRookLineKingsideCastling = Line.None;

            InitialWhiteRookLineQueensideCastling = (CastlingType & CastlingType.WhiteCastleQueenSide) == CastlingType.WhiteCastleQueenSide && InitialWhiteKingLine != Line.None
                ? (Line)Array.IndexOf(firstRank, new Rook(Player.White)) : Line.None;
            if (InitialWhiteRookLineQueensideCastling != Line.None && InitialWhiteRookLineQueensideCastling > InitialWhiteKingLine)
                InitialWhiteRookLineQueensideCastling = Line.None;

            InitialBlackRookLineKingsideCastling = (CastlingType & CastlingType.BlackCastleKingSide) == CastlingType.BlackCastleKingSide && InitialBlackKingLine != Line.None
                ? (Line)Array.LastIndexOf(eighthRank, new Rook(Player.Black)) : Line.None;
            if (InitialBlackRookLineKingsideCastling != Line.None && InitialBlackRookLineKingsideCastling < InitialBlackKingLine)
                InitialBlackRookLineKingsideCastling = Line.None;

            InitialBlackRookLineQueensideCastling = (CastlingType & CastlingType.BlackCastleQueenSide) == CastlingType.BlackCastleQueenSide && InitialBlackKingLine != Line.None
                ? (Line)Array.IndexOf(eighthRank, new Rook(Player.Black)) : Line.None;
            if (InitialBlackRookLineQueensideCastling != Line.None && InitialBlackRookLineQueensideCastling > InitialBlackKingLine)
                InitialBlackRookLineQueensideCastling = Line.None;

            if (InitialWhiteRookLineKingsideCastling == Line.None && (CastlingType & CastlingType.WhiteCastleKingSide) == CastlingType.WhiteCastleKingSide)
                CastlingType -= CastlingType.WhiteCastleKingSide;
            if (InitialWhiteRookLineQueensideCastling == Line.None && (CastlingType & CastlingType.WhiteCastleQueenSide) == CastlingType.WhiteCastleQueenSide)
                CastlingType -= CastlingType.WhiteCastleQueenSide;
            if (InitialBlackRookLineKingsideCastling == Line.None && (CastlingType & CastlingType.BlackCastleKingSide) == CastlingType.BlackCastleKingSide)
                CastlingType -= CastlingType.BlackCastleKingSide;
            if (InitialBlackRookLineQueensideCastling == Line.None && (CastlingType & CastlingType.BlackCastleQueenSide) == CastlingType.BlackCastleQueenSide)
                CastlingType -= CastlingType.BlackCastleQueenSide;

        }

        /// <summary>
        /// Reset CastlingType
        /// </summary>
        /// <param name="castling">         CastlingType for reset</param>
        /// <param name="resetCastle">      Reset CastlingType</param>
        /// <returns>
        /// Resetet CastlingType
        /// </returns>
        public CastlingType ResetCastlingType(CastlingType castling, CastlingType resetCastle)
        {
            if (CanCastle(castling, resetCastle))
            {
                castling -= resetCastle;
            }
            return castling;
        }

        /// <summary>
        /// Can caste castlingType
        /// </summary>
        /// <param name="castling">         Testet CastlingType</param>
        /// <param name="canCastle">        Can castle CastlingType</param>
        /// <returns>
        /// true when can caste castlingType
        /// </returns>
        public bool CanCastle(CastlingType castling, CastlingType canCastle)
        {
            return (castling & canCastle) == canCastle;
        }

        /// <summary>
        /// Use game creation data
        /// </summary>
        /// <param name="data">   Game creation data</param>
        protected virtual void UseGameCreationData(GameCreationData data)
        {
            Board = CloneBoard(data.Board);
            CurrentPlayer = data.CurrentPlayer;

            Piece?[] eighthRank = Board![0];
            Piece?[] firstRank = Board[7];

            InitialWhiteKingLine = (Line)Array.IndexOf(firstRank, FenMappings['K']);
            InitialBlackKingLine = (Line)Array.IndexOf(eighthRank, FenMappings['k']);
            CastlingType = data.CastlingType;
            EnPassant = data.EnPassant;
            SetInitialCastling(false);

            m_allmoves = data.AllMoves.ToList();

            EnPassant = data.EnPassant;
            m_halfMoveClock = data.HalfMoveClock;
            m_fullMoveNumber = data.FullMoveNumber;

            m_drawn = data.DrawClaimed;
            m_resigned = data.Resigned;
        }


        /// <summary>
        /// Get fen
        /// </summary>
        /// <returns>
        /// Fen
        /// </returns>
        public virtual string GetFen()
        {
            StringBuilder fenBuilder = new();
            Piece?[][]? board = GetBoard();
            for (int i = 0; i < board!.Length; i++)
            {
                Piece?[] row = board[i];
                int empty = 0;
                foreach (Piece? piece in row)
                {
                    char pieceChar = piece! == null! ? '\0' : piece.GetFenCharacter();
                    if (pieceChar == '\0')
                    {
                        empty++;
                        continue;
                    }
                    if (empty != 0)
                    {
                        fenBuilder.Append(empty);
                        empty = 0;
                    }
                    fenBuilder.Append(pieceChar);
                    if (piece!.IsPromotionResult && UseTildesInFenGeneration)
                    {
                        fenBuilder.Append('~');
                    }
                }
                if (empty != 0)
                {
                    fenBuilder.Append(empty);
                }
                if (i != board.Length - 1)
                {
                    fenBuilder.Append('/');
                }
            }

            fenBuilder.Append(' ');

            fenBuilder.Append(CurrentPlayer == Player.White ? 'w' : 'b');

            fenBuilder.Append(' ');

            bool hasAnyCastlingOptions = false;


            if (CanCastle(CastlingType, CastlingType.WhiteCastleKingSide))
            {
                fenBuilder.Append('K');
                hasAnyCastlingOptions = true;
            }
            if (CanCastle(CastlingType, CastlingType.WhiteCastleQueenSide))
            {
                fenBuilder.Append('Q');
                hasAnyCastlingOptions = true;
            }


            if (CanCastle(CastlingType, CastlingType.BlackCastleKingSide))
            {
                fenBuilder.Append('k');
                hasAnyCastlingOptions = true;
            }
            if (CanCastle(CastlingType, CastlingType.BlackCastleQueenSide))
            {
                fenBuilder.Append('q');
                hasAnyCastlingOptions = true;
            }
            if (!hasAnyCastlingOptions)
            {
                fenBuilder.Append('-');
            }

            fenBuilder.Append(' ');

            if (EnPassant! != null!)
            {
                fenBuilder.Append(EnPassant.ToString());
            }
            else
            {
                fenBuilder.Append("-");
            }

            fenBuilder.Append(' ');

            fenBuilder.Append(m_halfMoveClock);

            fenBuilder.Append(' ');

            fenBuilder.Append(m_fullMoveNumber);

            return fenBuilder.ToString();
        }

        /// <summary>
        /// Convert fen to game creation data
        /// </summary>
        /// <param name="fen">   Fen</param>
        /// <returns>
        /// Game creation data
        /// </returns>
        protected virtual GameCreationData FenStringToGameCreationData(string fen)
        {
            string[] parts = fen.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != AllowedFenPartsLength)
            {
                throw new ArgumentException("The FEN string has too much, or too few, parts.");
            }
            string[] rows = parts[0].Split('/');
            if (rows.Length != ValidFenBoardRows)
            {
                throw new ArgumentException("The board in the FEN string has an invalid number of rows.");
            }
            GameCreationData data = new();

            data.Board = InterpretBoardOfFen(parts[0]);

            if (parts[1] == "w")
            {
                data.CurrentPlayer = Player.White;
            }
            else if (parts[1] == "b")
            {
                data.CurrentPlayer = Player.Black;
            }
            else
            {
                throw new ArgumentException("Expected `w` or `b` for the active player in the FEN string.");
            }

            if (parts[2].Contains("K"))
            {
                data.CastlingType |= CastlingType.WhiteCastleKingSide;
            }
            else
            {
                data.CastlingType = ResetCastlingType(data.CastlingType, CastlingType.WhiteCastleKingSide);
            }

            if (parts[2].Contains("Q"))
            {
                data.CastlingType |= CastlingType.WhiteCastleQueenSide;
            }
            else
            {
                data.CastlingType = ResetCastlingType(data.CastlingType, CastlingType.WhiteCastleQueenSide);
            }

            if (parts[2].Contains("k"))
            {
                data.CastlingType |= CastlingType.BlackCastleKingSide;
            }
            else
            {
                data.CastlingType = ResetCastlingType(data.CastlingType, CastlingType.BlackCastleKingSide);
            }

            if (parts[2].Contains("q"))
            {
                data.CastlingType |= CastlingType.BlackCastleQueenSide;
            }
            else
            {
                data.CastlingType = ResetCastlingType(data.CastlingType, CastlingType.BlackCastleQueenSide);
            }

            if (parts[3] == "-")
            {
                data.EnPassant = null;
            }
            else
            {
                var ep = new Position(parts[3]);
                if ((data.CurrentPlayer == Player.White && (ep.Rank != 6 || !(data.Board![3][(int)ep.Line] is Pawn))) ||
                    (data.CurrentPlayer == Player.Black && (ep.Rank != 3 || !(data.Board![4][(int)ep.Line] is Pawn))))
                {
                    throw new ArgumentException("Invalid en passant field in FEN.");
                }
                data.EnPassant = ep;
            }

            int halfmoveClock;
            if (int.TryParse(parts[4], out halfmoveClock))
            {
                data.HalfMoveClock = halfmoveClock;
            }
            else
            {
                throw new ArgumentException("Halfmove clock in FEN is invalid.");
            }

            int fullMoveNumber;
            if (int.TryParse(parts[5], out fullMoveNumber))
            {
                data.FullMoveNumber = fullMoveNumber;
            }
            else
            {
                throw new ArgumentException("Fullmove number in FEN is invalid.");
            }

            return data;
        }


        /// <summary>
        /// Convert fen to chess bord
        /// </summary>
        /// <param name="fen">   Fen</param>
        /// <returns>
        /// Chess bord
        /// </returns>
        protected virtual Piece?[][]? InterpretBoardOfFen(string board)
        {
            var pieceArr = new Piece?[8][];
            string[] rows = board.Split('/');
            for (int i = 0; i < 8; i++)
            {
                string row = rows[i];
                var currentRow = new Piece?[8] { null, null, null, null, null, null, null, null };
                int j = 0;
                foreach (char c in row)
                {
                    if (char.IsDigit(c))
                    {
                        j += (int)char.GetNumericValue(c);
                        continue;
                    }
                    if (c == '~')
                    {
                        if (j == 0)
                        {
                            throw new ArgumentException("Error in FEN: misplaced '~'.");
                        }
                        if (currentRow[j - 1]! == null!)
                        {
                            throw new ArgumentException("Error in FEN: misplaced '~'.");
                        }
                        currentRow[j - 1] = currentRow[j - 1]!.AsPromotion();
                        continue;
                    }
                    if (!FenMappings.ContainsKey(c))
                        throw new ArgumentException("The FEN string contains an unknown piece.");
                    currentRow[j] = FenMappings[c];
                    j++;
                }
                if (j != 8)
                {
                    throw new ArgumentException("Not enough pieces provided for a row in the FEN string.");
                }
                pieceArr[i] = currentRow;
            }
            return pieceArr;
        }

        /// <summary>
        /// Get piece at position
        /// </summary>
        /// <param name="position">   Fen</param>
        /// <returns>
        /// Piece
        /// </returns>
        public Piece? GetPieceAt(Position position)
        {
            ChessUtilities.ThrowIfNull(position, nameof(position));
            return GetPieceAt(position.Line, position.Rank);
        }

        /// <summary>
        /// Get piece at Line and rank
        /// </summary>
        /// <param name="file">   Line</param>
        /// <param name="rank">   Rank</param>
        /// <returns>
        /// Piece
        /// </returns>
        public Piece? GetPieceAt(Line file, int rank)
        {
            return Board![8 - rank][(int)file];
        }

        /// <summary>
        /// Get piece at Bord index
        /// </summary>
        /// <param name="bordIndex">   Bord index</param>
        /// <returns>
        /// Piece
        /// </returns>
        public Piece? GetPieceAt(byte bordIndex)
        {
            Position position;
            position = ChessUtilities.GetPositionAt(bordIndex);
            return GetPieceAt(position);
        }

        /// <summary>
        /// Set piece at position
        /// </summary>
        /// <param name="position">     position</param>
        /// <param name="piece">        Piece</param>
        public virtual void SetPieceAt(Position position, Piece? piece)
        {
            SetPieceAt(position.Line, position.Rank, piece);
        }

        /// <summary>
        /// Set piece at position
        /// </summary>
        /// <param name="file">     Line</param>
        /// <param name="piece">    Piece</param>
        protected virtual void SetPieceAt(Line file, int rank, Piece? piece)
        {
            Board![8 - rank][(int)file] = piece;
        }

        /// <summary>
        /// Gets the description of a move
        /// </summary>
        /// <param name="dm"> Move to describe</param>
        /// <returns>
        /// Move description
        /// </returns>
        public string GetHumanPos(Move move)
        {
            return move.ToString();
        }

        /// <summary>
        /// Test if move is valid
        /// </summary>
        /// <param name="move">   Move</param>
        /// <returns>
        /// true when move is valid, otherwise false
        /// </returns>
        public bool IsValidMove(Move move)
        {
            ChessUtilities.ThrowIfNull(move, nameof(move));
            return IsValidMove(move, true, true);
        }

        /// <summary>
        /// Test if move is valid
        /// </summary>
        /// <param name="move">             Move</param>
        /// <param name="validateCheck">    validate Check</param>
        /// <returns>
        /// true when move is valid, otherwise false
        /// </returns>
        protected bool IsValidMove(Move move, bool validateCheck)
        {
            ChessUtilities.ThrowIfNull(move, nameof(move));
            return IsValidMove(move, validateCheck, true);
        }

        /// <summary>
        /// Test if move is valid
        /// </summary>
        /// <param name="move">             Move</param>
        /// <param name="validateCheck">    validate Check</param>
        /// <returns>
        /// true when move is valid, otherwise false
        /// </returns>
        protected virtual bool IsValidMove(Move move, bool validateCheck, bool careAboutCurrentPlayerItIs)
        {
            ChessUtilities.ThrowIfNull(move, nameof(move));
            if (move.OriginalPosition.Equals(move.NewPosition))
                return false;
            Piece piece = GetPieceAt(move.OriginalPosition.Line, move.OriginalPosition.Rank)!;
            if (careAboutCurrentPlayerItIs && move.Player != CurrentPlayer) return false;
            if (piece! == null! || piece.Owner != move.Player) return false;
            Piece? pieceAtDestination = GetPieceAt(move.NewPosition);
            bool isCastle = pieceAtDestination is Rook && piece is King && pieceAtDestination.Owner == piece.Owner;
            if (pieceAtDestination! != null! && pieceAtDestination.Owner == move.Player && !isCastle)
            {
                return false;
            }
            if (!piece.IsValidMove(move, this))
            {
                return false;
            }
            if (validateCheck)
            {
                if (!isCastle && WouldBeInCheckAfter(move, move.Player))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Apply castle
        /// </summary>
        /// <param name="move">            Move</param>
        protected virtual CastlingType ApplyCastle(Move move)
        {
            CastlingType castle;
            int rank = move.Player == Player.White ? 1 : 8;
            Line rookLine = move.NewPosition.Line;
            if (move.Player == Player.White)
            {
                if (rookLine == Line.c && GetPieceAt(Line.c, rank)! == null! && InitialWhiteKingLine == Line.e)
                {
                    rookLine = Line.a;
                }
                else if (rookLine == Line.g && GetPieceAt(Line.g, rank)! == null! && InitialWhiteKingLine == Line.e)
                {
                    rookLine = Line.h;
                }
            }
            else
            {
                if (rookLine == Line.c && GetPieceAt(Line.c, rank)! == null! && InitialBlackKingLine == Line.e)
                {
                    rookLine = Line.a;
                }
                else if (rookLine == Line.g && GetPieceAt(Line.g, rank)! == null! && InitialBlackKingLine == Line.e)
                {
                    rookLine = Line.h;
                }
            }

            Line newRookLine;
            Line newKingLine;
            if (rookLine == (move.Player == Player.White ? InitialWhiteRookLineKingsideCastling : InitialBlackRookLineKingsideCastling))
            {
                if (move.Player == Player.White)
                {
                    castle = CastlingType.WhiteCastleKingSide;
                }
                else
                {
                    castle = CastlingType.BlackCastleKingSide;
                }
                newRookLine = Line.f;
                newKingLine = Line.g;
            }
            else
            {
                if (move.Player == Player.White)
                {
                    castle = CastlingType.WhiteCastleQueenSide;
                }
                else
                {
                    castle = CastlingType.BlackCastleQueenSide;
                }
                newRookLine = Line.d;
                newKingLine = Line.c;
            }
            SetPieceAt(rookLine, rank, null);
            SetPieceAt(move.OriginalPosition.Line, rank, null);
            SetPieceAt(newRookLine, rank, new Rook(move.Player));
            SetPieceAt(newKingLine, rank, new King(move.Player));
            return castle;
        }

        /// <summary>
        /// Make move
        /// </summary>
        /// <param name="move">             Move</param>
        /// <param name="alreadyValidated"> move is already validated</param>
        /// <returns>
        /// MoveType
        /// </returns>
        public virtual MoveType MakeMove(Move move, bool alreadyValidated)
        {
            Piece? captured;
            return MakeMove(move, alreadyValidated, out captured);
        }

        /// <summary>
        /// Make move
        /// </summary>
        /// <param name="move">             Move</param>
        /// <param name="alreadyValidated"> move is already validated</param>
        /// <param name="captured">         Captured Piece</param>
        /// <returns>
        /// MoveType
        /// </returns>
        public virtual MoveType MakeMove(Move move, bool alreadyValidated, out Piece? captured, PgnMove? pgnMove = null)
        {
            string annotation;
            int nag;
            string comment;
            if (pgnMove == null)
            {
                annotation = "";
                nag = 0;
                comment = "";
            }
            else
            {
                annotation = pgnMove.Annotation;
                nag = pgnMove.NAG;
                comment = pgnMove.Comment;
            }
            Piece movingPiece = GetPieceAt(move.OriginalPosition)!;
            List<Position> ambiguities = GetAmbiguities(move, movingPiece);
            CastlingType castle;
            string san;
            int lastHalfMoveClock = m_halfMoveClock;
            Position? lastEnPasant = EnPassant;
            CastlingType lastCastlingType = CastlingType;
            MoveType mt = ApplyMove(move, alreadyValidated, out captured, out castle);
            if (mt == MoveType.Invalid)
            {
                return mt;
            }
            san = GetSanForMove(move, movingPiece, captured! != null!, castle, ambiguities);
            AddDetailedMove(new DetailedMove(move, movingPiece, captured, castle, san, lastHalfMoveClock, lastEnPasant, lastCastlingType, annotation, nag, comment)); // needed so caches work well in GetSanForMove
            return mt;
        }

        /// <summary>
        /// Apply move
        /// </summary>
        /// <param name="move">             Move</param>
        /// <param name="alreadyValidated"> move is already validated</param>
        /// <returns>
        /// MoveType
        /// </returns>
        protected MoveType ApplyMove(Move move, bool alreadyValidated)
        {
            Piece? captured;
            CastlingType castleType;
            return ApplyMove(move, alreadyValidated, out captured, out castleType);
        }

        /// <summary>
        /// Apply move
        /// </summary>
        /// <param name="move">             Move</param>
        /// <param name="alreadyValidated"> move is already validated</param>
        /// <param name="captured">         Captured Piece</param>
        /// <param name="castleType">       Castle type</param>
        /// <returns>
        /// MoveType
        /// </returns>
        protected virtual MoveType ApplyMove(Move move, bool alreadyValidated, out Piece? captured, out CastlingType castleType)
        {
            ChessUtilities.ThrowIfNull(move, nameof(move));
            captured = null;
            if (!alreadyValidated && !IsValidMove(move))
            {
                castleType = CastlingType.None;
                return MoveType.Invalid;
            }
            var type = MoveType.Move;
            Piece? movingPiece = GetPieceAt(move.OriginalPosition.Line, move.OriginalPosition.Rank);

            if (movingPiece! == null!)
            {
                throw new InvalidOperationException("Source piece does not exist.");
            }

            Piece? capturedPiece = GetPieceAt(move.NewPosition.Line, move.NewPosition.Rank);
            captured = capturedPiece;
            Piece newPiece = movingPiece;
            bool isCapture = capturedPiece! != null!;
            var castle = CastlingType.None;
            Position position;
            EnPassant = null!;
            if (movingPiece is Pawn)
            {
                m_halfMoveClock = 0;
                var pd = new PositionDistance(move.OriginalPosition, move.NewPosition);
                if (pd.DistanceX == 1 && pd.DistanceY == 1 && GetPieceAt(move.NewPosition)! == null!)
                {
                    type |= MoveType.EnPassant;
                    isCapture = true;
                    captured = GetPieceAt(move.NewPosition.Line, move.OriginalPosition.Rank);
                    SetPieceAt(move.NewPosition.Line, move.OriginalPosition.Rank, null);
                }
                if (move.NewPosition.Rank == (move.Player == Player.White ? 8 : 1))
                {
                    newPiece = MapPgnCharToPiece(move.Promotion!.Value, move.Player).AsPromotion();
                    type |= MoveType.Promotion;
                }
                if (pd.DistanceX == 0 && pd.DistanceY == 2)
                {
                    if (move.OriginalPosition.Line > Line.a)
                    {
                        position = new Position(move.NewPosition.Line - 1, move.NewPosition.Rank);
                        if (GetPieceAt(position)! == new Pawn(ChessUtilities.GetOpponentOf(move.Player)))
                            EnPassant = new Position(move.NewPosition.Line, (byte)(position.Rank + (move.Player == Player.White ? -1 : 1)));
                    }
                    if (EnPassant! == null! && move.OriginalPosition.Line < Line.h)
                    {
                        position = new Position(move.NewPosition.Line + 1, move.NewPosition.Rank);
                        if (GetPieceAt(position)! == new Pawn(ChessUtilities.GetOpponentOf(move.Player)))
                            EnPassant = new Position(move.NewPosition.Line, (byte)(position.Rank + (move.Player == Player.White ? -1 : 1)));
                    }
                }
            }
            else if (movingPiece is King)
            {
                if (movingPiece.Owner == Player.White)
                {
                    CastlingType = ResetCastlingType(CastlingType, CastlingType.WhiteCastleKingSide);
                    CastlingType = ResetCastlingType(CastlingType, CastlingType.WhiteCastleQueenSide);
                }
                else
                {
                    CastlingType = ResetCastlingType(CastlingType, CastlingType.BlackCastleKingSide);
                    CastlingType = ResetCastlingType(CastlingType, CastlingType.BlackCastleQueenSide);
                }

                if ((GetPieceAt(move.NewPosition) is Rook && GetPieceAt(move.NewPosition)!.Owner == move.Player) ||
                        ((move.NewPosition.Line == Line.c || move.NewPosition.Line == Line.g) &&
                        (move.Player == Player.White ? InitialWhiteKingLine : InitialBlackKingLine) == Line.e
                        && move.OriginalPosition.Line == Line.e))
                {
                    castle = ApplyCastle(move);
                    type |= MoveType.Castling;
                    isCapture = false;
                }
            }
            else if (movingPiece is Rook)
            {
                if (move.Player == Player.White)
                {
                    if (move.OriginalPosition.Line == Line.h && move.OriginalPosition.Rank == 1)
                    {
                        CastlingType = ResetCastlingType(CastlingType, CastlingType.WhiteCastleKingSide);
                    }
                    else if (move.OriginalPosition.Line == Line.a && move.OriginalPosition.Rank == 1)
                    {
                        CastlingType = ResetCastlingType(CastlingType, CastlingType.WhiteCastleQueenSide);
                    }
                }
                else
                {
                    if (move.OriginalPosition.Line == Line.h && move.OriginalPosition.Rank == 8)
                    {
                        CastlingType = ResetCastlingType(CastlingType, CastlingType.BlackCastleKingSide);
                    }
                    else if (move.OriginalPosition.Line == Line.a && move.OriginalPosition.Rank == 8)
                    {
                        CastlingType = ResetCastlingType(CastlingType, CastlingType.BlackCastleQueenSide);
                    }
                }
            }
            if (isCapture)
            {
                type |= MoveType.Capture;
                m_halfMoveClock = 0;
                if (move.NewPosition.Line == Line.h && move.NewPosition.Rank == 1)
                {
                    CastlingType = ResetCastlingType(CastlingType, CastlingType.WhiteCastleKingSide);
                }
                else if (move.NewPosition.Line == Line.a && move.NewPosition.Rank == 1)
                {
                    CastlingType = ResetCastlingType(CastlingType, CastlingType.WhiteCastleQueenSide);
                }
                else if (move.NewPosition.Line == Line.a && move.NewPosition.Rank == 8)
                {
                    CastlingType = ResetCastlingType(CastlingType, CastlingType.BlackCastleQueenSide);
                }
                else if (move.NewPosition.Line == Line.h && move.NewPosition.Rank == 8)
                {
                    CastlingType = ResetCastlingType(CastlingType, CastlingType.BlackCastleKingSide);
                }
            }
            if (!isCapture && !(movingPiece is Pawn))
            {
                m_halfMoveClock++;
                if (m_halfMoveClock >= 100)
                {
                    m_fiftyMoves = true;
                }
                else
                {
                    m_fiftyMoves = false;
                }
            }
            if (move.Player == Player.Black)
            {
                m_fullMoveNumber++;
            }
            if (castle == CastlingType.None)
            {
                SetPieceAt(move.NewPosition.Line, move.NewPosition.Rank, newPiece);
                SetPieceAt(move.OriginalPosition.Line, move.OriginalPosition.Rank, null);
            }
            CurrentPlayer = ChessUtilities.GetOpponentOf(move.Player);
            castleType = castle;
            return type;
        }

        /// <summary>
        /// Undo move
        /// </summary>
        public virtual bool Undo()
        {
            if (m_currentMovePosition < 0)
                return false;

            var movedPiece = LastMove!.Piece;
            SetPieceAt(LastMove.NewPosition.Line, LastMove.NewPosition.Rank, null);
            SetPieceAt(LastMove.OriginalPosition.Line, LastMove.OriginalPosition.Rank, movedPiece);

            if (LastMove.CapturedPiece! != null!)
            {
                int rank = LastMove.NewPosition.Rank;
                if (LastMove.LastEnPassant! != null!)
                {
                    if (LastMove.Player == Player.White)
                        rank--;
                    else if (LastMove.Player == Player.Black)
                        rank++;
                }

                SetPieceAt(LastMove.NewPosition.Line, rank, LastMove.CapturedPiece);
            }

            if (LastMove.Castling != CastlingType.None)
            {
                UndoCastle(LastMove);
            }

            CurrentPlayer = LastMove.Player;
            m_halfMoveClock = LastMove.LastHalfMoveClock!.Value;
            EnPassant = LastMove.LastEnPassant;
            CastlingType = LastMove.LastCastlingType;
            if (LastMove.Player == Player.Black) m_fullMoveNumber--;
            RemoveLastDetailedMove();
            m_whiteTimeout = false;
            m_blackTimeout = false;
            return true;
        }

        /// <summary>
        /// Undo castle
        /// </summary>
        /// <param name="move">             Move</param>
        /// <param name="anyRookMoves">     Any rook moves</param>
        private void UndoCastle(DetailedMove move)
        {
            int rookRank;
            Line originalRookLine;
            Line newRookLine;
            if (move.Player == Player.White)
            {
                rookRank = 1;
            }
            else
            {
                rookRank = 8;
            }
            if (move.Castling == CastlingType.WhiteCastleKingSide || move.Castling == CastlingType.BlackCastleKingSide)
            {
                newRookLine = Line.f;
                originalRookLine = Line.h;
            }
            else
            {
                newRookLine = Line.d;
                originalRookLine = Line.a;
            }

            var rook = GetPieceAt(newRookLine, rookRank);
            SetPieceAt(newRookLine, rookRank, null);
            SetPieceAt(originalRookLine, rookRank, rook);
        }

        /// <summary>
        /// Undo count moves
        /// </summary>
        /// <param name="count">    count</param>
        public int Undo(int count)
        {
            int i = 0;
            for (; i < count; i++)
            {
                if (!Undo())
                {
                    break;
                }
            }

            return i;
        }

        /// <summary>
        /// Redo move
        /// </summary>
        public virtual bool Redo()
        {
            if (NextMove! == null!)
                return false;
            Move move = new Move(NextMove.OriginalPosition, NextMove.NewPosition, NextMove.Player, NextMove.Promotion, NextMove.MoveTime);
            MakeMove(move, true);
            return true;
        }

        /// <summary>
        /// Redo count moves
        /// </summary>
        /// <param name="count">    count</param>
        public int Redo(int count)
        {
            int i = 0;
            for (; i < count; i++)
            {
                if (!Redo())
                {
                    break;
                }
            }

            return i;
        }

        /// <summary>
        /// Get ambiguities
        /// </summary>
        /// <param name="move">             Move</param>
        /// <param name="movingPiece">      Moving Piece</param>
        /// <returns>
        /// List of ambiguities positions
        /// </returns>
        protected virtual List<Position> GetAmbiguities(Move move, Piece? movingPiece)
        {
            if (movingPiece! == null!)
            {
                return [];
            }
            List<Position> ambiguities = [];
            foreach (Line f in Enum.GetValues(typeof(Line)))
            {
                if (f == Line.None) continue;
                for (byte r = 1; r <= 8; r++)
                {
                    var pos = new Position(f, r);
                    if (!move.OriginalPosition.Equals(pos))
                    {
                        Piece? p = GetPieceAt(f, r);
                        if (p! != null! && p.Equals(movingPiece) && p.IsValidMove(new Move(pos, move.NewPosition, move.Player), this))
                        {
                            ambiguities.Add(pos);
                        }
                    }
                }
            }
            return ambiguities;
        }

        /// <summary>
        /// Get san for move
        /// </summary>
        /// <param name="move">             Move</param>
        /// <param name="movingPiece">      Moving Piece</param>
        /// <param name="isCapture">        Is capture</param>
        /// <param name="castle">           Castle type</param>
        /// <param name="ambiguities">      List of ambiguities positions</param>
        /// <returns>
        /// San move
        /// </returns>
        protected virtual string GetSanForMove(Move move, Piece movingPiece, bool isCapture, CastlingType castle, List<Position> ambiguities)
        {
            if (castle == CastlingType.WhiteCastleKingSide || castle == CastlingType.BlackCastleKingSide)
            {
                return "O-O";
            }
            if (castle == CastlingType.WhiteCastleQueenSide || castle == CastlingType.BlackCastleQueenSide)
            {
                return "O-O-O";
            }

            bool needsUnambigLine = false;
            bool needsUnambigRank = false;
            if (ambiguities.Count > 0)
            {
                foreach (Position amb in ambiguities)
                {
                    if (amb.Rank == move.OriginalPosition.Rank)
                    {
                        needsUnambigLine = true;
                    }
                    if (amb.Line == move.OriginalPosition.Line)
                    {
                        needsUnambigRank = true;
                    }
                }
                if (!needsUnambigLine && !needsUnambigRank)
                {
                    needsUnambigLine = true;
                }
            }

            StringBuilder sanBuilder = new();

            if (!(movingPiece is Pawn))
            {
                sanBuilder.Append(char.ToUpperInvariant(movingPiece.GetFenCharacter()));
            }
            else if (isCapture)
            {
                sanBuilder.Append(move.OriginalPosition.Line.ToString().ToLowerInvariant());
                needsUnambigLine = false;
                needsUnambigRank = false;
            }
            if (needsUnambigLine)
            {
                sanBuilder.Append(move.OriginalPosition.Line.ToString().ToLowerInvariant());
            }
            if (needsUnambigRank)
            {
                sanBuilder.Append(move.OriginalPosition.Rank.ToString());
            }
            if (isCapture)
            {
                sanBuilder.Append("x");
            }
            sanBuilder.Append(move.NewPosition.ToString().ToLowerInvariant());
            if (move.Promotion.HasValue)
            {
                sanBuilder.Append("=");
                sanBuilder.Append(move.Promotion.Value);
            }
            if (IsCheckmated(CurrentPlayer))
            {
                sanBuilder.Append("#");
            }
            else if (IsInCheck(CurrentPlayer))
            {
                sanBuilder.Append("+");
            }
            return sanBuilder.ToString();
        }

        /// <summary>
        /// Remove last DetailedMove
        /// </summary>
        protected void RemoveLastDetailedMove()
        {
            if (m_currentMovePosition > -1)
                m_currentMovePosition--;
            m_zobristKeys.RemoveAt(m_zobristKeys.Count - 1);
        }

        /// <summary>
        /// Remove all detailed moves
        /// </summary>
        public virtual void RemoveAllDetailedMoves()
        {
            m_allmoves.Clear();
            m_currentMovePosition = -1;
            m_zobristKeys.RemoveRange(1, m_zobristKeys.Count - 1);
        }

        /// <summary>
        /// Add DetailedMove
        /// </summary>
        /// <param name="dm">               DetailedMove</param>
        protected virtual void AddDetailedMove(DetailedMove dm)
        {
            m_currentMovePosition++;
            if (m_currentMovePosition >= m_allmoves.Count)
            {
                m_allmoves.Add(dm);
            }
            else
            {
                if (m_allmoves[m_currentMovePosition] != dm)
                {
                    m_allmoves[m_currentMovePosition] = dm;
                    if (m_allmoves.Count > m_currentMovePosition)
                        m_allmoves.RemoveRange(m_currentMovePosition + 1, m_allmoves.Count - m_currentMovePosition - 1);
                }
            }
            m_zobristKeys.Add(CurrentZobristKey);
        }

        /// <summary>
        /// Get valid moves for Position
        /// </summary>
        /// <param name="from">                 Position from</param>
        /// <returns>
        /// Valid moves
        /// </returns>
        public ReadOnlyCollection<Move> GetValidMoves(Position from)
        {
            ChessUtilities.ThrowIfNull(from, nameof(from));
            return GetValidMoves(from, false);
        }

        /// <summary>
        /// Get valid moves for Position
        /// </summary>
        /// <param name="from">                 Position from</param>
        /// <param name="returnIfAny">          Return if any move is valid</param>
        /// <returns>
        /// Valid moves
        /// </returns>
        protected virtual ReadOnlyCollection<Move> GetValidMoves(Position from, bool returnIfAny)
        {
            ChessUtilities.ThrowIfNull(from, nameof(from));
            Piece? piece = GetPieceAt(from);
            if (piece! == null! || piece.Owner != CurrentPlayer) 
                return new ReadOnlyCollection<Move>(new List<Move>());
            return piece.GetValidMoves(from, returnIfAny, this, IsValidMove);
        }

        /// <summary>
        /// Get valid moves for player
        /// </summary>
        /// <param name="player">               Player</param>
        /// <returns>
        /// Valid moves
        /// </returns>
        public ReadOnlyCollection<Move> GetValidMoves(Player player)
        {
            return GetValidMoves(player, false);
        }

        /// <summary>
        /// Get valid moves for player
        /// </summary>
        /// <param name="player">               Player</param>
        /// <param name="returnIfAny">          Return if any move is valid</param>
        /// <returns>
        /// Valid moves
        /// </returns>
        protected virtual ReadOnlyCollection<Move> GetValidMoves(Player player, bool returnIfAny)
        {
            if (player != CurrentPlayer)
                return new ReadOnlyCollection<Move>([]);
            List<Move> validMoves = [];
            for (byte r = 1; r <= Board!.Length; r++)
            {
                for (int f = 0; f < Board[8 - r].Length; f++)
                {
                    Piece? p = GetPieceAt((Line)f, r);
                    if (p! != null! && p.Owner == player)
                    {
                        validMoves.AddRange(GetValidMoves(new Position((Line)f, r), returnIfAny));
                        if (returnIfAny && validMoves.Count > 0)
                        {
                            return new ReadOnlyCollection<Move>(validMoves);
                        }
                    }
                }
            }
            return new ReadOnlyCollection<Move>(validMoves);
        }

        /// <summary>
        /// return true when any valid move from position
        /// </summary>
        /// <param name="from">  Position from</param>
        public virtual bool HasAnyValidMoves(Position from)
        {
            ChessUtilities.ThrowIfNull(from, nameof(from));
            ReadOnlyCollection<Move> validMoves = GetValidMoves(from, true);
            return validMoves.Count > 0;
        }

        /// <summary>
        /// return true when player has any valid move
        /// </summary>
        /// <param name="player">  player</param>
        public virtual bool HasAnyValidMoves(Player player)
        {
            ReadOnlyCollection<Move> validMoves = GetValidMoves(player, true);
            return validMoves.Count > 0;
        }

        /// <summary>
        /// return true when player is in check
        /// </summary>
        /// <param name="player">  player</param>
        public virtual bool IsInCheck(Player player)
        {
            if (player == Player.None)
            {
                throw new ArgumentException("IsInCheck: Player.None is an invalid argument.");
            }

            var kingPos = new Position(Line.None, 8);

            for (byte r = 1; r <= Board!.Length; r++)
            {
                for (int f = 0; f < Board[8 - r].Length; f++)
                {
                    Piece? curr = GetPieceAt((Line)f, r);
                    if (curr is King && curr.Owner == player)
                    {
                        kingPos = new Position((Line)f, r);
                        break;
                    }
                }
                if (kingPos != new Position(Line.None, 8))
                {
                    break;
                }
            }

            if (kingPos.Line == Line.None)
                return false;

            for (byte r = 1; r <= Board.Length; r++)
            {
                for (int f = 0; f < Board[8 - r].Length; f++)
                {
                    Piece? curr = GetPieceAt((Line)f, r);
                    if (curr! == null!) continue;
                    Player p = curr.Owner;
                    var move = new Move(new Position((Line)f, r), kingPos, p);
                    List<Move> moves = [];
                    if (curr is Pawn && ((move.NewPosition.Rank == 8 && move.Player == Player.White) || (move.NewPosition.Rank == 1 && move.Player == Player.Black)))
                    {
                        moves.Add(new Move(move.OriginalPosition, move.NewPosition, move.Player, 'Q'));
                        moves.Add(new Move(move.OriginalPosition, move.NewPosition, move.Player, 'R'));
                        moves.Add(new Move(move.OriginalPosition, move.NewPosition, move.Player, 'B'));
                        moves.Add(new Move(move.OriginalPosition, move.NewPosition, move.Player, 'N'));
                    }
                    else
                    {
                        moves.Add(move);
                    }
                    foreach (Move m in moves)
                    {
                        if (IsValidMove(m, false, false))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// return true when player is checkmatet
        /// </summary>
        /// <param name="player">  player</param>
        public virtual bool IsCheckmated(Player player)
        {
            return IsInCheck(player) && !HasAnyValidMoves(player);
        }

        /// <summary>
        /// return true when no more move for the player
        /// </summary>
        /// <param name="player">  player</param>
        public virtual bool IsStalemated(Player player)
        {
            return CurrentPlayer == player && !IsInCheck(player) && !HasAnyValidMoves(player);
        }

        /// <summary>
        /// return true when not enough pieces to do a check mate
        /// </summary>
        public virtual bool IsInsufficientMaterial()
        {
            var pawnPieces = PiecesOnBoard.Count(p => p is Pawn);
            int iBigPieceCount;
            int iWhiteBishop;
            int iBlackBishop;
            int iWhiteKnight;
            int iBlackKnight;

            if (pawnPieces != 0)
            {
                return false;
            }
            else
            {
                iBigPieceCount = PiecesOnBoard.Count(p => p is Rook || p is Queen);
                if (iBigPieceCount != 0)
                {
                    return false;
                }
                else
                {
                    iWhiteBishop = PiecesOnBoard.Count(p => p == new Bishop(Player.White));
                    iBlackBishop = PiecesOnBoard.Count(p => p == new Bishop(Player.Black));
                    iWhiteKnight = PiecesOnBoard.Count(p => p == new Knight(Player.White));
                    iBlackKnight = PiecesOnBoard.Count(p => p == new Knight(Player.Black));
                    if ((iWhiteBishop + iWhiteKnight >= 2 && (iWhiteBishop > 0 || iWhiteKnight > 2))
                    || (iBlackBishop + iBlackKnight >= 2 && (iBlackBishop > 0 || iBlackKnight > 2)))
                    {
                        return false;
                    }
                    else
                    {
                        // Two knights is typically impossible... but who knows!
                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// return true when player is winner
        /// </summary>
        /// <param name="player">  player</param>
        private bool OnSameSquareColor(Piece whitePiece, Piece blackPiece)
        {
            int? whitesSquareColor = null;
            int? blacksSquareColor = null;

            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    var piece = Board![x - 1][y - 1];
                    if (piece! == null!) continue;

                    if (piece.Owner == Player.White)
                    {
                        if (piece.GetType() != whitePiece.GetType()) continue;

                        whitesSquareColor = (x + y) % 2; // 0 means dark, 1 means light
                    }

                    if (piece.Owner == Player.Black)
                    {
                        if (piece.GetType() != blackPiece.GetType()) continue;

                        blacksSquareColor = (x + y) % 2; // 0 means dark, 1 means light
                    }
                }
            }

            return whitesSquareColor == blacksSquareColor;
        }

        /// <summary>
        /// return true when player is winner
        /// </summary>
        /// <param name="player">  player</param>
        public virtual bool IsWinner(Player player)
        {
            return IsCheckmated(ChessUtilities.GetOpponentOf(player)) || (m_resigned == ChessUtilities.GetOpponentOf(player))
                || (player == Player.White && m_blackTimeout) || (player == Player.Black && m_whiteTimeout);
        }


        /// <summary>
        /// return true when game is draw
        /// </summary>
        public virtual bool IsDraw()
        {
            return DrawClaimed || IsStalemated(Player.White) || IsStalemated(Player.Black) || IsInsufficientMaterial() ||
                    ThreeFoldRepeatAndThisCanResultInDraw || FiftyMovesAndThisCanResultInDraw;
        }

        /// <summary>
        /// Would be in check After move
        /// </summary>
        /// <param name="move">     Move</param>
        /// <param name="player">   Player</param>
        /// <returns>
        /// return true when plyer would be in check After move
        /// </returns>
        public virtual bool WouldBeInCheckAfter(Move move, Player player)
        {
            ChessUtilities.ThrowIfNull(move, nameof(move));
            GameCreationData gcd = new();
            gcd.Board = Board;
            gcd.CastlingType = CastlingType;
            gcd.EnPassant = null;
            gcd.HalfMoveClock = m_halfMoveClock;
            gcd.FullMoveNumber = m_fullMoveNumber;
            var copy = new ChessGame(gcd);
            if (GetPieceAt(move.OriginalPosition) is Pawn)
            {
                copy.ApplyMove(move, true);
            }
            else
            {
                Piece p = copy.GetPieceAt(move.OriginalPosition)!;
                copy.SetPieceAt(move.OriginalPosition.Line, move.OriginalPosition.Rank, null);
                copy.SetPieceAt(move.NewPosition.Line, move.NewPosition.Rank, p);
            }
            return copy.IsInCheck(player);
        }

        /// <summary>
        /// Claim draw
        /// </summary>
        public void ClaimDraw()
        {
            m_drawn = true;
        }

        /// <summary>
        /// Player resigned
        /// </summary>
        /// <param name="player">   player/param>
        public void Resign(Player player)
        {
            m_resigned = player;
        }

        /// <summary>
        /// Player resigned
        /// </summary>
        /// <param name="player">   player/param>
        public GameCreationData GetGameCreationData()
        {
            return new GameCreationData
            {
                Board = Board,
                CastlingType = CastlingType,
                AllMoves = [..AllMoves.Select(x => x)],
                CurrentMovePosition = m_currentMovePosition,
                FullMoveNumber = m_fullMoveNumber,
                HalfMoveClock = m_halfMoveClock,
                CurrentPlayer = CurrentPlayer,
                DrawClaimed = DrawClaimed,
                Resigned = Resigned
            };
        }

        /// <summary>
        /// Find a move from the opening book
        /// </summary>
        /// <param name="book">         Book</param>
        /// <param name="randomMode">   Random mode</param>
        /// <param name="move">         Found move</param>
        /// <returns>
        /// true if succeed, false if no move found in book
        /// </returns>
        public bool FindBookMove(Book book, bool randomMode, out Move? move)
        {
            bool retVal;
            ushort bookmove;
            bookmove = book.FindMoveInBook(CurrentZobristKey, randomMode);
            if (bookmove == 0)
            {
                move = null;
                retVal = false;
            }
            else
            {
                move = ChessUtilities.BookMoveToMove(bookmove, CurrentPlayer, true);
                retVal = IsValidMove(move);
                if (!retVal)
                {
                    throw new Exception($"Invalid move from Book move {move}");
                }
            }
            return retVal;
        }

        /// <summary>
        /// Close design mode
        /// </summary>
        /// <param name="player">     Player</param>
        /// <param name="castling">   Castling type</param>
        public void CloseDesignMode(Player player, CastlingType castling)
        {
            CurrentPlayer = player;
            CastlingType = castling;
            m_halfMoveClock = 0;
            m_fullMoveNumber = 1;
            EnPassant = null;
            m_zobristKeys.Add(CurrentZobristKey);
        }

        /// <summary>
        /// Set Timeout
        /// </summary>
        /// <param name="gameResult">GameResult</param>
        public void SetTimeout(GameResult gameResult)
        {
            if (!m_whiteTimeout && !m_blackTimeout)
            {
                if (gameResult == GameResult.WhiteTimeout)
                    m_whiteTimeout = true;
                else
                    m_blackTimeout = true;
            }
        }
        #endregion
    }
}
