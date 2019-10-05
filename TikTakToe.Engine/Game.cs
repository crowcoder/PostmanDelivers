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
        public Game GenerateNewGame(string playerXName, string playerOName)
        {
            PlayerXName = playerXName;
            PlayerOName = playerOName;
            PlayerXID = GenerateID();
            PlayerOID = GenerateID();

            Board = new GameBoard();
            Moves = new List<PlayerMove>();

            this.Id = NewGame();
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

        public Game StartNew()
        {
            throw new NotImplementedException();
        }

        public Game GetGameByID(int gameID)
        {
            throw new NotImplementedException();
        }

        public void Move(Players player, int whichSquare)
        {

        }

        private int NewGame()
        {
            using (var conn = new SQLiteConnection("Data Source=C:\\ProgramData\\PostmanDeliversData\\GameDB.sqlite3;Version=3;"))
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