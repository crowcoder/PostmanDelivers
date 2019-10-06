using System;
using System.Linq;

namespace TikTakToe.Engine
{
    public class GameBoard
    {
        private string xGlyph = "X";
        private string oGlyph = "O";

        public GameBoard(string s1, string s2, string s3, string s4, string s5, string s6, string s7, string s8, string s9)
        {
            Squares = new string[] { s1, s2, s3, s4, s5, s6, s7, s8, s9 };
        }

        public GameBoard()
        {
            Squares = new string[9];
        }

        private string spacer = " ";

        private string Board
        {
            get
            {
                return @"[  {0}  ]     [  {1}  ]     [  {2}  ]

[  {3}  ]     [  {4}  ]     [  {5}  ]

[  {6}  ]     [  {7}  ]     [  {8}  ]";
            }
        }

        public string[] Squares { get; set; }

        public string Winner { get; set; }

        public string RenderBoard()
        {
            string[] allowedValues = new string[] { xGlyph, oGlyph };
            var transformed = Squares.Select(s => allowedValues.Contains(s?.ToUpper()) ? s.ToUpper() : spacer);

            return string.Format(Board, transformed.ToArray());
        }

        public (bool gameIsOver, string winner) IsGameOver()
        {
            bool xHasRow1 = Squares.Take(3).All(s => s == xGlyph);
            bool xHasRow2 = Squares.Skip(3).Take(3).All(s => s == xGlyph);
            bool xHasRow3 = Squares.Skip(6).Take(3).All(s => s == xGlyph);
            bool xHasColumn1 = Squares[0] == xGlyph && Squares[3] == xGlyph && Squares[6] == xGlyph;
            bool xHasColumn2 = Squares[1] == xGlyph && Squares[4] == xGlyph && Squares[7] == xGlyph;
            bool xHasColumn3 = Squares[2] == xGlyph && Squares[5] == xGlyph && Squares[8] == xGlyph;
            bool xHasDiagonal = Squares[2] == xGlyph && Squares[4] == xGlyph && Squares[6] == xGlyph;
            bool xHasOtherDiagonal = Squares[0] == xGlyph && Squares[4] == xGlyph && Squares[8] == xGlyph;

            bool yHasRow1 = Squares.Take(3).All(s => s == oGlyph);
            bool yHasRow2 = Squares.Skip(3).Take(3).All(s => s == oGlyph);
            bool yHasRow3 = Squares.Skip(6).Take(3).All(s => s == oGlyph);
            bool yHasColumn1 = Squares[0] == oGlyph && Squares[3] == oGlyph && Squares[6] == oGlyph;
            bool yHasColumn2 = Squares[1] == oGlyph && Squares[4] == oGlyph && Squares[7] == oGlyph;
            bool yHasColumn3 = Squares[2] == oGlyph && Squares[5] == oGlyph && Squares[8] == oGlyph;
            bool yHasDiagonal = Squares[2] == oGlyph && Squares[4] == oGlyph && Squares[6] == oGlyph;
            bool yHasOtherDiagonal = Squares[0] == oGlyph && Squares[4] == oGlyph && Squares[8] == oGlyph;

            bool xwins = xHasRow1 || 
                xHasRow2 || 
                xHasRow3 || 
                xHasColumn1 || 
                xHasColumn2 || 
                xHasColumn3 || 
                xHasDiagonal || 
                xHasOtherDiagonal ;

            bool ywins = yHasRow1 || 
                yHasRow2 || 
                yHasRow3 || 
                yHasColumn1 || 
                yHasColumn2 || 
                yHasColumn3 || 
                yHasDiagonal || 
                yHasOtherDiagonal;

            if (xwins && ywins)
            {
                throw new InvalidOperationException(
                    "Both X and Y occupy winning positions. Review game log to identify how to fix this bug and then write some covering tests.");
            }

            if (xwins)
            {
                Winner = xGlyph;
                return (true, xGlyph);
            }

            if (ywins)
            {
                Winner = oGlyph;
                return (true, oGlyph);
            }

            //Check for a draw
            string[] xsandos = new string[] { xGlyph, oGlyph };
            if (Squares.All(s => xsandos.Contains(s)))
            {
                Winner = "DRAW";
                return (true, Winner);
            }

            return (false, null);
        }

        public (string whosTurn, string gameBoard) WhosTurnIsIt()
        {
            string board = RenderBoard();

            var gameStatus = IsGameOver();
            if (gameStatus.gameIsOver)
            {
                return ("GAME OVER", board);
            }

            int xCount = Squares.Count(s => s == xGlyph);
            int oCount = Squares.Count(s => s == oGlyph);

            if (xCount == 0 || xCount == oCount)
            {
                return (xGlyph, board);
            }
            else
            {
                return (oGlyph, board);
            }
        }

        /// <summary>
        /// Logic for determining if a move is valid or not.
        /// </summary>
        /// <param name="XorO">the X or O</param>
        /// <param name="square">The number of the square on the gameboard to place the X or O.</param>
        /// <returns>Tuple with bool for if the move is a valid move and a string message describing why.</returns>
        public (bool isvalid, string msg) MoveIsValid(string XorO, int square)
        {
            string resultMessage = null;
            bool isvalid = false;

            if (square < 0 || square > 9)
            {
                return (isvalid, $"Chosen square must be between 0 and 9. You entered: {square}. Turn aborted, try again.");
            }

            if (Squares[square] == xGlyph || Squares[square] == oGlyph)
            {
                resultMessage = $"The square is already occupied by {Squares[square]}. Turn aborted, try again.";
                return (false, resultMessage);
            }

            int xCount = Squares.Count(s => s == xGlyph);
            int oCount = Squares.Count(s => s == oGlyph);

            switch (XorO)
            {
                case "X":
                    isvalid = xCount == oCount;
                    if (!isvalid) resultMessage = $"It is O's turn to move. Play fair.";
                    break;
                case "O":
                    isvalid = oCount == (xCount - 1);
                    if (!isvalid) resultMessage = $"It is X's turn to move. Play fair.";
                    break;
                default:
                    resultMessage = $"Received {XorO} instead of an X or an O. Turn aborted, try again.";
                    break;
            }

            return (isvalid, resultMessage);
        }

        /// <summary>
        /// Executes a player turn
        /// </summary>
        /// <param name="XorO"></param>
        /// <param name="square"></param>
        /// <returns></returns>
        public MoveResult ExecuteTurn(string XorO, int square)
        {
            var currentGameStatus = IsGameOver();

            if (currentGameStatus.gameIsOver)
            {
                return new MoveResult(false,
                    $"Game is Over. Congratulations to {currentGameStatus.winner}", RenderBoard(), true);
            }

            var moveResult = MoveIsValid(XorO, square);

            if (moveResult.isvalid)
            {
                Squares[square] = XorO;
            }

            var newGameStatus = IsGameOver();

            if (newGameStatus.gameIsOver)
            {
                return new MoveResult(true,
                    $"Game is Over. Congratulations to {newGameStatus.winner}", RenderBoard(), true);
            }

            return new MoveResult(moveResult.isvalid, moveResult.msg, RenderBoard(), false);
        }
    }
}
