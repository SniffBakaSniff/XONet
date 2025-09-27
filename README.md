# XONet
A  Game API for simple games like Tic Tac Toe.

## API Endpoints

### | Create a New Game
Generates a new Tic Tac Toe game.

```bash
curl -X POST http://localhost:5025/tictactoe/create
````

Returns:

```json
{
  "gameId": "fd1c37db-5374-46aa-8571-09e97bd12e1c",
  "playerId": "3c33af30-5643-4ead-bdb3-0174633a1714",
  "symbol": "X"
}
```

### | Get Game Data

Retrieves the current state of a game.  
Replace `<GameId>` with the actual game ID.

```bash
curl -X POST http://localhost:5025/tictactoe/state \
     -H "Content-Type: application/json" \
     -d '{ "gameId": "<GameId>" }'
```

Returns:

```json
{
  "gameId": "fd1c37db-5374-46aa-8571-09e97bd12e1c",
  "board": [
    [null, null, null],
    [null, null, null],
    [null, null, null]
  ],
  "currentPlayer": "X",
  "isFinished": false,
  "winner": null,
  "lastActivity": "2025-09-27T03:08:21.199823Z",
  "players": {
    "3c33af30-5643-4ead-bdb3-0174633a1714": "X"
  }
}
```

### | Join a Game

Joins an existing game as a player.  
Replace `<GameId>` with the actual game ID.

```bash
curl -X POST http://localhost:5025/tictactoe/join \
     -H "Content-Type: application/json" \
     -d '{ "gameId": "<GameId>" }'
```

Returns:

```json
{
  "gameId": "fd1c37db-5374-46aa-8571-09e97bd12e1c",
  "playerId": "24b4a0e9-01a0-4704-8bea-e686cc9d75ec",
  "symbol": "O"
}
```

### | Make a Move

Submits a move for a player in a game.  
Replace `<GameId>` and `<PlayerId>` with actual values.

```bash
curl -X POST http://localhost:5025/tictactoe/<GameId>/move \
     -H "Content-Type: application/json" \
     -d '{ "playerId": "<PlayerId>", "x": 1, "y": 1 }'
```

Returns:

```json
{
  "playerId": "3c33af30-5643-4ead-bdb3-0174633a1714",
  "x": 0,
  "y": 0,
  "gameId": "fd1c37db-5374-46aa-8571-09e97bd12e1c",
  "board": [
    ["X", null, null],
    [null, null, null],
    [null, null, null]
  ],
  "currentPlayer": "O",
  "isFinished": false,
  "winner": null,
  "lastActivity": "2025-09-27T03:14:44.9022958Z",
  "players": {
    "3c33af30-5643-4ead-bdb3-0174633a1714": "X",
    "24b4a0e9-01a0-4704-8bea-e686cc9d75ec": "O"
  }
}
```
