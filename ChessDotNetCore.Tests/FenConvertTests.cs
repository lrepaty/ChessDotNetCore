using ChessDotNetCore.Pieces;

namespace ChessDotNetCore.Tests
{
    [TestClass]
    public class FenConvertTests
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

        [TestMethod]
        public void TestStartPosition()
        {
            ChessGame game = new ChessGame();
            string fen = game.GetFen();
            fen.Should().Be("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }

        [TestMethod]
        public void TestAfter1e4()
        {
            ChessGame game = new ChessGame();
            game.MakeMove(new Move("E2", "E4", Player.White), true);
            string fen = game.GetFen();
            fen.Should().Be("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1");
        }

        [TestMethod]
        public void TestAfter1c5()
        {
            ChessGame game = new ChessGame();
            game.MakeMove(new Move("E2", "E4", Player.White), true);
            game.MakeMove(new Move("C7", "C5", Player.Black), true);
            string fen = game.GetFen();
            fen.Should().Be("rnbqkbnr/pp1ppppp/8/2p5/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 2");
        }

        [TestMethod]
        public void TestAfter2Nf3()
        {
            ChessGame game = new ChessGame();
            game.MakeMove(new Move("E2", "E4", Player.White), true);
            game.MakeMove(new Move("C7", "C5", Player.Black), true);
            game.MakeMove(new Move("G1", "F3", Player.White), true);
            string fen = game.GetFen();
            fen.Should().Be("rnbqkbnr/pp1ppppp/8/2p5/4P3/5N2/PPPP1PPP/RNBQKB1R b KQkq - 1 2");
        }

        [TestMethod]
        public void TestMovingWhiteKingLosingCastlingRights()
        {
            ChessGame game = new ChessGame();
            game.MakeMove(new Move("E2", "E4", Player.White), true);
            game.MakeMove(new Move("C7", "C5", Player.Black), true);
            game.MakeMove(new Move("E1", "E2", Player.White), true);
            string fen = game.GetFen();
            fen.Should().Be("rnbqkbnr/pp1ppppp/8/2p5/4P3/8/PPPPKPPP/RNBQ1BNR b kq - 1 2");
        }

        [TestMethod]
        public void TestMovingBlackKingLosingCastlingRights()
        {
            ChessGame game = new ChessGame();
            game.MakeMove(new Move("E2", "E4", Player.White), true);
            game.MakeMove(new Move("E7", "E5", Player.Black), true);
            game.MakeMove(new Move("G1", "F3", Player.White), true);
            game.MakeMove(new Move("E8", "E7", Player.Black), true);
            string fen = game.GetFen();
            fen.Should().Be("rnbq1bnr/ppppkppp/8/4p3/4P3/5N2/PPPP1PPP/RNBQKB1R w KQ - 2 3");
        }

        [TestMethod]
        public void TestMovingWhiteARookLosingCastlingRights()
        {
            ChessGame game = new ChessGame();
            game.MakeMove(new Move("A2", "A3", Player.White), true);
            game.MakeMove(new Move("E7", "E5", Player.Black), true);
            game.MakeMove(new Move("A1", "A2", Player.White), true);
            string fen = game.GetFen();
            fen.Should().Be("rnbqkbnr/pppp1ppp/8/4p3/8/P7/RPPPPPPP/1NBQKBNR b Kkq - 1 2");
        }

        [TestMethod]
        public void TestMovingWhiteHRookLosingCastlingRights()
        {
            ChessGame game = new ChessGame();
            game.MakeMove(new Move("H2", "H3", Player.White), true);
            game.MakeMove(new Move("E7", "E5", Player.Black), true);
            game.MakeMove(new Move("H1", "H2", Player.White), true);
            string fen = game.GetFen();
            fen.Should().Be("rnbqkbnr/pppp1ppp/8/4p3/8/7P/PPPPPPPR/RNBQKBN1 b Qkq - 1 2");
        }

        [TestMethod]
        public void TestMovingBlackARookLosingCastlingRights()
        {
            ChessGame game = new ChessGame();
            game.MakeMove(new Move("E2", "E4", Player.White), true);
            game.MakeMove(new Move("A7", "A6", Player.Black), true);
            game.MakeMove(new Move("G1", "F3", Player.White), true);
            game.MakeMove(new Move("A8", "A7", Player.Black), true);
            string fen = game.GetFen();
            fen.Should().Be("1nbqkbnr/rppppppp/p7/8/4P3/5N2/PPPP1PPP/RNBQKB1R w KQk - 2 3");
        }

        [TestMethod]
        public void TestMovingBlackHRookLosingCastlingRights()
        {
            ChessGame game = new ChessGame();
            game.MakeMove(new Move("E2", "E4", Player.White), true);
            game.MakeMove(new Move("H7", "H6", Player.Black), true);
            game.MakeMove(new Move("G1", "F3", Player.White), true);
            game.MakeMove(new Move("H8", "H7", Player.Black), true);
            string fen = game.GetFen();
            fen.Should().Be("rnbqkbn1/pppppppr/7p/8/4P3/5N2/PPPP1PPP/RNBQKB1R w KQq - 2 3");
        }

        [TestMethod]
        public void TestHalfmoveClockAndFullmoveNumber()
        {
            ChessGame game = new ChessGame();
            game.MakeMove(new Move("E2", "E4", Player.White), true);
            game.MakeMove(new Move("E7", "E5", Player.Black), true);
            game.MakeMove(new Move("E1", "E2", Player.White), true);
            game.MakeMove(new Move("E8", "E7", Player.Black), true);
            game.MakeMove(new Move("E2", "D3", Player.White), true);
            game.MakeMove(new Move("E7", "D6", Player.Black), true);
            game.MakeMove(new Move("D3", "C3", Player.White), true);
            game.MakeMove(new Move("D6", "C6", Player.Black), true);
            game.MakeMove(new Move("C3", "B3", Player.White), true);
            game.MakeMove(new Move("C6", "B6", Player.Black), true);
            game.MakeMove(new Move("B3", "A4", Player.White), true);
            game.MakeMove(new Move("B6", "C5", Player.Black), true);
            game.MakeMove(new Move("F1", "C4", Player.White), true);
            string fen = game.GetFen();
            fen.Should().Be("rnbq1bnr/pppp1ppp/8/2k1p3/K1B1P3/8/PPPP1PPP/RNBQ2NR b - - 11 7");
            game.MakeMove(new Move("C5", "C4", Player.Black), true);
            fen = game.GetFen();
            fen.Should().Be("rnbq1bnr/pppp1ppp/8/4p3/K1k1P3/8/PPPP1PPP/RNBQ2NR w - - 0 8");
            game.MakeMove(new Move("A4", "A5", Player.White), true);
            game.MakeMove(new Move("H7", "H5", Player.Black), true);
            fen = game.GetFen();
            fen.Should().Be("rnbq1bnr/pppp1pp1/8/K3p2p/2k1P3/8/PPPP1PPP/RNBQ2NR w - - 0 9");
        }

        [TestMethod]
        public void TestChessGameFenConstructorStartPosition()
        {
            ChessGame game = new ChessGame("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            Piece?[][] expected = new Piece?[8][]
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
            game.GetBoard().Should().BeEquivalentTo(expected);
            game.HalfMoveClock.Should().Be(0);
            game.FullMoveNumber.Should().Be(1);
            game.CurrentPlayer.Should().Be(Player.White);
            game.CastlingType.Should().Be(
                CastlingType.WhiteCastleKingSide | CastlingType.WhiteCastleQueenSide |
                CastlingType.BlackCastleKingSide | CastlingType.BlackCastleQueenSide);
        }

        [TestMethod]
        public void TestChessGameFenConstructorAfter1e4()
        {
            ChessGame game = new ChessGame("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1");
            Piece?[][] expected = new Piece?[8][]
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
            game.GetBoard().Should().BeEquivalentTo(expected);
            game.HalfMoveClock.Should().Be(0);
            game.FullMoveNumber.Should().Be(1);
            game.CurrentPlayer.Should().Be(Player.Black);
        }

        [TestMethod]
        public void TestChessGameFenConstructorAfter1e3()
        {
            ChessGame game = new ChessGame("rnbqkbnr/pppppppp/8/8/8/4P3/PPPP1PPP/RNBQKBNR w KQkq - 0 1");
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
            game.GetBoard().Should().BeEquivalentTo(expected);
            game.HalfMoveClock.Should().Be(0);
            game.FullMoveNumber.Should().Be(1);
            game.Moves.Should().HaveCount(0);
        }

        [TestMethod]
        public void TestChessGameFenConstructorPartialCastlingRights()
        {
            ChessGame game = new ChessGame("rnbqkbn1/pppppppr/7p/8/4P3/5N2/PPPP1PPP/RNBQKB1R w KQq - 2 3");
            Piece?[][] expected = new Piece?[8][]
            {
                new[] { rb, nb, bb, qb, kb, bb, nb, o },
                new[] { pb, pb, pb, pb, pb, pb, pb, rb },
                new[] { o, o, o, o, o, o, o, pb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, o, o, pw, o, o, o },
                new[] { o, o, o, o, o, nw, o, o },
                new[] { pw, pw, pw, pw, o, pw, pw, pw },
                new[] { rw, nw, bw, qw, kw, bw, o, rw }
             };
            game.GetBoard().Should().BeEquivalentTo(expected);
            game.HalfMoveClock.Should().Be(2);
            game.FullMoveNumber.Should().Be(3);
            game.CastlingType.Should().Be(
                CastlingType.WhiteCastleKingSide | CastlingType.WhiteCastleQueenSide | CastlingType.BlackCastleQueenSide);
        }

        [TestMethod]
        public void TestChessGameFenConstructorNoCastlingRights()
        {
            ChessGame game = new ChessGame("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w - - 16 9");
            Piece?[][] expected = new Piece?[8][]
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
            game.GetBoard().Should().BeEquivalentTo(expected);
            game.HalfMoveClock.Should().Be(16);
            game.FullMoveNumber.Should().Be(9);
            game.CurrentPlayer.Should().Be(Player.White);
            game.CastlingType.Should().Be(CastlingType.None);
        }

        [TestMethod]
        public void TestChessGameFenConstructorAfter1e4c5()
        {
            ChessGame game = new ChessGame("rnbqkbnr/pp1ppppp/8/2p5/4P3/8/PPPP1PPP/RNBQKBNR w KQkq c6 0 2");
            Piece?[][] expected = new Piece?[8][]
            {
                new[] { rb, nb, bb, qb, kb, bb, nb, rb },
                new[] { pb, pb, o, pb, pb, pb, pb, pb },
                new[] { o, o, o, o, o, o, o, o },
                new[] { o, o, pb, o, o, o, o, o },
                new[] { o, o, o, o, pw, o, o, o },
                new[] { o, o, o, o, o, o, o, o },
                new[] { pw, pw, pw, pw, o, pw, pw, pw },
                new[] { rw, nw, bw, qw, kw, bw, nw, rw }
            };
            game.GetBoard().Should().BeEquivalentTo(expected);
            game.HalfMoveClock.Should().Be(0);
            game.FullMoveNumber.Should().Be(2);
        }

        [TestMethod]
        public void TestChessGameFenConstructorInvalid()
        {
            Action action = () => new ChessGame("rnbqkbnr/pppppppp/8/8/8/4P3/PPPP1PPP/RNBQKBNR b KQkq e3 0 1");
            action.Should().Throw<ArgumentException>();
            action = () => new ChessGame("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e6 0 1");
            action.Should().Throw<ArgumentException>();
            action = () => new ChessGame("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq");
            action.Should().Throw<ArgumentException>();
            action = () => new ChessGame("rnbqkbnr/pp1ppppp/8/2p5/4P3/8/PPPP1PPP/RNBQKBNR w KQkq c3 0 2");
            action.Should().Throw<ArgumentException>();
            action = () => new ChessGame("rnbqkbnr/pp1ppppp/2p5/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq c6 0 2");
            action.Should().Throw<ArgumentException>();
            action = () => new ChessGame("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP w KQkq - 0 1");
            action.Should().Throw<ArgumentException>();
            action = () => new ChessGame("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNZ w KQkq - 0 1");
            action.Should().Throw<ArgumentException>();
            action = () => new ChessGame("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBN w KQkq - 0 1");
            action.Should().Throw<ArgumentException>();
        }
    }
}
