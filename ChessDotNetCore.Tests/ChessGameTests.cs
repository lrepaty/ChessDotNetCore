using System.Collections.ObjectModel;
using ChessDotNetCore.Pieces;
using ChessDotNetCore.PgnParsing;
using System.Net;
using System.Numerics;

namespace ChessDotNetCore.Tests
{
    [TestClass]
    public class ChessGameTests
    {
        readonly Piece? kw = new King(Player.White);
        readonly Piece? kb = new King(Player.Black);
        readonly Piece? qw = new Queen(Player.White);
        readonly Piece? qb = new Queen(Player.Black);
        readonly Piece? rw = new Rook(Player.White);
        readonly Piece? rb = new Rook(Player.Black);
        readonly Piece? nw = new Knight(Player.White);
        readonly Piece? nb = new Knight(Player.Black);
        readonly Piece? bw = new Bishop(Player.White);
        readonly Piece? bb = new Bishop(Player.Black);
        readonly Piece? pw = new Pawn(Player.White);
        readonly Piece? pb = new Pawn(Player.Black);
        readonly Piece? o = null;
        readonly CastlingType allCastlingType = CastlingType.WhiteCastleKingSide | CastlingType.WhiteCastleQueenSide | CastlingType.BlackCastleKingSide | CastlingType.BlackCastleQueenSide;
        readonly CastlingType whiteCastleCastlingType = CastlingType.WhiteCastleKingSide | CastlingType.WhiteCastleQueenSide;
        readonly CastlingType blackCastleCastlingType = CastlingType.BlackCastleKingSide | CastlingType.BlackCastleQueenSide;

        [TestMethod]
        public void TestArrayGetting()
        {
            ChessGame cb = new ChessGame();
            int a = (int)Line.a;
            int b = (int)Line.b;
            int c = (int)Line.c;
            int d = (int)Line.d;
            int e = (int)Line.e;
            int f = (int)Line.f;
            int g = (int)Line.g;
            int h = (int)Line.h;
            Piece?[][]? board = cb.GetBoard();

            board![7][a].Should().Be(rw);
            board![7][b].Should().Be(nw);
            board![7][c].Should().Be(bw);
            board![7][d].Should().Be(qw);
            board![7][e].Should().Be(kw);
            board![7][f].Should().Be(bw);
            board![7][g].Should().Be(nw);
            board![7][h].Should().Be(rw);
            board![6][a].Should().Be(pw);
            board![6][b].Should().Be(pw);
            board![6][c].Should().Be(pw);
            board![6][d].Should().Be(pw);
            board![6][e].Should().Be(pw);
            board![6][f].Should().Be(pw);
            board![6][g].Should().Be(pw);
            board![6][h].Should().Be(pw);
            board![5][a].Should().Be(o);
            board![5][b].Should().Be(o);
            board![5][c].Should().Be(o);
            board![5][d].Should().Be(o);
            board![5][e].Should().Be(o);
            board![5][f].Should().Be(o);
            board![5][g].Should().Be(o);
            board![5][h].Should().Be(o);
            board![4][a].Should().Be(o);
            board![4][b].Should().Be(o);
            board![4][c].Should().Be(o);
            board![4][d].Should().Be(o);
            board![4][e].Should().Be(o);
            board![4][f].Should().Be(o);
            board![4][g].Should().Be(o);
            board![4][h].Should().Be(o);
            board![3][a].Should().Be(o);
            board![3][b].Should().Be(o);
            board![3][c].Should().Be(o);
            board![3][d].Should().Be(o);
            board![3][e].Should().Be(o);
            board![3][f].Should().Be(o);
            board![3][g].Should().Be(o);
            board![3][h].Should().Be(o);
            board![2][a].Should().Be(o);
            board![2][b].Should().Be(o);
            board![2][c].Should().Be(o);
            board![2][d].Should().Be(o);
            board![2][e].Should().Be(o);
            board![2][f].Should().Be(o);
            board![2][g].Should().Be(o);
            board![2][h].Should().Be(o);
            board![1][a].Should().Be(pb);
            board![1][b].Should().Be(pb);
            board![1][c].Should().Be(pb);
            board![1][d].Should().Be(pb);
            board![1][e].Should().Be(pb);
            board![1][f].Should().Be(pb);
            board![1][g].Should().Be(pb);
            board![1][h].Should().Be(pb);
            board![0][a].Should().Be(rb);
            board![0][b].Should().Be(nb);
            board![0][c].Should().Be(bb);
            board![0][d].Should().Be(qb);
            board![0][e].Should().Be(kb);
            board![0][f].Should().Be(bb);
            board![0][g].Should().Be(nb);
            board![0][h].Should().Be(rb);
        }

        [TestMethod]
        public void TestGetPieceAt()
        {
            ChessGame cb = new ChessGame();
            Line a = Line.a;
            Line b = Line.b;
            Line c = Line.c;
            Line d = Line.d;
            Line e = Line.e;
            Line f = Line.f;
            Line g = Line.g;
            Line h = Line.h;
            cb.GetPieceAt(a, 1).Should().Be(rw);
            cb.GetPieceAt(b, 1).Should().Be(nw);
            cb.GetPieceAt(c, 1).Should().Be(bw);
            cb.GetPieceAt(d, 1).Should().Be(qw);
            cb.GetPieceAt(e, 1).Should().Be(kw);
            cb.GetPieceAt(f, 1).Should().Be(bw);
            cb.GetPieceAt(g, 1).Should().Be(nw);
            cb.GetPieceAt(h, 1).Should().Be(rw);
            cb.GetPieceAt(a, 2).Should().Be(pw);
            cb.GetPieceAt(b, 2).Should().Be(pw);
            cb.GetPieceAt(c, 2).Should().Be(pw);
            cb.GetPieceAt(d, 2).Should().Be(pw);
            cb.GetPieceAt(e, 2).Should().Be(pw);
            cb.GetPieceAt(f, 2).Should().Be(pw);
            cb.GetPieceAt(g, 2).Should().Be(pw);
            cb.GetPieceAt(h, 2).Should().Be(pw);
            cb.GetPieceAt(a, 3).Should().Be(o);
            cb.GetPieceAt(b, 3).Should().Be(o);
            cb.GetPieceAt(c, 3).Should().Be(o);
            cb.GetPieceAt(d, 3).Should().Be(o);
            cb.GetPieceAt(e, 3).Should().Be(o);
            cb.GetPieceAt(f, 3).Should().Be(o);
            cb.GetPieceAt(g, 3).Should().Be(o);
            cb.GetPieceAt(h, 3).Should().Be(o);
            cb.GetPieceAt(a, 4).Should().Be(o);
            cb.GetPieceAt(b, 4).Should().Be(o);
            cb.GetPieceAt(c, 4).Should().Be(o);
            cb.GetPieceAt(d, 4).Should().Be(o);
            cb.GetPieceAt(e, 4).Should().Be(o);
            cb.GetPieceAt(f, 4).Should().Be(o);
            cb.GetPieceAt(g, 4).Should().Be(o);
            cb.GetPieceAt(h, 4).Should().Be(o);
            cb.GetPieceAt(a, 5).Should().Be(o);
            cb.GetPieceAt(b, 5).Should().Be(o);
            cb.GetPieceAt(c, 5).Should().Be(o);
            cb.GetPieceAt(d, 5).Should().Be(o);
            cb.GetPieceAt(e, 5).Should().Be(o);
            cb.GetPieceAt(f, 5).Should().Be(o);
            cb.GetPieceAt(g, 5).Should().Be(o);
            cb.GetPieceAt(h, 5).Should().Be(o);
            cb.GetPieceAt(a, 6).Should().Be(o);
            cb.GetPieceAt(b, 6).Should().Be(o);
            cb.GetPieceAt(c, 6).Should().Be(o);
            cb.GetPieceAt(d, 6).Should().Be(o);
            cb.GetPieceAt(e, 6).Should().Be(o);
            cb.GetPieceAt(f, 6).Should().Be(o);
            cb.GetPieceAt(g, 6).Should().Be(o);
            cb.GetPieceAt(h, 6).Should().Be(o);
            cb.GetPieceAt(a, 7).Should().Be(pb);
            cb.GetPieceAt(b, 7).Should().Be(pb);
            cb.GetPieceAt(c, 7).Should().Be(pb);
            cb.GetPieceAt(d, 7).Should().Be(pb);
            cb.GetPieceAt(e, 7).Should().Be(pb);
            cb.GetPieceAt(f, 7).Should().Be(pb);
            cb.GetPieceAt(g, 7).Should().Be(pb);
            cb.GetPieceAt(h, 7).Should().Be(pb);
            cb.GetPieceAt(a, 8).Should().Be(rb);
            cb.GetPieceAt(b, 8).Should().Be(nb);
            cb.GetPieceAt(c, 8).Should().Be(bb);
            cb.GetPieceAt(d, 8).Should().Be(qb);
            cb.GetPieceAt(e, 8).Should().Be(kb);
            cb.GetPieceAt(f, 8).Should().Be(bb);
            cb.GetPieceAt(g, 8).Should().Be(nb);
            cb.GetPieceAt(h, 8).Should().Be(rb);
        }

