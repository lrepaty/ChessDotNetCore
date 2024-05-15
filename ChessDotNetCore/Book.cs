using System.Reflection;
using ChessDotNetCore.PgnParsing;

namespace ChessDotNetCore
{
    /// <summary>Handle the book opening.</summary>
    public class Book
    {
        #region Types
        /// <summary>
        /// BookEntry
        /// </summary>
        public struct BookEntry
        {
            /// <summary>ZobristKey</summary>
            public ulong Key;
            /// <summary>Move</summary>
            public ushort Move;
            /// <summary>How many child book entries this one has</summary>
            public ushort Weight;
            /// <summary>learning information</summary>
            public uint Learn;
        }

        /// <summary>
        /// Book key
        /// </summary>
        public struct BookKey : IComparable
        {
            /// <summary>ZobristKey</summary>
            public ulong Key { get; internal set; }
            /// <summary>Move</summary>
            public ushort Move;
            public int CompareTo(object? obj)
            {
                BookKey? bookKey = (BookKey?)obj;
                if (Key > bookKey!.Value.Key || (Key == bookKey!.Value.Key && Move > bookKey!.Value.Move))
                    return 1;
                else if (Key == bookKey!.Value.Key && Move == bookKey!.Value.Move)
                    return 0;
                else
                    return -1;
            }
        }

        /// <summary>
        /// Book value
        /// </summary>
        public struct BookValue
        {
            /// <summary>Wins</summary>
            public int Wins;
            /// <summary>Draws</summary>
            public int Draws;
            /// <summary>Lost</summary>
            public int Lost;
        };

        #endregion

        #region Members
        /// <summary>Size of book entry</summary>
        private const long SIZE_OF_BOOKENTRY = 16;
        /// <summary>Number of Book entrys</summary>
        private long m_size;
        /// <summary>Book entrys</summary>
        protected BookEntry[]? m_bookEntries;
        #endregion

