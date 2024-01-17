using System.Text;
using Newtonsoft.Json;

namespace ChessDotNetCore
{
    /// <summary>
    /// Defines class ChessUtilities
    /// </summary>
    public static class ChessUtilities
    {
        #region Methods
        /// <summary>
        /// Throw if value is null
        /// </summary>
        /// <param name="value">            Value</param>
        /// <param name="parameterName">    Parameter name</param>
        public static void ThrowIfNull(object? value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Get opponent of Player
        /// </summary>
        /// <param name="player">           Player</param>
        /// <returns>
        /// Opponet
        /// </returns>
        public static Player GetOpponentOf(Player player)
        {
            if (player == Player.None)
                throw new ArgumentException("`player` cannot be Player.None.");
            return player == Player.White ? Player.Black : Player.White;
        }

        /// <summary>
        /// Get list files between file1 and file2
        /// </summary>
        /// <param name="file1">            Line from</param>
        /// <param name="file2">            Line to</param>
        /// <param name="file1Inclusive">   Inclusuve file1</param>
        /// <param name="file2Inclusive">   Inclusuve file2</param>
        /// <returns>
        /// List of files
        /// </returns>
        public static Line[] LinesBetween(Line file1, Line file2, bool file1Inclusive, bool file2Inclusive)
        {
            if (file1 == file2)
            {
                if (file1Inclusive && file2Inclusive) { return new Line[] { file1 }; }
                else { return new Line[] { }; }
            }
            int min = Math.Min((int)file1, (int)file2);
            int max = Math.Max((int)file1, (int)file2);
            bool minInc;
            bool maxInc;
            if (min == (int)file1)
            {
                minInc = file1Inclusive;
                maxInc = file2Inclusive;
            }
            else
            {
                maxInc = file1Inclusive;
                minInc = file2Inclusive;
            }
            var files = new Line[] { Line.a, Line.b, Line.c, Line.d, Line.e, Line.f, Line.g, Line.h };
            return [..files.Skip(min + (minInc ? 0 : 1)).Take(max - min + (maxInc ? 1 : 0) - (minInc ? 0 : 1))];
        }

        /// <summary>
        /// Get Fen for Chess960 starting array
        /// </summary>
        /// <param name="n">                Number</param>
        /// <returns>
        /// Fen
        /// </returns>
        private static string FenForChess960StartingArray(int n)
        {
            var fenParts = new string[8];

            int n2 = n / 4;
            int b1 = n % 4;
            fenParts[1 + b1 * 2] = "B";

            int n3 = n2 / 4;
            int b2 = n2 % 4;
            fenParts[b2 * 2] = "B";

            int n4 = n3 / 6;
            int q = n3 % 6;

            int free = 0;
            for (int i = 0; i < fenParts.Length; i++)
            {
                if (fenParts[i] == null)
                {
                    if (free == q)
                    {
                        fenParts[i] = "Q";
                        break;
                    }
                    free++;
                }
            }

            var knightPositioning = new bool[][]
            {
                new bool[] { true, true, false, false, false },
                new bool[] { true, false, true, false, false },
                new bool[] { true, false, false, true, false },
                new bool[] { true, false, false, false, true },
                new bool[] { false, true, true, false, false },
                new bool[] { false, true, false, true, false },
                new bool[] { false, true, false, false, true },
                new bool[] { false, false, true, true, false },
                new bool[] { false, false, true, false, true },
                new bool[] { false, false, false, true, true }
            }[n4];
            int knightPosCounter = 0;
            for (int i = 0; i < fenParts.Length; i++)
            {
                if (fenParts[i] == null)
                {
                    if (knightPositioning[knightPosCounter])
                    {
                        fenParts[i] = "N";
                    }
                    knightPosCounter++;
                }
            }

            free = 0;
            for (int i = 0; i < fenParts.Length; i++)
            {
                if (fenParts[i] == null)
                {
                    switch (free)
                    {
                        case 0:
                            fenParts[i] = "R";
                            break;
                        case 1:
                            fenParts[i] = "K";
                            break;
                        case 2:
                            fenParts[i] = "R";
                            break;
                    }
                    free++;
                    if (free > 2)
                    {
                        break;
                    }
                }
            }

            return string.Join("", fenParts);
        }

        /// <summary>
        /// Get Fen for Chess960 symmetrical
        /// </summary>
        /// <param name="n">                Number</param>
        /// <returns>
        /// Fen
        /// </returns>
        public static string FenForChess960Symmetrical(int n)
        {
            if (n < 0 || n > 959)
            {
                throw new ArgumentException("'n' must be greater than or equal to 0, and smaller than or equal to 959.");
            }

            string startingPos = FenForChess960StartingArray(n);
            return $"{startingPos.ToLower()}/pppppppp/8/8/8/8/PPPPPPPP/{startingPos} w KQkq - 0 1";
        }

        /// <summary>
        /// Get Fen for Chess960 asymmetrical
        /// </summary>
        /// <param name="n">                Number</param>
        /// <returns>
        /// Fen
        /// </returns>
        public static string FenForChess960Asymmetrical(int nWhite, int nBlack)
        {
            if (nWhite < 0 || nWhite > 959 || nBlack < 0 || nBlack > 959)
            {
                throw new ArgumentException("'nWhite' and 'nBlack' must be greater than or equal to 0, and smaller than or equal to 959.");
            }

            string startingPosWhite = FenForChess960StartingArray(nWhite);
            string startingPosBlack = FenForChess960StartingArray(nBlack).ToLower();
            return $"{startingPosBlack}/pppppppp/8/8/8/8/PPPPPPPP/{startingPosWhite} w KQkq - 0 1";
        }

        /// <summary>
        /// Get Fen for Horde960
        /// </summary>
        /// <param name="n">                Number</param>
        /// <returns>
        /// Fen
        /// </returns>
        public static string FenForHorde960(int n)
        {
            if (n < 0 || n > 959)
            {
                throw new ArgumentException("'n' must be greater than or equal to 0, and smaller than or equal to 959.");
            }

            string startingPos = FenForChess960StartingArray(n);
            return $"{startingPos.ToLower()}/pppppppp/8/1PP2PP1/PPPPPPPP/PPPPPPPP/PPPPPPPP/PPPPPPPP w kq - 0 1";
        }

        // https://github.com/ProgramFOX/Chess.NET/wiki/Algorithm-for-RacingKings1440-positions
        /// <summary>
        /// Get White starting array
        /// </summary>
        /// <param name="n">                Number</param>
        /// <returns>
        /// Starting array for white
        /// </returns>
        private static string[] RK1440WhiteStartingArray(int n)
        {
            var setup = new string[][] { new string[4], new string[4] };

            int n2 = n / 4;
            int k = n % 4;

            switch (k)
            {
                case 0:
                    setup[0][3] = "K";
                    break;
                case 1:
                    setup[1][3] = "K";
                    break;
                case 2:
                    setup[1][2] = "K";
                    break;
                case 3:
                    setup[0][2] = "K";
                    break;
            }

            int n3 = n2 / 3;
            int b1 = n2 % 3;

            var possibleB1Squares = k % 2 == 0 ? new int[][] { new int[] { 0, 1 }, new int[] { 0, 3 }, new int[] { 1, 2 }, new int[] { 1, 0 } }
                                                   : new int[][] { new int[] { 0, 0 }, new int[] { 0, 2 }, new int[] { 1, 3 }, new int[] { 1, 1 } };
            int counter = 0;
            for (int i = 0; i < possibleB1Squares.Length; i++)
            {
                int[] curr = possibleB1Squares[i];
                if (setup[curr[0]][curr[1]] == null)
                {
                    if (counter == b1)
                    {
                        setup[curr[0]][curr[1]] = "B";
                        break;
                    }
                    counter++;
                }
            }

            int n4 = n3 / 4;
            int b2 = n3 % 4;
            var possibleB2Squares = k % 2 != 0 ? new int[][] { new int[] { 0, 1 }, new int[] { 0, 3 }, new int[] { 1, 2 }, new int[] { 1, 0 } }
                                                   : new int[][] { new int[] { 0, 0 }, new int[] { 0, 2 }, new int[] { 1, 3 }, new int[] { 1, 1 } };
            int[] chosenB2Square = possibleB2Squares[b2];
            setup[chosenB2Square[0]][chosenB2Square[1]] = "B";

            int n5 = n4 / 5;
            int q = n4 % 5;
            var possibleQNRSquares = new int[][] { new int[] { 0, 0 }, new int[] { 0, 1 }, new int[] { 0, 2 }, new int[] { 0, 3 }, new int[] { 1, 3 }, new int[] { 1, 2 }, new int[] { 1, 1 }, new int[] { 1, 0 } };
            counter = 0;
            for (int i = 0; i < possibleQNRSquares.Length; i++)
            {
                int[] curr = possibleQNRSquares[i];
                if (setup[curr[0]][curr[1]] == null)
                {
                    if (counter == q)
                    {
                        setup[curr[0]][curr[1]] = "Q";
                        break;
                    }
                    counter++;
                }
            }

            string[] remainingConfiguration = new string[][]
            {
                new string[] { "N", "N", "R", "R" },
                new string[] { "N", "R", "N", "R" },
                new string[] { "N", "R", "R", "N" },
                new string[] { "R", "N", "N", "R" },
                new string[] { "R", "N", "R", "N" },
                new string[] { "R", "R", "N", "N" }
            }[n5];
            counter = 0;
            for (int i = 0; i < possibleQNRSquares.Length; i++)
            {
                int[] curr = possibleQNRSquares[i];
                if (setup[curr[0]][curr[1]] == null)
                {
                    setup[curr[0]][curr[1]] = remainingConfiguration[counter];
                    counter++;
                }
            }

            return [..setup.Select(x => string.Join("", x))];
        }

        /// <summary>
        /// Get Fen for RacingKings1440 symmetrical
        /// </summary>
        /// <param name="n">                Number</param>
        /// <returns>
        /// Fen
        /// </returns>
        public static string FenForRacingKings1440Symmetrical(int n)
        {
            string[] whiteRows = RK1440WhiteStartingArray(n);
            string[] blackRows = new string[] { string.Concat(whiteRows[0].ToLowerInvariant().Reverse()), string.Concat(whiteRows[1].ToLowerInvariant().Reverse()) };
            return $"8/8/8/8/8/8/{blackRows[0]}{whiteRows[0]}/{blackRows[1]}{whiteRows[1]} w - - 0 1";
        }

        /// <summary>
        /// Get Fen for RacingKings1440 asymmetrical
        /// </summary>
        /// <param name="n">                Number</param>
        /// <returns>
        /// Fen
        /// </returns>
        public static string FenForRacingKings1440Asymmetrical(int nWhite, int nBlack)
        {
            string[] whiteRows = RK1440WhiteStartingArray(nWhite);

            string[] whiteRowsForBlack = RK1440WhiteStartingArray(nBlack);
            var blackRows = new string[] { string.Concat(whiteRowsForBlack[0].ToLowerInvariant().Reverse()), string.Concat(whiteRowsForBlack[1].ToLowerInvariant().Reverse()) };
            return $"8/8/8/8/8/8/{blackRows[0]}{whiteRows[0]}/{blackRows[1]}{whiteRows[1]} w - - 0 1";
        }

        /// <summary>
        /// Get position at bordIndex
        /// </summary>
        /// <param name="bordIndex">   Bord index</param>
        /// <returns>
        /// Position
        /// </returns>
        public static Position GetPositionAt(byte bordIndex)
        {
            Line file = (Line)(bordIndex % 8);
            byte rank = (byte)(1 + bordIndex / 8);

            return new Position(file, rank);
        }

        /// <summary>
        /// Get bord index at position
        /// </summary>
        /// <param name="position">   Position</param>
        /// <returns>
        /// Bord index
        /// </returns>
        public static byte GetBoardIndexAt(Position position)
        {
            byte boardindex;
            boardindex = (byte)((position.Rank - 1) * 8 + (byte)position.Line);

            return boardindex;
        }

        /// <summary>
        /// Convert Move to ushort fok Book
        /// </summary>
        /// <param name="move">   Move</param>
        /// <returns>
        /// Book move
        /// </returns>
        public static ushort MoveToBookMove(Move move)
        {
            ushort retVal;
            int originalBoardIndex;
            int newBoardIndex;
            originalBoardIndex = GetBoardIndexAt(move.OriginalPosition);
            newBoardIndex = GetBoardIndexAt(move.NewPosition);
            retVal = (ushort)(originalBoardIndex * 64 + newBoardIndex);
            if (move.Promotion != null)
            {
                switch (move.Promotion)
                {
                    case 'Q':
                        retVal += 16384;
                        break;
                    case 'R':
                        retVal += 12288;
                        break;
                    case 'B':
                        retVal += 8192;
                        break;
                    case 'N':
                        retVal += 4096;
                        break;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Convert Book move to Move
        /// </summary>
        /// <param name="bookmove">   Book move</param>
        /// <returns>
        /// move
        /// </returns>
        public static Move BookMoveToMove(ushort bookmove, Player currentPlayer, bool moveFromBook = false)
        {
            Move retVal;
            Position originalPosition;
            Position newPosition;
            char? promotion;
            originalPosition = GetPositionAt((byte)(bookmove % 4096 >> 6));
            newPosition = GetPositionAt((byte)(bookmove % 64));
            promotion = null;
            switch (bookmove >> 12)
            {
                case 4:
                    promotion = 'Q';
                    break;
                case 3:
                    promotion = 'R';
                    break;
                case 2:
                    promotion = 'B';
                    break;
                case 1:
                    promotion = 'N';
                    break;
            }
            retVal = new Move(originalPosition, newPosition, currentPlayer, promotion, TimeSpan.Zero, moveFromBook);
            return retVal;
        }

        /// JsonSerialize to path
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <param name="path">The path and name of the file to create</param>
        /// <param name="encoding">Encoding</param>
        public static void JsonSerializePath(object obj, string path)
        {
            JsonSerializer serializer = new();
            using (var fs = new FileStream(path, FileMode.Create))
            {
                StreamWriter streamWriter;
                streamWriter = new StreamWriter(fs);
                serializer.Serialize(streamWriter, obj);
                streamWriter.Dispose();
            }
        }

        /// <summary>
        /// JsonDeserialize to object from path
        /// </summary>
        /// <param name="path">The path and name of the file</param>
        /// <returns>
        /// Object Deserialized from path
        /// </returns>
        public static T? JsonDeserializePath<T>(string path)
        {
            JsonSerializer serializer = new();
            T? t;
            using (var fs = new FileStream(path, FileMode.Open))
            {
                StreamReader streamReader;
                streamReader = new StreamReader(fs);
                t = (T?)serializer.Deserialize(streamReader, typeof(T));
            }
            return t;
        }
        #endregion
    }
}