        [TestMethod]
        public void TestCustomInitialize()
        {
            Line a = Line.a;
            Line b = Line.b;
            Line c = Line.c;
            Line d = Line.d;
            Line e = Line.e;
            Line f = Line.f;
            Line g = Line.g;
            Line h = Line.h;
            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, o, bb, qb, kb, bb, nb, rb },
                new[] { pb, pb, pb, pb, pb, pb, pb, pb },
                new[] { o, o, nb, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, pw, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { pw, pw, pw, pw, o, pw, pw, pw },
                new[] { rw, nw, bw, qw, kw, bw, nw, rw }
            };

            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            cb.GetPieceAt(a, 1).Should().Be(rw);
            cb.GetPieceAt(a, 1).Should().Be(rw);
            cb.GetPieceAt(b, 1).Should().Be(nw);
            cb.GetPieceAt(c, 1).Should().Be(bw);
            cb.GetPieceAt(d, 1).Should().Be(qw);
            cb.GetPieceAt(e, 1).Should().Be(kw);
            cb.GetPieceAt(f, 1).Should().Be(bw);
            cb.GetPieceAt(g, 1).Should().Be(nw);
            cb.GetPieceAt(h, 1).Should().Be(rw);
            cb.GetPieceAt(a, 2).Should().Be(pw);
            cb.GetPieceAt(b, 2).Should().Be(pw);
            cb.GetPieceAt(c, 2).Should().Be(pw);
            cb.GetPieceAt(d, 2).Should().Be(pw);
            cb.GetPieceAt(e, 2).Should().Be(o);
            cb.GetPieceAt(f, 2).Should().Be(pw);
            cb.GetPieceAt(g, 2).Should().Be(pw);
            cb.GetPieceAt(h, 2).Should().Be(pw);
            cb.GetPieceAt(a, 3).Should().Be(o);
            cb.GetPieceAt(b, 3).Should().Be(o);
            cb.GetPieceAt(c, 3).Should().Be(o);
            cb.GetPieceAt(d, 3).Should().Be(o);
            cb.GetPieceAt(e, 3).Should().Be(o);
            cb.GetPieceAt(f, 3).Should().Be(o);
            cb.GetPieceAt(g, 3).Should().Be(o);
            cb.GetPieceAt(h, 3).Should().Be(o);
            cb.GetPieceAt(a, 4).Should().Be(o);
            cb.GetPieceAt(b, 4).Should().Be(o);
            cb.GetPieceAt(c, 4).Should().Be(o);
            cb.GetPieceAt(d, 4).Should().Be(o);
            cb.GetPieceAt(e, 4).Should().Be(pw);
            cb.GetPieceAt(f, 4).Should().Be(o);
            cb.GetPieceAt(g, 4).Should().Be(o);
            cb.GetPieceAt(h, 4).Should().Be(o);
            cb.GetPieceAt(a, 5).Should().Be(o);
            cb.GetPieceAt(b, 5).Should().Be(o);
            cb.GetPieceAt(c, 5).Should().Be(o);
            cb.GetPieceAt(d, 5).Should().Be(o);
            cb.GetPieceAt(e, 5).Should().Be(o);
            cb.GetPieceAt(f, 5).Should().Be(o);
            cb.GetPieceAt(g, 5).Should().Be(o);
            cb.GetPieceAt(h, 5).Should().Be(o);
            cb.GetPieceAt(a, 6).Should().Be(o);
            cb.GetPieceAt(b, 6).Should().Be(o);
            nb.Should().Be(cb.GetPieceAt(c, 6));
            cb.GetPieceAt(d, 6).Should().Be(o);
            cb.GetPieceAt(e, 6).Should().Be(o);
            cb.GetPieceAt(f, 6).Should().Be(o);
            cb.GetPieceAt(g, 6).Should().Be(o);
            cb.GetPieceAt(h, 6).Should().Be(o);
            cb.GetPieceAt(a, 7).Should().Be(pb);
            cb.GetPieceAt(b, 7).Should().Be(pb);
            cb.GetPieceAt(c, 7).Should().Be(pb);
            cb.GetPieceAt(d, 7).Should().Be(pb);
            cb.GetPieceAt(e, 7).Should().Be(pb);
            cb.GetPieceAt(f, 7).Should().Be(pb);
            cb.GetPieceAt(g, 7).Should().Be(pb);
            cb.GetPieceAt(h, 7).Should().Be(pb);
            cb.GetPieceAt(a, 8).Should().Be(rb);
            cb.GetPieceAt(b, 8).Should().Be(o);
            cb.GetPieceAt(c, 8).Should().Be(bb);
            cb.GetPieceAt(d, 8).Should().Be(qb);
            cb.GetPieceAt(e, 8).Should().Be(kb);
            cb.GetPieceAt(f, 8).Should().Be(bb);
            cb.GetPieceAt(g, 8).Should().Be(nb);
            cb.GetPieceAt(h, 8).Should().Be(rb);
        }

        [TestMethod]
        public void TestValidMoveWhitePawn()
        {
            ChessGame cb = new ChessGame();

            Move move1 = new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");
        }

        [TestMethod]
        public void TestValidMoveWhitePawn_2Steps()
        {
            ChessGame cb = new ChessGame();

            Move move1 = new Move(new Position(Line.d, 2), new Position(Line.d, 4), Player.White);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");
        }

