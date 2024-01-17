using ChessDotNetCore.Pieces;

namespace ChessDotNetCore.Tests
{
    [TestClass]
    public class ChessPieceTests
    {
        [TestMethod]
        public void TestEquality()
        {
            Piece piece1 = new King(Player.White);
            Piece piece2 = new King(Player.White);
            piece2.Should().BeEquivalentTo(piece1, "piece1 and piece2 are not equal");
            piece1.Equals(piece2).Should().BeTrue("piece1.Equals(piece2) should be true");
            piece2.Equals(piece1).Should().BeTrue("piece2.Equals(piece1) should be true");
            (piece1 == piece2).Should().BeTrue("piece1 == piece2 should be true");
            (piece2 == piece1).Should().BeTrue("piece2 == piece1 should be true");
            (piece1 != piece2).Should().BeFalse("piece1 != piece2 should be false");
            (piece2 != piece1).Should().BeFalse("piece2 != piece1 should be false");
            piece2.GetHashCode().Should().Be(piece1.GetHashCode(), "Hash codes are different");
        }

        [TestMethod]
        public void TestInequality_DifferentPlayer()
        {
            Piece piece1 = new King(Player.White);
            Piece piece2 = new King(Player.Black);
            piece2.Should().NotBeEquivalentTo(piece1, "piece1 and piece2 are equal");
            piece1.Equals(piece2).Should().BeFalse("piece1.Equals(piece2) should be false");
            piece2.Equals(piece1).Should().BeFalse("piece2.Equals(piece1) should be false");
            (piece1 == piece2).Should().BeFalse("piece1 == piece2 should be false");
            (piece2 == piece1).Should().BeFalse("piece2 == piece1 should be false");
            (piece1 != piece2).Should().BeTrue("piece1 != piece2 should be true");
            (piece2 != piece1).Should().BeTrue("piece2 != piece1 should be true");
            piece2.GetHashCode().Should().NotBe(piece1.GetHashCode(), "Hash codes are equal");
        }

        [TestMethod]
        public void TestInequality_DifferentPiece()
        {
            Piece piece1 = new King(Player.Black);
            Piece piece2 = new Queen(Player.Black);
            piece2.Should().NotBeEquivalentTo(piece1, "piece1 and piece2 are equal");
            piece1.Equals(piece2).Should().BeFalse("piece1.Equals(piece2) should be false");
            piece2.Equals(piece1).Should().BeFalse("piece2.Equals(piece1) should be false");
            (piece1 == piece2).Should().BeFalse("piece1 == piece2 should be false");
            (piece2 == piece1).Should().BeFalse("piece2 == piece1 should be false");
            (piece1 != piece2).Should().BeTrue("piece1 != piece2 should be true");
            (piece2 != piece1).Should().BeTrue("piece2 != piece1 should be true");
            piece2.GetHashCode().Should().NotBe(piece1.GetHashCode(), "Hash codes are equal");
        }

        [TestMethod]
        public void TestInequality_DifferentPieceAndPlayer()
        {
            Piece piece1 = new King(Player.White);
            Piece piece2 = new Queen(Player.Black);
            piece2.Should().NotBeEquivalentTo(piece1, "piece1 and piece2 are equal");
            piece1.Equals(piece2).Should().BeFalse("piece1.Equals(piece2) should be false");
            piece2.Equals(piece1).Should().BeFalse("piece2.Equals(piece1) should be false");
            (piece1 == piece2).Should().BeFalse("piece1 == piece2 should be false");
            (piece2 == piece1).Should().BeFalse("piece2 == piece1 should be false");
            (piece1 != piece2).Should().BeTrue("piece1 != piece2 should be true");
            (piece2 != piece1).Should().BeTrue("piece2 != piece1 should be true");
            piece2.GetHashCode().Should().NotBe(piece1.GetHashCode(), "Hash codes are equal");
        }

        [TestMethod]
        public void TestInEquality_OneIsNull()
        {
            Piece? piece1 = new Rook(Player.White);
            Piece? piece2 = null;
            piece2.Should().NotBeEquivalentTo(piece1, "piece1 and piece2 are equal");
            piece1.Equals(piece2).Should().BeFalse("piece1.Equals(piece2) should be false");
            (piece1 == piece2).Should().BeFalse("piece1 == piece2 should be false");
            (piece2 == piece1).Should().BeFalse("piece2 == piece1 should be false");
            (piece1 != piece2).Should().BeTrue("piece1 != piece2 should be true");
            (piece2 != piece1).Should().BeTrue("piece2 != piece1 should be true");
            piece1.Should().NotBeEquivalentTo(piece2, "piece1 and piece2 are equal");
            (piece1 == piece2).Should().BeFalse("piece1 == piece2 should be false");
            (piece2 == piece1).Should().BeFalse("piece2 == piece1 should be false");
            (piece1 != piece2).Should().BeTrue("piece1 != piece2 should be true");
            (piece2 != piece1).Should().BeTrue("piece2 != piece1 should be true");

            piece1 = null;
            piece2 = new Bishop(Player.Black);
            piece2.Should().NotBeEquivalentTo(piece1, "piece1 and piece2 are equal");
            piece2.Equals(piece1).Should().BeFalse("piece1.Equals(piece2) should be false");
            (piece1 == piece2).Should().BeFalse("piece1 == piece2 should be false");
            (piece2 == piece1).Should().BeFalse("piece2 == piece1 should be false");
            (piece1 != piece2).Should().BeTrue("piece1 != piece2 should be true");
            (piece2 != piece1).Should().BeTrue("piece2 != piece1 should be true");
            piece1.Should().NotBeEquivalentTo(piece2, "piece1 and piece2 are equal");
            (piece1 == piece2).Should().BeFalse("piece1 == piece2 should be false");
            (piece2 == piece1).Should().BeFalse("piece2 == piece1 should be false");
            (piece1 != piece2).Should().BeTrue("piece1 != piece2 should be true");
            (piece2 != piece1).Should().BeTrue("piece2 != piece1 should be true");
        }
    }
}
