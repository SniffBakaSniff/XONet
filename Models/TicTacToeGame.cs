public class TicTacToeGame
{
    // Unique ID for this game
    public string GameId { get; set; } = Guid.NewGuid().ToString();

    // 3x3 game board
    public string[][] Board { get; set; } =
    {
        new string[3],
        new string[3],
        new string[3]
    };

    // Other important variables.
    public string CurrentPlayer { get; set; } = "X";
    public bool IsFinished { get; set; } = false;
    public string? Winner { get; set; }
    public DateTime LastActivity { get; set; } = DateTime.UtcNow; 
    public Dictionary<string, string> Players { get; } = new();

    // Add a player to the game
    public string AddPlayer()
    {
        if (Players.Count >= 2) throw new InvalidOperationException("Game already full");

        var playerId = Guid.NewGuid().ToString();
        var symbol = Players.Count == 0 ? "X" : "O";
        Players[playerId] = symbol;
        return playerId;
    }

    // Make a move on the board
    public bool MakeMove(string playerId, int x, int y)
    {
        if (IsFinished) return false;
        if (!Players.ContainsKey(playerId)) return false;

        var symbol = Players[playerId];
        if (symbol != CurrentPlayer) return false;
        if (Board[x][y] != null) return false;

        Board[x][y] = symbol;
        CurrentPlayer = symbol == "X" ? "O" : "X";
        Winner = CheckWinner();
        if (Winner != null) IsFinished = true;
        LastActivity = DateTime.UtcNow;
        return true;
    }

    private string? CheckWinner()
    {
        // checks rows
        for (int i = 0; i < 3; i++)
            if (Board[i][0] != null && Board[i][0] == Board[i][1] && Board[i][1] == Board[i][2])
                return Board[i][0];

        // checks columns
        for (int i = 0; i < 3; i++)
            if (Board[0][i] != null && Board[0][i] == Board[1][i] && Board[1][i] == Board[2][i])
                return Board[0][i];

        // Checks the top left and bottom left corners for a value, 
        // if a value is present then it checks the corisponding diagonal spaces.  
        if (Board[0][0] != null && Board[0][0] == Board[1][1] && Board[1][1] == Board[2][2]) return Board[0][0];
        if (Board[0][2] != null && Board[0][2] == Board[1][1] && Board[1][1] == Board[2][0]) return Board[0][2];

        // check draw
        if (Board.All(row => row.All(cell => cell != null))) return "Draw";

        return null;
    }
}
