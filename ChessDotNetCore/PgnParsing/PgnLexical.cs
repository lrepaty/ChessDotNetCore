using System.Text;

namespace ChessDotNetCore.PgnParsing
{
    /// <summary>
    /// Do the lexical analysis of a PGN document
    /// </summary>
    public class PgnLexical
    {

        #region Types
        /// <summary>
        /// Token type
        /// </summary>
        public enum TokenType : byte
        {
            /// <summary>Integer value</summary>
            Integer,
            /// <summary>String value</summary>
            String,
            /// <summary>Symbol</summary>
            Symbol,
            /// <summary>Single DOT</summary>
            Dot,
            /// <summary>NAG value</summary>
            Nag,
            /// <summary>Openning square bracket</summary>
            OpenSBracket,
            /// <summary>Closing square bracket</summary>
            CloseSBracket,
            /// <summary>Termination symbol</summary>
            Termination,
            /// <summary>Dash symbol</summary>
            Dash,
            /// <summary>Unknown token</summary>
            UnknownToken,
            /// <summary>Move suffix annotations</summary>
            Annotation,
            /// <summary>Comment</summary>
            Comment,
            /// <summary>End of file</summary>
            Eof
        }

        /// <summary>
        /// Token value
        /// </summary>
        public struct Token
        {
            /// <summary>Token type</summary>
            public  TokenType Type;
            /// <summary>Token string value if any</summary>
            public  string    StrValue;
            /// <summary>Token integer value if any</summary>
            public  int       IntValue;
            /// <summary>Token starting position</summary>
            public  long      StartPos;
            /// <summary>Token size</summary>
            public  int       Size;
        }
        #endregion

