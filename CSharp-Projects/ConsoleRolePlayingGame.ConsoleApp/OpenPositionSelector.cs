using ConsoleRolePlayingGame.Overworld;

/// <summary>
/// Selects an unoccupied map position within a distance range of a source coordinate.
/// </summary>
/// <param name="map">The world map used to determine which positions are already occupied.</param>
internal class OpenPositionSelector(WorldMap map)
{
    /// <summary>
    /// Returns a random <see cref="Position"/> that is between <paramref name="min"/> and
    /// <paramref name="max"/> steps from <paramref name="source"/> and is not already
    /// occupied by any entity on the map.
    /// </summary>
    /// <param name="source">The origin position from which to search.</param>
    /// <param name="min">The minimum Manhattan distance from <paramref name="source"/>.</param>
    /// <param name="max">The maximum Manhattan distance from <paramref name="source"/>.</param>
    /// <returns>An unoccupied <see cref="Position"/> within the specified distance range.</returns>
    public Position GetOpenPositionNear(Position source, int min, int max)
    {
        HashSet<Position> occupied = [.. map.Entities.Select(e => e.MapPosition)];
        Random random = Random.Shared;
        Position position;
        do
        {
            int offset = random.Next(min, max + 1);
            int xOffset = (int)Math.Round(random.NextDouble() * offset);
            int yOffset = offset - xOffset;

            if (random.NextDouble() < 0.5)
            {
                xOffset *= -1;
            }

            if (random.NextDouble() < 0.5)
            {
                yOffset *= -1;
            }

            position = new Position(source.X + xOffset, source.Y + yOffset);
        } while (occupied.Contains(position));

        return position;
    }
}