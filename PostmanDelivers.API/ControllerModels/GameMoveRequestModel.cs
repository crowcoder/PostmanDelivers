namespace PostmanDelivers.API.ControllerModels
{
    public class GameMoveRequestModel
    {
        public string xoro { get; set; }
        public int whichSquare { get; set; }
        public string playerID { get; set; }
        public long gameId { get; set; }
    }
}
