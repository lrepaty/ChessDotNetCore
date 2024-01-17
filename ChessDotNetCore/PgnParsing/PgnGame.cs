namespace ChessDotNetCore.PgnParsing
{

    #region Types
    /// <summary>
    /// Type of player (human of computer program)
    /// </summary>
    public enum PgnPlayerType: byte
    {
        /// <summary>Player is a human</summary>
        Human,
        /// <summary>Player is a computer program</summary>
        Program
    };
    #endregion

    /// <summary>
    /// PGN raw game. Attributes and undecoded move list
    /// </summary>
    public class PgnGame
    {
        #region Members
        /// <summary>
        /// Attribute which has been read
        /// </summary>
        [Flags]
        public enum AttrRead
        {
            None        = 0,
            Event       = 1,
            Site        = 2,
            GameDate    = 4,
            Round       = 8,
            WhitePlayer = 16,
            BlackPlayer = 32,
            WhiteElo    = 64,
            BlackElo    = 128,
            GameResult  = 256,
            GameTime    = 512,
            MoveTime    = 1024,
            Moves       = 2048,
            Increment   = 4096,
            WhiteType   = 8192,
            BlackType   = 16384,
            Fen         = 32768,
            TimeControl = 65536,
            Termination = 131072,
            Date        = 262144
        }

        /// <summary>Read attributes</summary>
        private AttrRead                   m_attrRead;
        /// <summary>Event</summary>
        private string?                    m_event;
        /// <summary>Site of the event</summary>
        private string?                    m_site;
        /// <summary>Date of the game</summary>
        private string?                    m_gameDate;
        /// <summary>Date of the game</summary>
        private DateTime?                  m_date;
        /// <summary>Round</summary>
        private string?                    m_round;
        /// <summary>White Player name</summary>
        private string?                    m_whitePlayerName;
        /// <summary>Black Player name</summary>
        private string?                    m_blackPlayerName;
        /// <summary>White ELO (-1 if none)</summary>
        private int                        m_whiteElo = -1;
        /// <summary>Black ELO (-1 if none)</summary>
        private int                        m_blackElo = -1;
        /// <summary>Game result 1-0, 0-1, 1/2-1/2 or *</summary>
        private string?                    m_gameResult;
        /// <summary>White Human/program</summary>
        private PgnPlayerType              m_whitePlayerType;
        /// <summary>White Human/program</summary>
        private PgnPlayerType              m_blackPlayerType;
        /// <summary>FEN defining the board</summary>
        private string?                    m_fen;
        /// <summary>Time control</summary>
        private string?                    m_timeControl;
        /// <summary>Game termination</summary>
        private string?                    m_termination;
        /// <summary>Time span per x moves or game time when moves = 0</summary>
        private TimeSpan                   m_gameTime;
        /// <summary>Time span per move</summary>
        private TimeSpan                   m_moveTime;
        /// <summary>Moves</summary>
        private int                        m_movesCount;
        /// <summary>Time span increment per move</summary>
        private TimeSpan                   m_increment;
        #endregion

        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="createAttrList"> true to create an attribute list</param>
        /// <param name="createMoveList"> true to create a move list</param>
        public PgnGame(bool createAttrList, bool createMoveList)
        {
            Attrs           = createAttrList ? new Dictionary<string, string>(10, StringComparer.InvariantCultureIgnoreCase) : null;
            PgnMoves        = createMoveList ? new List<PgnMove>(256) : null;
            m_attrRead      = AttrRead.None;
        }
        #endregion

        #region Properties
        /// <summary>Game starting position in the PGN text file</summary>
        public long StartingPos { get; set; }

        /// <summary>Game length in the PGN text file</summary>
        public int Length { get; set; }

        /// <summary>Attributes</summary>
        public Dictionary<string, string>? Attrs { get; set; }

        /// <summary>list of Pgn moves defines as PgnMove object</summary>
        public List<PgnMove>? PgnMoves { get; set; }

        /// <summary>list of Pgn moves defines as PgnMove object</summary>
        public List<Move>? ChessMoves { get; set; }

        /// <summary>
        /// Event
        /// </summary>
        public string? Event
        {
            get
            {
                if (Attrs != null && (m_attrRead & AttrRead.Event) == 0)
                {
                    m_attrRead |= AttrRead.Event;
                    if (!Attrs.TryGetValue("Event", out m_event))
                    {
                        m_event = null;
                    }
                }
                return m_event;
            }
        }

        /// <summary>
        /// Site
        /// </summary>
        public string? Site
        {
            get
            {
                if (Attrs != null && (m_attrRead & AttrRead.Site) == 0)
                {
                    m_attrRead |= AttrRead.Site;
                    if (!Attrs.TryGetValue("Site", out m_site))
                    {
                        m_site = null;
                    }
                }
                return m_site;
            }
        }

        /// <summary>
        /// Round
        /// </summary>
        public string? Round
        {
            get
            {
                if (Attrs != null && (m_attrRead & AttrRead.Round) == 0)
                {
                    m_attrRead |= AttrRead.Round;
                    if (!Attrs.TryGetValue("Round", out m_round))
                    {
                        m_round = null;
                    }
                }
                return m_round;
            }
        }

        /// <summary>
        /// Date of the game as string
        /// </summary>
        public string? Date
        {
            get
            {
                if (Attrs != null && (m_attrRead & AttrRead.GameDate) == 0)
                {
                    m_attrRead |= AttrRead.GameDate;

                    if (!Attrs.TryGetValue("Date", out m_gameDate))
                    {
                        m_gameDate = null;
                    }
                }
                return m_gameDate;
            }
        }

        /// <summary>
        /// Date of the game
        /// </summary>
        public DateTime? DateTime
        {
            get
            {
                if (Attrs != null && (m_attrRead & AttrRead.Date) == 0)
                {
                    m_attrRead |= AttrRead.Date;
                    DateTime value;
                    if (Date != null)
                    {
                        System.DateTime.TryParse(Date.Replace("??", "01"), out value);
                        m_date = value;
                    }
                    else
                        m_date = null;
                }
                return m_date;
            }
        }

        /// <summary>
        /// White Player
        /// </summary>
        public string? WhitePlayerName
        {
            get
            {
                if (Attrs != null && (m_attrRead & AttrRead.WhitePlayer) == 0)
                {
                    m_attrRead |= AttrRead.WhitePlayer;
                    if (!Attrs.TryGetValue("White", out m_whitePlayerName))
                    {
                        m_whitePlayerName = null;
                    }
                }
                return m_whitePlayerName;
            }
        }

        /// <summary>
        /// Black Player
        /// </summary>
        public string? BlackPlayerName
        {
            get
            {
                if (Attrs != null && (m_attrRead & AttrRead.BlackPlayer) == 0)
                {
                    m_attrRead |= AttrRead.BlackPlayer;
                    if (!Attrs.TryGetValue("Black", out m_blackPlayerName))
                    {
                        m_blackPlayerName = null;
                    }
                }
                return m_blackPlayerName;
            }
        }

        /// <summary>
        /// White ELO
        /// </summary>
        public int WhiteElo
        {
            get
            {
                if (Attrs != null && (m_attrRead & AttrRead.WhiteElo) == 0)
                {
                    m_attrRead |= AttrRead.WhiteElo;
                    if (!Attrs.TryGetValue("WhiteElo", out string? txtValue) || !int.TryParse(txtValue, out m_whiteElo))
                    {
                        m_whiteElo = 0;
                    }
                }
                return m_whiteElo;
            }
            set
            {
                m_whiteElo = value;
                Attrs!["WhiteElo"] = value.ToString();
            }
        }

        /// <summary>
        /// Black ELO
        /// </summary>
        public int BlackElo
        {
            get
            {
                if (Attrs != null && (m_attrRead & AttrRead.BlackElo) == 0)
                {
                    m_attrRead |= AttrRead.BlackElo;
                    if (!Attrs.TryGetValue("BlackElo", out string? txtValue) || !int.TryParse(txtValue, out m_blackElo))
                    {
                        m_blackElo = 0;
                    }
                }
                return m_blackElo;
            }
            set
            {
                m_blackElo = value;
                Attrs!["BlackElo"] = value.ToString();
            }
        }

        /// <summary>
        /// Game Result
        /// </summary>
        public string? GameResult
        {
            get
            {
                if (Attrs != null && (m_attrRead & AttrRead.GameResult) == 0)
                {
                    m_attrRead |= AttrRead.GameResult;
                    if (!Attrs.TryGetValue("Result", out m_gameResult))
                    {
                        m_gameResult = "*";
                    }
                }
                return m_gameResult;
            }
            set
            {
                m_gameResult = value;
            }
        }

        /// <summary>
        /// White player type
        /// </summary>
        public PgnPlayerType WhiteType
        {
            get
            {
                if (Attrs != null && (m_attrRead & AttrRead.WhiteType) == 0)
                {
                    m_attrRead |= AttrRead.WhiteType;
                    if (Attrs.TryGetValue("WhiteType", out string? value))
                    {
                        m_whitePlayerType = string.Compare(value, "Program", ignoreCase: true) == 0 ? PgnPlayerType.Program : PgnPlayerType.Human;
                    }
                    else
                    {
                        m_whitePlayerType = PgnPlayerType.Human;
                    }
                }
                return m_whitePlayerType;
            }
        }

        /// <summary>
        /// Black player type
        /// </summary>
        public PgnPlayerType BlackType
        {
            get
            {
                if (Attrs != null && (m_attrRead & AttrRead.BlackType) == 0)
                {
                    m_attrRead |= AttrRead.BlackType;
                    if (Attrs.TryGetValue("BlackType", out string? value))
                    {
                        m_blackPlayerType = string.Compare(value, "Program", ignoreCase: true) == 0 ? PgnPlayerType.Program : PgnPlayerType.Human;
                    }
                    else
                    {
                        m_blackPlayerType = PgnPlayerType.Human;
                    }
                }
                return m_blackPlayerType;
            }
        }

        /// <summary>
        /// FEN defining the board
        /// </summary>
        public string? Fen
        {
            get
            {
                if (Attrs != null && (m_attrRead & AttrRead.Fen) == 0)
                {
                    m_attrRead |= AttrRead.Fen;
                    if (Attrs == null || !Attrs.TryGetValue("Fen", out m_fen))
                    {
                        m_fen = null;
                    }
                }
                return m_fen;
            }
        }

        /// <summary>
        /// Start player
        /// </summary>
        public Player StartPlayer
        {
            get
            {
                if (Fen == null)
                    return Player.White;
                string[] parts = Fen!.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts[1] == "w")
                {
                    return Player.White;
                }
                else if (parts[1] == "b")
                {
                    return Player.Black;
                }
                else
                {
                    return Player.None;
                }
            }
         }


        /// <summary>
        /// Time control
        /// </summary>
        public string? TimeControl
        {
            get
            {
                if (Attrs != null && (m_attrRead & AttrRead.TimeControl) == 0)
                {
                    m_attrRead |= AttrRead.TimeControl;
                    if (!Attrs.TryGetValue("TimeControl", out m_timeControl))
                    {
                        m_timeControl = null;
                    }
                }
                return m_timeControl;
            }
        }

        /// <summary>
        /// Game termination
        /// </summary>
        public string? Termination
        {
            get
            {
                if (Attrs != null && (m_attrRead & AttrRead.Termination) == 0)
                {
                    m_attrRead |= AttrRead.Termination;
                    if (!Attrs.TryGetValue("Termination", out m_termination))
                    {
                        m_termination = null;
                    }
                }
                return m_termination;
            }
        }

        /// <summary>
        /// Time span per x moves or game time when moves = 0
        /// </summary>
        public TimeSpan GameTime
        {
            get
            {
                if ((m_attrRead & AttrRead.GameTime) == 0)
                {
                    m_attrRead |= AttrRead.GameTime;
                    InitPlayerSpan();
                }
                return m_gameTime;
            }
        }

        /// <summary>
        /// Time span per x moves or game time when moves = 0
        /// </summary>
        public TimeSpan MoveTime
        {
            get
            {
                if ((m_attrRead & AttrRead.MoveTime) == 0)
                {
                    m_attrRead |= AttrRead.MoveTime;
                    InitPlayerSpan();
                }
                return m_moveTime;
            }
        }

        /// <summary>
        /// Moves
        /// </summary>
        public int MovesCount
        {
            get
            {
                if ((m_attrRead & AttrRead.Moves) == 0)
                {
                    m_attrRead |= AttrRead.Moves;
                    InitPlayerSpan();
                }
                return m_movesCount;
            }
        }

        /// <summary>
        /// Time span increment per move
        /// </summary>
        public TimeSpan Increment
        {
            get
            {
                if ((m_attrRead & AttrRead.Increment) == 0)
                {
                    m_attrRead |= AttrRead.Increment;
                    InitPlayerSpan();
                }
                return m_increment;
            }
        }

        /// <summary>
        /// Pgn Header
        /// </summary>
        public string? Header
        {
            get
            {
                return PgnUtil.GetPgnHeader(Attrs);
            }
        }

        /// <summary>
        /// Pgn Annotated Moves
        /// </summary>
        public string? AnnotatedMoves
        {
            get
            {
                return PgnUtil.GetPgnMoves(PgnMoves!, StartPlayer, true);
            }
        }

        /// <summary>
        /// Pgn Moves
        /// </summary>
        public string? Moves
        {
            get
            {
                return PgnUtil.GetPgnMoves(PgnMoves!, StartPlayer, false);
            }
        }

        /// <summary>
        /// Current position
        /// </summary>
        public long CurrentPosition { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize the proprietary time control
        /// </summary>
        private void InitPlayerSpan()
        {
            string? timeControl;
            string[] timeControls;
            int poz, timestan;

            m_gameTime = TimeSpan.Zero;
            m_moveTime = TimeSpan.Zero;
            m_movesCount = 0;
            m_increment = TimeSpan.Zero;
            timeControl = TimeControl;
            if (timeControl != null)
            {
                poz = timeControl.IndexOf(":");
                if (poz > -1)
                {
                    timeControl = timeControl.Substring(0, poz);
                }
                timeControls = timeControl.Split('/');
                if (timeControls.Length > 1)
                {
                    Int32.TryParse(timeControls[0], out m_movesCount);
                    timeControls = timeControls[1].Split('+');
                    Int32.TryParse(timeControls[0], out timestan);
                    m_gameTime = TimeSpan.FromSeconds(timestan);
                    if (timeControls.Length > 1)
                    {
                        Int32.TryParse(timeControls[1], out timestan);
                        m_increment = TimeSpan.FromSeconds(timestan);
                    }
                }
                else
                {
                    timeControls = timeControl.Split('+');
                    Int32.TryParse(timeControls[0], out timestan);
                    m_gameTime = TimeSpan.FromSeconds(timestan);
                    if (timeControls.Length > 1)
                    {
                        Int32.TryParse(timeControls[1], out timestan);
                        if (m_gameTime == new TimeSpan())
                        {
                            m_moveTime = TimeSpan.FromSeconds(timestan);
                        }
                        else
                        {
                            m_increment = TimeSpan.FromSeconds(timestan);
                        }
                    }
                }
            }
            m_attrRead |= AttrRead.GameTime | AttrRead.MoveTime | AttrRead.Moves | AttrRead.Increment;
        }
        #endregion
    } // Class PgnGame
} // Namespace
