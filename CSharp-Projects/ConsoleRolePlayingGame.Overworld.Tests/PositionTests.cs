using Shouldly;

namespace ConsoleRolePlayingGame.Overworld.Tests;

/// <summary>
/// Unit tests for <see cref="Position"/> movement behavior.
/// </summary>
public class PositionTests
{
    /// <summary>
    /// Verifies that moving in each cardinal direction updates the expected coordinate.
    /// </summary>
    [Theory]
    [InlineData(Direction.North, 5, 4)]
    [InlineData(Direction.South, 5, 6)]
    [InlineData(Direction.East, 6, 5)]
    [InlineData(Direction.West, 4, 5)]
    public void Move_ShiftsCoordinates(Direction direction, int expectedX, int expectedY)
    {
        Position position = new(5, 5);

        Position moved = position.Move(direction);

        moved.ShouldBe(new Position(expectedX, expectedY));
    }

    /// <summary>
    /// Verifies that an unknown direction leaves the position unchanged.
    /// </summary>
    [Fact]
    public void Move_WithUnknownDirection_ReturnsTheSamePosition()
    {
        Position position = new(2, 3);
        Direction unknownDirection = (Direction)999;

        Position moved = position.Move(unknownDirection);

        moved.ShouldBe(position);
    }
}
