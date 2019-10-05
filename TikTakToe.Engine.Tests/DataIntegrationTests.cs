using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe.Engine.Tests
{
    [TestClass]
    public class DataIntegrationTests
    {
        [TestMethod]
        public void GenerateGame_Test()
        {
            Game g = new Game();
            g.GenerateNewGame("Bob", "Janel");
            Assert.IsTrue(g.Id > 0);
        }
    }
}
