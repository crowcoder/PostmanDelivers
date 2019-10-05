namespace TikTakToe.Engine
{
    public class MoveResult
    {
        public MoveResult()        {        }
        public MoveResult(bool isValid, string reason, string board, bool gameIsOver)
        {
            IsValid = isValid;
            Reason = reason;
            GameBoard = board;
            GameIsOver = gameIsOver;
        }

        public bool IsValid { get; set; }
        public bool GameIsOver { get; set; }
        public string Reason { get; set; }
        public string GameBoard { get; set; }
    }
}