        #region Properties
        /// <summary>List of book entries</summary>
        public BookEntry[]? BookEntries
        {
            get => m_bookEntries;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Read the book from a BinaryReader
        /// </summary>
        private bool ReadBookFromReader(BinaryReader reader)
        {
            bool retVal = false;
            ushort maxWeight;
            if (reader.BaseStream.Length % SIZE_OF_BOOKENTRY != 0)
                throw new Exception("Invalid Book File");
            m_size = (reader.BaseStream.Length / SIZE_OF_BOOKENTRY);
            m_bookEntries = new BookEntry[m_size];

            maxWeight = 0;
            for (long i = 0; i < m_size; i++)
            {
                byte[] t = reader.ReadBytes((int)SIZE_OF_BOOKENTRY);

                m_bookEntries[i].Key =
                    (((UInt64)t[0]) << 56) |
                    (((UInt64)t[1]) << 48) |
                    (((UInt64)t[2]) << 40) |
                    (((UInt64)t[3]) << 32) |
                    (((UInt64)t[4]) << 24) |
                    (((UInt64)t[5]) << 16) |
                    (((UInt64)t[6]) << 8) |
                    (((UInt64)t[7]) << 0);

                m_bookEntries[i].Move = (UInt16)(
                    (((UInt64)t[8]) << 8) |
                    (((UInt64)t[9]) << 0));

                m_bookEntries[i].Weight = (UInt16)(
                    (((UInt64)t[10]) << 8) |
                    (((UInt64)t[11]) << 0));

                m_bookEntries[i].Learn = (UInt32)(
                    (((UInt64)t[12]) << 24) |
                    (((UInt64)t[13]) << 16) |
                    (((UInt64)t[14]) << 8) |
                    (((UInt64)t[15]) << 0));

                if (m_bookEntries[i].Weight > maxWeight)
                    maxWeight = m_bookEntries[i].Weight;

            }
            retVal = true;
            return retVal;
        }

        /// <summary>
        /// Read the book from a binary file
        /// </summary>
        /// <param name="path">   The path string from which to get the extension.</param>
        public bool ReadBookFromFile(string path)
        {
            bool retVal = false;
            FileStream fileStream;
            BinaryReader reader;
            if (Path.GetExtension(path) != ".bin")
                throw new Exception("Invalid Book File");
            if (File.Exists(path))
            {
                using (fileStream = File.OpenRead(path))
                {
                    reader = new BinaryReader(fileStream);
                    retVal = ReadBookFromReader(reader);
                }
            }
            return retVal;
        }

        /// <summary>
        /// Read the book from the specified resource
        /// </summary>
        /// <param name="asm">          Assembly</param>
        /// <param name="resName">      Resource Name</param>
        public bool ReadBookFromResource(Assembly asm, string resName)
        {
            bool retVal = false;
            BinaryReader reader;
            Stream? stream;

            if (asm == null)
            {
                asm = GetType().Assembly;
            }
            stream = asm.GetManifestResourceStream(resName);
            if (stream == null)
                throw new FileNotFoundException();
            using (stream)
            {
                reader = new BinaryReader(stream);
                retVal = ReadBookFromReader(reader);
            }
            return retVal;
        }

        /// <summary>
        /// Read the book from the specified resource
        /// </summary>
        /// <param name="resName">   Resource Name</param>
        public bool ReadBookFromResource(string resName)
        {
            bool retVal;
            Assembly asm;

            asm = GetType().Assembly;
            retVal = ReadBookFromResource(asm, resName);
            return retVal;
        }

        /// <summary>
        /// Save the book to a binary file
        /// </summary>
        /// <param name="path"> The path and name of the file to create.
        public void SaveBookToFile(string path)
        {
            FileStream fileStream;
            BinaryWriter    writer;
            byte[]          bKey  = new byte[8];
            byte[]          bMove = new byte[2];
            byte[]          bWeight = new byte[2];
            byte[]          bLearn= new byte[4];
            ulong           lKey;
            ushort          sMove;
            ushort          sWeight;
            uint            iLearn;

            using (fileStream = File.Create(path))
            {
                writer = new BinaryWriter(fileStream);
                foreach (var bookEntry in m_bookEntries!)
                {
                    lKey        = bookEntry.Key;
                    bKey[7]     = (byte)(lKey % 256);
                    lKey        >>= 8;
                    bKey[6]     = (byte)(lKey % 256);
                    lKey        >>= 8;
                    bKey[5]     = (byte)(lKey % 256);
                    lKey        >>= 8;
                    bKey[4]     = (byte)(lKey % 256);
                    lKey        >>= 8;
                    bKey[3]     = (byte)(lKey % 256);
                    lKey        >>= 8;
                    bKey[2]     = (byte)(lKey % 256);
                    lKey        >>= 8;
                    bKey[1]     = (byte)(lKey % 256);
                    bKey[0]     = (byte)(lKey >> 8);

                    sMove       = bookEntry.Move;
                    bMove[1]    = (byte)(sMove % 256);
                    bMove[0]    = (byte)(sMove >> 8);

                    sWeight     = bookEntry.Weight;
                    bWeight[1]  = (byte)(sWeight % 256);
                    bWeight[0]  = (byte)(sWeight >> 8);

                    iLearn      = bookEntry.Learn;
                    bLearn[3]   = (byte)(iLearn % 256);
                    iLearn      >>= 8;
                    bLearn[2]   = (byte)(iLearn % 256);
                    iLearn      >>= 8;
                    bLearn[1]   = (byte)(iLearn % 256);
                    bLearn[0]   = (byte)(iLearn >> 8);

                    writer.Write(bKey);
                    writer.Write(bMove);
                    writer.Write(bWeight);
                    writer.Write(bLearn);
                }
                m_size = (fileStream.Length / SIZE_OF_BOOKENTRY);
            }
        }

        /// <summary>
        /// Find a move from the book
        /// </summary>
        /// <param name="zobristKey">      ZobristKey</param>
        /// <param name="randomMode">      Random to use to pickup a move from a list.</param>
        /// <returns>
        /// Move in the form of StartPos + (EndPos * 256) or 0 if none found
        /// </returns>
        public ushort FindMoveInBook(ulong zobristKey, bool randomMode)
        {
            ushort result = 0;
            long low, high, mid;
            double best, aktual;
            Random rnd = new();
            low = 0;
            best = 0;
            high = m_size - 1;
            while (low < high)
            {
                mid = (low + high) / 2;

                if (zobristKey <= m_bookEntries![mid].Key)
                    high = mid;
                else
                    low = mid + 1;
            }
            while (low < m_bookEntries!.Length && (m_bookEntries[low].Key == zobristKey))
            {
                aktual = m_bookEntries[low].Weight * (randomMode ? rnd.NextDouble() : 1);

                // Choose book move according to its score. If a move has a very
                // high score it has higher probability to be choosen than a move
                // with lower score. Note that first entry is always chosen
                if (aktual > best)
                {
                    result = m_bookEntries[low].Move;
                    best = aktual;
                }
                low++;
            }
            return result;
        }

        /// <summary>
        /// Create the book entries from a series of move list
        /// </summary>
        /// <param name="soredListBookKeyList">     List of BookKeys</param>
        /// <param name="minWeight">                Minimal games</param>
        /// <param name="minGames">                 Minimal weight</param>
        /// <param name="callback">                 Callback to call to show progress</param>
        /// <param name="cookie">                   Cookie for callback</param>
        /// <returns>
        /// Nb of entries created
        /// </returns>
        public int CreateBookList(SortedList<BookKey, BookValue> soredListBookKeyList, int minWeight, int minGames, PgnParser.DelProgressCallBack callback, object cookie)
        {
            m_bookEntries = soredListBookKeyList
                .Where(item => item.Value.Wins + item.Value.Draws + item.Value.Lost >= minGames)
                .OrderBy(item => item.Key.Key)
                .Select(item => new BookEntry()
                {
                    Key = item.Key.Key,
                    Move = item.Key.Move,
                    Weight = (ushort)((item.Value.Wins * 2 + item.Value.Draws) * 1000 / (item.Value.Wins * 2 + item.Value.Draws + item.Value.Lost * 2)),
                    Learn = 0,
                }).Where(item => item.Weight >= minWeight)
                .OrderBy(item => item.Key)
                .ThenByDescending(item => item.Weight)
                .Select(item => new BookEntry()
                {
                    Key = item.Key,
                    Move = item.Move,
                    Weight = (ushort)(item.Weight - minWeight),
                    Learn = 0,
                }).ToArray();

            if (callback != null)
            {
                callback(cookie, PgnParser.ParsingPhase.CreatingBook, 0, 0, null, m_bookEntries.Length, m_bookEntries.Length);
            }
            return m_bookEntries.Length;
        }
        #endregion
    } // Class Book
} // Namespace
