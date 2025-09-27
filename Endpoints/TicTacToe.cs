public record JoinGameRequest(string GameId);
public record MoveRequest(string GameId, string PlayerId, int X, int Y);

public static class TicTacToeEndpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        var games = app.Services.GetRequiredService<TicTacToeService>();

        // Create a new game
        app.MapPost("/tictactoe/create", () =>
        {
            var game = games.CreateGame(out var playerId);
            return Results.Ok(new { game.GameId, playerId, symbol = "X" });
        });

        // Join an existing game
        app.MapPost("/tictactoe/join", (JoinGameRequest request) =>
        {
            var (game, playerId) = games.JoinGame(request.GameId);
            return game == null || playerId == null
                ? Results.BadRequest("Game not found or full")
                : Results.Ok(new { game.GameId, playerId, symbol = game.Players[playerId] });
        });

        // Get game state
        app.MapPost("/tictactoe/state", (JoinGameRequest request) =>
        {
            var game = games.GetGame(request.GameId);
            return game is null ? Results.NotFound() : Results.Ok(game);
        });

        // Make a move
        app.MapPost("/tictactoe/move", (MoveRequest request) =>
        {
            var success = games.MakeMove(request.GameId, request.PlayerId, request.X, request.Y);
            return success ? Results.Ok(games.GetGame(request.GameId)) : Results.BadRequest("Invalid move or not your turn");
        });
    }
}
