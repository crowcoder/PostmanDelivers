using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TikTakToe.Engine
{
    public class Game
    {
        public Game GenerateNewGame(string playerXName, string playerOName, string connection)
        {
            PlayerXName = playerXName;
            PlayerOName = playerOName;
            PlayerXID = GenerateID();
            PlayerOID = GenerateID();

            Board = new GameBoard();
            Moves = new List<PlayerMove>();

            this.Id = NewGame(connection);
            return this;
        }

        public int Id { get; set; }
        public GameBoard Board { get; set; }
        public List<PlayerMove> Moves { get; set; }

        public string PlayerXName { get; set; }
        public string PlayerXID { get; set; }
        public string PlayerOName { get; set; }
        public string PlayerOID { get; set; }

        public Winners Winner
        {
            get
            {
                var state = Board.IsGameOver();

                switch (state.winner)
                {
                    case "X":
                        return Winners.X;
                    case "O":
                        return Winners.O;
                    case "DRAW":
                        return Winners.Draw;
                    default:
                        return Winners.None;
                }
            }
        }

        public GameStates GameState
        {
            get
            {
                var state = Board.IsGameOver();
                switch (state.gameIsOver)
                {
                    case true:
                        return GameStates.Complete;
                    default:
                        return GameStates.InProgress;
                }
            }
        }

        public Game GetGameByID(int gameID, string connection)
        {
            using (var conn = new SQLiteConnection(connection))
            {
                conn.Open();
                Game gameId = conn.Query<Game>(
                    @"SELECT Id, PlayerXName, PlayerOName, PlayerXID, PlayerOID, GameState, Winner
                        FROM Games WHERE Id = @Id", new { @Id = gameID }
                    ).SingleOrDefault();

                return gameId;
            }
        }

        public MoveResult Move(string xoro, int whichSquare, string playerID, int gameID, string connection)
        {
            Players whichPlayer = ConvertXorOToPlayer(xoro);
            Game theGame = GetGameByID(gameID, connection);

            if (theGame == null)
            {
                throw new ArgumentException("Bad game id");
            }

            switch (whichPlayer)
            {
                case Players.None:
                    throw new ArgumentException("Bad player");                    
                case Players.X:
                    if (theGame.PlayerXID != playerID)
                    {
                        throw new ArgumentException("Bad player id");
                    }
                    break;
                case Players.O:
                    if (theGame.PlayerOID != playerID)
                    {
                        throw new ArgumentException("Bad player id");
                    }
                    break;
                default:
                    break;
            }

            string currentBoard = Board.RenderBoard();
            MoveResult mr = Board.ExecuteTurn(xoro, whichSquare);

            if (mr.IsValid)
            {
                PlayerMove playerMove = new PlayerMove();
                playerMove.GameboardBefore = currentBoard;
                playerMove.GameID = this.Id;
                playerMove.GameboardAfter = Board.RenderBoard();
                playerMove.Square = whichSquare;
                playerMove.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                playerMove.Player = whichPlayer;

                InsertMove(playerMove, connection);

                return mr;
            }
            else
            {
                throw new InvalidOperationException($"Illegal Move. Reason: {mr.Reason}, Game Board:{mr.GameBoard}");
            }
        }

        private Players ConvertXorOToPlayer(string xoro)
        {
            switch (xoro)
            {
                case "X":
                    return Players.X;
                case "O":
                    return Players.O;
                default:
                    return Players.None;
            }
        }

        private int InsertMove(PlayerMove playerMove, string connection)
        {
            using (var conn = new SQLiteConnection(connection))
            {
                conn.Open();
                int moveId = conn.Query<int>(
                    @"INSERT INTO PlayerMoves
                        (GameID, Square, GameBoardAfter, GameBoardBefore, Timestamp, Player) 
                        VALUES (@GameID, @Square, @GameBoardAfter, @GameBoardBefore, @Timestamp, @Player);
                        select last_insert_rowid();", playerMove
                    ).First();

                return moveId;
            }
        }

        private int NewGame(string connection)
        {
            using (var conn = new SQLiteConnection(connection))
            {
                conn.Open();
                int gameId = conn.Query<int>(
                    @"INSERT INTO Games 
                        (PlayerXName, PlayerOName, PlayerXID, PlayerOID, GameState, Winner) 
                        VALUES (@PlayerXName, @PlayerOName, @PlayerXID, @PlayerOID, @GameState, @Winner);
                        select last_insert_rowid();", this
                    ).First();

                return gameId;
            }
        }

        private string GenerateID()
        {
            var csp = RNGCryptoServiceProvider.Create();
            byte[] newid = new byte[8];
            csp.GetBytes(newid);
            Base62.Base62Converter base62Converter = new Base62.Base62Converter();
            string newStringID = base62Converter.Encode(Encoding.UTF8.GetString(newid));
            return newStringID;
        }
    }
}