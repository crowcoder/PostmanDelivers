using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe.Engine
{
    public class PlayerMove
    {
        public int GameID { get; set; }
        public int Square { get; set; }
        public string GameboardBefore { get; set; }
        public string GameboardAfter { get; set; }
        public DateTime? Timestamp { get; set; }
        public Players Player { get; set; }
    }
}
