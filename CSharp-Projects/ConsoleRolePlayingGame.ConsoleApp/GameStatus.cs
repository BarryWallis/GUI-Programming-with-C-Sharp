namespace ConsoleRolePlayingGame.ConsoleApp;

/// <summary>
/// Represents the high-level state of the game loop.
/// </summary>
public enum GameStatus
{
    /// <summary>The player has chosen to exit; the game loop should stop.</summary>
    Terminated,
    /// <summary>The player is navigating the overworld map.</summary>
    Overworld,
    /// <summary>The player's party health reached zero; the game is over.</summary>
    GameOver,
}