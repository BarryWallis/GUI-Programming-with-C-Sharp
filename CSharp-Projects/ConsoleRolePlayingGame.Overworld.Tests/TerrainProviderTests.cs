using Shouldly;

namespace ConsoleRolePlayingGame.Overworld.Tests;

public class TerrainProviderTests
{
    [Theory]
    [InlineData(8, 0, TerrainType.Mountain)]
    [InlineData(0, -2, TerrainType.Desert)]
    [InlineData(11, -1, TerrainType.DeepWater)]
    [InlineData(10, -1, TerrainType.Water)]
    public void ProducesExpectedResult(int x, int y, TerrainType expected)
    {
        //Arrange
        MapGenerator mapGenerator = new();
        Position position = new(x, y);

        // Act
        TerrainType actual = mapGenerator.CalculateTerrain(position);

        // Assert
        actual.ShouldBe(expected);
    }
}
