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

        public long Id { get; set; }
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

        public List<Game> GetAllGames(string connection)
        {
            using (var conn = new SQLiteConnection(connection))
            {
                conn.Open();
                var gamesResult = conn.Query(
                    @"SELECT g.Id, g.PlayerXName, g.PlayerOName, g.PlayerXID, g.PlayerOID, g.GameState, g.Winner,
                            g.SquareOne, g.SquareTwo, g.SquareThree, g.SquareFour, g.SquareFive,
                            g.SquareSix, g.SquareSeven, g.SquareEight, g.SquareNine,
	                        m.Id, m.GameID, m.Square, m.GameBoardAfter, m.GameBoardBefore, m.Timestamp, m.Player
                        FROM Games g
                        LEFT OUTER JOIN PlayerMoves m
	                        ON g.Id = m.GameID
                        ORDER BY g.ID, m.Id");

                if (gamesResult != null)
                {
                    var theGames = gamesResult.GroupBy(g => new
                    {
                        g.Id,
                        g.PlayerXName,
                        g.PlayerOName,
                        g.PlayerXID,
                        g.PlayerOID,
                        g.GameState,
                        g.Winner,
                        g.SquareOne,
                        g.SquareTwo,
                        g.SquareThree,
                        g.SquareFour,
                        g.SquareFive,
                        g.SquareSix,
                        g.SquareSeven,
                        g.SquareEight,
                        g.SquareNine
                    }).Select(m => new Game
                    {
                        Id = m.Key.Id,
                        PlayerXName = m.Key.PlayerXName,
                        PlayerOName = m.Key.PlayerOName,
                        PlayerXID = m.Key.PlayerXID,
                        PlayerOID = m.Key.PlayerOID,
                        Board = new GameBoard(
                            m.Key.SquareOne,
                            m.Key.SquareTwo,
                            m.Key.SquareThree,
                            m.Key.SquareFour,
                            m.Key.SquareFive,
                            m.Key.SquareSix,
                            m.Key.SquareSeven,
                            m.Key.SquareEight,
                            m.Key.SquareNine),
                        Moves = m.Select(mv => new PlayerMove
                        {
                            GameID = mv.GameID,
                            Square = mv.Square,
                            GameboardAfter = mv.GameBoardAfter,
                            GameboardBefore = mv.GameBoardBefore,
                            Timestamp = mv.Timestamp,
                            Player = mv.Player == null ? Players.None : (Players)mv.Player
                        }).ToList()
                    }).ToList();

                    return theGames;
                }

                return null;
            }
        }

        public Game GetGameByID(long gameID, string connection)
        {
            using (var conn = new SQLiteConnection(connection))
            {
                conn.Open();
                var theGame = conn.Query(
                    @"SELECT g.Id, g.PlayerXName, g.PlayerOName, g.PlayerXID, g.PlayerOID, g.GameState, g.Winner,
                            g.SquareOne, g.SquareTwo, g.SquareThree, g.SquareFour, g.SquareFive,
                            g.SquareSix, g.SquareSeven, g.SquareEight, g.SquareNine,
	                        m.Id, m.GameID, m.Square, m.GameBoardAfter, m.GameBoardBefore, m.Timestamp, m.Player
                        FROM Games g
                        LEFT OUTER JOIN PlayerMoves m
	                        ON g.Id = m.GameID
                        WHERE g.Id = @Id", new { @Id = gameID }
                    );

                if (theGame != null)
                {
                    var fullGame = theGame.GroupBy(g => new
                    {
                        g.Id,
                        g.PlayerXName,
                        g.PlayerOName,
                        g.PlayerXID,
                        g.PlayerOID,
                        g.GameState,
                        g.Winner,
                        g.SquareOne,
                        g.SquareTwo,
                        g.SquareThree,
                        g.SquareFour,
                        g.SquareFive,
                        g.SquareSix,
                        g.SquareSeven,
                        g.SquareEight,
                        g.SquareNine
                    }).Select(m => new Game
                    {
                        Id = m.Key.Id,
                        PlayerXName = m.Key.PlayerXName,
                        PlayerOName = m.Key.PlayerOName,
                        PlayerXID = m.Key.PlayerXID,
                        PlayerOID = m.Key.PlayerOID,
                        Board = new GameBoard(
                            m.Key.SquareOne, 
                            m.Key.SquareTwo, 
                            m.Key.SquareThree, 
                            m.Key.SquareFour, 
                            m.Key.SquareFive, 
                            m.Key.SquareSix, 
                            m.Key.SquareSeven, 
                            m.Key.SquareEight, 
                            m.Key.SquareNine),
                        Moves = m.Select(mv => new PlayerMove
                        {
                            GameID = mv.GameID,
                            Square = mv.Square,
                            GameboardAfter = mv.GameBoardAfter,
                            GameboardBefore = mv.GameBoardBefore,
                            Timestamp = mv.Timestamp,
                            Player = mv.Player == null ? Players.None : (Players)mv.Player
                        }).ToList()
                    }).SingleOrDefault();

                    return fullGame;
                }

                return null;
            }
        }

        public MoveResult Move(string xoro, int whichSquare, string playerID, long gameID, string connection)
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

            string currentBoard = theGame.Board.RenderBoard();
            MoveResult mr = theGame.Board.ExecuteTurn(xoro, whichSquare);

            if (mr.IsValid)
            {
                PlayerMove playerMove = new PlayerMove();
                playerMove.GameboardBefore = currentBoard;
                playerMove.GameID = theGame.Id;
                playerMove.GameboardAfter = theGame.Board.RenderBoard();
                playerMove.Square = whichSquare;
                playerMove.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                playerMove.Player = whichPlayer;

                InsertMove(playerMove, connection);
                UpdateGameBoard(theGame.Id, theGame.Board.Squares, connection);
                return mr;
            }
            else
            {
                throw new InvalidOperationException($@"Illegal Move. Reason: {mr.Reason}, Game Board:
{mr.GameBoard}");
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

        private void UpdateGameBoard(long gameId, string[] squares, string connection)
        {
            using (var conn = new SQLiteConnection(connection))
            {
                conn.Open();
                conn.Execute(
                    @"UPDATE Games 
                    SET SquareOne = @SquareOne, 
                        SquareTwo = @SquareTwo, 
                        SquareThree = @SquareThree, 
                        SquareFour = @SquareFour, 
                        SquareFive = @SquareFive, 
                        SquareSix = @SquareSix, 
                        SquareSeven = @SquareSeven, 
                        SquareEight = @SquareEight, 
                        SquareNine = @SquareNine
                    WHERE Id = @Id", new
                    {
                        @Id = gameId,
                        @SquareOne = squares[0],
                        @SquareTwo = squares[1],
                        @SquareThree = squares[2],
                        @SquareFour = squares[3],
                        @SquareFive = squares[4],
                        @SquareSix = squares[5],
                        @SquareSeven = squares[6],
                        @SquareEight = squares[7],
                        @SquareNine = squares[8]
                    }
                    );
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