namespace ChessDotNetCore.Tests
{
    [TestClass]
    public class MoveTests
    {
        [TestMethod]
        public void TestEquality()
        {
            Position position1 = new Position(Line.g, 4);
            Position position2 = new Position(Line.h, 5);
            Position position3 = new Position(Line.g, 4);
            Position position4 = new Position(Line.h, 5);
            Move move1 = new Move(position1, position2, Player.White);
            Move move2 = new Move(position3, position4, Player.White);
            Move? move3 = null;
            Move? move4 = null;
            move2.Should().BeEquivalentTo(move1, "move1 and move2 should be equal");
            move3.Should().BeEquivalentTo(move4, "move3 and move4 should be equal");

            (move1 == move2).Should().BeTrue("move1 == move2 should be true");
            (move3 == move4).Should().BeTrue("move3 == move4 should be true");
            (move1 != move2).Should().BeFalse("move1 != move2 should be false");
            (move3 != move4).Should().BeFalse("move3 != move4 should be false");
            move1.GetHashCode().Should().Be(move2.GetHashCode());
        }

        [TestMethod]
        public void TestInequality_DifferentPlayer()
        {
            Position position1 = new Position(Line.g, 4);
            Position position2 = new Position(Line.h, 5);
            Position position3 = new Position(Line.g, 4);
            Position position4 = new Position(Line.h, 5);
            Move move1 = new Move(position1, position2, Player.White);
            Move move2 = new Move(position3, position4, Player.Black);

            move2.Should().NotBeEquivalentTo(move1, "move1 and move2 are equal");
            move1.Equals(move2).Should().BeFalse("move1.Equals(move2) should be false");
            move2.Equals(move1).Should().BeFalse("move2.Equals(move1) should be false");
            (move1 == move2).Should().BeFalse("move1 == move2 should be false");
            (move2 == move1).Should().BeFalse("move2 == move1 should be false");
            (move1 != move2).Should().BeTrue("move1 != move2 should be true");
            (move2 != move1).Should().BeTrue("move2 != move1 should be true");
            move2.GetHashCode().Should().NotBe(move1.GetHashCode());
        }

        [TestMethod]
        public void TestInequality_DifferentLine()
        {
            Position position1 = new Position(Line.g, 4);
            Position position2 = new Position(Line.h, 5);
            Position position3 = new Position(Line.f, 4);
            Position position4 = new Position(Line.h, 5);
            Move move1 = new Move(position1, position2, Player.Black);
            Move move2 = new Move(position3, position4, Player.Black);

            move2.Should().NotBeEquivalentTo(move1, "move1 and move2 are equal");
            move1.Equals(move2).Should().BeFalse("move1.Equals(move2) should be false");
            move2.Equals(move1).Should().BeFalse("move2.Equals(move1) should be false");
            (move1 == move2).Should().BeFalse("move1 == move2 should be false");
            (move2 == move1).Should().BeFalse("move2 == move1 should be false");
            (move1 != move2).Should().BeTrue("move1 != move2 should be true");
            (move2 != move1).Should().BeTrue("move2 != move1 should be true");
            move2.GetHashCode().Should().NotBe(move1.GetHashCode());
        }

        [TestMethod]
        public void TestInequality_DifferentRank()
        {
            Position position1 = new Position(Line.g, 4);
            Position position2 = new Position(Line.h, 5);
            Position position3 = new Position(Line.f, 4);
            Position position4 = new Position(Line.h, 6);
            Move move1 = new Move(position1, position2, Player.Black);
            Move move2 = new Move(position3, position4, Player.Black);

            move2.Should().NotBeEquivalentTo(move1, "move1 and move2 are equal");
            move1.Equals(move2).Should().BeFalse("move1.Equals(move2) should be false");
            move2.Equals(move1).Should().BeFalse("move2.Equals(move1) should be false");
            (move1 == move2).Should().BeFalse("move1 == move2 should be false");
            (move2 == move1).Should().BeFalse("move2 == move1 should be false");
            (move1 != move2).Should().BeTrue("move1 != move2 should be true");
            (move2 != move1).Should().BeTrue("move2 != move1 should be true");
            move2.GetHashCode().Should().NotBe(move1.GetHashCode());
        }

        [TestMethod]
        public void TestInequality_DifferentRankAndLine()
        {
            Position position1 = new Position(Line.g, 4);
            Position position2 = new Position(Line.h, 5);
            Position position3 = new Position(Line.a, 1);
            Position position4 = new Position(Line.b, 2);
            Move move1 = new Move(position1, position2, Player.White);
            Move move2 = new Move(position3, position4, Player.White);

            move2.Should().NotBeEquivalentTo(move1, "move1 and move2 are equal");
            move1.Equals(move2).Should().BeFalse("move1.Equals(move2) should be false");
            move2.Equals(move1).Should().BeFalse("move2.Equals(move1) should be false");
            (move1 == move2).Should().BeFalse("move1 == move2 should be false");
            (move2 == move1).Should().BeFalse("move2 == move1 should be false");
            (move1 != move2).Should().BeTrue("move1 != move2 should be true");
            (move2 != move1).Should().BeTrue("move2 != move1 should be true");
            move2.GetHashCode().Should().NotBe(move1.GetHashCode());
        }

        [TestMethod]
        public void TestInequality_DifferentPromotion()
        {
            Position position1 = new Position(Line.a, 7);
            Position position2 = new Position(Line.a, 8);
            Move move1 = new Move(position1, position2, Player.White, 'Q');
            Move move2 = new Move(position1, position2, Player.White, 'N');

            move2.Should().NotBeEquivalentTo(move1, "move1 and move2 are equal");
            move1.Equals(move2).Should().BeFalse("move1.Equals(move2) should be false");
            move2.Equals(move1).Should().BeFalse("move2.Equals(move1) should be false");
            (move1 == move2).Should().BeFalse("move1 == move2 should be false");
            (move2 == move1).Should().BeFalse("move2 == move1 should be false");
            (move1 != move2).Should().BeTrue("move1 != move2 should be true");
            (move2 != move1).Should().BeTrue("move2 != move1 should be true");
            move2.GetHashCode().Should().NotBe(move1.GetHashCode());
        }

        [TestMethod]
        public void TestInequality_OneIsNull()
        {
            Position position1 = new Position(Line.g, 4);
            Position position2 = new Position(Line.g, 8);
            Move move1 = new Move(position1, position2, Player.Black);
            Move? move2 = null;
            (move1 == move2).Should().BeFalse("move1 == move2 should be false");
            Move? move3 = null;
            Move move4 = new Move(position1, position2, Player.Black);
            (move3 == move4).Should().BeFalse("move3 == move4 should be false");
        }

        [TestMethod]
        public void TestInequality_DifferentType()
        {
            Position position1 = new Position(Line.g, 4);
            Position position2 = new Position(Line.g, 8);
            Move move1 = new Move(position1, position2, Player.Black);
            move1.Equals(position1).Should().BeFalse("move1.Equals(position1) should be false");
        }
    }
}
