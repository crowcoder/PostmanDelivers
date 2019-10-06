using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe.Engine.Tests
{
    [TestClass]
    public class DataIntegrationTests
    {
        string connection = "Data Source=C:\\ProgramData\\PostmanDeliversData\\GameDB.sqlite3;Version=3;";

        [TestMethod]
        public void GenerateGame_Test()
        {
            Game g = new Game();
            g.GenerateNewGame("Bob", "Janel", connection);
            Assert.IsTrue(g.Id > 0);
        }

        [TestMethod]
        public void GenerateGameAndExecuteMoves_Test()
        {
            Game g = new Game();
            g.GenerateNewGame("Bob", "Janel", connection);
            Assert.IsTrue(g.Id > 0);

            g.Move("X", 4, g.PlayerXID, g.Id, connection);
            g.Move("O", 1, g.PlayerOID, g.Id, connection);
            g.Move("X", 8, g.PlayerXID, g.Id, connection);
            g.Move("O", 6, g.PlayerOID, g.Id, connection);
        }

        [TestMethod]
        public void GetGameById_Test()
        {
            Game g = new Game();
            var theGame = g.GetGameByID(14, connection);
        }
    }
}