        [TestMethod]
        public void TestValidMoveWhitePawn_Capture()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.e, 2), new Position(Line.e, 4), Player.White);
            Move move2 = new Move(new Position(Line.d, 7), new Position(Line.d, 5), Player.Black);
            Move move3 = new Move(new Position(Line.e, 4), new Position(Line.d, 5), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeTrue("move3 should be valid");
        }

        [TestMethod]
        public void TestValidMoveWhitePawn_EnPassant()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.e, 2), new Position(Line.e, 4), Player.White);
            Move move2 = new Move(new Position(Line.e, 7), new Position(Line.e, 6), Player.Black);
            Move move3 = new Move(new Position(Line.e, 4), new Position(Line.e, 5), Player.White);
            Move move4 = new Move(new Position(Line.d, 7), new Position(Line.d, 5), Player.Black);
            Move move5 = new Move(new Position(Line.e, 5), new Position(Line.d, 6), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);
            cb.MakeMove(move4, true);

            cb.IsValidMove(move5).Should().BeTrue("move5 should be valid");
        }

        [TestMethod]
        public void TestInvalidMoveWhitePawn_EnPassant()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.e, 2), new Position(Line.e, 4), Player.White);
            Move move2 = new Move(new Position(Line.e, 7), new Position(Line.e, 6), Player.Black);
            Move move3 = new Move(new Position(Line.e, 4), new Position(Line.e, 5), Player.White);
            Move move4 = new Move(new Position(Line.d, 7), new Position(Line.d, 6), Player.Black);
            Move move5 = new Move(new Position(Line.b, 7), new Position(Line.b, 6), Player.Black);
            Move move6 = new Move(new Position(Line.d, 6), new Position(Line.d, 5), Player.Black);
            Move move7 = new Move(new Position(Line.e, 5), new Position(Line.d, 6), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);
            cb.MakeMove(move4, true);
            cb.MakeMove(move5, true);
            cb.MakeMove(move6, true);

            cb.IsValidMove(move7).Should().BeFalse("move7 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhitePawn_EnPassant_NoPawn()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, rb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, pw, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame game = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });

            Move move1 = new Move(new Position(Line.e, 7), new Position(Line.e, 5), Player.Black);
            Move move2 = new Move(new Position(Line.f, 5), new Position(Line.e, 6), Player.White);
            game.MakeMove(move1, true);

            game.IsValidMove(move2).Should().BeFalse("move2 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhitePawn_NoCapture()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.e, 2), new Position(Line.e, 4), Player.White);
            Move move2 = new Move(new Position(Line.d, 7), new Position(Line.d, 6), Player.Black);
            Move move3 = new Move(new Position(Line.e, 4), new Position(Line.d, 5), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeFalse("move3 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhitePawn_2StepsBlockingPiece()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kw, o, kb, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, pb, o, o, o },
                new[] { o, o, o, o, pw, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move = new Move(new Position(Line.e, 2), new Position(Line.e, 4), Player.White);

            cb.IsValidMove(move).Should().BeFalse("move should be invalid");

        }

        [TestMethod]
        public void TestInvalidMoveWhitePawn_2StepsNotOnRank2()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.e, 2), new Position(Line.e, 3), Player.White);
            Move move2 = new Move(new Position(Line.e, 3), new Position(Line.e, 5), Player.White);

            cb.MakeMove(move1, true);

            cb.IsValidMove(move2).Should().BeFalse("move2 should be invalid");
        }

        [TestMethod]
        public void TestValidMoveWhiteKnight()
        {
            ChessGame cb = new ChessGame();

            Move move1 = new Move(new Position(Line.b, 1), new Position(Line.c, 3), Player.White);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");
        }

        [TestMethod]
        public void TestValidMoveWhiteBishopC()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.d, 2), new Position(Line.d, 3), Player.White);
            Move move2 = new Move(new Position(Line.a, 7), new Position(Line.a, 6), Player.Black);
            Move move3 = new Move(new Position(Line.c, 1), new Position(Line.f, 4), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeTrue("move3 should be valid");
        }

        [TestMethod]
        public void TestValidMoveWhiteBishopF()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.e, 2), new Position(Line.e, 3), Player.White);
            Move move2 = new Move(new Position(Line.a, 7), new Position(Line.a, 6), Player.Black);
            Move move3 = new Move(new Position(Line.f, 1), new Position(Line.c, 4), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeTrue("move3 should be valid");
        }

        [TestMethod]
        public void TestValidMoveWhiteRookA()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.a, 7), new Position(Line.a, 6), Player.Black);
            Move move3 = new Move(new Position(Line.a, 1), new Position(Line.a, 2), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeTrue("move3 should be valid");
        }

        [TestMethod]
        public void TestValidMoveWhiteRookH()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.h, 2), new Position(Line.h, 3), Player.White);
            Move move2 = new Move(new Position(Line.a, 7), new Position(Line.a, 6), Player.Black);
            Move move3 = new Move(new Position(Line.h, 1), new Position(Line.h, 2), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeTrue("move3 should be valid");
        }

        [TestMethod]
        public void TestValidMoveWhiteQueenDiagonal()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.e, 2), new Position(Line.e, 3), Player.White);
            Move move2 = new Move(new Position(Line.a, 7), new Position(Line.a, 6), Player.Black);
            Move move3 = new Move(new Position(Line.d, 1), new Position(Line.h, 5), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeTrue("move3 should be valid");
        }

        [TestMethod]
        public void TestValidMoveWhiteQueenVertical()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.d, 2), new Position(Line.d, 3), Player.White);
            Move move2 = new Move(new Position(Line.a, 7), new Position(Line.a, 6), Player.Black);
            Move move3 = new Move(new Position(Line.d, 1), new Position(Line.d, 2), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeTrue("move3 should be valid");
        }

        [TestMethod]
        public void TestValidMoveWhiteQueenHorizontal()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.b, 1), new Position(Line.c, 3), Player.White);
            Move move2 = new Move(new Position(Line.a, 7), new Position(Line.a, 6), Player.Black);
            Move move3 = new Move(new Position(Line.d, 2), new Position(Line.d, 3), Player.White);
            Move move4 = new Move(new Position(Line.b, 7), new Position(Line.b, 6), Player.Black);
            Move move5 = new Move(new Position(Line.c, 1), new Position(Line.d, 2), Player.White);
            Move move6 = new Move(new Position(Line.c, 7), new Position(Line.c, 6), Player.Black);
            Move move7 = new Move(new Position(Line.d, 1), new Position(Line.c, 1), Player.White);
            Move move8 = new Move(new Position(Line.d, 1), new Position(Line.b, 1), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);
            cb.MakeMove(move4, true);
            cb.MakeMove(move5, true);
            cb.MakeMove(move6, true);

            cb.IsValidMove(move7).Should().BeTrue("move7 should be valid");
            cb.IsValidMove(move8).Should().BeTrue("move8 should be valid");
        }

        [TestMethod]
        public void TestValidMoveWhiteKingDiagonal()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.f, 2), new Position(Line.f, 3), Player.White);
            Move move2 = new Move(new Position(Line.a, 7), new Position(Line.a, 6), Player.Black);
            Move move3 = new Move(new Position(Line.e, 1), new Position(Line.f, 2), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeTrue("move3 should be valid");

        }

        [TestMethod]
        public void TestValidMoveWhiteKingHorizontal()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.d, 2), new Position(Line.d, 3), Player.White);
            Move move2 = new Move(new Position(Line.a, 7), new Position(Line.a, 6), Player.Black);
            Move move3 = new Move(new Position(Line.d, 1), new Position(Line.d, 2), Player.White);
            Move move4 = new Move(new Position(Line.h, 7), new Position(Line.h, 6), Player.Black);
            Move move5 = new Move(new Position(Line.e, 1), new Position(Line.d, 1), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);
            cb.MakeMove(move4, true);

            cb.IsValidMove(move5).Should().BeTrue("move5 should be valid");
        }

        [TestMethod]
        public void TestValidMoveWhiteKingVertical()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.e, 2), new Position(Line.e, 3), Player.White);
            Move move2 = new Move(new Position(Line.a, 7), new Position(Line.a, 6), Player.Black);
            Move move3 = new Move(new Position(Line.e, 1), new Position(Line.e, 2), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeTrue("move3 should be valid");
        }

        [TestMethod]
        public void TestValidMoveWhiteKing_KingsideCastling()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, rw }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.g, 1), Player.White);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");

            cb.GetValidMoves(Player.White).Should().Contain(new Move("E1", "G1", Player.White));
            cb.GetValidMoves(new Position("E1")).Should().Contain(new Move("E1", "G1", Player.White));
        }

        [TestMethod]
        public void TestValidMoveWhiteKing_KingsideCastling2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, rw }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.h, 1), Player.White);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");

            cb.GetValidMoves(Player.White).Should().Contain(new Move("E1", "H1", Player.White));
            cb.GetValidMoves(new Position("E1")).Should().Contain(new Move("E1", "H1", Player.White));
        }

        [TestMethod]
        public void TestValidMoveWhiteKing_QueensideCastling()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, kb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { rw, o, o, o, kw, o, o, rw }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.c, 1), Player.White);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");

            cb.GetValidMoves(Player.White).Should().Contain(new Move("E1", "C1", Player.White));
            cb.GetValidMoves(new Position("E1")).Should().Contain(new Move("E1", "C1", Player.White));
        }

        [TestMethod]
        public void TestValidMoveWhiteKing_QueensideCastling2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, kb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { rw, o, o, o, kw, o, o, rw }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.a, 1), Player.White);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");

            cb.GetValidMoves(Player.White).Should().Contain(new Move("E1", "A1", Player.White));
            cb.GetValidMoves(new Position("E1")).Should().Contain(new Move("E1", "A1", Player.White));
        }

        [TestMethod]
        public void TestInvalidMoveWhiteKing_KingsideCastling_WouldPassThroughCheck()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, rb, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, rw }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.g, 1), Player.White);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhiteKing_KingsideCastling_WouldPassThroughCheck2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, rb, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, rw }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.h, 1), Player.White);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhiteKing_QueensideCastling_WouldPassThroughCheck()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, rb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { rw, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.c, 1), Player.White);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestValidMoveWhiteKing_KingsideCastling_WouldNotPassThroughCheck()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, rb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { rw, o, o, o, kw, o, o, rw }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.g, 1), Player.White);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");
        }

        [TestMethod]
        public void TestValidMoveWhiteKing_QueensideCastling_WouldNotPassThroughCheck()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, rb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { rw, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.c, 1), Player.White);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");
        }

        [TestMethod]
        public void TestInvalidMoveWhiteKing_KingsideCastling_BlockingPiece1()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, rw, rw }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.g, 1), Player.White);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhiteKing_QueensideCastling_BlockingPiece1()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, kb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { rw, rw, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.c, 1), Player.White);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhiteKing_KingsideCastling_BlockingPiece2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, rw, o, rw }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.g, 1), Player.White);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhiteKing_QueensideCastling_BlockingPiece2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { rw, o, rw, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.c, 1), Player.White);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhiteKing_QueensideCastling_BlockingPiece3()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { rw, o, o, rw, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.c, 1), Player.White);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhiteKing_KingsideCastling_NoRook()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.g, 1), Player.White);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhiteKing_QueensideCastling_NoRook()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.c, 1), Player.White);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhiteKing_KingsideCastling_RookMoved()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, rw }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.h, 1), new Position(Line.h, 2), Player.White);
            Move move2 = new Move(new Position(Line.b, 8), new Position(Line.b, 7), Player.Black);
            Move move3 = new Move(new Position(Line.e, 1), new Position(Line.g, 1), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeFalse("move3 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhiteKing_QueensideCastling_RookMoved()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { rw, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.a, 1), new Position(Line.a, 2), Player.White);
            Move move2 = new Move(new Position(Line.b, 8), new Position(Line.b, 7), Player.Black);
            Move move3 = new Move(new Position(Line.e, 1), new Position(Line.c, 1), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeFalse("move3 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhiteKing_KingsideCastling_KingMoved()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, rw }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.d, 1), Player.White);
            Move move2 = new Move(new Position(Line.b, 8), new Position(Line.b, 7), Player.Black);
            Move move3 = new Move(new Position(Line.d, 1), new Position(Line.f, 1), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeFalse("move3 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhiteKing_QueensideCastling_KingMoved()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { rw, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 1), new Position(Line.d, 1), Player.White);
            Move move2 = new Move(new Position(Line.b, 8), new Position(Line.b, 7), Player.Black);
            Move move3 = new Move(new Position(Line.d, 1), new Position(Line.b, 1), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeFalse("move3 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhiteKing_KingsideCastling_Checkmated()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, rb, rb, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, pw, o, o, o, o },
                new[] { o, o, o, rw, kw, o, o, rw }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move castling = new Move(new Position(Line.e, 1), new Position(Line.g, 1), Player.White);
            cb.IsValidMove(castling).Should().BeFalse("castling move should be invalid; king is checkmated");
        }

        [TestMethod]
        public void TestValidMoveBlackPawn()
        {
            ChessGame cb = new ChessGame();
            cb.MakeMove(new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White), true);

            Move move1 = new Move(new Position(Line.a, 7), new Position(Line.a, 6), Player.Black);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");
        }

        [TestMethod]
        public void TestValidMoveBlackPawn_2Steps()
        {
            ChessGame cb = new ChessGame();
            cb.MakeMove(new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White), true);

            Move move1 = new Move(new Position(Line.d, 7), new Position(Line.d, 5), Player.Black);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");
        }

        [TestMethod]
        public void TestValidMoveBlackPawn_Capture()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.e, 7), new Position(Line.e, 5), Player.Black);
            Move move3 = new Move(new Position(Line.d, 2), new Position(Line.d, 4), Player.White);
            Move move4 = new Move(new Position(Line.e, 5), new Position(Line.d, 4), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);

            cb.IsValidMove(move4).Should().BeTrue("move4 should be valid");
        }

        [TestMethod]
        public void TestValidMoveBlackPawn_EnPassant()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.b, 1), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.e, 7), new Position(Line.e, 5), Player.Black);
            Move move3 = new Move(new Position(Line.e, 2), new Position(Line.e, 3), Player.White);
            Move move4 = new Move(new Position(Line.e, 5), new Position(Line.e, 4), Player.Black);
            Move move5 = new Move(new Position(Line.d, 2), new Position(Line.d, 4), Player.White);
            Move move6 = new Move(new Position(Line.e, 4), new Position(Line.d, 3), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);
            cb.MakeMove(move4, true);
            cb.MakeMove(move5, true);

            cb.IsValidMove(move6).Should().BeTrue("move6 should be valid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackPawn_EnPassant()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.b, 1), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.e, 7), new Position(Line.e, 5), Player.Black);
            Move move3 = new Move(new Position(Line.e, 2), new Position(Line.e, 3), Player.White);
            Move move4 = new Move(new Position(Line.e, 5), new Position(Line.e, 4), Player.Black);
            Move move5 = new Move(new Position(Line.h, 2), new Position(Line.h, 4), Player.White);
            Move move6 = new Move(new Position(Line.e, 4), new Position(Line.d, 3), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);
            cb.MakeMove(move4, true);
            cb.MakeMove(move5, true);

            cb.IsValidMove(move6).Should().BeFalse("move6 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackPawn_NoCapture()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.e, 7), new Position(Line.e, 5), Player.Black);
            Move move3 = new Move(new Position(Line.d, 2), new Position(Line.d, 3), Player.White);
            Move move4 = new Move(new Position(Line.e, 5), new Position(Line.d, 4), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);

            cb.IsValidMove(move4).Should().BeFalse("move4 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackPawn_2StepsBlockingPiece()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, pb, o, o, o },
                new[] { o, o, o, o, pw, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { kw, o, kb, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move = new Move(new Position(Line.e, 7), new Position(Line.e, 5), Player.Black);

            cb.IsValidMove(move).Should().BeFalse("move should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackPawn_2StepsNotOnRank7()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.e, 7), new Position(Line.e, 6), Player.Black);
            Move move3 = new Move(new Position(Line.h, 2), new Position(Line.h, 3), Player.White);
            Move move4 = new Move(new Position(Line.e, 6), new Position(Line.e, 4), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);

            cb.IsValidMove(move4).Should().BeFalse("move4 should be invalid");
        }

        [TestMethod]
        public void TestValidMoveBlackKnight()
        {
            ChessGame cb = new ChessGame();
            cb.MakeMove(new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White), true);

            Move move1 = new Move(new Position(Line.b, 8), new Position(Line.c, 6), Player.Black);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");
        }

        [TestMethod]
        public void TestValidMoveBlackBishopC()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.d, 7), new Position(Line.d, 6), Player.Black);
            Move move3 = new Move(new Position(Line.h, 2), new Position(Line.h, 3), Player.White);
            Move move4 = new Move(new Position(Line.c, 8), new Position(Line.f, 5), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);
            cb.IsValidMove(move4).Should().BeTrue("move4 should be valid");
        }

        [TestMethod]
        public void TestValidMoveBlackBishopF()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.e, 7), new Position(Line.e, 6), Player.Black);
            Move move3 = new Move(new Position(Line.h, 2), new Position(Line.h, 3), Player.White);
            Move move4 = new Move(new Position(Line.f, 8), new Position(Line.c, 5), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);

            cb.IsValidMove(move4).Should().BeTrue("move4 should be valid");
        }

        [TestMethod]
        public void TestValidMoveBlackQueenDiagonal()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.e, 7), new Position(Line.e, 6), Player.Black);
            Move move3 = new Move(new Position(Line.h, 2), new Position(Line.h, 3), Player.White);
            Move move4 = new Move(new Position(Line.d, 8), new Position(Line.h, 4), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);

            cb.IsValidMove(move4).Should().BeTrue("move4 should be valid");
        }

        [TestMethod]
        public void TestValidMoveBlackQueenVertical()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.d, 7), new Position(Line.d, 6), Player.Black);
            Move move3 = new Move(new Position(Line.h, 2), new Position(Line.h, 3), Player.White);
            Move move4 = new Move(new Position(Line.d, 8), new Position(Line.d, 7), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);

            cb.IsValidMove(move4).Should().BeTrue("move4 should be valid");
        }

        [TestMethod]
        public void TestValidMoveBlackQueenHorizontal()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.b, 8), new Position(Line.c, 6), Player.Black);
            Move move3 = new Move(new Position(Line.b, 2), new Position(Line.b, 3), Player.White);
            Move move4 = new Move(new Position(Line.d, 7), new Position(Line.d, 6), Player.Black);
            Move move5 = new Move(new Position(Line.c, 2), new Position(Line.c, 3), Player.White);
            Move move6 = new Move(new Position(Line.c, 8), new Position(Line.d, 7), Player.Black);
            Move move7 = new Move(new Position(Line.d, 2), new Position(Line.d, 3), Player.White);
            Move move8 = new Move(new Position(Line.d, 8), new Position(Line.c, 8), Player.Black);
            Move move9 = new Move(new Position(Line.d, 8), new Position(Line.b, 8), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);
            cb.MakeMove(move4, true);
            cb.MakeMove(move5, true);
            cb.MakeMove(move6, true);
            cb.MakeMove(move7, true);

            cb.IsValidMove(move8).Should().BeTrue("move8 should be valid");
            cb.IsValidMove(move9).Should().BeTrue("move9 should be valid");
        }

        [TestMethod]
        public void TestValidMoveBlackKingDiagonal()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.f, 7), new Position(Line.f, 6), Player.Black);
            Move move3 = new Move(new Position(Line.h, 2), new Position(Line.h, 3), Player.White);
            Move move4 = new Move(new Position(Line.e, 8), new Position(Line.f, 7), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);

            cb.IsValidMove(move4).Should().BeTrue("move4 should be valid");
        }

        [TestMethod]
        public void TestValidMoveBlackKingHorizontal()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.d, 7), new Position(Line.d, 6), Player.Black);
            Move move3 = new Move(new Position(Line.h, 2), new Position(Line.h, 3), Player.White);
            Move move4 = new Move(new Position(Line.d, 8), new Position(Line.d, 7), Player.Black);
            Move move5 = new Move(new Position(Line.b, 2), new Position(Line.b, 3), Player.White);
            Move move6 = new Move(new Position(Line.e, 8), new Position(Line.d, 8), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);
            cb.MakeMove(move4, true);
            cb.MakeMove(move5, true);

            cb.IsValidMove(move6).Should().BeTrue("move6 should be valid");
        }

        [TestMethod]
        public void TestValidMoveBlackKingVertical()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.e, 7), new Position(Line.e, 6), Player.Black);
            Move move3 = new Move(new Position(Line.h, 2), new Position(Line.h, 3), Player.White);
            Move move4 = new Move(new Position(Line.e, 8), new Position(Line.e, 7), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);

            cb.IsValidMove(move4).Should().BeTrue("move4 should be valid");
        }

        [TestMethod]
        public void TestValidMoveBlackKing_KingsideCastling()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, rb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.g, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");

            cb.GetValidMoves(Player.Black).Should().Contain(new Move("E8", "G8", Player.Black));
            cb.GetValidMoves(new Position("E8")).Should().Contain(new Move("E8", "G8", Player.Black));
        }

        [TestMethod]
        public void TestValidMoveBlackKing_KingsideCastling2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, rb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.h, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");

            cb.GetValidMoves(Player.Black).Should().Contain(new Move("E8", "H8", Player.Black));
            cb.GetValidMoves(new Position("E8")).Should().Contain(new Move("E8", "H8", Player.Black));
        }

        [TestMethod]
        public void TestValidMoveBlackKing_QueensideCastling()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.c, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");

            cb.GetValidMoves(Player.Black).Should().Contain(new Move("E8", "C8", Player.Black));
            cb.GetValidMoves(new Position("E8")).Should().Contain(new Move("E8", "C8", Player.Black));
        }

        [TestMethod]
        public void TestValidMoveBlackKing_QueensideCastling2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.a, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");

            cb.GetValidMoves(Player.Black).Should().Contain(new Move("E8", "A8", Player.Black));
            cb.GetValidMoves(new Position("E8")).Should().Contain(new Move("E8", "A8", Player.Black));
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_KingsideCastling_WouldPassThroughCheck()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, rb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, rw, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.g, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_KingsideCastling_WouldPassThroughCheck2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, rb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, rw, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.h, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_QueensideCastling_WouldPassThroughCheck()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, rw, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.c, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_QueensideCastling_WouldPassThroughCheck2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, rw, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.a, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestValidMoveBlackKing_KingsideCastling_WouldNotPassThroughCheck()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, rb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, rw }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.g, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");
        }

        [TestMethod]
        public void TestValidMoveBlackKing_KingsideCastling_WouldNotPassThroughCheck2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, rb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, rw }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.h, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");
        }

        [TestMethod]
        public void TestValidMoveBlackKing_QueensideCastling_WouldNotPassThroughCheck()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, rw, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.c, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeTrue("move1 should be valid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_KingsideCastling_BlockingPiece1()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, rb, rb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o}
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.g, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_QueensideCastling_BlockingPiece1()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, rb, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.c, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_KingsideCastling_BlockingPiece2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, rb, o, rb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.g, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_QueensideCastling_BlockingPiece2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, o, rb, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.c, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_QueensideCastling_BlockingPiece3()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, o, o, rb, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.c, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_KingsideCastling_NoRook()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.g, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_QueensideCastling_NoRook()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.c, 8), Player.Black);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_KingsideCastling_RookMoved()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, rb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, kw, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.h, 8), new Position(Line.h, 7), Player.Black);
            Move move2 = new Move(new Position(Line.b, 1), new Position(Line.b, 2), Player.White);
            Move move3 = new Move(new Position(Line.e, 8), new Position(Line.g, 8), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeFalse("move3 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_QueensideCastling_RookMoved()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, kw, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.a, 8), new Position(Line.a, 7), Player.Black);
            Move move2 = new Move(new Position(Line.b, 1), new Position(Line.b, 2), Player.White);
            Move move3 = new Move(new Position(Line.e, 8), new Position(Line.c, 8), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeFalse("move3 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_KingsideCastling_KingMoved()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, rb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, kw, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.d, 8), Player.Black);
            Move move2 = new Move(new Position(Line.b, 1), new Position(Line.b, 2), Player.White);
            Move move3 = new Move(new Position(Line.d, 8), new Position(Line.f, 8), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeFalse("move3 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_QueensideCastling_KingMoved()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, kw, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.e, 8), new Position(Line.d, 8), Player.Black);
            Move move2 = new Move(new Position(Line.b, 1), new Position(Line.b, 2), Player.White);
            Move move3 = new Move(new Position(Line.d, 8), new Position(Line.b, 8), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);

            cb.IsValidMove(move3).Should().BeFalse("move3 should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhite_WouldBeInCheck()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kw, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, qb },
                new[] { pw, o, o, pb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move = new Move(new Position(Line.a, 8), new Position(Line.a, 7), Player.White);

            cb.IsValidMove(move).Should().BeFalse("move should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhite_WouldBeCheckmated()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, rb },
                new[] { o, o, o, kw, nb, o, rb, o }
            };

            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move = new Move(new Position(Line.d, 1), new Position(Line.e, 1), Player.White);

            cb.IsValidMove(move).Should().BeFalse("move should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhiteRook_NoPassThrough()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, o },
                new[] { rw, o, o, o, pw, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, kw, o, kb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            Move move = new Move(new Position(Line.a, 7), new Position(Line.g, 7), Player.White);

            cb.IsValidMove(move).Should().BeFalse("move should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackKing_NoAdjacentKings()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, kb, o, kw, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            Move move1 = new Move(new Position(Line.d, 6), new Position(Line.e, 6), Player.Black);
            Move move2 = new Move(new Position(Line.d, 6), new Position(Line.e, 7), Player.Black);
            Move move3 = new Move(new Position(Line.d, 6), new Position(Line.e, 5), Player.Black);

            cb.IsValidMove(move1).Should().BeFalse("move1 should be invalid");
            cb.IsValidMove(move3).Should().BeFalse("move3 should be invalid");
        }

        [TestMethod]
        public void TestMakeMoveWhitePawn()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.e, 2), new Position(Line.e, 3), Player.White);
            cb.MakeMove(move1, false).Should().NotBe(MoveType.Invalid);
            Piece?[][] expected = new Piece?[8][]
            {
                new[] { rb, nb, bb, qb, kb, bb, nb, rb },
                new[] { pb, pb, pb, pb, pb, pb, pb, pb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, pw, o, o, o },
                new[] { pw, pw, pw, pw, o, pw, pw, pw },
                new[] { rw, nw, bw, qw, kw, bw, nw, rw }
            };
            cb.GetBoard().Should().BeEquivalentTo(expected, "Unexpected board layout after applying move1");
            Move move2 = new Move(new Position(Line.e, 3), new Position(Line.e, 4), Player.White);
            cb.MakeMove(move2, true).Should().NotBe(MoveType.Invalid);
            expected = new Piece?[8][]
            {
                new[] { rb, nb, bb, qb, kb, bb, nb, rb },
                new[] { pb, pb, pb, pb, pb, pb, pb, pb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, pw, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { pw, pw, pw, pw, o, pw, pw, pw },
                new[] { rw, nw, bw, qw, kw, bw, nw, rw }
            };
            cb.GetBoard().Should().BeEquivalentTo(expected, "Unexpected board layout after applying move2");
        }

        [TestMethod]
        public void TestMakeMoveWhitePawn_EnPassant()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.e, 2), new Position(Line.e, 4), Player.White);
            Move move2 = new Move(new Position(Line.e, 7), new Position(Line.e, 6), Player.Black);
            Move move3 = new Move(new Position(Line.e, 4), new Position(Line.e, 5), Player.White);
            Move move4 = new Move(new Position(Line.d, 7), new Position(Line.d, 5), Player.Black);
            Move move5 = new Move(new Position(Line.e, 5), new Position(Line.d, 6), Player.White);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);
            cb.MakeMove(move4, true);
            cb.MakeMove(move5, true);

            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, nb, bb, qb, kb, bb, nb, rb },
                new[] { pb, pb, pb, o, o, pb, pb, pb },
                new[] { o, o, o, pw, pb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { pw, pw, pw, pw, o, pw, pw, pw },
                new[] { rw, nw, bw, qw, kw, bw, nw, rw }
            };

            cb.GetBoard().Should().BeEquivalentTo(board, "Unexpected board layout after en passant capture.");
        }

        [TestMethod]
        public void TestMakeMoveWhitePawn_PromotionToQueen()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, o },
                new[] { pw, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, kw, o, kb, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            Move move = new Move(new Position(Line.a, 7), new Position(Line.a, 8), Player.White, 'Q');
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            cb.MakeMove(move, false).Should().NotBe(MoveType.Invalid, "move should be valid");

            Piece?[][] expected = new Piece?[8][]
            {
                new[] { qw, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, kw, o, kb, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            cb.GetBoard().Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestMakeMoveBlackPawn_PromotionToQueen()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, kw, o, kb, pb, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            Move move = new Move(new Position(Line.g, 2), new Position(Line.g, 1), Player.Black, 'Q');
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            cb.MakeMove(move, false).Should().NotBe(MoveType.Invalid, "move should be valid");

            Piece?[][] expected = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, kw, o, kb, o, o },
                new[] { o, o, o, o, o, o, qb, o }
            };
            cb.GetBoard().Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestInvalidMoveWhitePawnPromotion_NoPieceSpecified()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, o },
                new[] { pw, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, kw, o, kb, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            Move move = new Move(new Position(Line.a, 7), new Position(Line.a, 8), Player.White);
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            cb.IsValidMove(move).Should().BeFalse("move should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveWhitePawnPromotion_PromotionToKing()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, o },
                new[] { pw, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, kw, o, kb, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            Move move = new Move(new Position(Line.a, 7), new Position(Line.a, 8), Player.White, 'K');
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            cb.IsValidMove(move).Should().BeFalse("move should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackPawnPromotion_NoPieceSpecified()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, kw, o, kb, pb, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            Move move = new Move(new Position(Line.g, 2), new Position(Line.g, 1), Player.Black);
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            cb.IsValidMove(move).Should().BeFalse("move should be invalid");
        }

        [TestMethod]
        public void TestInvalidMoveBlackPawnPromotion_PromotionToPawn()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, kw, o, kb, pb, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            Move move = new Move(new Position(Line.g, 2), new Position(Line.g, 1), Player.Black, 'P');
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            cb.IsValidMove(move).Should().BeFalse("move should be invalid");
        }

        [TestMethod]
        public void TestMakeMoveBlackPawn_EnPassant()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.b, 1), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.e, 7), new Position(Line.e, 5), Player.Black);
            Move move3 = new Move(new Position(Line.e, 2), new Position(Line.e, 3), Player.White);
            Move move4 = new Move(new Position(Line.e, 5), new Position(Line.e, 4), Player.Black);
            Move move5 = new Move(new Position(Line.d, 2), new Position(Line.d, 4), Player.White);
            Move move6 = new Move(new Position(Line.e, 4), new Position(Line.d, 3), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);
            cb.MakeMove(move4, true);
            cb.MakeMove(move5, true);
            cb.MakeMove(move6, true);

            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, nb, bb, qb, kb, bb, nb, rb },
                new[] { pb, pb, pb, pb, o, pb, pb, pb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { nw, o, o, pb, pw, o, o, o },
                new[] { pw, pw, pw, o, o, pw, pw, pw },
                new[] { rw, o, bw, qw, kw, bw, nw, rw }
            };

            cb.GetBoard().Should().BeEquivalentTo(board, "Unexpected board layout after en passant capture.");
        }

        [TestMethod]
        public void TestMakeMoveWhiteKing_KingsideCastling()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, rw }
            };
            Move move = new Move(new Position(Line.e, 1), new Position(Line.g, 1), Player.White);
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            cb.MakeMove(move, true);

            Piece?[][] expected = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, rw, kw, o }
            };

            cb.GetBoard().Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestMakeMoveWhiteKing_KingsideCastling2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, rw }
            };
            Move move = new Move(new Position(Line.e, 1), new Position(Line.h, 1), Player.White);
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            cb.MakeMove(move, true);

            Piece?[][] expected = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, rw, kw, o }
            };

            cb.GetBoard().Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestMakeMoveWhiteKing_QueensideCastling()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { rw, o, o, o, kw, o, o, o }
            };
            Move move = new Move(new Position(Line.e, 1), new Position(Line.c, 1), Player.White);
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            cb.MakeMove(move, true);

            Piece?[][] expected = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, kw, rw, o, o, o, o }
            };

            cb.GetBoard().Should().BeEquivalentTo(expected);
        }


        [TestMethod]
        public void TestMakeMoveWhiteKing_QueensideCastling2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { rw, o, o, o, kw, o, o, o }
            };
            Move move = new Move(new Position(Line.e, 1), new Position(Line.a, 1), Player.White);
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            cb.MakeMove(move, true);

            Piece?[][] expected = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, kw, rw, o, o, o, o }
            };

            cb.GetBoard().Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestMakeMoveBlackKing_KingsideCastling()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, rb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            Move move = new Move(new Position(Line.e, 8), new Position(Line.g, 8), Player.Black);
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            cb.MakeMove(move, true);

            Piece?[][] expected = new Piece?[8][]
            {
                new[] { o, o, o, o, o, rb, kb, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };

            cb.GetBoard().Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestMakeMoveBlackKing_KingsideCastling2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, rb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            Move move = new Move(new Position(Line.e, 8), new Position(Line.h, 8), Player.Black);
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            cb.MakeMove(move, true);

            Piece?[][] expected = new Piece?[8][]
            {
                new[] { o, o, o, o, o, rb, kb, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };

            cb.GetBoard().Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestMakeMoveBlackKing_QueensideCastling()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            Move move = new Move(new Position(Line.e, 8), new Position(Line.c, 8), Player.Black);
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            cb.MakeMove(move, true);

            Piece?[][] expected = new Piece?[8][]
            {
                new[] { o, o, kb, rb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };

            cb.GetBoard().Should().BeEquivalentTo(expected);
        }


        [TestMethod]
        public void TestMakeMoveBlackKing_QueensideCastling2()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };
            Move move = new Move(new Position(Line.e, 8), new Position(Line.a, 8), Player.Black);
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            cb.MakeMove(move, true);

            Piece?[][] expected = new Piece?[8][]
            {
                new[] { o, o, kb, rb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o }
            };

            cb.GetBoard().Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestGetValidMovesWhiteStartingPosition()
        {
            ChessGame cb = new ChessGame();
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(Player.White);
            List<Move> expected = new List<Move>()
            {
                new Move("A2", "A3", Player.White),
                new Move("A2", "A4", Player.White),
                new Move("B2", "B3", Player.White),
                new Move("B2", "B4", Player.White),
                new Move("C2", "C3", Player.White),
                new Move("C2", "C4", Player.White),
                new Move("D2", "D3", Player.White),
                new Move("D2", "D4", Player.White),
                new Move("E2", "E3", Player.White),
                new Move("E2", "E4", Player.White),
                new Move("F2", "F3", Player.White),
                new Move("F2", "F4", Player.White),
                new Move("G2", "G3", Player.White),
                new Move("G2", "G4", Player.White),
                new Move("H2", "H3", Player.White),
                new Move("H2", "H4", Player.White),
                new Move("B1", "A3", Player.White),
                new Move("B1", "C3", Player.White),
                new Move("G1", "F3", Player.White),
                new Move("G1", "H3", Player.White)
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestGetValidMovesBlackStartingPosition()
        {
            ChessGame cb = new ChessGame();
            cb.MakeMove(new Move(new Position(Line.a, 2), new Position(Line.a, 3), Player.White), true);
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(Player.Black);
            List<Move> expected = new List<Move>()
            {
                new Move("A7", "A6", Player.Black),
                new Move("A7", "A5", Player.Black),
                new Move("B7", "B6", Player.Black),
                new Move("B7", "B5", Player.Black),
                new Move("C7", "C6", Player.Black),
                new Move("C7", "C5", Player.Black),
                new Move("D7", "D6", Player.Black),
                new Move("D7", "D5", Player.Black),
                new Move("E7", "E6", Player.Black),
                new Move("E7", "E5", Player.Black),
                new Move("F7", "F6", Player.Black),
                new Move("F7", "F5", Player.Black),
                new Move("G7", "G6", Player.Black),
                new Move("G7", "G5", Player.Black),
                new Move("H7", "H6", Player.Black),
                new Move("H7", "H5", Player.Black),
                new Move("B8", "A6", Player.Black),
                new Move("B8", "C6", Player.Black),
                new Move("G8", "F6", Player.Black),
                new Move("G8", "H6", Player.Black)
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestGetValidMovesWhiteKing()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, kb, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, kw, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(new Position(Line.d, 4));
            List<Move> expected = new List<Move>()
            {
                new Move("D4", "C3", Player.White),
                new Move("D4", "C4", Player.White),
                new Move("D4", "C5", Player.White),
                new Move("D4", "D3", Player.White),
                new Move("D4", "D5", Player.White),
                new Move("D4", "E3", Player.White),
                new Move("D4", "E4", Player.White),
                new Move("D4", "E5", Player.White)
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestGetValidMovesWhiteKnight()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, kw, o, kb, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, nw, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(new Position(Line.d, 4));
            List<Move> expected = new List<Move>()
            {
                new Move("D4", "C2", Player.White),
                new Move("D4", "B3", Player.White),
                new Move("D4", "C6", Player.White),
                new Move("D4", "B5", Player.White),
                new Move("D4", "E2", Player.White),
                new Move("D4", "E6", Player.White),
                new Move("D4", "F3", Player.White),
                new Move("D4", "F5", Player.White)
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestGetValidMovesWhiteBishop()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, kw, o, kb, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, bw, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(new Position(Line.d, 4));
            List<Move> expected = new List<Move>()
            {
                new Move("D4", "A1", Player.White),
                new Move("D4", "B2", Player.White),
                new Move("D4", "C3", Player.White),
                new Move("D4", "E5", Player.White),
                new Move("D4", "F6", Player.White),
                new Move("D4", "G7", Player.White),
                new Move("D4", "H8", Player.White),
                new Move("D4", "C5", Player.White),
                new Move("D4", "B6", Player.White),
                new Move("D4", "A7", Player.White),
                new Move("D4", "E3", Player.White),
                new Move("D4", "F2", Player.White),
                new Move("D4", "G1", Player.White)
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestGetValidMovesWhiteRook()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kw, o, kb, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, rw, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(new Position(Line.d, 4));
            List<Move> expected = new List<Move>()
            {
                new Move("D4", "D1", Player.White),
                new Move("D4", "D2", Player.White),
                new Move("D4", "D3", Player.White),
                new Move("D4", "D5", Player.White),
                new Move("D4", "D6", Player.White),
                new Move("D4", "D7", Player.White),
                new Move("D4", "D8", Player.White),
                new Move("D4", "A4", Player.White),
                new Move("D4", "B4", Player.White),
                new Move("D4", "C4", Player.White),
                new Move("D4", "E4", Player.White),
                new Move("D4", "F4", Player.White),
                new Move("D4", "G4", Player.White),
                new Move("D4", "H4", Player.White)
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestGetValidMovesWhiteQueen()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kw, o, kb, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, qw, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(new Position(Line.d, 4));
            List<Move> expected = new List<Move>()
            {
                new Move("D4", "D1", Player.White),
                new Move("D4", "D2", Player.White),
                new Move("D4", "D3", Player.White),
                new Move("D4", "D5", Player.White),
                new Move("D4", "D6", Player.White),
                new Move("D4", "D7", Player.White),
                new Move("D4", "D8", Player.White),
                new Move("D4", "A4", Player.White),
                new Move("D4", "B4", Player.White),
                new Move("D4", "C4", Player.White),
                new Move("D4", "E4", Player.White),
                new Move("D4", "F4", Player.White),
                new Move("D4", "G4", Player.White),
                new Move("D4", "H4", Player.White),
                new Move("D4", "A1", Player.White),
                new Move("D4", "B2", Player.White),
                new Move("D4", "C3", Player.White),
                new Move("D4", "E5", Player.White),
                new Move("D4", "F6", Player.White),
                new Move("D4", "G7", Player.White),
                new Move("D4", "H8", Player.White),
                new Move("D4", "G7", Player.White),
                new Move("D4", "F6", Player.White),
                new Move("D4", "E5", Player.White),
                new Move("D4", "C3", Player.White),
                new Move("D4", "B2", Player.White),
                new Move("D4", "A1", Player.White)
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestGetValidMovesWhitePawn()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kw, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, kb, o, o, o, o },
                new[] { pb, o, pb, o, o, o, o, o },
                new[] { o, pw, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(new Position(Line.b, 2));
            List<Move> expected = new List<Move>()
            {
                new Move("B2", "B3", Player.White),
                new Move("B2", "B4", Player.White),
                new Move("B2", "A3", Player.White),
                new Move("B2", "C3", Player.White)
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestGetValidMovesWhitePawnPromotion()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kw, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, pw },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, kb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(new Position(Line.h, 7));
            List<Move> expected = new List<Move>()
            {
                new Move("H7", "H8", Player.White, 'Q'),
                new Move("H7", "H8", Player.White, 'r'),
                new Move("H7", "H8", Player.White, 'B'),
                new Move("H7", "H8", Player.White, 'n')
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestGetValidMovesBlackKing()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kw, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, kb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(new Position(Line.d, 4));
            List<Move> expected = new List<Move>()
            {
                new Move("D4", "C3", Player.Black),
                new Move("D4", "C4", Player.Black),
                new Move("D4", "C5", Player.Black),
                new Move("D4", "D3", Player.Black),
                new Move("D4", "D5", Player.Black),
                new Move("D4", "E3", Player.Black),
                new Move("D4", "E4", Player.Black),
                new Move("D4", "E5", Player.Black)
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestGetValidMovesBlackKnight()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, kw, o, kb, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, nb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(new Position(Line.d, 4));
            List<Move> expected = new List<Move>()
            {
                new Move("D4", "C2", Player.Black),
                new Move("D4", "B3", Player.Black),
                new Move("D4", "C6", Player.Black),
                new Move("D4", "B5", Player.Black),
                new Move("D4", "E2", Player.Black),
                new Move("D4", "E6", Player.Black),
                new Move("D4", "F3", Player.Black),
                new Move("D4", "F5", Player.Black)
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestGetValidMovesBlackBishop()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, kw, o, kb, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, bb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(new Position(Line.d, 4));
            List<Move> expected = new List<Move>()
            {
                new Move("D4", "A1", Player.Black),
                new Move("D4", "B2", Player.Black),
                new Move("D4", "C3", Player.Black),
                new Move("D4", "E5", Player.Black),
                new Move("D4", "F6", Player.Black),
                new Move("D4", "G7", Player.Black),
                new Move("D4", "H8", Player.Black),
                new Move("D4", "C5", Player.Black),
                new Move("D4", "B6", Player.Black),
                new Move("D4", "A7", Player.Black),
                new Move("D4", "E3", Player.Black),
                new Move("D4", "F2", Player.Black),
                new Move("D4", "G1", Player.Black)
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestGetValidMovesBlackRook()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kw, o, kb, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, rb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(new Position(Line.d, 4));
            List<Move> expected = new List<Move>()
            {
                new Move("D4", "D1", Player.Black),
                new Move("D4", "D2", Player.Black),
                new Move("D4", "D3", Player.Black),
                new Move("D4", "D5", Player.Black),
                new Move("D4", "D6", Player.Black),
                new Move("D4", "D7", Player.Black),
                new Move("D4", "D8", Player.Black),
                new Move("D4", "A4", Player.Black),
                new Move("D4", "B4", Player.Black),
                new Move("D4", "C4", Player.Black),
                new Move("D4", "E4", Player.Black),
                new Move("D4", "F4", Player.Black),
                new Move("D4", "G4", Player.Black),
                new Move("D4", "H4", Player.Black)
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestGetValidMovesBlackQueen()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kw, o, kb, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, qb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(new Position(Line.d, 4));
            List<Move> expected = new List<Move>()
            {
                new Move("D4", "D1", Player.Black),
                new Move("D4", "D2", Player.Black),
                new Move("D4", "D3", Player.Black),
                new Move("D4", "D5", Player.Black),
                new Move("D4", "D6", Player.Black),
                new Move("D4", "D7", Player.Black),
                new Move("D4", "D8", Player.Black),
                new Move("D4", "A4", Player.Black),
                new Move("D4", "B4", Player.Black),
                new Move("D4", "C4", Player.Black),
                new Move("D4", "E4", Player.Black),
                new Move("D4", "F4", Player.Black),
                new Move("D4", "G4", Player.Black),
                new Move("D4", "H4", Player.Black),
                new Move("D4", "A1", Player.Black),
                new Move("D4", "B2", Player.Black),
                new Move("D4", "C3", Player.Black),
                new Move("D4", "E5", Player.Black),
                new Move("D4", "F6", Player.Black),
                new Move("D4", "G7", Player.Black),
                new Move("D4", "H8", Player.Black),
                new Move("D4", "G7", Player.Black),
                new Move("D4", "F6", Player.Black),
                new Move("D4", "E5", Player.Black),
                new Move("D4", "C3", Player.Black),
                new Move("D4", "B2", Player.Black),
                new Move("D4", "A1", Player.Black)
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestGetValidMovesBlackPawn()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kw, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, kb, o, o, o, o },
                new[] { o, pb, o, o, o, o, o, o },
                new[] { pw, o, pw, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(new Position(Line.b, 3));
            List<Move> expected = new List<Move>()
            {
                new Move("B3", "B2", Player.Black),
                new Move("B3", "A2", Player.Black),
                new Move("B3", "C2", Player.Black)
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestGetValidMovesBlackPawnPromotion()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kw, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, kb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, pb },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });
            ReadOnlyCollection<Move> actual = cb.GetValidMoves(new Position(Line.h, 2));
            List<Move> expected = new List<Move>()
            {
                new Move("H2", "H1", Player.Black, 'q'),
                new Move("H2", "H1", Player.Black, 'R'),
                new Move("H2", "H1", Player.Black, 'N'),
                new Move("H2", "H1", Player.Black, 'b')
            };

            actual.Count.Should().Be(expected.Count);
            foreach (Move move in expected)
            {
                actual.Should().Contain(move, "Actual does not contain move " + move.ToString());
            }
        }

        [TestMethod]
        public void TestIsWhiteInCheck()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, o },
                new[] { kw, o, o, o, o, o, o, qb },
                new[] { pw, o, o, pb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { kb, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });

            cb.IsInCheck(Player.White).Should().BeTrue();
            cb.IsInCheck(Player.White).Should().BeTrue();

            cb.MakeMove(new Move(new Position(Line.a, 7), new Position(Line.a, 8), Player.White), true);
            cb.IsInCheck(Player.White).Should().BeFalse();
        }

        [TestMethod]
        public void TestIsWhiteInCheck_OnRank1()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, pb, o },
                new[] { kb, o, o, o, o, o, o, kw }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });

            cb.IsInCheck(Player.White).Should().BeTrue();
        }

        [TestMethod]
        public void TestIsWhiteNotInCheck()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kw, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, qb },
                new[] { pw, o, o, pb, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });

            cb.IsInCheck(Player.White).Should().BeFalse();
        }

        [TestMethod]
        public void TestIsWhiteNotInCheck_PawnsCanOnlyCheckDiagonally()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, o },
                new[] { o, o, o, o, pb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { kb, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });

            cb.IsInCheck(Player.White).Should().BeFalse();
        }

        [TestMethod]
        public void TestIsBlackInCheck()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, nw, o, o, o, o },
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { kw, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, pb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });

            cb.IsInCheck(Player.Black).Should().BeTrue();
            cb.MakeMove(new Move(new Position(Line.b, 7), new Position(Line.a, 7), Player.Black), true);

            cb.IsInCheck(Player.Black).Should().BeFalse();
        }

        [TestMethod]
        public void TestIsBlackInCheck_OnRank8()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, kb, o, o, o },
                new[] { o, o, o, pw, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { kw, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });

            cb.IsInCheck(Player.Black).Should().BeTrue();
        }

        [TestMethod]
        public void TestIsBlackNotInCheck()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, nb, o, o, o, o },
                new[] { o, kb, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { kw, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, pb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });

            cb.IsInCheck(Player.Black).Should().BeFalse();
        }

        [TestMethod]
        public void TestIsBlackNotInCheck_PawnsCanOnlyCheckDiagonally()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kb, o, o, o },
                new[] { o, o, o, o, pw, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { kw, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });

            cb.IsInCheck(Player.Black).Should().BeFalse();
        }

        [TestMethod]
        public void TestBlackCheckmated()
        {
            ChessGame cb = new ChessGame();
            cb.MakeMove(new Move("E2", "E4", Player.White), true);
            cb.MakeMove(new Move("E7", "E5", Player.Black), true);
            cb.MakeMove(new Move("F1", "C4", Player.White), true);
            cb.MakeMove(new Move("D7", "D6", Player.Black), true);
            cb.MakeMove(new Move("D1", "F3", Player.White), true);
            cb.MakeMove(new Move("H7", "H6", Player.Black), true);
            cb.MakeMove(new Move("F3", "F7", Player.White), false).Should().NotBe(MoveType.Invalid);
            cb.IsCheckmated(Player.Black).Should().BeTrue();
            cb.IsWinner(Player.White).Should().BeTrue();
        }

        [TestMethod]
        public void TestBlackStalemated()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kb, o, kw, o, o, o, o, o },
                new[] { o, o, qw, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });

            cb.IsStalemated(Player.Black).Should().BeTrue();
            cb.IsDraw().Should().BeTrue();
        }

        [TestMethod]
        public void TestBlackNotStalemated()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kb, o, kw, o, o, o, o, o },
                new[] { o, o, qw, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });

            cb.IsStalemated(Player.Black).Should().BeFalse();
        }

        [TestMethod]
        public void TestBlackNotStalematedAfterMakeMove()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { o, o, kw, o, o, o, o, o },
                new[] { kb, o, qw, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.Black, CastlingType = allCastlingType });

            cb.MakeMove(new Move("A7", "A8", Player.Black), false).Should().NotBe(MoveType.Invalid);

            cb.IsStalemated(Player.Black).Should().BeFalse();
        }

        [TestMethod]
        public void TestNonInsufficientMaterial()
        {
            var cb = new ChessGame();

            cb.IsDraw().Should().BeFalse();
        }

        [TestMethod]
        public void TestNotInsufficientMaterialKingVsKingBishopAndKnight()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kb, o, kw, nb, bb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.None });

            cb.IsDraw().Should().BeFalse();
        }

        [TestMethod]
        public void TestInsufficientMaterialKingVsKing()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kb, o, kw, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });

            cb.IsDraw().Should().BeTrue();
        }

        [TestMethod]
        public void TestInsufficientMaterialKingVsKingAndBishop()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kb, o, kw, o, bb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.None });

            cb.IsDraw().Should().BeTrue();
        }

        [TestMethod]
        public void TestInsufficientMaterialKingVsKingAndBishopFlipped()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kb, o, kw, o, bw, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.None });

            cb.IsDraw().Should().BeTrue();
        }

        [TestMethod]
        public void TestInsufficientMaterialKingVsKingAndKnight()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kb, o, kw, o, nb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.None });

            cb.IsDraw().Should().BeTrue();
        }

        [TestMethod]
        public void TestInsufficientMaterialKingVsKingAndKnightFlipped()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kb, o, kw, o, nw, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.None });

            cb.IsDraw().Should().BeTrue();
        }

        [TestMethod]
        public void TestInsufficientMaterialKingVsKingAndTwoKnight()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kb, o, kw, nb, nb, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.None });

            cb.IsDraw().Should().BeTrue();
        }

        [TestMethod]
        public void TestInsufficientMaterialKingAndBishopVsKingAndBishopOnSameColor()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kb, o, kw, o, o, o, o, o },
                new[] { bb, o, o, o, o, o, o, o },
                new[] { o, bw, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.None });

            cb.IsDraw().Should().BeTrue();
        }

        [TestMethod]
        public void TestInsufficientMaterialKingAndBishopVsKingAndBishopOnDifferentColor()
        {
            Piece?[][] board = new Piece?[8][]
            {
                new[] { kb, o, kw, o, o, o, o, o },
                new[] { bb, o, o, o, o, o, o, o },
                new[] { bw, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o }
            };
            ChessGame cb = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.None });

            cb.IsDraw().Should().BeTrue();
        }

        [TestMethod]
        public void TestMakeMove_ReturnedMoveType()
        {
            ChessGame game = new ChessGame();
            MoveType type = game.MakeMove(new Move("E2", "E4", Player.White), true);
            type.Should().Be(MoveType.Move);
            type = game.MakeMove(new Move("D7", "D5", Player.Black), true);
            type.Should().Be(MoveType.Move);
            type = game.MakeMove(new Move("E4", "D5", Player.White), true);
            type.Should().Be(MoveType.Move | MoveType.Capture);
            type = game.MakeMove(new Move("A5", "A4", Player.White), false);
            type.Should().Be(MoveType.Invalid);

            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, o, o, kb, o, o, o, o },
                new[] { o, pw, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { pb, o, o, o, o, o, o, o },
                new[] { o, o, o, o, kw, o, o, rw }
            };
            game = new ChessGame(new GameCreationData { Board = board, CurrentPlayer = Player.White, CastlingType = allCastlingType });
            type = game.MakeMove(new Move("E1", "G1", Player.White), true);
            type.Should().Be(MoveType.Move | MoveType.Castling);
            type = game.MakeMove(new Move("A2", "A1", Player.Black, 'Q'), true);
            type.Should().Be(MoveType.Move | MoveType.Promotion);
            type = game.MakeMove(new Move("B7", "A8", Player.White, 'Q'), true);
            type.Should().Be(MoveType.Move | MoveType.Capture | MoveType.Promotion);
        }

        [TestMethod]
        public void TestFenCastlingFieldAfterRookCapture_BlackKingside()
        {
            ChessGame game = new ChessGame("rnbqkbnr/p1pppp1p/1p4p1/8/8/1P6/PBPPPPPP/RN1QKBNR w KQkq - 0 3");
            game.MakeMove(new Move("b2", "h8", Player.White), true);
            game.CastlingType.Should().Be(whiteCastleCastlingType | CastlingType.BlackCastleQueenSide);
            game.GetFen().Should().Be("rnbqkbnB/p1pppp1p/1p4p1/8/8/1P6/P1PPPPPP/RN1QKBNR b KQq - 0 3");
        }

        [TestMethod]
        public void TestFenCastlingFieldAfterRookCapture_BlackQueenside()
        {
            ChessGame game = new ChessGame("rnbqkbnr/p1pppp1p/1p4p1/8/8/6P1/PPPPPPBP/RNBQK1NR w KQkq - 0 3");
            game.MakeMove(new Move("g2", "a8", Player.White), true);
            game.CastlingType.Should().Be(whiteCastleCastlingType | CastlingType.BlackCastleKingSide);
            game.GetFen().Should().Be("Bnbqkbnr/p1pppp1p/1p4p1/8/8/6P1/PPPPPP1P/RNBQK1NR b KQk - 0 3");
        }

        [TestMethod]
        public void TestFenCastlingFieldAfterRookCapture_WhiteKingside()
        {
            ChessGame game = new ChessGame("rn1qkbnr/pbpppppp/1p6/8/1P6/6P1/P1PPPP1P/RNBQKBNR b KQkq - 0 3");
            game.MakeMove(new Move("b7", "h1", Player.Black), true);
            (game.CastlingType & CastlingType.WhiteCastleKingSide).Should().Be(CastlingType.None);
            game.GetFen().Should().Be("rn1qkbnr/p1pppppp/1p6/8/1P6/6P1/P1PPPP1P/RNBQKBNb w Qkq - 0 4");
        }

        [TestMethod]
        public void TestFenCastlingFieldAfterRookCapture_WhiteQueenside()
        {
            ChessGame game = new ChessGame("rnbqk1nr/ppppppbp/6p1/8/6P1/1P6/P1PPPP1P/RNBQKBNR b KQkq - 0 3");
            game.MakeMove(new Move("g7", "a1", Player.Black), true);
            (game.CastlingType & CastlingType.WhiteCastleQueenSide).Should().Be(CastlingType.None);
            game.GetFen().Should().Be("rnbqk1nr/pppppp1p/6p1/8/6P1/1P6/P1PPPP1P/bNBQKBNR w Kkq - 0 4");
        }

        [TestMethod]
        public void TestPgnGeneration()
        {
            string pgn = "[Event \"Test\"]\r\n1. e4 c5 2. Bc4 e6 3. Nc3 d5 4. exd5 exd5 5. Bxd5 Nf6 6. Bb3 Be7 7. Nge2 O-O 8. O-O Nc6 9. a3 Bd6 10. d3 Bxh2+ 11. Kxh2 Ng4+ 12. Kg3 h5 13. f3 h4+ 14. Kf4 Qf6+ 15. Ke4 Qe5# 0-1";
            ChessGame game = new ChessGame();
            PgnParser m_pgnParser = new PgnParser(game);
            m_pgnParser.InitFromString(pgn);
            m_pgnParser.ParseSingle(false,
                                    out int skipped,
                                    out int truncated,
                                    out PgnGame? pgnGame,
                                    out string? errTxt);
            m_pgnParser.PgnLexical!.GetStringAtPos(pgnGame!.StartingPos, pgnGame.Length).Should().Be(pgn);
        }

        [TestMethod]
        public void TestRedoMakeMoveBlackPawn_EnPassant()
        {
            ChessGame cb = new ChessGame();
            Move move1 = new Move(new Position(Line.b, 1), new Position(Line.a, 3), Player.White);
            Move move2 = new Move(new Position(Line.e, 7), new Position(Line.e, 5), Player.Black);
            Move move3 = new Move(new Position(Line.e, 2), new Position(Line.e, 3), Player.White);
            Move move4 = new Move(new Position(Line.e, 5), new Position(Line.e, 4), Player.Black);
            Move move5 = new Move(new Position(Line.d, 2), new Position(Line.d, 4), Player.White);
            Move move6 = new Move(new Position(Line.e, 4), new Position(Line.d, 3), Player.Black);

            cb.MakeMove(move1, true);
            cb.MakeMove(move2, true);
            cb.MakeMove(move3, true);
            cb.MakeMove(move4, true);
            cb.MakeMove(move5, true);
            cb.MakeMove(move6, true);

            Piece?[][] board = new Piece?[8][]
            {
                new[] { rb, nb, bb, qb, kb, bb, nb, rb },
                new[] { pb, pb, pb, pb, o, pb, pb, pb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { nw, o, o, pb, pw, o, o, o },
                new[] { pw, pw, pw, o, o, pw, pw, pw },
                new[] { rw, o, bw, qw, kw, bw, nw, rw }
            };

            cb.GetBoard().Should().BeEquivalentTo(board, "Unexpected board layout after en passant capture.");
            cb.Undo();
            cb.Undo();
            cb.Redo();
            cb.Redo();
            cb.GetBoard().Should().BeEquivalentTo(board, "Unexpected board layout after en passant capture.");
        }

        [TestMethod]
        public void TestSanAmbiguity1()
        {
            ChessGame game = new ChessGame("k7/8/K7/8/8/Q7/8/QQ6 w - - 0 1");
            game.MakeMove(new Move("B1", "B2", Player.White), true);
            game.Moves[game.Moves.Count - 1].SAN.Should().Be("Qbb2");

            game = new ChessGame("k7/8/K7/8/8/Q7/8/QQ6 w - - 0 1");
            game.MakeMove(new Move("A1", "B2", Player.White), true);
            game.Moves[game.Moves.Count - 1].SAN.Should().Be("Qa1b2");

            game = new ChessGame("k7/8/K7/8/8/Q7/8/QQ6 w - - 0 1");
            game.MakeMove(new Move("A3", "B2", Player.White), true);
            game.Moves[game.Moves.Count - 1].SAN.Should().Be("Q3b2");
        }

        [TestMethod]
        public void TestSanAmbiguity2()
        {
            ChessGame game = new ChessGame("7k/8/4n2K/8/8/5n2/2n5/8 b - - 0 1");
            game.MakeMove(new Move("F3", "D4", Player.Black), true);
            game.Moves[game.Moves.Count - 1].SAN.Should().Be("Nfd4");

            game = new ChessGame("7k/8/4n2K/8/8/5n2/2n5/8 b - - 0 1");
            game.MakeMove(new Move("C2", "D4", Player.Black), true);
            game.Moves[game.Moves.Count - 1].SAN.Should().Be("Ncd4");

            game = new ChessGame("7k/8/4n2K/8/8/5n2/2n5/8 b - - 0 1");
            game.MakeMove(new Move("E6", "D4", Player.Black), true);
            game.Moves[game.Moves.Count - 1].SAN.Should().Be("Ned4");
        }

        [TestMethod]
        public void TestSanAmbiguity3()
        {
            ChessGame game = new ChessGame("k7/8/K7/Q7/8/3B4/8/3B4 w - - 0 1");
            game.MakeMove(new Move("D1", "E2", Player.White), true);
            game.Moves[game.Moves.Count - 1].SAN.Should().Be("B1e2");

            game = new ChessGame("k7/8/K7/Q7/8/3B4/8/3B4 w - - 0 1");
            game.MakeMove(new Move("D3", "E2", Player.White), true);
            game.Moves[game.Moves.Count - 1].SAN.Should().Be("B3e2");
        }

        [TestMethod]
        public void TestSanAmbiguity4()
        {
            ChessGame game = new ChessGame("k7/8/K7/8/8/8/8/R6R w - - 0 1");
            game.MakeMove(new Move("A1", "E1", Player.White), true);
            game.Moves[game.Moves.Count - 1].SAN.Should().Be("Rae1");

            game = new ChessGame("k7/8/K7/8/8/8/8/R6R w - - 0 1");
            game.MakeMove(new Move("H1", "E1", Player.White), true);
            game.Moves[game.Moves.Count - 1].SAN.Should().Be("Rhe1");
        }

        [TestMethod]
        public void TestSanPawnCapturePromotionCheckmate()
        {
            ChessGame game = new ChessGame("k1r5/3P4/K7/8/8/8/8/8 w - - 0 1");
            game.MakeMove(new Move("D7", "C8", Player.White, 'R'), true);
            game.Moves[game.Moves.Count - 1].SAN.Should().Be("dxc8=R#");
        }

        [TestMethod]
        public void TestIsValidMoveEnPassantAfterCheck()
        {
            ChessGame cg = new ChessGame("2r5/p1pn1p2/1p6/5nkp/5Np1/4P1P1/PB3P1P/6K1 w - - 2 32");
            cg.MakeMove(new Move("H2", "H4", Player.White), true);
            cg.IsValidMove(new Move("G4", "H3", Player.Black)).Should().BeTrue();
        }

        [TestMethod]
        public void TestUndoOnNewGame()
        {
            const string initialBoard = "8/8/3k4/8/8/4K3/8/Q6R w - - 0 1";
            ChessGame game = new ChessGame(initialBoard);
            game.Undo().Should().BeFalse();
        }

        [TestMethod]
        public void TestUndoLastWhiteMove()
        {
            const string initialBoard = "8/8/3k4/8/8/4K3/8/Q6R w - - 0 1";
            ChessGame game = new ChessGame(initialBoard);
            game.MakeMove(new Move("a1", "h8", Player.White), true);
            game.Undo().Should().BeTrue();
            game.GetFen().Should().Be(initialBoard);
        }

        [TestMethod]
        public void TestUndoLastBlackMove()
        {
            const string initialBoard = "8/8/3k4/8/8/4K3/8/Q6R b - - 0 1";
            ChessGame game = new ChessGame(initialBoard);
            game.MakeMove(new Move("d6", "d7", Player.Black), true);
            game.Undo().Should().BeTrue();
            game.GetFen().Should().Be(initialBoard);
        }

        [TestMethod]
        public void TestUndoZeroMove()
        {
            const string initialBoard = "8/8/3k4/8/8/4K3/8/Q6R w - - 0 1";
            ChessGame game = new ChessGame(initialBoard);
            game.Undo(0).Should().Be(0);
            game.GetFen().Should().Be(initialBoard);
        }

        [TestMethod]
        public void TestUndoOneMove()
        {
            const string initialBoard = "8/8/3k4/8/8/4K3/8/Q6R w - - 0 1";
            ChessGame game = new ChessGame(initialBoard);
            game.MakeMove(new Move("a1", "h8", Player.White), true);
            game.Undo(1).Should().Be(1);
            game.GetFen().Should().Be(initialBoard);
        }

        [TestMethod]
        public void TestUndoUpToThreeMoves()
        {
            const string initialBoard = "8/8/3k4/8/8/4K3/8/Q6R w - - 0 1";
            ChessGame game = new ChessGame(initialBoard);
            game.MakeMove(new Move("a1", "h8", Player.White), true);
            game.MakeMove(new Move("d6", "d7", Player.Black), true);
            game.Undo(3).Should().Be(2);
            game.GetFen().Should().Be(initialBoard);
        }

        [TestMethod]
        public void TestUndoCaptureWhite()
        {
            const string initialBoard = "rnbqk1nr/pppp1ppp/8/4p3/1b6/2PP4/PP2PPPP/RNBQKBNR w KQkq - 1 3";
            ChessGame game = new ChessGame(initialBoard);
            game.MakeMove(new Move("c3", "b2", Player.White), true);
            game.Undo();
            game.GetFen().Should().Be(initialBoard);
        }

        [TestMethod]
        public void TestUndoCaptureBlack()
        {
            const string initialBoard = "rnbqkbnr/pppp1ppp/4p3/8/8/BP6/P1PPPPPP/RN1QKBNR b KQkq - 1 2";
            ChessGame game = new ChessGame(initialBoard);
            game.MakeMove(new Move("f8", "a3", Player.Black), true);
            game.Undo();
            game.GetFen().Should().Be(initialBoard);
        }

        [TestMethod]
        public void TestUndoPromotionWhite()
        {
            const string initialBoard = "8/1k2P3/8/8/8/8/1K2p3/8 w - - 0 1";
            ChessGame game = new ChessGame(initialBoard);
            game.MakeMove(new Move("e7", "e8", Player.White, 'q'), true);
            game.Undo();
            game.GetFen().Should().Be(initialBoard);
        }

        [TestMethod]
        public void TestUndoPromotionBlack()
        {
            const string initialBoard = "8/1k2P3/8/8/8/8/1K2p3/8 b - - 0 1";
            ChessGame game = new ChessGame(initialBoard);
            game.MakeMove(new Move("e2", "e1", Player.Black, 'N'), true);
            game.Undo();
            game.GetFen().Should().Be(initialBoard);
        }

        [TestMethod]
        public void TestUndoEnPassantWhite()
        {
            const string initialBoard = "8/1k6/8/3pP3/8/8/1K6/8 w - d6 0 2";
            ChessGame game = new ChessGame(initialBoard);
            game.MakeMove(new Move("e5", "d6", Player.White), true);
            game.Undo();
            game.GetFen().Should().Be(initialBoard);
        }

        [TestMethod]
        public void TestUndoEnPassantBlack()
        {
            const string initialBoard = "8/1k2P3/8/8/4pP2/8/1K6/8 b - f3 0 1";
            ChessGame game = new ChessGame(initialBoard);
            game.MakeMove(new Move("e4", "f3", Player.Black), true);
            game.Undo();
            game.GetFen().Should().Be(initialBoard);
        }

        [TestMethod]
        public void TestUndoQueenSideCastles()
        {
            const string pgn = "[Event \"Test\"]\r\n1. d4 d5 2. Be3 Be6 3. Nc3 Nc6 4. Qd3 Qd6 5. O-O-O O-O-O";
            ChessGame game = new ChessGame();
            PgnParser m_pgnParser = new PgnParser(game);
            m_pgnParser.InitFromString(pgn);
            m_pgnParser.ParseSingle(false,
                                    out int skipped,
                                    out int truncated,
                                    out PgnGame? pgnGame,
                                    out string? errTxt);

            game.Undo();
            game.CastlingType.Should().Be(blackCastleCastlingType);
            game.Undo();
            game.CastlingType.Should().Be(allCastlingType);
            game.Redo();
        }

        [TestMethod]
        public void TestUndoMoveRookQueenSide()
        {
            const string pgn = "[Event \"Test\"]\r\n1. d4 d5 2. Be3 Be6 3. Nc3 Nc6 4. Qd3 Qd6 5. Rb1 Rb8";
            ChessGame game = new ChessGame();
            PgnParser m_pgnParser = new PgnParser(game);
            m_pgnParser.InitFromString(pgn);
            m_pgnParser.ParseSingle(false,
                                    out int skipped,
                                    out int truncated,
                                    out PgnGame? pgnGame,
                                    out string? errTxt);

            game.Undo();
            game.CastlingType.Should().Be(CastlingType.WhiteCastleKingSide | CastlingType.BlackCastleKingSide | CastlingType.BlackCastleQueenSide);
            game.Undo();
            game.CastlingType.Should().Be(allCastlingType);
        }

        [TestMethod]
        public void TestUndoQueenSideCastlesWithKingSideInactive()
        {
            const string pgn = "[Event \"Test\"]\r\n1. d4 d5 2. Be3 Be6 3. Nc3 Nc6 4. Qd3 Qd6 5. h3 h6 6. Rh2 Rh7 7. Rh1 Rh8 8. O-O-O O-O-O";
            ChessGame game = new ChessGame();
            PgnParser m_pgnParser = new PgnParser(game);
            m_pgnParser.InitFromString(pgn);
            m_pgnParser.ParseSingle(false,
                                    out int skipped,
                                    out int truncated,
                                    out PgnGame? pgnGame,
                                    out string? errTxt);

            game.Undo();
            game.CastlingType.Should().Be(CastlingType.BlackCastleQueenSide);
            game.Undo();
            game.CastlingType.Should().Be(CastlingType.WhiteCastleQueenSide | CastlingType.BlackCastleQueenSide);
        }

        [TestMethod]
        public void TestUndoKingSideCastles()
        {
            const string pgn = "[Event \"Test\"]\r\n1. e4 e5 2. Bc4 Bc5 3. Nf3 Nf6 4. a3 a6 5. O-O O-O";
            ChessGame game = new ChessGame();
            PgnParser m_pgnParser = new PgnParser(game);
            m_pgnParser.InitFromString(pgn);
            m_pgnParser.ParseSingle(false,
                                    out int skipped,
                                    out int truncated,
                                    out PgnGame? pgnGame,
                                    out string? errTxt);

            game.Undo();
            game.CastlingType.Should().Be(blackCastleCastlingType);
            game.Undo();
            game.CastlingType.Should().Be(allCastlingType);
        }

        [TestMethod]
        public void TestUndoMoveRookKingSideCastles()
        {
            const string pgn = "[Event \"Test\"]\r\n1. e4 e5 2. Bc4 Bc5 3. Nf3 Nf6 4. a3 a6 5. Rg1 Rg8";
            ChessGame game = new ChessGame();
            PgnParser m_pgnParser = new PgnParser(game);
            m_pgnParser.InitFromString(pgn);
            m_pgnParser.ParseSingle(false,
                                    out int skipped,
                                    out int truncated,
                                    out PgnGame? pgnGame,
                                    out string? errTxt);

            game.Undo();
            game.CastlingType.Should().Be(blackCastleCastlingType | CastlingType.WhiteCastleQueenSide);
            game.Undo();
            game.CastlingType.Should().Be(allCastlingType);
        }

        [TestMethod]
        public void TestUndoKingSideCastlesWithQueenSideInactive()
        {
            const string pgn = "[Event \"Test\"]\r\n1. e4 e5 2. Bc4 Bc5 3. Nf3 Nf6 4. a3 a6 5. Ra2 Ra7 6. Ra1 Ra8 7. O-O O-O";
            ChessGame game = new ChessGame();
            PgnParser m_pgnParser = new PgnParser(game);
            m_pgnParser.InitFromString(pgn);
            m_pgnParser.ParseSingle(false,
                                    out int skipped,
                                    out int truncated,
                                    out PgnGame? pgnGame,
                                    out string? errTxt);

            game.Undo();
            game.CastlingType.Should().Be(CastlingType.BlackCastleKingSide);
            game.Undo();
            game.CastlingType.Should().Be(CastlingType.WhiteCastleKingSide | CastlingType.BlackCastleKingSide);
        }

        [TestMethod]
        public void ThrowIOEOnMoveWithNullSourcePiece()
        {
            ChessGame game = new ChessGame();
            Action action = () => game.MakeMove(new Move("a3", "a4", Player.White), true);
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void TestParseSingle()
        {
            ChessGame game = new ChessGame();
            PgnParser m_pgnParser = new PgnParser(game);
            m_pgnParser.InitFromFile("TestGame.pgn");
            m_pgnParser.ParseSingle(false,
                                    out int skipped,
                                    out int truncated,
                                    out PgnGame? pgnGame,
                                    out string? errTxt);
            skipped.Should().Be(0);
            truncated.Should().Be(0);
            pgnGame.Should().NotBeNull();
            errTxt.Should().BeNull();
        }

        [TestMethod]
        public void TestParseSingleErrorSkipped()
        {
            ChessGame game = new ChessGame();
            PgnParser m_pgnParser = new PgnParser(game);
            m_pgnParser.InitFromFile("TestGame_Error_Skipped.pgn");
            m_pgnParser.ParseSingle(false,
                                    out int skipped,
                                    out int truncated,
                                    out PgnGame? pgnGame,
                                    out string? errTxt);
            skipped.Should().Be(1);
            truncated.Should().Be(0);
            pgnGame.Should().NotBeNull();
            errTxt.Should().NotBeNull();
        }

        [TestMethod]
        public void TestParseSingleErrorTruncated()
        {
            ChessGame game = new ChessGame();
            PgnParser m_pgnParser = new PgnParser(game);
            m_pgnParser.InitFromFile("TestGame_Error_Truncated.pgn");
            m_pgnParser.ParseSingle(false,
                                    out int skipped,
                                    out int truncated,
                                    out PgnGame? pgnGame,
                                    out string? errTxt);
            skipped.Should().Be(0);
            truncated.Should().Be(1);
            pgnGame.Should().NotBeNull();
            errTxt.Should().NotBeNull();
        }

        [TestMethod]
        public void TestParseMultiple()
        {
            PgnParser m_pgnParser = new PgnParser(isDiagnoseOn: true);
            m_pgnParser.InitFromFile("Kasparovs_Games.pgn");
            m_pgnParser.ParseMultiple(out List<PgnGame> pgnGameList,
                                    null,
                                    null,
                                    out int processed,
                                    out int skipped,
                                    out int truncated);
            skipped.Should().Be(0);
            truncated.Should().Be(0);
        }

        [TestMethod]
        public void TestExtractBookKeyListFromMultipleLines()
        {
            string[] m_arrLineNames = { "Kasparovs_Games.pgn" };
            SortedList<Book.BookKey, Book.BookValue> m_soredListBookKeyList;
            Book book = new Book();
            PgnParser.ExtractBookKeyListFromMultipleFiles(m_arrLineNames,
                                                                    20 /*maxDepth*/,
                                                                    null,
                                                                    this,
                                                                    out m_soredListBookKeyList,
                                                                    out int processed,
                                                                    out int skipped,
                                                                    out int truncated,
                                                                    out string? errTxt);
            skipped.Should().Be(0);
            truncated.Should().Be(0);
            errTxt.Should().BeNull();
            book.CreateBookList(m_soredListBookKeyList, 3000, 2, null, null);
            book.SaveBookToFile("Kasparov.bin");
        }
    }
}
