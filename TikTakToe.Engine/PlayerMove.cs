namespace TikTakToe.Engine
{
    public class PlayerMove
    {
        public long? GameID { get; set; }
        public long? Square { get; set; }
        public string GameboardBefore { get; set; }
        public string GameboardAfter { get; set; }
        public long? Timestamp { get; set; }
        public Players Player { get; set; }
    }
}
