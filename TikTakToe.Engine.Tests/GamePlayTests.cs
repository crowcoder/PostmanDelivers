using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TikTakToe.Engine.Tests
{
    [TestClass]
    public class GamePlayTests
    {
        [TestMethod]
        public void XMustGoFirst_Test()
        {
            GameBoard gameBoard = new GameBoard();
            string startBoard = gameBoard.RenderBoard();

            var whosTurn = gameBoard.WhosTurnIsIt();

            Assert.AreEqual("X", whosTurn.whosTurn, $"First turn should go to X but it was {whosTurn.whosTurn}");

            MoveResult result = gameBoard.ExecuteTurn("O", 4);

            Assert.IsFalse(result.IsValid, "Move should have been invalid but was computed as valid.");
            Assert.AreEqual(startBoard, gameBoard.RenderBoard());
        }

        [TestMethod]
        public void XGoesAndNowItIsOsTurn_Test()
        {
            GameBoard gameBoard = new GameBoard();

            var whosTurn = gameBoard.WhosTurnIsIt();

            Assert.AreEqual("X", whosTurn.whosTurn, $"First turn should go to X but it was {whosTurn.whosTurn}");

            MoveResult result = gameBoard.ExecuteTurn("X", 4);

            Assert.IsTrue(result.IsValid, "Move should have been valid but was computed as invalid.");
            whosTurn = gameBoard.WhosTurnIsIt();
            Assert.AreEqual("O", whosTurn.whosTurn, $"Next move should go to O but it was computed as {whosTurn.whosTurn}");
        }

        [TestMethod]
        public void XWins_Test()
        {
            GameBoard gameBoard = new GameBoard();
            MoveResult move = gameBoard.ExecuteTurn("X", 8);
            var next = gameBoard.WhosTurnIsIt();
            Assert.AreEqual("O", next.whosTurn, "Should be O's turn but was not computed that way.");

            move = gameBoard.ExecuteTurn("O", 6);
            next = gameBoard.WhosTurnIsIt();
            Assert.AreEqual("X", next.whosTurn, "Should be X's turn but was not computed that way.");

            move = gameBoard.ExecuteTurn("X", 4);
            next = gameBoard.WhosTurnIsIt();
            Assert.AreEqual("O", next.whosTurn, "Should be O's turn but was not computed that way.");

            move = gameBoard.ExecuteTurn("O", 7);
            next = gameBoard.WhosTurnIsIt();
            Assert.AreEqual("X", next.whosTurn, "Should be X's turn but was not computed that way.");

            move = gameBoard.ExecuteTurn("X", 0);
            Console.WriteLine(move.GameBoard);
            next = gameBoard.WhosTurnIsIt();
            var gameover = gameBoard.IsGameOver();
            Assert.IsTrue(gameover.gameIsOver, "Game should be over but was not computed as such.");
            Assert.AreEqual("X", gameover.winner);
        }

        [TestMethod]
        public void OWinsTest()
        {
            GameBoard gameBoard = new GameBoard();

            MoveResult move = gameBoard.ExecuteTurn("X", 3);
            move = gameBoard.ExecuteTurn("O", 0);
            move = gameBoard.ExecuteTurn("X", 1);
            move = gameBoard.ExecuteTurn("O", 4);
            move = gameBoard.ExecuteTurn("X", 5);
            move = gameBoard.ExecuteTurn("O", 8);

            var gameover = gameBoard.IsGameOver();
            Assert.IsTrue(gameover.gameIsOver, "Game should be over but was not computed as such.");
            Assert.AreEqual("O", gameover.winner);
        }

        [TestMethod]
        public void GameIsDraw_Test()
        {
            GameBoard gameBoard = new GameBoard();

            MoveResult move = gameBoard.ExecuteTurn("X", 1);
            move = gameBoard.ExecuteTurn("O", 0);
            move = gameBoard.ExecuteTurn("X", 4);
            move = gameBoard.ExecuteTurn("O", 2);
            move = gameBoard.ExecuteTurn("X", 5);
            move = gameBoard.ExecuteTurn("O", 3);
            move = gameBoard.ExecuteTurn("X", 6);
            move = gameBoard.ExecuteTurn("O", 7);
            move = gameBoard.ExecuteTurn("X", 8);

            Assert.IsTrue(move.GameIsOver, "Game should be over but was not computed as such.");
            Assert.AreEqual("DRAW", gameBoard.Winner);
        }

        [TestMethod]
        public void XGoesWhenItIsOsTurn_Test()
        {
            GameBoard gameBoard = new GameBoard();

            MoveResult move = gameBoard.ExecuteTurn("X", 1);
            move = gameBoard.ExecuteTurn("O", 0);
            move = gameBoard.ExecuteTurn("X", 4);
            string gameBoardStateAtLastValidTurn = gameBoard.RenderBoard();
            move = gameBoard.ExecuteTurn("X", 2);

            Assert.IsFalse(move.IsValid, "X going twice should be invalid but it was not computed as such.");
            Assert.IsFalse(move.GameIsOver, "Game should not be over but it was computed as over.");
            Assert.AreEqual("It is O's turn to move. Play fair.", move.Reason);
            Assert.AreEqual(gameBoardStateAtLastValidTurn, gameBoard.RenderBoard(), "Gameboard state should not have changed on an invalid move but it did.");
        }

        [TestMethod]
        public void OGoesWhenItIsXsTurn_Test()
        {
            GameBoard gameBoard = new GameBoard();

            MoveResult move = gameBoard.ExecuteTurn("X", 1);
            move = gameBoard.ExecuteTurn("O", 0);
            move = gameBoard.ExecuteTurn("X", 4);
            move = gameBoard.ExecuteTurn("O", 2);
            string gameBoardStateAtLastValidTurn = gameBoard.RenderBoard();
            move = gameBoard.ExecuteTurn("O", 3);

            Assert.IsFalse(move.IsValid, "O going twice should be invalid but it was not computed as such.");
            Assert.IsFalse(move.GameIsOver, "Game should not be over but it was computed as over.");
            Assert.AreEqual("It is X's turn to move. Play fair.", move.Reason);
            Assert.AreEqual(gameBoardStateAtLastValidTurn, gameBoard.RenderBoard(), "Gameboard state should not have changed on an invalid move but it did.");
        }

        [TestMethod]
        public void GameReportsOver_Test()
        {
            GameBoard gameBoard = new GameBoard();
            MoveResult move = gameBoard.ExecuteTurn("X", 8);
            
            move = gameBoard.ExecuteTurn("O", 6);            
            move = gameBoard.ExecuteTurn("X", 4);            
            move = gameBoard.ExecuteTurn("O", 7);            
            move = gameBoard.ExecuteTurn("X", 0);

            move = gameBoard.ExecuteTurn("O", 1);
            Assert.IsTrue(move.GameIsOver, "Game should be over but was not computed as such.");
            Assert.IsFalse(move.IsValid, "Move was not valid");
            Assert.AreEqual("Game is Over. Congratulations to X", move.Reason);
        }

        [TestMethod]
        public void XMovesInSpotOccupiedByO_Test()
        {
            GameBoard gameBoard = new GameBoard();
            MoveResult move = gameBoard.ExecuteTurn("X", 8);

            move = gameBoard.ExecuteTurn("O", 6);
            move = gameBoard.ExecuteTurn("X", 4);
            move = gameBoard.ExecuteTurn("O", 7);
            move = gameBoard.ExecuteTurn("X", 7);

            Assert.IsFalse(move.GameIsOver, "Game should not be over but was computed as such.");
            Assert.IsFalse(move.IsValid, "Move should be invalid");
            Assert.AreEqual("The square is already occupied by O. Turn aborted, try again.", move.Reason);
        }

        [TestMethod]
        public void XMovesInSpotOccupiedByX_Test()
        {
            GameBoard gameBoard = new GameBoard();
            MoveResult move = gameBoard.ExecuteTurn("X", 8);

            move = gameBoard.ExecuteTurn("O", 6);
            move = gameBoard.ExecuteTurn("X", 4);
            move = gameBoard.ExecuteTurn("O", 7);
            move = gameBoard.ExecuteTurn("X", 4);

            Assert.IsFalse(move.GameIsOver, "Game should not be over but was computed as such.");
            Assert.IsFalse(move.IsValid, "Move should be invalid");
            Assert.AreEqual("The square is already occupied by X. Turn aborted, try again.", move.Reason);
        }

        [TestMethod]
        public void OMovesInSpotOccupiedByO_Test()
        {
            GameBoard gameBoard = new GameBoard();
            MoveResult move = gameBoard.ExecuteTurn("X", 8);

            move = gameBoard.ExecuteTurn("O", 6);
            move = gameBoard.ExecuteTurn("X", 4);
            move = gameBoard.ExecuteTurn("O", 6);

            Assert.IsFalse(move.GameIsOver, "Game should not be over but was computed as such.");
            Assert.IsFalse(move.IsValid, "Move should be invalid");
            Assert.AreEqual("The square is already occupied by O. Turn aborted, try again.", move.Reason);
        }

        [TestMethod]
        public void OMovesInSpotOccupiedByX_Test()
        {
            GameBoard gameBoard = new GameBoard();
            MoveResult move = gameBoard.ExecuteTurn("X", 8);

            move = gameBoard.ExecuteTurn("O", 6);
            move = gameBoard.ExecuteTurn("X", 4);
            move = gameBoard.ExecuteTurn("O", 4);

            Assert.IsFalse(move.GameIsOver, "Game should not be over but was computed as such.");
            Assert.IsFalse(move.IsValid, "Move should be invalid");
            Assert.AreEqual("The square is already occupied by X. Turn aborted, try again.", move.Reason);
        }
    }
}
