using ConsoleRolePlayingGame.Overworld;

internal class OpenPositionSelector(WorldMap map)
{
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