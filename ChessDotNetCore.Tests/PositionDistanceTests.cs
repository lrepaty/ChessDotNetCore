namespace ChessDotNetCore.Tests
{
    [TestClass]
    public class PositionDistanceTests
    {
        [TestMethod]
        public void TestPositionDistance()
        {
            Position position1 = new Position(Line.a, 2);
            Position position2 = new Position(Line.a, 3);
            PositionDistance distance1 = new PositionDistance(position1, position2);
            distance1.DistanceX.Should().Be(0);
            distance1.DistanceY.Should().Be(1);

            PositionDistance distance2 = new PositionDistance(position2, position1);
            distance2.DistanceX.Should().Be(0);
            distance2.DistanceY.Should().Be(1);
        }
    }
}
