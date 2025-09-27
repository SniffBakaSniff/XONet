public static class TicTacToeEndpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        var games = app.Services.GetRequiredService<TicTacToeService>();

        // create game then returns gameId + playerId
        app.MapPost("/tictactoe/create", () =>
        {
            var game = games.CreateGame(out var playerId);
            return Results.Ok(new { game.GameId, playerId, symbol = "X" });
        });

        // join game then returns playerId + assigned symbol
        app.MapPost("/tictactoe/{gameId}/join", (string gameId) =>
        {
            var (game, playerId) = games.JoinGame(gameId);
            return game == null || playerId == null
                ? Results.BadRequest("Game not found or full")
                : Results.Ok(new { game.GameId, playerId, symbol = game.Players[playerId] });
        });

        // get game state
        app.MapGet("/tictactoe/{gameId}", (string gameId) =>
        {
            var game = games.GetGame(gameId);
            return game is null ? Results.NotFound() : Results.Ok(game);
        });

        // make move
        app.MapPost("/tictactoe/{gameId}/move", (string gameId, PlayerMove move) =>
        {
            var success = games.MakeMove(gameId, move.PlayerId, move.X, move.Y);
            return success ? Results.Ok(games.GetGame(gameId)) : Results.BadRequest("Invalid move or not your turn");
        });
    }
}
