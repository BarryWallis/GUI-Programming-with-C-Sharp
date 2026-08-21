namespace ConsoleRolePlayingGame.Overworld;

public interface IMapEntity
{
    EntityType EntityType { get; }
    Position MapPosition { get; set; }
}