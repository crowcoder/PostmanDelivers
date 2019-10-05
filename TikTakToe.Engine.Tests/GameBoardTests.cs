using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TikTakToe.Engine.Tests
{
    [TestClass]
    public class GameBoardTests
    {
        [TestMethod]
        public void BasicBoardRenderWithAllNulls_Test()
        {
            Game g = new Game();
            g.Board = new GameBoard();
            //g.Board.Squares = new string[] { null, null, null, null, null, null, null, null };
            string gameboard = g.Board.RenderBoard();
            System.Console.WriteLine(gameboard);

            string expected = @"[     ]     [     ]     [     ]

[     ]     [     ]     [     ]

[     ]     [     ]     [     ]";

            Assert.AreEqual(expected, gameboard, "Gameboard layout did not render as expected");
        }

        [TestMethod]
        public void RenderBoardWithSomeSquaresOccupied_Test()
        {
            Game g = new Game();
            g.Board = new GameBoard();

            g.Board.Squares[0] = null;
            g.Board.Squares[1] = null;
            g.Board.Squares[2] = null;
            g.Board.Squares[3] = null;
            g.Board.Squares[4] = "x";
            g.Board.Squares[5] = null;
            g.Board.Squares[6] = null;
            g.Board.Squares[7] = "O";
            g.Board.Squares[8] = "Z";

            string gameboard = g.Board.RenderBoard();
            System.Console.WriteLine(gameboard);

            string expected = @"[     ]     [     ]     [     ]

[     ]     [  X  ]     [     ]

[     ]     [  O  ]     [     ]";

            Assert.AreEqual(expected, gameboard, "Gameboard layout did not render as expected");
        }

        [TestMethod]
        public void RenderBoardWithAllSquaresOccupied_Test()
        {
            Game g = new Game();
            g.Board = new GameBoard();

            g.Board.Squares[0] = "X";
            g.Board.Squares[1] = "O";
            g.Board.Squares[2] = "X";
            g.Board.Squares[3] = "O";
            g.Board.Squares[4] = "x";
            g.Board.Squares[5] = "o";
            g.Board.Squares[6] = "X";
            g.Board.Squares[7] = "O";
            g.Board.Squares[8] = "X";

            string gameboard = g.Board.RenderBoard();
            System.Console.WriteLine(gameboard);

            string expected = @"[  X  ]     [  O  ]     [  X  ]

[  O  ]     [  X  ]     [  O  ]

[  X  ]     [  O  ]     [  X  ]";

            Assert.AreEqual(expected, gameboard, "Gameboard layout did not render as expected");
        }
    }
}
