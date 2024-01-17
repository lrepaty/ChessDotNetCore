using ChessDotNetCore.Pieces;

namespace ChessDotNetCore.Tests
{
    [TestClass]
    public class UtilitiesTests
    {

        [TestMethod]
        public void TestThrowIfNull()
        {
            object? value = null;
            Action action = () => ChessUtilities.ThrowIfNull(value, "value");
            action.Should().Throw<ArgumentNullException>();

            Piece piece = new Bishop(Player.White);
            action = () => ChessUtilities.ThrowIfNull(piece, "piece");
            action.Should().NotThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void TestGetOpponentOf()
        {
            ChessUtilities.GetOpponentOf(Player.White).Should().Be(Player.Black);
            ChessUtilities.GetOpponentOf(Player.Black).Should().Be(Player.White);
            Action action = () => ChessUtilities.GetOpponentOf(Player.None);
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void LinesBetween()
        {
            new Line[] { Line.b, Line.c, Line.d }.Should().BeEquivalentTo(ChessUtilities.LinesBetween(Line.b, Line.d, true, true));
            new Line[] { Line.c }.Should().BeEquivalentTo(ChessUtilities.LinesBetween(Line.b, Line.d, false, false));
            new Line[] { Line.c, Line.d }.Should().BeEquivalentTo(ChessUtilities.LinesBetween(Line.b, Line.d, false, true));
            new Line[] { Line.b, Line.c }.Should().BeEquivalentTo(ChessUtilities.LinesBetween(Line.b, Line.d, true, false));

            new Line[] { Line.b, Line.c, Line.d }.Should().BeEquivalentTo(ChessUtilities.LinesBetween(Line.d, Line.b, true, true));
            new Line[] { Line.c }.Should().BeEquivalentTo(ChessUtilities.LinesBetween(Line.d, Line.b, false, false));
            new Line[] { Line.b, Line.c }.Should().BeEquivalentTo(ChessUtilities.LinesBetween(Line.d, Line.b, false, true));
            new Line[] { Line.c, Line.d }.Should().BeEquivalentTo(ChessUtilities.LinesBetween(Line.d, Line.b, true, false));

            new Line[] { }.Should().BeEquivalentTo(ChessUtilities.LinesBetween(Line.f, Line.f, false, false));
            new Line[] { Line.f }.Should().BeEquivalentTo(ChessUtilities.LinesBetween(Line.f, Line.f, true, true));
        }
    }
}