        #region Members
        /// <summary>Maximal bufferSize</summary>
        private const int     MaxBufferSize = 1048576;
        /// <summary>List of buffers</summary>
        private List<Char[]?> m_buffer;
        /// <summary>Position in the buffer</summary>
        private int           m_posInBuffer;
        /// <summary>Position in the list</summary>
        private int           m_posInList;
        /// <summary>Current array</summary>
        private char[]?       m_curArray;
        /// <summary>Current array size</summary>
        private int           m_curArraySize;
        /// <summary>Position within the raw array</summary>
        private long          m_curBasePos;
        /// <summary>Text size</summary>
        private long          m_textSize;
        /// <summary>Pushed character if any</summary>
        private Char?         m_chrPushed;
        /// <summary>true if at the first character of a line</summary>
        private bool          m_isFirstChrInLine;
        /// <summary>Pushed token</summary>
        private Token?        m_tokPushed;
        #endregion

        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public PgnLexical()
        {
            m_buffer = new List<char[]?>(0);
            Clear(allocateEmpty: true);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Current position
        /// </summary>
        public long CurrentPosition => m_curBasePos + m_posInBuffer;

        /// <summary>
        /// Text size
        /// </summary>
        public long TextSize => m_textSize;

        /// <summary>
        /// Gets the number of buffer which has been allocated
        /// </summary>
        public int BufferCount => m_buffer.Count;

        /// <summary>
        /// Current buffer position
        /// </summary>
        public int CurrentBufferPos => m_posInList;

        /// <summary>
        /// Game Result
        /// </summary>
        public string? GameResult { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Clear all buffers
        /// </summary>
        /// <param name="allocateEmpty">   true to allocate an empty block</param>
        public void Clear(bool allocateEmpty)
        {
            m_buffer = new List<char[]?>(256);
            m_posInBuffer = 0;
            m_posInList = 0;
            m_curBasePos = 0;
            m_textSize = 0;
            m_chrPushed = null;
            m_tokPushed = null;
            m_isFirstChrInLine = true;
            if (allocateEmpty)
            {
                m_buffer.Add(Array.Empty<char>());
            }
        }

        /// <summary>
        /// Initialize the buffer from a file
        /// </summary>
        /// <param name="inpFileName">  File name to open</param>
        /// <param name="startpos">     Starting position</param>
        /// <returns>
        /// Stream or null if unable to open the file.
        /// </returns>
        public bool InitFromFile(string inpFileName, long startpos = 0)
        {
            bool          retVal       = false;
            StreamReader? streamReader;

            try
            {
                using (FileStream? streamInp = File.OpenRead(inpFileName))
                {
                    if (streamInp != null)
                    {
                        streamReader = new StreamReader(streamInp, Encoding.GetEncoding("utf-8"), true, 65536);
                        ReadInMemory(streamReader, startpos);
                        retVal = true;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Unable to read the file - {inpFileName}.\r\n" + ex.Message);
            }
            return retVal;
        }

        /// <summary>
        /// Initialize from string
        /// </summary>
        /// <param name="text"> Text string</param>
        public void InitFromString(string text)
        {
            Clear(allocateEmpty: false);
            m_buffer.Add([..text]);
            m_curArray     = m_buffer[0];
            m_curArraySize = m_curArray!.Length;
            m_textSize     = m_curArraySize;
        }

        /// <summary>
        /// Fill the buffer
        /// </summary>
        /// <param name="streamReader"> StreamReader</param>
        /// <param name="startpos">     Starting position</param>
        private void ReadInMemory(StreamReader streamReader, long startpos = 0)
        {
            char[] arr;
            char[] tmpArr;
            int    readSize;

            Clear(allocateEmpty: false);
            arr        = new char[MaxBufferSize];
            readSize   = streamReader.ReadBlock(arr, 0, MaxBufferSize);
            m_textSize = 0;
            while (readSize == MaxBufferSize)
            {
                m_textSize += MaxBufferSize;
                m_buffer.Add(arr);
                arr      = new char[MaxBufferSize];
                readSize = streamReader.ReadBlock(arr, 0, MaxBufferSize);
            }
            if (readSize != 0)
            {
                m_textSize += readSize;
                tmpArr      = new char[readSize];
                for (int i = 0; i < readSize; i++)
                {
                    tmpArr[i] = arr[i];
                }
                m_buffer.Add(tmpArr);
            }
            if (m_buffer.Count == 0)
            {
                m_buffer.Add(Array.Empty<char>());
            }
            m_posInList = (int)(startpos / MaxBufferSize);
            m_curBasePos = m_posInList * MaxBufferSize;
            m_posInBuffer = (int)(startpos - m_curBasePos);
            m_curArray     = m_buffer[m_posInList];
            m_curArraySize = m_curArray!.Length;
        }

        /// <summary>
        /// Select the next buffer in list
        /// </summary>
        /// <returns>
        /// true if succeed, false if EOF
        /// </returns>
        private bool SelectNextBuffer()
        {
            bool retVal;

            if (m_posInList + 1 < m_buffer.Count)
            {
                m_curBasePos  += m_curArray!.Length;
                m_curArray     = m_buffer[++m_posInList];
                m_posInBuffer  = 0;
                m_curArraySize = m_curArray!.Length;
                retVal         = true;
            }
            else
            {
                retVal         = false;
            }
            return retVal;
        }

        /// <summary>
        /// Select the prev character
        /// </summary>
        /// <returns>
        /// true if succeed, false if EOF
        /// </returns>
        public bool SelectPrevChr()
        {
            m_posInBuffer--;
            if (m_posInBuffer < 0)
            {
                return SelectPrevBuffer();
            }
            return true;
        }

        /// <summary>
        /// Select the prev buffer in list
        /// </summary>
        /// <returns>
        /// true if succeed, false if EOF
        /// </returns>
        private bool SelectPrevBuffer()
        {
            bool retVal;

            if (m_posInList > 0)
            {

                m_curArray = m_buffer[--m_posInList];
                m_curBasePos -= m_curArray!.Length;
                m_posInBuffer += m_curArray!.Length;
                m_curArraySize = m_curArray!.Length;
                retVal = true;
            }
            else
            {
                retVal = false;
            }
            return retVal;
        }

        /// <summary>
        /// Peek a character
        /// </summary>
        /// <returns>
        /// Character or 0 if EOF
        /// </returns>
        public char PeekChr()
        {
            char   retVal;
            char[] arr;

            if (m_chrPushed.HasValue)
            {
                retVal = m_chrPushed.Value;
            }
            else if (m_posInBuffer < m_curArraySize)
            {
                retVal = m_curArray![m_posInBuffer];
            }
            else if (m_posInList + 1 < m_buffer.Count)
            {
                arr    = m_buffer[m_posInList + 1]!;
                retVal = (arr.Length == 0) ? '\0' : arr[0];
            }
            else
            {
                retVal = '\0';
            }
            return retVal;
        }

        /// <summary>
        /// Get the next character
        /// </summary>
        /// <returns>
        /// Character or 0 if EOF
        /// </returns>
        private char GetChrInt()
        {
            char retVal;

            if (m_chrPushed.HasValue)
            {
                retVal      = m_chrPushed.Value;
                m_chrPushed = null;
            }
            else if (m_posInBuffer < m_curArraySize)
            {
                retVal      = m_curArray![m_posInBuffer++];
            }
            else if (SelectNextBuffer())
            {
                if (m_curArraySize > 0)
                {
                    m_posInBuffer = 1;
                    retVal        = m_curArray![0];
                }
                else
                {
                    m_posInBuffer = 0;
                    retVal        = '\0';
                }
            }
            else
            {
                retVal = '\0';
            }
            if (retVal == '\r')
            {
                m_isFirstChrInLine = true;
            }
            else if (retVal != '\n')
            {
                m_isFirstChrInLine = false;
            }
            return retVal;
        }

        /// <summary>
        /// Push back a character
        /// </summary>
        /// <param name="chr">  Character to push</param>
        public void PushChr(Char chr)
        {
            if (m_chrPushed == null)
            {
                m_chrPushed = chr;
            }
            else
            {
                throw new MethodAccessException("Cannot push two characters!");
            }
        }

        /// <summary>
        /// Skip whitespace
        /// </summary>
        public void SkipSpace()
        {
            char chr;
            bool isNextArray;

            if (!m_chrPushed.HasValue || (chr = m_chrPushed.Value) == ' ' || chr == '\r' || chr == '\n' || chr == (char)9)
            {
                m_chrPushed = null;
                do
                {
                    while (m_posInBuffer < m_curArraySize && ((chr = m_curArray![m_posInBuffer]) == ' ' || chr == '\r' || chr == '\n' || chr == (char)9))
                    {
                        if (chr == '\r')
                        {
                            m_isFirstChrInLine = true;
                        }
                        else if (chr != '\n')
                        {
                            m_isFirstChrInLine = false;
                        }
                        m_posInBuffer++;
                    }
                    if (m_posInBuffer < m_curArraySize)
                    {
                        isNextArray  = false;
                    }
                    else
                    {
                        isNextArray  = SelectNextBuffer();
                    }
                } while (isNextArray);
            }
        }

        /// <summary>
        /// Skip the rest of the line
        /// </summary>
        private void SkipLine()
        {
            char chr;

            do
            {
                chr = GetChrInt();
            }
            while (chr != '\r' && chr != '\0');
            while (PeekChr() == '\n')
            {
                GetChrInt();
            }
            m_isFirstChrInLine = true;
        }

        /// <summary>
        /// Get a character
        /// </summary>
        /// <returns>
        /// Character
        /// </returns>
        public char GetChr()
        {
            char retVal;
            bool toContinue;

            do
            {
                retVal     = GetChrInt();
                toContinue = (m_isFirstChrInLine && (retVal == ';' || retVal == '%'));
                if (toContinue)
                {
                    SkipLine();
                }
            } while (toContinue);
            return retVal;
        }

        /// <summary>
        /// Select Next OpenSBracket
        /// </summary>
        public void SelectNextOpenSBracket()
        {
            char chr = ' ';
            while (chr != '[' && chr != '\0')
            {
                chr = GetChr();
            }
            SelectPrevChr();
        }

        /// <summary>
        /// Select Next Event
        /// </summary>
        public void SelectNextEvent()
        {
            char chr = ' ';
            bool toContinue = true;
            StringBuilder strb = new StringBuilder();
            while (chr != '\0' && toContinue)
            {
                chr = GetChr();
                strb.Append(chr);
                if (strb.ToString().Contains("[Event "))
                {
                    m_posInBuffer -= 7;
                    if (m_posInBuffer < 0)
                    {
                        SelectPrevBuffer();
                    }
                    toContinue = false;
                }
            }
        }

        /// <summary>
        /// Gets the string at the specified position
        /// </summary>
        /// <param name="startingPos"> Starting position in text</param>
        /// <param name="length">      String size</param>
        /// <returns>
        /// String or null if bad position specified
        /// </returns>
        public string? GetStringAtPos(long startingPos, int length)
        {
            string?       retVal;
            int           posInBuf;
            int           posInList;
            int           maxSize;
            int           size;
            char[]?       arr;
            StringBuilder strb;

            if (length > MaxBufferSize)
            {
                throw new ArgumentException("Length too big");
            }
            else if (length == 0)
            {
                retVal = "<empty>";
            }
            else
            {
                strb      = new StringBuilder(length + 1);
                posInList = (int)(startingPos / MaxBufferSize);
                posInBuf  = (int)(startingPos % MaxBufferSize);
                if (posInList < m_buffer.Count)
                {
                    arr     = m_buffer[posInList];
                    maxSize = arr!.Length - posInBuf;
                    if (length <= maxSize)
                    {
                        strb.Append(arr, posInBuf, length);
                    }
                    else if (posInList < m_buffer.Count)
                    {
                        if (posInBuf + maxSize > arr.Length)
                            size = arr.Length - posInBuf;
                        else
                            size = maxSize;
                        strb.Append(arr, posInBuf, size);
                        if (m_buffer.Count > posInList + 1)
                        {
                            if (m_buffer[posInList + 1]!.Length < length - maxSize)
                                strb.Append(m_buffer[posInList + 1], 0, m_buffer[posInList + 1]!.Length);
                            else
                                strb.Append(m_buffer[posInList + 1], 0, length - maxSize);
                        }
                    }
                }
                retVal = (posInList == -1) ? null : strb.ToString();
            }
            return retVal;
        }

        /// <summary>
        /// Flush old buffer to save memory
        /// </summary>
        public void FlushOldBuffer()
        {
            int index;

            index  = m_posInList - 2;
            while (index >= 0 && m_buffer[index] != null)
            {
                m_buffer[index] = null;
                index--;
            }
        }

        /// <summary>
        /// Fetch a string token
        /// </summary>
        /// <returns>
        /// String
        /// </returns>
        private string GetStringToken()
        {
            char            chr;
            StringBuilder   strb;
            string          str;
            int             pos;

            strb = new StringBuilder();
            do
            {
                chr = GetChr();
                switch(chr)
                {
                    case '\r':
                    case '\n':
                        break;
                    case '\0':
                        throw new PgnParserException("Missing string termination quote");
                    case '\\':
                        chr = GetChr();
                        if (chr == '"')
                        {
                            strb.Append(chr);
                        }
                        else
                        {
                            strb.Append('\\');
                            strb.Append(chr);
                        }
                        break;
                    default:
                        strb.Append(chr);
                        break;
                }
            } while (chr != '\r' && chr != '\n');
            str = strb.ToString();
            pos = str.LastIndexOf("\"");
            if (pos > -1)
            {
                m_posInBuffer -= str.Length - pos;
                if (m_posInBuffer < 0)
                    SelectPrevBuffer();
                str = str.Substring(0, pos);
            }
            else
            {
                SelectPrevChr();
            }
            return str;
        }

        /// <summary>
        /// Get an integer
        /// </summary>
        /// <param name="firstChr">    First character</param>
        /// <returns>
        /// Integer value
        /// </returns>
        private int GetNagToken(char firstChr)
        {
            int  retVal;
            char chr;

            retVal = (firstChr - '0');
            while ((chr = GetChr()) >= '0' && chr <= '9' || chr == ')')
            {
                if (chr >= '0' && chr <= '9')
                    retVal = retVal * 10 + (chr - '0');
            }
            PushChr(chr);
            return retVal;
        }

        /// <summary>
        /// Get an annotation
        /// </summary>
        /// <param name="firstChr">    First character</param>
        /// <returns>
        /// string value
        /// </returns>
        private string GetAnnotationToken(Char firstChr)
        {
            string retVal;
            Char chr;

            retVal = firstChr.ToString();
            while ((chr = GetChr()) == '!' || chr == '?')
            {
                retVal = retVal + chr;
            }
            PushChr(chr);
            return retVal;
        }

        /// <summary>
        /// Fetch a string token
        /// </summary>
        /// <param name="firstChr">    First character</param>
        /// <returns>
        /// String
        /// </returns>
        private string GetCommentToken(char firstChr)
        {
            Char chr;
            Char lastChr;
            int iParCount;
            if (firstChr == '{')
                lastChr = '}';
            else
                lastChr = ')';
            StringBuilder strb;
            string ret;
            strb = new StringBuilder();
            strb.Append(firstChr);
            iParCount = 1;
            while ((chr = GetChr()) != 0 && iParCount > 0 && chr != '\0')
            {
                if (chr == firstChr)
                    iParCount++;
                if (chr == lastChr)
                    iParCount--;
                strb.Append(chr);
                if (strb.ToString().Contains("\n[Event"))
                {
                    m_posInBuffer -= 7;
                    if (m_posInBuffer < 0)
                    {
                        SelectPrevBuffer();
                    }
                    throw new Exception($"Invalid Comment = {strb}");
                }
            }
            if (chr == '\0')
            {
                throw new Exception($"Invalid Comment = {strb}");
            }
            ret = strb.ToString();
            return ret;
        }

        /// <summary>
        /// Fetch a symbol token
        /// </summary>
        /// <param name="firstChr">     First character</param>
        /// <param name="isAllDigit">   true if symbol is only composed of digit</param>
        /// <param name="isSlashFound"> Found a slash in the symbol. Only valid for 1/2-1/2</param>
        /// <returns>
        /// Symbol
        /// </returns>
        private string GetSymbolToken(Char firstChr, out bool isAllDigit, out bool isSlashFound)
        {
            char          chr;
            StringBuilder strb;

            isSlashFound = false;
            isAllDigit   = (firstChr >= '0' && firstChr <= '9');
            strb         = new StringBuilder();
            strb.Append(firstChr);
            chr          = GetChr();
            while ((chr >= 'a' && chr <= 'z')   ||
                   (chr >= 'A' && chr <= 'Z')   ||
                   (chr >= '0' && chr <= '9')   ||
                   (chr == '_')                 ||
                   (chr == '+')                 ||
                   (chr == '#')                 ||
                   (chr == '=')                 ||
                   (chr == ':')                 ||
                   (chr == '-')                 ||
                   (chr == '/'))
            {
                if (chr == '/')
                {
                    isSlashFound = true;
                }
                strb.Append(chr);
                if (isAllDigit && (chr < '0' || chr > '9'))
                {
                    isAllDigit = false;
                }
                chr = GetChr();
            }
            PushChr(chr);
            return strb.ToString();
        }

        /// <summary>
        /// Get the next token
        /// </summary>
        /// <returns>
        /// Token
        /// </returns>
        public Token GetNextToken()
        {
            Token retVal;
            char  chr;

            if (m_tokPushed.HasValue)
            {
                retVal      = m_tokPushed.Value;
                m_tokPushed = null;
            }
            else
            {
                retVal   = new();
                SkipSpace();
                retVal.StartPos = CurrentPosition;
                chr             = GetChr();
                switch(chr)
                {
                    case '\0':
                        retVal.Type = TokenType.Eof;
                        break;
                    case '\"':
                        retVal.Type     = TokenType.String;
                        retVal.StrValue = GetStringToken();
                        retVal.Size     = (int)(CurrentPosition - retVal.StartPos);
                        break;
                    case '.':
                        retVal.Type = TokenType.Dot;
                        while (PeekChr() == '.')
                        {
                            GetChr();
                        }
                        retVal.Size = (int)(CurrentPosition - retVal.StartPos + 1);
                        break;
                    case '$':
                        chr = GetChr();
                        if (chr < '0' || chr > '9')
                        {
                            throw new PgnParserException("Invalid NAG");
                        }
                        else
                        {
                            retVal.Type     = TokenType.Nag;
                            retVal.IntValue = GetNagToken(chr);
                        }
                        retVal.Size = (int)(CurrentPosition - retVal.StartPos - 1);
                        break;
                    case '[':
                        retVal.Type = TokenType.OpenSBracket;
                        retVal.Size = 1;
                        break;
                    case ']':
                        retVal.Type = TokenType.CloseSBracket;
                        retVal.Size = 1;
                        break;
                    case '{':
                        retVal.Type = TokenType.Comment;
                        retVal.StrValue = GetCommentToken('{');
                        retVal.Size = (int)(CurrentPosition - retVal.StartPos - 1);
                        break;
                    case '(':
                        retVal.Type = TokenType.Comment;
                        retVal.StrValue = GetCommentToken('(');
                        retVal.Size = (int)(CurrentPosition - retVal.StartPos - 1);
                        break;
                    case '!':
                    case '?':
                        retVal.Type = TokenType.Annotation;
                        retVal.StrValue = GetAnnotationToken(chr);
                        retVal.Size = (int)(CurrentPosition - retVal.StartPos);
                        break;
                    case '-':
                        retVal.Type = TokenType.Dash;
                        retVal.StrValue = GetSymbolToken('-', out bool _, out bool _);
                        break;
                    case '*':
                        retVal.Type     = TokenType.Termination;
                        retVal.StrValue = "*";
                        retVal.Size     = 1;
                        break;
                    case '+':
                    case '#':
                        break;
                    default:
                        if ((chr >= 'a' && chr <= 'z') ||
                            (chr >= 'A' && chr <= 'Z') ||
                            (chr >= '0' && chr <= '9') ||
                            (this.GameResult != null && chr == this.GameResult[0]))
                        {
                            retVal.StrValue = GetSymbolToken(chr, out bool isAllDigit, out bool isSlashFound);
                            retVal.Size     = (int)(CurrentPosition - retVal.StartPos - 1);
                            if (isAllDigit)
                            {
                                retVal.Type     = TokenType.Integer;
                                retVal.IntValue = int.Parse(retVal.StrValue);
                            }
                            else
                            {
                                switch(retVal.StrValue)
                                {
                                    case "0-1":
                                    case "1-0":
                                    case "1/2-1/2":
                                    case "1/2":
                                        retVal.Type = TokenType.Termination;
                                        break;
                                    default:
                                        if (retVal.StrValue == this.GameResult)
                                        {
                                            retVal.Type = TokenType.Termination;
                                        }
                                        else
                                        {
                                            if (isSlashFound)
                                            {
                                                throw new PgnParserException("'/' character found at an unexpected location.");
                                            }
                                            retVal.Type = TokenType.Symbol;
                                        }
                                        break;
                                }
                            }
                        }
                        else
                        {
                            retVal.Type = TokenType.UnknownToken;
                            retVal.StrValue = GetSymbolToken(chr, out bool isAllDigit, out bool isSlashFound);
                        }
                        break;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Assume the specified token
        /// </summary>
        /// <param name="tokType"> Token type</param>
        /// <param name="tok">     Assumed token</param>
        /// <returns>
        /// Token
        /// </returns>
        public void AssumeToken(TokenType tokType, Token tok)
        {
            if (tok.Type != tokType)
            {
                throw new PgnParserException($"Expecing a token of type - {tokType}", GetStringAtPos(tok.StartPos, tok.Size));
            }
        }

        /// <summary>
        /// Assume the specified token
        /// </summary>
        /// <param name="tokType">  Token type</param>
        /// <returns>
        /// Token
        /// </returns>
        public Token AssumeToken(TokenType tokType)
        {
            Token retVal;

            retVal = GetNextToken();
            AssumeToken(tokType, retVal);
            return retVal;
        }

        /// <summary>
        /// Push back a token
        /// </summary>
        /// <returns>
        /// Token
        /// </returns>
        public void PushToken(Token tok)
        {
            if (!m_tokPushed.HasValue)
            {
                m_tokPushed = tok;
            }
            else
            {
                throw new MethodAccessException("Cannot push two tokens!");
            }
        }

        /// <summary>
        /// Peek a token
        /// </summary>
        /// <returns>
        /// Token
        /// </returns>
        public Token PeekToken()
        {
            Token retVal;

            retVal = GetNextToken();
            PushToken(retVal);
            return retVal;
        }
        #endregion
    }
}
