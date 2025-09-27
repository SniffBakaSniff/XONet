public class TicTacToeService
{
    private readonly Dictionary<string, TicTacToeGame> _games = new();
    private readonly System.Timers.Timer _cleanupTimer;

    public TicTacToeService()
    {
        // Timer runs every 60 seconds and cleans up games that are finished
        // or inactive for more than 10 minutes.
        _cleanupTimer = new System.Timers.Timer(60000);
        _cleanupTimer.Elapsed += (s, e) => CleanupExpiredGames(TimeSpan.FromMinutes(10));
        _cleanupTimer.AutoReset = true;
        _cleanupTimer.Start();
    }
    
    // Create a new TicTacToe game and store it in memory.
        public TicTacToeGame CreateGame(out string playerId)
    {
        var game = new TicTacToeGame();
        playerId = game.AddPlayer(); // first player = X
        _games[game.GameId] = game;
        return game;
    }

    public (TicTacToeGame? Game, string? PlayerId) JoinGame(string gameId)
    {
        if (!_games.TryGetValue(gameId, out var game)) return (null, null);

        try
        {
            var playerId = game.AddPlayer();
            return (game, playerId);
        }
        catch
        {
            return (null, null); // game full
        }
    }
    
    // Delete a game from memory by its ID.
    public bool DeleteGame(string gameId)
    {
        return _games.Remove(gameId);
    }

    // Get an active game by ID, or null if it doesn't exist.
    public TicTacToeGame? GetGame(string gameId)
    {
        _games.TryGetValue(gameId, out var game);
        return game;
    }

    // Apply a player's move to the specified game.
    // Returns false if the game doesn't exist or the move is invalid.
    public bool MakeMove(string gameId, string playerId, int x, int y)
    {
        var game = GetGame(gameId);
        if (game is null) return false;
        return game.MakeMove(playerId, x, y);
    }

    // Remove any games that are finished or inactive beyond the allowed idle time.
    // Called automatically by the cleanup timer, but can also be invoked manually.
    private void CleanupExpiredGames(TimeSpan maxIdleTime)
    {
        var expired = _games
            .Where(entry => entry.Value.IsFinished ||
                            DateTime.UtcNow - entry.Value.LastActivity > maxIdleTime)
            .Select(entry => entry.Key)
            .ToList();

        foreach (var gameId in expired)
        {
            _games.Remove(gameId);
        }
    }
}
